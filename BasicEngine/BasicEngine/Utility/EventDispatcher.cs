using System.Collections.Generic;

namespace BasicEngine.Utility
{
    public delegate void EventDispatcherDelegate();

    public class EventDispatcher
    {
        private static Dictionary<string, List<EventDispatcherDelegate>> table = new Dictionary<string, List<EventDispatcherDelegate>>();

        public static void AddListener(string name, EventDispatcherDelegate callback)
        {
            lock(table)
            {
                List<EventDispatcherDelegate> eventListeners = null;
                if(table.TryGetValue(name, out eventListeners)) //there exists a List of Listeners for this name
                {
                    eventListeners.Remove(callback);
                    eventListeners.Add(callback);   // avoid dublicate listeners
                }
                else
                {
                    eventListeners = new List<EventDispatcherDelegate>();   //no listeners for this, so create new list
                    eventListeners.Add(callback);

                    table.Add(name, eventListeners);
                }
            }
        }

        public static void RemoveListener(string name, EventDispatcherDelegate callback)
        {
            lock (table)
            {
                List<EventDispatcherDelegate> eventListeners = null;
                if (table.TryGetValue(name, out eventListeners))
                {
                    for(int i=0; i<eventListeners.Count; i++)
                    {
                        eventListeners.Remove(callback);
                    }
                }
            }
        }

        public static void Invoke(string name)
        {
            lock (table)
            {
                List<EventDispatcherDelegate> eventListeners = null;
                if (table.TryGetValue(name, out eventListeners))
                {
                    for (int i = 0; i < eventListeners.Count; i++)
                    {
                        eventListeners[i]();
                    }
                }
            }
        }
    }
}
