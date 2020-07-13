using System;
using System.Collections.Generic;
using System.Text;

namespace Robot
{
    public struct RobotMsg
    {
        public bool isFullAward;
        public int dailyValue;
        public int limitValue;
        public int value;
        public int startTimeSeconds;
        public int endTimeSeconds;
        public int gameId;
        public float interval;
        public string username;
        public string password;

        public RobotMsg(bool isFullAward, float interval, int dailyValue, int limitValue, int value, int startTimeSeconds, int endTimeSeconds, int gameId, string username, string password)
        {
            this.isFullAward = isFullAward;
            this.interval = interval;
            this.dailyValue = dailyValue;
            this.limitValue = limitValue;
            this.value = value;
            this.startTimeSeconds = startTimeSeconds;
            this.endTimeSeconds = endTimeSeconds;
            this.gameId = gameId;
            this.username = username;
            this.password = password;
        }
    }

    public struct RobotMsgList
    {
        public int startTime;
        public int endTime;
        public string[] robotMsgFileNames;
        public RobotMsgList(string[] robotMsgFileNames, int sTime, int eTime)
        {
            this.robotMsgFileNames = new string[robotMsgFileNames.Length];
            for (int i = 0; i < robotMsgFileNames.Length; i++)
                this.robotMsgFileNames[i] = robotMsgFileNames[i];
            startTime = sTime;
            endTime = eTime;
        }
    }
}
