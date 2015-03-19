using System;
using System.Collections.Generic;

namespace AdventureWorksCatalog.ViewModel.Messages
{
    public class Messenger
    {
        private static Messenger _Instance;
        public static Messenger Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (typeof(Messenger))
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Messenger();
                        }
                    }
                }
                return _Instance;
            }
        }

        private Dictionary<Type, List<object>> Subscriptions { get; set; }

        private Messenger()
        {
            Subscriptions = new Dictionary<Type, List<object>>();
        }

        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            var messageType = typeof(TMessage);
            if (!Subscriptions.ContainsKey(messageType))
                Subscriptions[messageType] = new List<object>();
            Subscriptions[messageType].Add(handler);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> handler)
        {
            var messageType = typeof(TMessage);
            if (Subscriptions.ContainsKey(messageType))
            {
                Subscriptions[messageType].Remove(handler);
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            if (Subscriptions.ContainsKey(messageType))
            {
                foreach (var subscriber in Subscriptions[messageType].ToArray())
                {
                    var action = (Action<TMessage>)subscriber;
                    action(message);
                }
            }
        }
    }
}
