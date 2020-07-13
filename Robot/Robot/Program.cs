using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Robot
{
    class Program
    {
        public static Dictionary<string, string> gameUrls = new Dictionary<string, string>();
        public static string[] gameNames = new string[]
        {
            "crush", "crush", "billiards", "mythAnimals", "basketball", "marbles", "fiveBless", "mythAnimals2"
        };

        static bool IsOwnLocalFiles(params string[] files)
        {
            int fileCouts = 0;
            for (int i = 0; i < files.Length; i++)
            {
                if (!File.Exists(files[i]))
                    Console.WriteLine("没有" + files[i] + "文件");
                else
                    fileCouts++;
            }
            if (fileCouts == files.Length)
                return true;
            else
                return false;
        }

        static void Main(string[] args)
        {
            string dirName = Directory.GetCurrentDirectory() + "\\localRobotsDir";
            if (!Directory.Exists(dirName))
            {
                Console.WriteLine("没有localRobotsDir文件夹");
                Console.Read();
                return;
            }
            else
            {
                if (!IsOwnLocalFiles(dirName + "\\RobotListMsg.txt", dirName + "\\TimeSetting.txt", dirName + "\\url.txt"))
                {
                    Console.Read();
                    return;
                }
            }
            //获取本地数据
            string robotMsgListStr = Tools.GetFileString(dirName, "RobotListMsg.txt");
            JObject gameUrlsJObject = JsonConvert.DeserializeObject(Tools.GetFileString(dirName, "url.txt")) as JObject;
            foreach (var keyPair in gameUrlsJObject)
            {
                gameUrls[keyPair.Key] = keyPair.Value.ToString();
            }
            int interval = (int)(float.Parse(Tools.GetFileString(dirName, "TimeSetting.txt")) * 1000);
            List<RobotMsg> robotMsgs = new List<RobotMsg>();
            RobotMsgList[] robotMsgLists;
            int curRobotMsgListIndex = 0;
            string[] strs = robotMsgListStr.Split("\n");
            robotMsgLists = new RobotMsgList[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                JObject jObject = JsonConvert.DeserializeObject(strs[i]) as JObject;
                string str = jObject["names"].ToString();
                string[] fileNames = jObject["names"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace(" ", "").Replace("\"", "").Split(",");
                int sHours = int.Parse(jObject["startTimeHours"].ToString());
                int sMinutes = int.Parse(jObject["startTimeMinutes"].ToString());
                int eHours = int.Parse(jObject["endTimeHours"].ToString());
                int eMinutes = int.Parse(jObject["endTimeMinutes"].ToString());
                robotMsgLists[i] = new RobotMsgList(fileNames, (sHours * 60 + sMinutes) * 60, (eHours * 60 + eMinutes) * 60);
            }
            //添加机器人及机器人运行
            int curRobotIndex = 0;
            List<HttpConnection> robotList = new List<HttpConnection>();
            while (true)
            {
                float curTime = Tools.GetToDaySeconds();
                if (robotList.Count == 0)
                {
                    if (curRobotMsgListIndex == robotMsgLists.Length)
                    {
                        if (curTime == 0)
                            curRobotMsgListIndex = 0;
                        continue;
                    }
                    if (curTime >= robotMsgLists[curRobotMsgListIndex].startTime)
                    {
                        if (curTime >= robotMsgLists[curRobotMsgListIndex].endTime)
                        {
                            curRobotMsgListIndex++;
                        }
                        else
                        {
                            int fileIndex = Tools.GetRate(0, robotMsgLists[curRobotMsgListIndex].robotMsgFileNames.Length);
                            string str = Tools.GetFileString(dirName, robotMsgLists[curRobotMsgListIndex].robotMsgFileNames[fileIndex]);
                            GetRobotMsgs(robotMsgs, str);
                            for (int i = 0; i < robotMsgs.Count; i++)
                                AddRobot(robotMsgs[i], robotList);
                        }
                    }

                }
                else
                {
                    if (curTime < robotMsgLists[curRobotMsgListIndex].endTime)
                    {
                        Thread.Sleep(interval);
                        if (robotList[curRobotIndex].Msg.gameId == 7)
                            robotList[curRobotIndex].OtherUserBetting();
                        else
                            robotList[curRobotIndex].PlayGame();
                        curRobotIndex = (++curRobotIndex == robotList.Count) ? 0 : curRobotIndex;
                    }
                    else
                    {
                        DeleteAllRobot(robotList);
                        curRobotMsgListIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// 添加机器人及机器人登陆
        /// </summary>
        /// <param name="robotMsg"></param>
        /// <param name="robotList"></param>
        static void AddRobot(RobotMsg robotMsg,List<HttpConnection> robotList)
        {
            HttpConnection httpConnection = new HttpConnection(robotMsg);
            httpConnection.Login();
            robotList.Add(httpConnection);
        }

        /// <summary>
        /// 删除当前所有机器人
        /// </summary>
        /// <param name="robotList"></param>
        static void DeleteAllRobot(List<HttpConnection> robotList)
        {
            robotList.Clear();
        }

        /// <summary>
        /// 获取机器人信息
        /// </summary>
        /// <param name="robotMsgs"></param>
        /// <param name="str"></param>
        static void GetRobotMsgs(List<RobotMsg> robotMsgs,string str)
        {
            robotMsgs.Clear();
            string[] strs = str.Split("\n");
            for (int i = 0; i < strs.Length; i++)
            {
                JObject jObject = JsonConvert.DeserializeObject(strs[i]) as JObject;
                RobotMsg robotMsg = new RobotMsg();
                robotMsg.isFullAward = jObject["isFullAward"].ToString().Equals("true");
                robotMsg.username = jObject["user"].ToString();
                robotMsg.password = jObject["pwd"].ToString();
                robotMsg.dailyValue = int.Parse(jObject["dailyValue"].ToString());
                robotMsg.limitValue = int.Parse(jObject["limitValue"].ToString());
                robotMsg.value = int.Parse(jObject["value"].ToString());
                string[] startTime = jObject["startTime"].ToString().Split(":");
                robotMsg.startTimeSeconds = (int.Parse(startTime[0]) * 60 + int.Parse(startTime[1])) * 60;
                string[] endTime = jObject["endTime"].ToString().Split(":");
                robotMsg.endTimeSeconds = (int.Parse(endTime[0]) * 60 + int.Parse(endTime[1])) * 60;
                robotMsg.interval = float.Parse(jObject["interval"].ToString());
                string test = jObject["games"].ToString();
                string[] gameStrs = jObject["games"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace(" ", "").Split(",");
                List<int> curGameId = new List<int>();
                for (int j = 0; j < gameStrs.Length; j++)
                {
                    if (gameStrs[j].Equals("1"))
                    {
                        curGameId.Add(j);
                    }
                }
                robotMsg.gameId = curGameId[Tools.GetRate(0, curGameId.Count)] + 1;
                robotMsgs.Add(robotMsg);
            }
        }
    }
}
