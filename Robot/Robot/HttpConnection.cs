using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Robot
{
    class HttpConnection
    {
        private bool isResult = false;
        private bool isPrepare = false;
        private bool isLogined = false;
        private string curGameUrl;
        private HttpClient client = new HttpClient();
        private RobotMsg robotMsg;
        public RobotMsg Msg { get { return robotMsg; } }

        public HttpConnection(RobotMsg robotMsg)
        {
            this.robotMsg = robotMsg;
            client.DefaultRequestHeaders.Host = Program.gameUrls["loginHost"];
            client.DefaultRequestHeaders.Add("Origin", Program.gameUrls["header"]);
            client.DefaultRequestHeaders.Add("App-Channel", "100001");
        }

        /// <summary>
        /// 登陆
        /// </summary>
        public void Login()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("username", robotMsg.username);
            content.Add("password", robotMsg.password);
            content.Add("type", "account");
            HttpContent postContent = new FormUrlEncodedContent(content);
            client.PostAsync(Program.gameUrls["packGameUrl"], postContent).ContinueWith(
            (postTask) => {
                var result = postTask.Result;
                result.Content.ReadAsStringAsync().ContinueWith(
                (readTask) =>
                {
                    Debug.Log(readTask.Result,robotMsg.username + ".txt");
                    JObject jObject = JsonConvert.DeserializeObject(readTask.Result) as JObject;
                    GetToken(jObject);
                });
            });
        }
        
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="loginData"></param>
        private void GetToken(JObject loginData)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            content.Add("code", loginData["data"].ToString());
            HttpContent postContent = new FormUrlEncodedContent(content);
            client.PostAsync(Program.gameUrls["accessTokeUrl"], postContent).ContinueWith(
            (postTask) => {
                var result = postTask.Result;
                result.Content.ReadAsStringAsync().ContinueWith(
                (readTask) =>
                {
                    Debug.Log(readTask.Result, robotMsg.username + ".txt");
                    JObject jObject = JsonConvert.DeserializeObject(readTask.Result) as JObject;
                    JObject tokenData = JsonConvert.DeserializeObject(jObject["data"].ToString()) as JObject;
                    client.DefaultRequestHeaders.Host = Program.gameUrls["gameHost"];
                    client.DefaultRequestHeaders.Add("Authorization", tokenData["access_token"].ToString());
                    isLogined = true;
                    ChooseGame();
                });
            });
        }

        /// <summary>
        /// 网络数据发送和接收
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <param name="sucCall"></param>
        /// <param name="sucCode"></param>
        /// <param name="sucNum"></param>
        /// <param name="data"></param>
        private async void HttpPostAsync(string url, string host, Action<JObject> sucCall, string sucCode, int sucNum, JObject data = null)
        {
            client.DefaultRequestHeaders.Host = host;
            HttpContent postContent = JObjectToHttpContent(data);
            var result = await client.PostAsync(url, postContent);
            var read = await result.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject(read) as JObject;
            if ((int)jObject[sucCode] == sucNum)
                sucCall?.Invoke(jObject);
            else
                Debug.Log("url:" + url + "\nhost:" + host  + "\nsend:" + data.ToString() + "\nread:" + read, robotMsg.username + ".txt");
        }

        /// <summary>
        /// 选择加入的游戏
        /// </summary>
        private void ChooseGame(Action<JObject> callBack = null)
        {
            JObject data = new JObject();
            data["msgId"] = "login";
            curGameUrl = Program.gameUrls["gameUrl"] + Program.gameNames[robotMsg.gameId];
            HttpPostAsync(curGameUrl + "/login", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                if (robotMsg.gameId != 7)
                    GetSupportRank();

            }, "code", 200, data);
        }

        private int curDailyValue = 0;
        /// <summary>
        /// 获取流水数据
        /// </summary>
        /// <param name="callBack"></param>
        private void GetSupportRank(Action<JObject> callBack = null)
        {
            if (!isLogined)
                return;
            curPlayCounts = 0;
            JObject data = new JObject();
            data["msgId"] = "getSupportRank";
            HttpPostAsync(curGameUrl + "/getSupportRank", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                curDailyValue = (int)readData["myRankData"]["amount"];
                if (curDailyValue >= robotMsg.dailyValue)
                {
                    Console.WriteLine(robotMsg.username + "->流水达标");
                    isFinished = true;
                }
                else
                    GetFeed();
            }, "code", 200, data);
        }

        private int curDailyFeed = 0;
        /// <summary>
        /// 获取盈利数据
        /// </summary>
        private void GetFeed(Action<JObject> callBack = null)
        {
            if (!isLogined)
                return;
            JObject data = new JObject();
            data["page"] = 1;
            data["pageSize"] = 100;
            HttpPostAsync(Program.gameUrls["profigRankUrl"], Program.gameUrls["platHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                curDailyFeed = (int)readData["data"]["me"]["score"];
                if (curDailyFeed >= robotMsg.limitValue)
                {
                    Console.WriteLine(robotMsg.username + "->盈利达标");
                    isFinished = true;
                }
                else
                    isFinished = false;
            }, "errcode", 200, data);
        }
        
        private int curPlayCounts = 0;
        private int checkPlayCounts = 5;
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="callBack"></param>
        private void StartGame(Action<JObject> callBack = null)
        {
            if (!isLogined || Tools.GetToDaySeconds() < robotMsg.startTimeSeconds || Tools.GetToDaySeconds() > robotMsg.endTimeSeconds)
                return;
            JObject data = new JObject();
            int cellScore = GetCellScore();
            data["cellScore"] = cellScore;
            data["msgId"] = "startGame";
            HttpPostAsync(curGameUrl + "/startGame", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                curDailyValue += cellScore;
                curDailyFeed += (int)readData["winScores"] - cellScore;
                curPlayCounts++;
                Console.WriteLine(robotMsg.username + 
                                  "->流水上限:" + robotMsg.dailyValue + "|当前流水:" + 
                                  curDailyValue + "|盈利上限:" + robotMsg.limitValue + "|当前盈利:" + curDailyFeed + 
                                  "|当前游戏:" + Program.gameNames[robotMsg.gameId]);
            }, "code", 200, data);
        }

        private bool isFinished = true;
        private float lastPlayTime = 0;
        public void PlayGame()
        {
            if (isFinished || Math.Abs(Tools.GetToDayTime() - lastPlayTime) < robotMsg.interval)
            {
                return;
            }
            lastPlayTime = Tools.GetToDayTime();
            if (curPlayCounts >= checkPlayCounts)
                GetSupportRank();
            else
                StartGame();
        }

        /// <summary>
        /// 获取当前流水值
        /// </summary>
        /// <returns></returns>
        private int GetCellScore()
        {
            int residueValue = (robotMsg.dailyValue - curDailyValue) % robotMsg.value;
            if (residueValue >= 50000)
            {
                return 50000;
            }
            else if (residueValue >= 10000)
            {
                return 10000;
            }
            else if (residueValue >= 5000)
            {
                return 5000;
            }
            else if (residueValue >= 1000)
            {
                return 1000;
            }
            else if (residueValue >= 500)
            {
                return 500;
            }
            else if (residueValue > 0)
            {
                return 100;
            }
            else
            {
                return robotMsg.value;
            }
        }

        /// <summary>
        /// 灵宠在线
        /// </summary>
        /// <param name="callBack"></param>
        public void OtherUserBetting(Action<JObject> callBack = null)
        {
            if (!isLogined || Tools.GetToDaySeconds() < robotMsg.startTimeSeconds || Tools.GetToDaySeconds() > robotMsg.endTimeSeconds)
                return;
            JObject data = new JObject();
            data["msgId"] = "otherUsersBetting";
            HttpPostAsync(curGameUrl + "/otherUsersBetting", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                Interact();
                if (readData.ContainsKey("amount"))
                    OnSeat(readData["seatArr"], (int)readData["amount"]);
                if ((int)readData["status"] == 1)
                {
                    isResult = false;
                    isPrepare = false;
                    Betting();
                }
                else
                {
                    if (!isResult && (int)readData["status"] == 2)
                    {
                        Console.WriteLine(robotMsg.username + "->结算中");
                        isResult = true;
                        onSeatBetType.Clear();
                        onSeatBetTypeLength = Tools.GetRate(1, 3);
                    }
                    if(!isPrepare && (int)readData["status"] == 3)
                    {
                        isPrepare = true;
                        Console.WriteLine(robotMsg.username + "->准备阶段");
                    }
                }
            }, "code", 200, data);
        }

        private List<int> onSeatBetType = new List<int>();
        private int onSeatBetTypeLength = Tools.GetRate(1,3);
        /// <summary>
        /// 灵宠下注
        /// </summary>
        /// <param name="callBack"></param>
        private void Betting(Action<JObject> callBack = null)
        {
            JObject data = new JObject();
            int betType = GetBetType();
            if (isOnSeat)
            {
                if (onSeatBetType.Count < onSeatBetTypeLength)
                    onSeatBetType.Add(betType);
                else
                    betType = onSeatBetType[Tools.GetRate(0, onSeatBetTypeLength)];
            }
            data["type"] = betType;
            data["amount"] = GetBetValue();
            data["msgId"] = "betting";
            HttpPostAsync(curGameUrl + "/betting", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
                Console.WriteLine(robotMsg.username + "->下注->" + "下注类型:" + GetBetName((int)data["type"]) + "|下注金额:" + data["amount"]);
            }, "code", 200, data);
        }

        private int lastInteract = 0;
        /// <summary>
        /// 灵宠发表情
        /// </summary>
        /// <param name="callBack"></param>
        private void Interact(Action<JObject> callBack = null)
        {
            if (!isLogined || Math.Abs(Tools.GetToDaySeconds() - lastInteract) <= 3 || Tools.GetRate() > 2)
                return;
            JObject data = new JObject();
            data["no"] = 0;
            data["id"] = Tools.GetRate() >= 50 ? Tools.GetRate(1,23) : Tools.GetRate(50,69);
            data["type"] = 2;
            data["msgId"] = "interact";
            lastInteract = Tools.GetToDaySeconds();
            HttpPostAsync(curGameUrl + "/interact", Program.gameUrls["gameHost"], (JObject readData) =>
            {
                callBack?.Invoke(readData);
            }, "code", 200, data);
        }

        private bool isOnSeat = false;
        /// <summary>
        /// 上座
        /// </summary>
        /// <param name="seatMsg"></param>
        private void OnSeat(JToken seatMsg, int selfAmount)
        {
            if (isOnSeat || selfAmount > 4000000 || selfAmount < 100000)
            {
                if (Tools.GetToDaySeconds() == 0 && isOnSeat)
                    isOnSeat = false;
                return;
            }
            int lastSeatNo = 0;
            foreach (var seat in seatMsg)
            {
                int curSeatNo = (int)seat["seatNo"];
                if (curSeatNo != lastSeatNo + 1)
                {
                    JObject data = new JObject();
                    data["no"] = curSeatNo;
                    data["msgId"] = "seat";
                    HttpPostAsync(curGameUrl + "/seat", Program.gameUrls["gameHost"], (JObject readData) =>
                    {
                        isOnSeat = true;
                    }, "code", 200, data);
                }
                else
                {
                    lastSeatNo++;
                }
            }
            if (lastSeatNo + 1 <= 6)
            {
                JObject data = new JObject();
                data["no"] = lastSeatNo + 1;
                data["msgId"] = "seat";
                HttpPostAsync(curGameUrl + "/seat", Program.gameUrls["gameHost"], (JObject readData) =>
                {
                    isOnSeat = true;
                }, "code", 200, data);
            }
        }

        /// <summary>
        /// 灵宠下注类型
        /// </summary>
        /// <returns></returns>
        private int GetBetType()
        {
            int rate = Tools.GetRate();
            if (rate <= 5)
                return 1;
            else if (rate <= 25)
                return 2;
            else if (rate <= 45)
                return 3;
            else if (rate <= 50)
                return 4;
            else if (rate <= 55)
                return 5;
            else if (rate <= 75)
                return 6;
            else if (rate <= 95)
                return 7;
            else
                return 8;
        }

        /// <summary>
        /// 根据下注类型获取下注的名字
        /// </summary>
        /// <param name="betType"></param>
        /// <returns></returns>
        private string GetBetName(int betType)
        {
            switch (betType)
            {
                case 1:
                    return "青龙";
                case 2:
                    return "小乌龟";
                case 3:
                    return "小凤凰";
                case 4:
                    return "白虎";
                case 5:
                    return "朱雀";
                case 6:
                    return "小老虎";
                case 7:
                    return "小白龙";
                case 8:
                    return "白虎";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 灵宠下注值
        /// </summary>
        /// <returns></returns>
        private int GetBetValue()
        {
            int rate = Tools.GetRate();
            if (rate <= 20)
                return 100;
            else if (rate <= 40)
                return 1000;
            else if (rate <= 70)
                return 5000;
            else if (rate <= 90)
                return 10000;
            else
                return 50000;
        }

        /// <summary>
        /// JObject类型转HttpContent类型
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        private HttpContent JObjectToHttpContent(JObject jObject)
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            foreach (var keyPair in jObject)
            {
                content.Add(keyPair.Key, keyPair.Value.ToString());
            }
            return new FormUrlEncodedContent(content);
        }
    }
}
