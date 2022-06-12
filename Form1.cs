using AX;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace ANote
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public Form1()
        {
            InitializeComponent();
        }
        log log = new log();
        string time = DateTime.Now.ToString("HH:mm:ss");
        string filenap = System.Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("yyyy-HH-mm-ss") +"log.alf";
        public enum MouseDirection
        {
            East,
            West,
            South,
            North,
            Southeast,
            Southwest,
            Northeast,
            Northwest,
            Vertical,
            Herizontal,
            Declining,
            None
        }
        bool save = false;
        void Init()
        {
            this.AllowDrop = true;
        }
        void RegisteEvent()
        {
            this.DragEnter += textBox1_DragEnter;
            this.DragDrop += textBox1_DragDrop;
        }
        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject ido = e.Data;    
            string[] paths = (string[])ido.GetData(DataFormats.FileDrop);
            foreach (var path in paths)
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string text=sr.ReadToEnd();
                    textBox1.Text = string.Empty;
                    sr.Close();
                    textBox1.Text = text;
                }
            }
            
        }
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "TextFile";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                sw.Write(textBox1.Text);
                sw.Flush();
                save = true;
                sw.Close();
            }
        }       
        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileContent = string.Empty;
            openFileDialog1 =new OpenFileDialog();
            openFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                filePath = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                var fileStream = openFileDialog1.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                    reader.Close();
                    textBox1.Text =string.Empty;
                    sr.Close();
                    textBox1.Text = fileContent;
                    sr.Close();
                    save = true;
                }
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (save == false)
            {
                if (textBox1.Text == string.Empty)
                {
                    Close();
                }
                else
                {
                    if (MessageBox.Show("File not save!,Save it?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        saveFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = "TextFile";
                        DialogResult result = saveFileDialog1.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                        {
                            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                            sw.Flush();
                            sw.Write(textBox1.Text);
                            save = true;
                            sw.Close();
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            else
            {
                    Close();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            save = false;
            if(save == false)
            {
                log.WriteWarn(filenap,"textBox1","text changed!");
            }
        }
        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.BackColor = Color.Red;
        }
        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.BackColor = Color.Blue;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Init();
            this.RegisteEvent();
            log.Create(filenap);
            log.WriteInfo(filenap, "main", "ANote Running,ver 1.0.3");
        }
         bool isMouseDown = false; 
        MouseDirection direction = MouseDirection.None;
        private void ResizeWindow()
        {
            if (!isMouseDown)
                return;
            if (direction == MouseDirection.Declining)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.Width = MousePosition.X - this.Left;
                this.Height = MousePosition.Y - this.Top;
            }
            if (direction == MouseDirection.Herizontal)
            {
                this.Cursor = Cursors.SizeWE;
                this.Width = MousePosition.X - this.Left;
            }
            else if (direction == MouseDirection.Vertical)
            {
                this.Cursor = Cursors.SizeNS;
                this.Height = MousePosition.Y - this.Top;
            }
            else
                this.Cursor = Cursors.Arrow;
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (isMouseDown && direction != MouseDirection.None)
            {
                ResizeWindow();
                return;
            }
            if (e.Location.X >= this.Width - 10 && e.Location.Y > this.Height - 10)
            {
                this.Cursor = Cursors.SizeNWSE;
                direction = MouseDirection.Declining;
            }
            else if (e.Location.X >= this.Width - 10)
            {
                this.Cursor = Cursors.SizeWE;
                direction = MouseDirection.Herizontal;
            }
            else if (e.Location.Y >= this.Height - 10)
            {
                this.Cursor = Cursors.SizeNS;
                direction = MouseDirection.Vertical;

            }             
            else
                this.Cursor = Cursors.Arrow;
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendToBack();
        }
        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            textBox1.Size = this.ClientSize;
            button3.Left = this.ClientSize.Width - 27;
            button4.Left = this.ClientSize.Width - 81;
            button6.Left = this.ClientSize.Width - 54;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            AboutBox1 form=new AboutBox1();
            form.Show();
        }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            TopMost=true;
            TopMost=false;
        }
        bool biggest = false;
        private void button6_Click(object sender, EventArgs e)
        {
            if (biggest==false)
            {
                this.ClientSize = Screen.PrimaryScreen.WorkingArea.Size;
                this.Top = 0;
                this.Left = 0;
                biggest = true;
            }
            else
            {
                this.ClientSize=new System.Drawing.Size(758,484);
                this.Top = Screen.PrimaryScreen.WorkingArea.Height / 4;
                this.Left = Screen.PrimaryScreen.WorkingArea.Width / 4;
                biggest = false;
            }    
        }
    }
}