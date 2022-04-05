using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ArrySorter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private float[] numbers;
        private string[] words;

        private void btnStart_Click(object sender, EventArgs e)
        {
            GetInput();
            SortArray();
            PrintOut();
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "";
        }

        private void GetInput()
        {
            int intCounter = 0;
            int stringCounter = 0;
            
            foreach (var line in tbInput.Lines)
            {
                if (float.TryParse(line, out float tmp))
                {
                    intCounter++;
                }
                else
                {
                    stringCounter++;
                }
            }
            
            numbers = new float[intCounter];
            words = new string[stringCounter];
            
            SaveArrays();

        }

        private void SaveArrays()
        {
            int intCounter = 0;
            int stringCounter = 0;
            
            for (int i = 0; i <= tbInput.Lines.Length - 1; i++)
            {
                if (float.TryParse(tbInput.Lines[i], out float tmp))
                {
                    numbers[intCounter] = tmp;
                    intCounter++;
                }
                else
                {
                    words[stringCounter] = tbInput.Lines[i] + " ";
                    stringCounter++;
                }
            }
        }

        private void SortArray()
        {
            SortNumbers sn = new SortNumbers(numbers);
            SortWords sw = new SortWords(words);
            
            if (rbAuf.Checked)
            {
                sn.SortAufInt();
                sw.SortAufString();
            }
            else
            {
                sn.SortAbInt();
                sw.SortAbString();
            }
        }

        private void PrintOut()
        {
            tbOutput.Text = "";
            
            foreach (var word in words)
            {
                tbOutput.Text += word.Remove(word.Length - 1, 1) + Environment.NewLine;
            }

            tbOutput.Text += "----------Numbers------------" + Environment.NewLine;

            foreach (var number in numbers)
            {
                tbOutput.Text += number + Environment.NewLine;
            }
        }
    }
}