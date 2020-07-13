using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Robot
{
    class Tools
    {
        /// <summary>
        /// 随机获取1到100的整数
        /// </summary>
        /// <returns></returns>
        public static int GetRate()
        {
            Random rd = new Random();
            return rd.Next(1, 101);
        }
        /// <summary>
        /// 随机获取min到max-1的整数
        /// </summary>
        /// <param name="min">最小整数(包含)</param>
        /// <param name="max">最大整数(不包含)</param>
        /// <returns></returns>
        public static int GetRate(int min, int max)
        {
            Random rd = new Random();
            return rd.Next(min, max);
        }
        /// <summary>
        /// 获取当前时间(秒数)
        /// </summary>
        /// <returns></returns>
        public static long GetCurTime()
        {
            return DateTime.Now.Ticks / 10000000;
        }
        /// <summary>
        /// 获取今天当前的秒数
        /// </summary>
        /// <returns></returns>
        public static int GetToDaySeconds()
        {
            return (DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second;
        }
        /// <summary>
        /// 获取今天当前的秒数，保留小数
        /// </summary>
        /// <returns></returns>
        public static float GetToDayTime()
        {
            return (DateTime.Now.Hour * 60 + DateTime.Now.Minute) * 60 + DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0f;
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件夹目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetFileString(string path,string fileName)
        {
            string str = null;
            if (Directory.Exists(path) && File.Exists(path + "\\" + fileName))
            {
                FileStream fs = new FileStream(path + "\\" + fileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                str = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            return str;
        }
    }
}
