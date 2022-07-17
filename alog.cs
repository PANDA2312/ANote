using System;
using System.IO;
namespace AX
{
    class log
    {
        public enum Writetype
        { Info, Error, Warn }
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
                File.AppendAllText(filepath, "ALog,version 1.0.2" + enter);
            }
        }
        public void Write(Writetype writetype,string filepath, string type,string contents)
        {
            if (writetype==Writetype.Info)
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/INFO]" + contents+enter);
            } else if(writetype==Writetype.Error)
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/ERROR]" + contents + enter);
            } else
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/WARN]" + contents+enter); 
            }
        }
    }
}