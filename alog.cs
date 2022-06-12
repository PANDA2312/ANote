using System;
using System.IO;
using System.Threading;
namespace AX
{
    class log
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string enter = "\r\n";
        public void Create(string filepath)
        {
            File.Create(filepath).Close();
            File.AppendAllText(filepath,"ALog,version 1.0.0"+enter);
        }
        public void WriteInfo(string filepath, string type,string contents)
        {
            try
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/INFO]" + contents+enter);
            }
            catch (Exception)
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[ALog/ERROR]" + "Cannot write,file in use" + enter);
            }
        }
        public void WriteError(string filepath, string type, string contents)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/ERROR]" + contents + enter);
            }
            catch(Exception)
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[ALog/ERROR]" + "Cannot write,file in use" + enter);
            }
        }
        public void WriteWarn(string filepath, string type, string contents)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[" + type + "/WARN]" + contents+enter);
            }
            catch (Exception)
            {
                File.AppendAllText(filepath, "[" + time + "]" + "[ALog/ERROR]" + "Cannot write,file in use"+enter);
            }   
        }
    }
}