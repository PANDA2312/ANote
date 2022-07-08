using System;
using System.IO;
namespace AX
{
    class log
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string enter = "\r\n";
        public void Create(string filepath)
        {
            if (filepath == string.Empty)
            {
                File.Create(System.Environment.CurrentDirectory + @"\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "log.alf");
            }
            else
            {
                File.Create(filepath).Close();
                File.AppendAllText(filepath, "ALog,version 1.0.1" + enter);
            }
        }
        public void WriteInfo(string filepath, string type,string contents)
        {
            File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/INFO]" + contents+enter);
        }
        public void WriteError(string filepath, string type, string contents)
        {
            File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/ERROR]" + contents + enter);
        }
        public void WriteWarn(string filepath, string type, string contents)
        {
            File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/WARN]" + contents+enter); 
        }
    }
}