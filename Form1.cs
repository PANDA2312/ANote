using AX;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace ANote
{
    public partial class Form1 : Form
    {
        public string arg;
        public void clc()
        {
            btnSave.Text = "保存";
            btnOpen.Text = "打开";
            btnAbout.Text = "关于";
            btnLang.Text = "语言";
            btnFont.Text = "字体";
        }
        public void cle()
        {
            btnSave.Text = "Save As";
            btnOpen.Text = "Open";
            btnAbout.Text = "About";
            btnLang.Text = "Langugue";
            btnFont.Text = "Font";
        }
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
        string filenap = System.Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") +"log.alf"; 
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
        void RegisteEvent()
        {
            this.DragEnter += textBox1_DragEnter;
            this.DragDrop += textBox1_DragDrop;
        }
        public void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject ido = e.Data;    
            string[] paths = (string[])ido.GetData(DataFormats.FileDrop);
            foreach (var path in paths)
            {
                StreamReader sr = new StreamReader(path);
                string text=sr.ReadToEnd();
                txtNote.Text = string.Empty;
                sr.Close();
                txtNote.Text = text;
            }
        }
        public void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        public void button1_Click(object sender, EventArgs e)
        {
            if (!zhcn)
            {
                saveFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "TextFile";
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    sw.Write(txtNote.Text);
                    sw.Flush();
                    save = true;
                    sw.Close();
                }
            }
            else
            {
                saveFileDialog1.Filter = "文本文件|*.txt|ANo文件|*.anf|ANote日志文件|*.alf|所有文件|*.*";
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "TextFile";
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                    sw.Write(txtNote.Text);
                    sw.Flush();
                    save = true;
                    sw.Close();
                }
            }
        }
        public void button2_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileContent = string.Empty;
            openFileDialog1 = new OpenFileDialog();
            if (zhcn)
            {
                openFileDialog1.Filter = "文本文件|*.txt|ANo文件|*.anf|ANote日志文件|*.alf|所有文件|*.*";
                openFileDialog1.RestoreDirectory = true;
                DialogResult reslt = openFileDialog1.ShowDialog();
                if (reslt == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    filePath = openFileDialog1.FileName;
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    var fileStream = openFileDialog1.OpenFile();
                    StreamReader reader = new StreamReader(fileStream);
                    fileContent = reader.ReadToEnd();
                    reader.Close();
                    txtNote.Text = string.Empty;
                    sr.Close();
                    txtNote.Text = fileContent;
                    sr.Close();
                    save = true;
                }
            }
            else
            {
                openFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
                openFileDialog1.RestoreDirectory = true;
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    filePath = openFileDialog1.FileName;
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    var fileStream = openFileDialog1.OpenFile();
                    StreamReader reader = new StreamReader(fileStream);
                    fileContent = reader.ReadToEnd();
                    reader.Close();
                    txtNote.Text = string.Empty;
                    sr.Close();
                    txtNote.Text = fileContent;
                    sr.Close();
                    save = true;
                }
            }
        }
        public void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }
        public void button3_Click(object sender, EventArgs e)
        {
            if (save == false)
            {
                if (txtNote.Text == string.Empty)
                {
                    Application.Exit();
                }
                else if (zhcn)
                {
                    DialogResult msgresult = MessageBox.Show("文件未保存，是否保存？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (msgresult ==DialogResult.Yes)
                    {
                        saveFileDialog1.Filter = "文本文件|*.txt|ANo文件|*.anf|ANote日志文件|*.alf|所有文件|*.*";
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = "文档";
                        DialogResult result = saveFileDialog1.ShowDialog();
                        if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                        {
                            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                            sw.Flush();
                            sw.Write(txtNote.Text);
                            save = true;
                            sw.Close();
                        }
                    }
                    else if (msgresult==DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    DialogResult msgresult = MessageBox.Show("File not save!,Save it?", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (msgresult == DialogResult.Yes)
                    {
                        saveFileDialog1.Filter = "TextFile|*.txt|ANoteFile|*.anf|ANoteLogFile|*.alf|All files|*.*";
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = "TextFile";
                        DialogResult result = saveFileDialog1.ShowDialog();
                        if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                        {
                            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8);
                            sw.Flush();
                            sw.Write(txtNote.Text);
                            save = true;
                            sw.Close();
                        }
                    }
                    else if (msgresult==DialogResult.No)
                    {
                        Application.Exit();
                    }
                } 
            }
            else
            {
                Application.Exit();
            }
        }
        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            save = false;
            if(save == false)
            {
                log.Write(log.Writetype.Warn,filenap,"textNote","text changed!");
            }
        }  
        public void Form1_Load(object sender, EventArgs e)
        {
            pnlDrag.Width = Screen.PrimaryScreen.Bounds.Width;
            txtNote.Height = this.Height - 80;
            this.RegisteEvent();
            timer1.Start();
            log.Create(filenap);
            log.Write(log.Writetype.Info,filenap, "Load", "ANote v1.0.8");
            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            if (Thread.CurrentThread.CurrentCulture.Name=="zh-CN")
            {
                clc();
            }
            if (Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
            {
                zhcn = true;
            }
            else
            {
                zhcn = false;
            }
        }
        bool isMouseDown = false; 
        MouseDirection direction = MouseDirection.None;
        public void ResizeWindow()
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
        public void Form1_MouseMove(object sender, MouseEventArgs e)
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
        public void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        public void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        public void button4_Click(object sender, EventArgs e)
        {
            SendToBack();
        }
        public void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            txtNote.Width = this.Width;
            txtNote.Height = this.Height - 80;
            btnExit.Left = this.ClientSize.Width - 27;
            btnBack.Left = this.ClientSize.Width - 81;
            btnFull.Left = this.ClientSize.Width - 54;
        }
        public void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1=new AboutBox1();
            ab1.Show();
        }
        public void notifyIcon1_Click(object sender, EventArgs e)
        {
            TopMost=true;
            TopMost=false;
        }
        bool biggest = false;
        public void button6_Click(object sender, EventArgs e)
        {
            if (biggest==false)
            {
                this.Height = 480;
                this.Width = 760;
                this.Top = 0;
                this.Left = 0;
                for (int i=Screen.PrimaryScreen.Bounds.Height/1080*68; this.Width < Screen.PrimaryScreen.Bounds.Width;i-=2)
                {
                    this.Height+=i;
                    this.Width+=i;  
                }
                this.ClientSize = Screen.PrimaryScreen.WorkingArea.Size;
                this.Top = 0;
                this.Left = 0;
                biggest = true;
            }
            else
            {
                for (int i=0;this.Width > 760;i++)
                {
                    this.Height -= i*2;
                    this.Width -= i*2;
                } 
                for (; this.Left < Screen.PrimaryScreen.WorkingArea.Width / 4;)
                {
                    this.Top+=1;this.Left += 1;
                    this.Opacity = 0;
                }
                for (int i = 0; i < 24; i++)
                {
                    this.Height+=Convert.ToInt32(Convert.ToDouble(i)*1.8);
                }
                this.Opacity = 10;
                this.Opacity = 40;
                this.Opacity = 80;
                this.Opacity = 100;
                this.Top = Screen.PrimaryScreen.WorkingArea.Height / 4;
                this.Left = Screen.PrimaryScreen.WorkingArea.Width / 4;
                biggest = false;
            }    
        }
        bool zhcn;
        public void button7_Click(object sender, EventArgs e)
        {
            if (zhcn==true)
            {
                cle();
                zhcn = false;
            }
            else
            {
                clc();
                zhcn = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (zhcn)
            {
                label2.Text = "共"+ (txtNote.GetLineFromCharIndex(txtNote.TextLength)+1).ToString()+"行";
            }
            else
            {
                label2.Text = txtNote.GetLineFromCharIndex(txtNote.TextLength).ToString()+" Lines";
            }
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog1.ShowDialog();
            if (result==DialogResult.OK)
            {
                txtNote.Font = fontDialog1.Font;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
    }
}