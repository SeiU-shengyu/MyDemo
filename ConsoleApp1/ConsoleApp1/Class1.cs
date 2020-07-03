using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ConsoleApp1
{
    class Class1
    {
        public void ReadWriteTest()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()) + "\\test.txt");

            StreamReader sr = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()) + "\\test.json");
            JsonTextReader jtr = new JsonTextReader(sr);
            JObject jObject = JToken.ReadFrom(jtr) as JObject;
            Console.WriteLine(jObject["user"]);
            sr.Close();
        }

        public int[][] GetMap()
        {
            StreamReader sr = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()) + "\\map.txt");
            string[] strs = sr.ReadToEnd().Split("\n");
            int[][] map = new int[strs.Length][];
            for (int i = 0; i < strs.Length; i++)
            {
                string[] strs1 = strs[i].Split(",");
                map[i] = new int[strs.Length];
                for (int j = 0; j < strs1.Length; j++)
                {
                    map[i][j] = int.Parse(strs1[j]);
                }
            }
            return map;
        }
    }
}
