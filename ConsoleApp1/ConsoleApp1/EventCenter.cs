using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public enum EventType
    {
        TEST,
        TEST1,
        TEST2,
        TEST3,
        TEST4,
        TEST5,
    }
    class EventCenter
    {
        public delegate void Action0();
        public delegate void Action1<T1>(T1 arg);
        public delegate void Action2<T1,T2>(T1 arg1, T2 arg2);
        public delegate void Action3<T1,T2,T3>(T1 arg1, T2 arg2, T3 arg3);
        public delegate void Action4<T1,T2,T3,T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        public delegate void Action5<T1,T2,T3,T4,T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        private class CallBack
        {
        }
        private class CallBack0 : CallBack
        {
            private Action0 action;
            public CallBack0(Action0 action)
            {
                this.action = action;
            }
            public void Call()
            {
                action.Invoke();
            }
        }
        private class CallBack1<T1> : CallBack
        {
            private Action1<T1> action;
            public CallBack1(Action1<T1> action)
            {
                this.action = action;
            }
            public void Call(T1 arg1)
            {
                action.Invoke(arg1);
            }
        }
        private class CallBack2<T1,T2> : CallBack
        {
            private Action2<T1, T2> action;
            public CallBack2(Action2<T1,T2> action)
            {
                this.action = action;
            }
            public void Call(T1 arg1, T2 arg2)
            {
                action.Invoke(arg1, arg2);
            }
        }
        private class CallBack3<T1,T2,T3> : CallBack
        {
            private Action3<T1, T2, T3> action;
            public CallBack3(Action3<T1, T2, T3> action)
            {
                this.action = action;
            }
            public void Call(T1 arg1, T2 arg2, T3 arg3)
            {
                action.Invoke(arg1, arg2, arg3);
            }
        }
        private class CallBack4<T1,T2,T3,T4> : CallBack
        {
            private Action4<T1, T2, T3, T4> action;
            public CallBack4(Action4<T1, T2, T3, T4> action)
            {
                this.action = action;
            }
            public void Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
        }
        private class CallBack5<T1,T2,T3,T4,T5> : CallBack
        {
            private Action5<T1, T2, T3, T4, T5> action;
            public CallBack5(Action5<T1, T2, T3, T4, T5> action)
            {
                this.action = action;
            }
            public void Call(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            {
                action.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
        }

        private static Dictionary<EventType, CallBack> events = new Dictionary<EventType, CallBack>();
        private static Dictionary<EventType, CallBack> events1 = new Dictionary<EventType, CallBack>();
        private static Dictionary<EventType, CallBack> events2 = new Dictionary<EventType, CallBack>();
        private static Dictionary<EventType, CallBack> events3 = new Dictionary<EventType, CallBack>();
        private static Dictionary<EventType, CallBack> events4 = new Dictionary<EventType, CallBack>();
        private static Dictionary<EventType, CallBack> events5 = new Dictionary<EventType, CallBack>();

        public static void RegistEvent(EventType eventType, Action0 action)
        {
            if (events.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack0 callBack = new CallBack0(action);
                events.Add(eventType, callBack);
            }
        }
        public static void RegistEvent<T1>(EventType eventType, Action1<T1> action)
        {
            if (events1.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack1<T1> callBack = new CallBack1<T1>(action);
                events1.Add(eventType, callBack);
            }
        }
        public static void RegistEvent<T1, T2>(EventType eventType, Action2<T1,T2> action)
        {
            if (events2.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack2<T1, T2> callBack = new CallBack2<T1, T2>(action);
                events2.Add(eventType, callBack);
            }
        }
        public static void RegistEvent<T1, T2, T3>(EventType eventType, Action3<T1, T2, T3> action)
        {
            if (events3.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack3<T1, T2, T3> callBack = new CallBack3<T1, T2, T3>(action);
                events3.Add(eventType, callBack);
            }
        }
        public static void RegistEvent<T1, T2, T3, T4>(EventType eventType, Action4<T1, T2, T3, T4> action)
        {
            if (events4.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack4<T1, T2, T3, T4> callBack = new CallBack4<T1, T2, T3, T4>(action);
                events4.Add(eventType, callBack);
            }
        }
        public static void RegistEvent<T1, T2, T3, T4, T5>(EventType eventType, Action5<T1, T2, T3, T4, T5> action)
        {
            if (events5.ContainsKey(eventType))
            {
                Console.WriteLine("events has registed");
                return;
            }
            else
            {
                CallBack5<T1, T2, T3, T4, T5> callBack = new CallBack5<T1, T2, T3, T4, T5>(action);
                events5.Add(eventType, callBack);
            }
        }

        public static void InvokeEvent(EventType eventType)
        {
            if (!events.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events[eventType] as CallBack0).Call();
            }
        }
        public static void InvokeEvent<T1>(EventType eventType, T1 arg1)
        {
            if (!events1.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events1[eventType] as CallBack1<T1>).Call(arg1);
            }
        }
        public static void InvokeEvent<T1, T2>(EventType eventType, T1 arg1, T2 arg2)
        {
            if (!events2.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events2[eventType] as CallBack2<T1, T2>).Call(arg1, arg2);
            }
        }
        public static void InvokeEvent<T1, T2, T3>(EventType eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (!events3.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events3[eventType] as CallBack3<T1, T2, T3>).Call(arg1, arg2, arg3);
            }
        }
        public static void InvokeEvent<T1, T2, T3, T4>(EventType eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!events4.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events4[eventType] as CallBack4<T1, T2, T3, T4>).Call(arg1, arg2, arg3, arg4);
            }
        }
        public static void InvokeEvent<T1, T2, T3, T4, T5>(EventType eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (!events5.ContainsKey(eventType))
            {
                Console.WriteLine("no events");
                return;
            }
            else
            {
                (events5[eventType] as CallBack5<T1, T2, T3, T4, T5>).Call(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }
}
