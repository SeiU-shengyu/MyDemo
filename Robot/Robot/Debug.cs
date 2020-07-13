using System;
using System.IO;

namespace Robot
{
    class Debug
    {
        static bool isDebug = false;
        public static void Log(string str,string fileName)
        {
            if (isDebug)
                Console.WriteLine(str);
            string logDiretory = Directory.GetCurrentDirectory() + "\\localRobotsDir\\" + DateTime.Now.Month.ToString() + DateTime.Now.Day;
            if (!Directory.Exists(logDiretory))
                Directory.CreateDirectory(logDiretory);
            FileStream fs = new FileStream(logDiretory + "\\Log" + fileName, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            str = DateTime.Now + "\n" + str;
            sw.WriteLine(str);
            sw.Close();
            fs.Close();
        }
    }
}
