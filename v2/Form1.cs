//
//  ANote v2 by Austin_Xu
//  version 1.1.2
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace ANote_v2
{
    public partial class Form1 : Form
    {
        static string savedFileName = string.Empty;
        static bool isRead = false;
        static string fileContent = string.Empty;
        static bool isSave = false;
        static string fileName = string.Empty;
        //已弃用:
        //static bool isUndoMode = false;
        //static uint textChangeCount = 1;
        public Form1()
        {
            InitializeComponent();
        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
        //获取文件编码
        public System.Text.Encoding GetFileEncodeType(string filename)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                byte[] buffer = br.ReadBytes(2);
                if (buffer[0] >= 0xEF)
                {
                    if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                    {
                        br.Close();
                        return System.Text.Encoding.UTF8;
                    }
                    else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                        br.Close();
                        return System.Text.Encoding.BigEndianUnicode;
                    }
                    else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        br.Close();
                        return System.Text.Encoding.Unicode;
                    }
                    else
                    {
                        br.Close();
                        return System.Text.Encoding.Default;
                    }
                }
                else
                {
                    br.Close();
                    return System.Text.Encoding.Default;
                }
            }
        }
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*|C语言文件|*.c|C++文件|*.cpp|Java文件|*.java|JavaScript文件|*.js|Html文件|*.html|C#文件|*.cs|php文件|*.php|所有文件|*.*";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                ThreadPool.SetMaxThreads(1000, 1000);
                byte[] bytes = new byte[134217728];
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                FileData fd = new FileData();
                bytes = Encoding.Unicode.GetBytes(mainTextBox.Text);
                fs.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(CallbackWrite), fs);
                fs.Flush();
            }
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (savedFileName == string.Empty)
            {
                saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*|C语言文件|*.c|C++文件|*.cpp|Java文件|*.java|JavaScript文件|*.js|Html文件|*.html|C#文件|*.cs|php文件|*.php|所有文件|*.*";
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "";
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                {
                    ThreadPool.SetMaxThreads(1000, 1000);
                    byte[] bytes = new byte[134217728];
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                    FileData fd = new FileData();
                    bytes = Encoding.Unicode.GetBytes(mainTextBox.Text);
                    fs.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(CallbackWrite), fs);
                    fs.Flush();
                }
            }
            else
            {
                ThreadPool.SetMaxThreads(1000, 1000);
                byte[] bytes = new byte[134217728];
                FileStream fs = new FileStream(savedFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                FileData fd = new FileData();
                bytes = Encoding.Unicode.GetBytes(mainTextBox.Text);
                fs.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(CallbackWrite), fs);
                fs.Flush();
            }
        }
        private void mainTextBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void mainTextBox_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject ido = e.Data;
            string[] paths = (string[])ido.GetData(DataFormats.FileDrop);
            foreach (var path in paths)
            {
                ThreadPool.SetMaxThreads(1000, 1000);
                byte[] byteData = new byte[134217728];
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                FileData fd = new FileData();
                fd.Stream = fs;
                fd.Length = (int)fs.Length;
                fd.ByteData = byteData;
                fs.BeginRead(byteData, 0, fd.Length, new AsyncCallback(CallbackRead), fd);
                while (!isRead) { }
                mainTextBox.Text = fileContent;
                isRead = false;
                fileContent = string.Empty;
            }
        }
        public class FileData
        {
            public FileStream Stream;
            public int Length;
            public byte[] ByteData;
        }
        //异步写入文件
        static void CallbackWrite(IAsyncResult result)
        {
            FileStream stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Close();
            isSave = true;
        }
        //异步读取文件
        public void CallbackRead(IAsyncResult result)
        {
            FileData fileData = (FileData)result.AsyncState;
            int length = fileData.Stream.EndRead(result);
            fileData.Stream.Close();
            if (length != fileData.Length)
                throw new Exception("Stream is not complete!");
            string data = GetFileEncodeType(fileName).GetString(fileData.ByteData, 0, fileData.Length);
            fileContent += data;
            isRead = true;
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*|C语言文件|*.c|C++文件|*.cpp|Java文件|*.java|JavaScript文件|*.js|Html文件|*.html|C#文件|*.cs|php文件|*.php|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            DialogResult reslt = openFileDialog1.ShowDialog();
            if (reslt == DialogResult.OK && !string.IsNullOrEmpty(openFileDialog1.FileName))
            {
                fileName = openFileDialog1.FileName;
                ThreadPool.SetMaxThreads(1000, 1000);
                byte[] byteData = new byte[134217728];
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                FileData fd = new FileData();
                fd.Stream = fs;
                fd.Length = (int)fs.Length;
                fd.ByteData = byteData;
                fs.BeginRead(byteData, 0, fd.Length, new AsyncCallback(CallbackRead), fd);
                //等待读取完成
                while (!isRead) { }
                mainTextBox.Text = fileContent;
                isRead = false;
                fileContent = string.Empty;
                fileName = string.Empty;
            }
        }
        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                mainTextBox.Font = fontDialog1.Font;
            }
        }
        private void mainTextBox_TextChanged(object sender, EventArgs e)
        {
            isSave = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainTextBox.Text != string.Empty&&isSave)
            {
                DialogResult dialog = MessageBox.Show("文件未保存,是否保存?", "提示", MessageBoxButtons.OKCancel);
                if (dialog == DialogResult.OK)
                {
                    saveFileDialog1.Filter = "文本文件|*.txt|所有文件|*.*|C语言文件|*.c|C++文件|*.cpp|Java文件|*.java|JavaScript文件|*.js|Html文件|*.html|C#文件|*.cs|php文件|*.php|所有文件|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = "";
                    DialogResult result = saveFileDialog1.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog1.FileName))
                    {
                        ThreadPool.SetMaxThreads(1000, 1000);
                        byte[] bytes = new byte[134217728];
                        FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);
                        FileData fd = new FileData();
                        bytes = Encoding.Unicode.GetBytes(mainTextBox.Text);
                        fs.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(CallbackWrite), fs);
                        fs.Flush();
                    }
                }
            }
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            mainTextBox.Height = this.Height-70;
            mainTextBox.Width = this.Width-18;
            mainTextBox.Location = new Point(1,29);
        }
    }
}