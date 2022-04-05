using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FromExcelToSQL
{
    public struct Zeile
    {
        public List<string> cellA;
        public List<string> cellB;
        public string cellC;
        public string cellD;
    }
    
    public partial class Form1 : Form
    {
        private readonly List<Zeile> zeilen = new List<Zeile>();
        private List<string> allItems = new List<string>();
        
        public Form1()
        {
            InitializeComponent();
            GetData();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
            FilterData();
            
            //Displaying the Data
            tbOutput.Text = "";
            tbOutput.Text += "declare @UrsachenID as INT;" + "\r\n";
            tbOutput.Text += "declare @BehebungsID as INT;" + "\r\n";
            foreach (string item in allItems)
            {
                Tuple<string, string> commands = IntoSQLCommand(item);
                tbOutput.Text += commands.Item1;
                tbOutput.Text += "set @UrsachenID = SCOPE_IDENTITY();" + "\r\n";
                tbOutput.Text += commands.Item2;
                tbOutput.Text += "set @BehebungsID = SCOPE_IDENTITY();" + "\r\n";
                tbOutput.Text += "INSERT INTO Map (Ursachen_ID, Behebungs_ID) VALUES (@UrsachenID, @BehebungsID);" + "\r\n";
            }
        }

        private void GetData()
        {
            using (StreamReader sr = new StreamReader(@"##", Encoding.Default))
            {
                string currentLine;

                while ((currentLine = sr.ReadLine()) != null)
                {
                    Zeile zeile = new Zeile();
                    zeile.cellA = new List<string>();
                    zeile.cellB = new List<string>();

                    string[] tmpLine;

                    //Zu erst wird die Zelle in der Spalte A hinzugefügt
                    //Fügt alle zeilen in der Zelle hinzu
                    if (currentLine.Substring(0, 1).Equals("\""))
                    {
                        zeile.cellA.Add(currentLine);
                        
                        while (!(currentLine = sr.ReadLine()).Contains(";"))
                        {
                            zeile.cellA.Add(currentLine);
                        }
                        
                        tmpLine = currentLine.Split(';');
                        zeile.cellA.Add(tmpLine[0]);
                    }
                    //Fügt die eine zeile in der Zelle hinzu
                    else
                    {
                        tmpLine = currentLine.Split(';');
                        zeile.cellA.Add(tmpLine[0]);
                    }
                    
                    //Danach wird die Zelle in der Spalte B hinzugefügt
                    //Fügt alle zeilen in der Zelle hinzu
                    if (tmpLine[1].Substring(0, 1).Equals("\""))
                    {
                        zeile.cellB.Add(tmpLine[1]);

                        while (!(currentLine = sr.ReadLine()).Contains("\""))
                        {
                            zeile.cellB.Add(currentLine);
                        }

                        tmpLine = currentLine.Split(';');
                        zeile.cellB.Add(tmpLine[0]);
                    }
                    //Fügt die eine zeile in der Zelle hinzu
                    else
                    {
                        zeile.cellB.Add(tmpLine[1]);
                    }
                    
                    //Hier wird der Component gespeichert
                    zeile.cellC = tmpLine[tmpLine.Length - 2];

                    //Hier wird die Gruppe gespeichert
                    if (tmpLine[tmpLine.Length - 1].Contains("["))
                    {
                        string GruppenSymbol = tmpLine[tmpLine.Length - 1].Split('[')[1];
                        zeile.cellD = Regex.Match(GruppenSymbol, "[a-zA-Z]").Groups[0].Value;
                    }
                    if (string.IsNullOrWhiteSpace(zeile.cellD))
                    {
                        zeile.cellD = "NONE";
                    }

                    zeilen.Add(zeile);
                }
            }
        }

        private void FilterData()
        {
            allItems = new List<string>();
            foreach (Zeile zeile in zeilen)
            {
                string stringCellA = "";
                string stringCellB = "";
                string stringCellC = zeile.cellC;
                string stringCellD = zeile.cellD;
                
                foreach (string s in zeile.cellA)
                {
                    stringCellA += s + ";";
                }
                foreach (string s in zeile.cellB)
                {
                    stringCellB += s + ";";
                }

                stringCellA = stringCellA.TrimEnd(';');
                stringCellB = stringCellB.TrimEnd(';');

                stringCellA = stringCellA.Replace("\"", "");
                stringCellB = stringCellB.Replace("\"", "");
                

                string textZeile = stringCellA + "|" + stringCellB + "|" + stringCellC;

                if (!tbOutput.Text.Contains(textZeile))
                {
                    tbOutput.Text += textZeile;
                    textZeile += "|" + stringCellD;
                    allItems.Add(textZeile);
                }
            }
        }

        private Tuple<string, string> IntoSQLCommand(string text)
        {
            string[] allCells = text.Split('|');
            string[] cellA = allCells[0].Split(';');
            string[] cellB = allCells[1].Split(';');
            string cellC = allCells[2];
            string cellD = allCells[3];
            
            string outputStringA = "";
            string outputStringB = "";

            switch (cellB.Length)
            {
                case 1:
                    outputStringB = "INSERT INTO Behebungen (Behebung1, Behebung2, Behebung3, Behebung4, Component) VALUES " + $"('{cellB[0]}', '', '', '', '{cellC}'); " + "\r\n";
                    break;
                case 2:
                    outputStringB = "INSERT INTO Behebungen (Behebung1, Behebung2, Behebung3, Behebung4, Component) VALUES " + $"('{cellB[0]}', '{cellB[1]}', '', '', '{cellC}'); " + "\r\n";
                    break;
                case 3:
                    outputStringB = "INSERT INTO Behebungen (Behebung1, Behebung2, Behebung3, Behebung4, Component) VALUES " + $"('{cellB[0]}', '{cellB[1]}', '{cellB[2]}', '', '{cellC}'); " + "\r\n";
                    break;
                case 4:
                    outputStringB = "INSERT INTO Behebungen (Behebung1, Behebung2, Behebung3, Behebung4, Component) VALUES " + $"('{cellB[0]}', '{cellB[1]}', '{cellB[2]}', '{cellB[3]}', '{cellC}'); " + "\r\n";
                    break;
            }
            
            switch (cellA.Length)
            {
                case 1:
                    outputStringA = "INSERT INTO Ursachen (Ursache1, Ursache2, Ursache3, Ursache4, Component, Gruppe) VALUES " + $"('{cellA[0]}', '', '', '', '{cellC}', '{cellD}'); " + "\r\n";
                    break;
                case 2:
                    outputStringA = "INSERT INTO Ursachen (Ursache1, Ursache2, Ursache3, Ursache4, Component, Gruppe) VALUES " + $"('{cellA[0]}', '{cellA[1]}', '', '', '{cellC}', '{cellD}'); " + "\r\n";
                    break;
                case 3:
                    outputStringA = "INSERT INTO Ursachen (Ursache1, Ursache2, Ursache3, Ursache4, Component, Gruppe) VALUES " + $"('{cellA[0]}', '{cellA[1]}', '{cellA[2]}', '', '{cellC}', '{cellD}'); " + "\r\n";
                    break;
                case 4:
                    outputStringA = "INSERT INTO Ursachen (Ursache1, Ursache2, Ursache3, Ursache4, Component, Gruppe) VALUES " + $"('{cellA[0]}', '{cellA[1]}', '{cellA[2]}', '{cellA[3]}', '{cellC}', '{cellD}'); " + "\r\n";
                    break;
            }
            

            return new Tuple<string, string>(outputStringA, outputStringB);
        }
    }
}