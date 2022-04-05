using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GUISocketClient2
{
    public partial class Form1 : Form
    {
        #region Vars
        private static readonly IPAddress Ip = IPAddress.Parse("127.0.0.1");
        private const int Port = 8080;
        private static readonly IPEndPoint localEndPoint = new IPEndPoint(Ip, Port);
        private static Socket ClientSocket;

        private string Name;
        private byte[] nameIdxBuffer;
        private readonly NameGiver NG;

        private bool ServerOnline = true;
        private bool closing = false;

        private BackgroundWorker bk;
        private Thread refresher;
        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        #endregion
        public Form1(string name, NameGiver nameGiver)
        {
            InitializeComponent();
            
            this.Text = @"Chat from: " + name;
            Name = name;
            nameIdxBuffer = Encoding.UTF8.GetBytes(name + "_");
            NG = nameGiver;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            
            //try to Connect to the Server
            //he is doing it in the Background
            bk = new BackgroundWorker();
            bk.DoWork += ConnectToServer;
            bk.WorkerReportsProgress = true;
            bk.RunWorkerAsync();
        }
        
        #region GetInput
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!ClientSocket.Connected)
            {
                MessageBox.Show("Chill mal du bist nicht connected.\r\nGuckmal ob der Server läuft", @"Info");
                return;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Kein anderer Client Connected (such dir Freunde).", "Info");
                return;
            }
            if (string.IsNullOrEmpty(tbInput.Text))
            {
                MessageBox.Show("Du muss auch was schreiben", "Info");
                return;
            }
            
            byte[] inputBuffer = Encoding.UTF8.GetBytes(tbInput.Text);
            SendData(GetBytesFromInput(nameIdxBuffer, inputBuffer));
            
            tbOutput.Text += Name + @": " + tbInput.Text + "\r\n";
            tbInput.Text = "";
        }
        
        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.Enter) && tbInput.Text.Length != 0)
            {
                if (!ClientSocket.Connected)
                {
                    MessageBox.Show("Chill mal du bist nicht connected.\r\nGuckmal ob der Server läuft", @"Info");
                    return;
                }
                if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    MessageBox.Show("Kein anderer Client Connected (such dir Freunde).", "Info");
                    return;
                }
                if (string.IsNullOrEmpty(tbInput.Text))
                {
                    MessageBox.Show("Du muss auch was schreiben", "Info");
                    return;
                }

                byte[] inputBuffer = Encoding.UTF8.GetBytes(tbInput.Text);
                SendData(GetBytesFromInput(nameIdxBuffer, inputBuffer));

                tbOutput.Text += Name + @": " + tbInput.Text + "\r\n";
                tbInput.Text = "";
            }
        }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            byte[] fileBuffer = File.ReadAllBytes(files[0]);
            string fileName = Path.GetFileName(files[0]);
            
            if (fileName.Contains("_"))
            {
                MessageBox.Show("'_' sind im Namen der Datei nicht gestattet", "Info");
                return;
            }
            if (fileBuffer.Length > 1038576)
            {
                MessageBox.Show("Die Datei ist zu groß!!");
                return;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Kein anderer Client Connected (such dir Freunde).", "Info");
                return;
            }
            
            SendData(GetBytesFromFile(nameIdxBuffer, fileBuffer, fileName));
        }
        #endregion
        
        private void ConnectToServer(Object sender, DoWorkEventArgs eventArgs)
        {
            ClientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    lInfo.Text = @"Connection attempts: " + attempts;
                    ClientSocket.Connect(Ip, Port);
                }
                catch (SocketException e)
                {
                    /*MessageBox.Show(e.Message);*/
                }
            }
            lInfo.Text = @"Connected";
            ServerOnline = true;
            
            DoHandShacke();
            
            //Starting the Refresher
            refresher = new Thread(ReceiveResponse) {IsBackground = true};
            refresher.Start();
        }
        
        private void ReceiveResponse()
        {
            while (ServerOnline)
            {
                try
                {
                    byte[] buffer = new byte[1038576];
                    int received = ClientSocket.Receive(buffer, 0);
                    if (received == 0) return;
                    
                    byte[] tmpBuffer = new byte[received];
                    Array.Copy(buffer, tmpBuffer, received);

                    string receivedText = Encoding.UTF8.GetString(tmpBuffer);

                    if (receivedText.Equals("closing"))
                    {
                        lInfo.Invoke((MethodInvoker) delegate
                        {
                            lInfo.Text = @"Connection Lost";
                        });
                        MessageBox.Show(@"Server wurde geschlossen", "Info");
                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                        bk.RunWorkerAsync();
                        break;
                    }
                    
                    DisplayData(tmpBuffer);
                }
                catch (SocketException e)
                {
                    lInfo.Invoke((MethodInvoker) delegate
                    {
                        lInfo.Text = @"Connection Lost";
                    });
                    MessageBox.Show(@"Verbindung verloren", "Info");
                    bk.RunWorkerAsync();
                    break;
                }
            }
        }
        
        private static void SendData(byte[] sendData)
        {
            ClientSocket.Send(sendData, 0, sendData.Length, 0);
        }

        #region GetBytes
        private byte[] GetBytesFromInput(byte[] nameIdx, byte[] input)
        {
            //exsample: <TEXT>_Sender_SendTo_Data
            byte[] textIdx = Encoding.UTF8.GetBytes("<TEXT>_");
            byte[] toSend = Encoding.UTF8.GetBytes(comboBox1.SelectedItem + "_");
            int returnBytesLength = textIdx.Length + nameIdx.Length + toSend.Length + input.Length;
            
            byte[] returnBytes = new byte[returnBytesLength];
            textIdx.CopyTo(returnBytes, 0);
            nameIdx.CopyTo(returnBytes, textIdx.Length);
            toSend.CopyTo(returnBytes, textIdx.Length + nameIdx.Length);
            input.CopyTo(returnBytes, textIdx.Length + nameIdx.Length + toSend.Length);

            return returnBytes;
        }

        private byte[] GetBytesFromFile(byte[] nameIdx, byte[] file, string name)
        {
            //exsample: <FILE>_Filename_Sender_SendTo_Data
            byte[] fileIdx = Encoding.UTF8.GetBytes("<FILE>_");
            byte[] fileName = Encoding.UTF8.GetBytes( name + "_");
            int returnBytesLength = fileIdx.Length + fileName.Length + nameIdx.Length + file.Length;
            
            byte[] returnFileBytes = new byte[returnBytesLength];
            fileIdx.CopyTo(returnFileBytes, 0);
            fileName.CopyTo(returnFileBytes, fileIdx.Length);
            nameIdx.CopyTo(returnFileBytes, fileIdx.Length + fileName.Length);
            file.CopyTo(returnFileBytes, fileIdx.Length + fileName.Length + nameIdx.Length);

            return returnFileBytes;
        }

        private byte[] GetBytesForHandshack(byte[] name)
        {
            //exsample: <NAME>_Sender_
            byte[] nameIdx = Encoding.UTF8.GetBytes("<NAME>_");
            int length = nameIdx.Length + name.Length;

            byte[] returnBytes = new byte[length];
            nameIdx.CopyTo(returnBytes, 0);
            name.CopyTo(returnBytes, nameIdx.Length);

            return returnBytes;
        }

        private byte[] GetImageBytes(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
        #endregion

        #region DisplayAllData
        private void DisplayData(byte[] data)
        {
            string sData = Encoding.UTF8.GetString(data);
            string[] textData = sData.Split('_');
            
            switch (textData[0])
            {
                case "<TEXT>":
                    tbOutput.Text += GetTextString(textData);
                    tbOutput.Text += "\r\n";
                    break;
                case "<FILE>":
                    SaveFile(data);
                    break;
                case "<NAME>":
                    SetComboBoxItems(textData);
                    break;
            }
        }

        private string GetTextString(string[] textData)
        {
            string namePrefix = textData[1] + ": ";
            string joinedText1 = String.Join("_", textData, 3, textData.Length - 3);
            return  namePrefix + joinedText1;
        }

        private void SaveFile(byte[] dataFile)
        {
            string sDataFile = Encoding.Default.GetString(dataFile);
            string[] fileNameWithSenderAndText = sDataFile.Split('_');
            string fileName = fileNameWithSenderAndText[1];
            string sData = String.Join("_", fileNameWithSenderAndText, 3, fileNameWithSenderAndText.Length - 3);
            byte[] data = Encoding.Default.GetBytes(sData);
            
            
            DialogResult dr = MessageBox.Show($"Du hast eine Datei von {fileNameWithSenderAndText[2]} erhalten." + "\r\n willst du es speichern?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
                OpenSaveFileDialog(fileName, data);
        }

        private void SetComboBoxItems(string[] Names)
        {
            comboBox1.Invoke((MethodInvoker)delegate
            {
                comboBox1.Items.Clear();
                comboBox1.Text = "";

                foreach (string name in Names)
                {
                    if(name.Equals("<NAME>") || name.Equals(Name)) continue;
                    comboBox1.Items.Add(name);
                }

                try
                {
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception e)
                {
                    //Ignore
                }
            });

        }

        private void OpenSaveFileDialog(string filename, byte[] data)
        {
            Invoke((MethodInvoker)delegate
            {
                saveFileDialog.FileName = filename;
                saveFileDialog.Filter = "All files (*.*)|*.*";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, data);
                }
            });
        }
        #endregion

        private void DoHandShacke()
        {
            SendData(GetBytesForHandshack(nameIdxBuffer));
            
            byte[] buffer = new byte[1038576];
            int received = ClientSocket.Receive(buffer, 0);
            if (received == 0) return;
            
            byte[] tmpBuffer = new byte[received];
            Array.Copy(buffer, tmpBuffer, received);
            
            string receibedText = Encoding.UTF8.GetString(tmpBuffer);
            string[] splitedText = receibedText.Split('_');

            if (splitedText[0].Equals("<CHANGE>"))
            {
                Name = splitedText[1];
                nameIdxBuffer = Encoding.UTF8.GetBytes(Name + "_");
                this.Text = @"Chat from: " + Name;
            }
            
            byte[] finishedBuffer = Encoding.UTF8.GetBytes("<FINISHED>");
            ClientSocket.Send(finishedBuffer);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                refresher.Abort();
            }
            catch (Exception)
            {
                //Ignore
            }

            ServerOnline = false;
            byte[] exitBytes = Encoding.UTF8.GetBytes(Name + "_exit");
            if(ClientSocket.Connected)
            {
                SendData(exitBytes);
                ClientSocket.Shutdown(SocketShutdown.Both);
            }
            ClientSocket.Close();
            NG.Close();
        }
    }
}