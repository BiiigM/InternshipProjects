using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WBH_Rezept
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                if (args[0] == "a")
                {
                    //Auftragsnummer
                    string customNumber = args[1];
                    if(customNumber.Length >= 1 && customNumber.Length <= 10)
                    {
                        Form1 form = new Form1(customNumber);
                    }
                }
                else
                {
                    MessageBox.Show("ne");
                    Console.WriteLine("Missing parameters: xxx.exe a 'customNumber' ");
                }
            }
        }
    }
}