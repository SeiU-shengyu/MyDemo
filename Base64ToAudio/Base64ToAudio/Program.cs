using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Base64ToAudio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DecoderBase64File("D:/Mine/MyTest/Base64ToAudio/Resourses/main.json","main");
            DecoderBase64File("D:/Mine/MyTest/Base64ToAudio/Resourses/track.json", "track");
            Console.ReadLine();
        }

        private static void DecoderBase64File(string jsonPath,string audioSavePath)
        {
            StreamReader sr = new StreamReader(jsonPath);
            JsonTextReader jsr = new JsonTextReader(sr);
            JObject jObject = (JObject)JToken.ReadFrom(jsr);
            foreach (var json in jObject)
            {
                Console.WriteLine(json.Key);
                string name = json.Key;
                string base64Str = json.Value.ToString().Split(",")[1];
                byte[] buffer = Convert.FromBase64String(base64Str);
                FileStream fs = new FileStream("D:/Mine/MyTest/Base64ToAudio/Resourses/" + audioSavePath + "/" + name, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(buffer);
                fs.Close();
            }
        }
    }
}
