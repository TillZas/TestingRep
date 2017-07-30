using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataBasesAndModels.Models
{
    public class ChatModel
    {
        public List<ChatUser> Users;
        private List<ChatMessage> Messages;

        private int cacheLimit = 0;
        private int userMaxAmount_ = 0;

        public ChatModel(int messageCacheLimit,int userMaxAmount)
        {
            Users = new List<ChatUser>();
            Messages = new List<ChatMessage>();

            Messages.Add(new ChatMessage()
            {
                Text = "Чат стартовал в " + DateTime.Now,
            });

            userMaxAmount_ = userMaxAmount;
        }


        public void AddMessage(string Message,ChatUser User)
        {
            Messages.Add(new ChatMessage()
            {
                User = User,
                Text = Message,
                Date = DateTime.Now
            });
            Messages.Sort(new MessagesComparer());
            if (cacheLimit > 0 && Messages.Count > cacheLimit) Messages.RemoveRange(0, Messages.Count - cacheLimit);
        }

        public void AddMessage(string Message, ChatUser User,DateTime TimeStamp)
        {
            Messages.Add(new ChatMessage()
            {
                User = User,
                Text = Message,
                Date = TimeStamp
            });
            
            Messages.Sort(new MessagesComparer());
            if (cacheLimit > 0 && Messages.Count > cacheLimit) Messages.RemoveRange(0, Messages.Count - cacheLimit);
        }

        public List<ChatMessage> GetMessages()
        {
            return Messages.FindAll(x => x.Date.ToFileTime()<= DateTime.Now.ToFileTime());
        }

        /*
        public bool IsUserConnected(string userName)
        {
            return Users.FirstOrDefault(user => user.Name == userName) != null;
        }

        public bool IsHavePlaceToAnother()
        {
            return (userMaxAmount_ <= 0) || (Users.Count < userMaxAmount_);
        }*/

        public static int NULL_POINTER_RESULT = -3, ALREADY_CONNECTED_RESULT = -1, NO_MORE_ROOM_RESULT = -2;

        //Спроси о том как писать описание функциям
        public int AddUser(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return -3;
            if (Users.FirstOrDefault(user => user.Name == userName) != null) return -1;
            if (!((userMaxAmount_ <= 0) || (Users.Count < userMaxAmount_))) return -2;
            Users.Add(new ChatUser()
            {
                Name = userName,
                LoginTime = DateTime.Now,
                LastPing = DateTime.Now
            });
            AddMessage(userName + " присоеденился к чату.",null);
            return 0;
        }

        public void RemoveDelayedUsers(int seconds)
        {
            List<ChatUser> toRemove = Users.FindAll(usr => ((TimeSpan)(DateTime.Now - usr.LastPing)).TotalSeconds > seconds);
            foreach(ChatUser chu in toRemove)
            {
                Users.Remove(chu);
                AddMessage(chu.Name + " потерял соединение.", null);
            }
        }

        public void LogOff(string userName)
        {
            if(Users.RemoveAll(usr => usr.Name == userName)>0)
                AddMessage(userName + " вышел.",null);
        }

        public ChatUser TouchUser(string userName)
        {
            ChatUser ch = Users.FirstOrDefault(usr => usr.Name == userName);
            if (ch == null) return null;
            ch.LastPing = DateTime.Now;
            return ch;
        }

        private class MessagesComparer : IComparer<ChatMessage>
        {
            public int Compare(ChatMessage x, ChatMessage y)
            {
                
                return x.Date.CompareTo(y.Date);

            }
        }
    }

    public class ChatUser
    {
        public string Name;
        public DateTime LoginTime;
        public DateTime LastPing;
    }

    public class ChatMessage
    {
        public ChatUser User;
        public DateTime Date = DateTime.Now;
        public string Text = "";
    }
}