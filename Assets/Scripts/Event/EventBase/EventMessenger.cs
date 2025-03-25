//Author : _SourceCode
//CreateTime : 2025-03-03-19:20:52
//Version : 1.0
//UnityVersion : 2022.3.53f1c1



//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
#define REQUIRE_LISTENER
#define LOG_EVENT_CREATED_OR_DESTROY_MESSAGES

using System;
using System.Collections.Generic;
using UnityEngine;



namespace MyFrame.Event{
    public class EventMessenger
    {
        #region static parameters
        public static Dictionary<string,Delegate> eventTable = new Dictionary<string,Delegate>();
        public static List<string> permanentMessages = new List<string>();
        #endregion

        #region Helper Methods
        public static void MarkAsPermanent(string eventType) {
#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
            permanentMessages.Add(eventType);
        }

        public static void CleanUP()
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif
            List<string> messageToMove = new List<string>();
            foreach(KeyValuePair<string,Delegate> pair in eventTable)
            {
                bool wasFound = false;
                foreach(string permanent in permanentMessages)
                {
                    if(pair.Key == permanent)
                    {
                        wasFound = true;
                        break;
                    }
                }
                if (!wasFound)
                {
                    messageToMove.Add(pair.Key);
                }
            }

            foreach (string message in messageToMove) { 
                eventTable.Remove(message);
            }
        }

        public static void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<string, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }
        #endregion

        #region Message logging and exception throwing
        public static void OnListenerAdding(string eventType,Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
#if LOG_EVENT_CREATED_OR_DESTROY_MESSAGES
                Debug.Log(string.Format("new Event {0} has been created", eventType));
#endif
            }
            Delegate d = eventTable[eventType];
            if (d != null&&d.GetType()!= listenerBeingAdded.GetType())
            {
                throw new ListenerException(
                    string.Format("Attempting to add listener with inconsistent signature for event type {0}. " +
                    "Current listeners have type {1} and listener being added has type {2}"
                    , eventType, d.GetType().Name, listenerBeingAdded.GetType().Name)
                    );
            }
#if LOG_EVENT_CREATED_OR_DESTROY_MESSAGES
            Debug.Log(string.Format("Event {0} adds listener : {1}", eventType,listenerBeingAdded.ToString()));
#endif
        }


        public static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) 
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif
            if (!eventTable.ContainsKey(eventType)) 
            {
                throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
            Delegate d = eventTable[eventType];
            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(
                    string.Format("Attempting to remove listener with inconsistent signature for event type {0}." +
                    " Current listeners have type {1} and listener being removed has type {2}"
                    , eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name)
                    );
            }
        }
        public static void OnListenerRemoving(string eventType)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif
            if (!eventTable.ContainsKey(eventType))
            {
                throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }
        public static void OnListenerRemoved(string eventType) 
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
#if LOG_EVENT_CREATED_OR_DESTROY_MESSAGES
                Debug.Log(string.Format("new Event {0} has been destroyed", eventType));
#endif
            }
        }

        public static void OnBroadcasting(string eventType)
        {

            if (!eventTable.ContainsKey(eventType))
            {
#if REQUIRE_LISTENER
                throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
#else
                return;
#endif
            }
#if LOG_EVENT_CREATED_OR_DESTROY_MESSAGES
            Debug.Log(string.Format("Event {0} has been Invoked", eventType));
#endif
        }
        public static BroadcastException CreateBroadcastSignatureException(string eventType)
        {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }
#endregion

    }
    #region No parameter
    public class MessengerMgr : EventMessenger
    {
        public static void AddListener(string eventType, CallBack handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CallBack)eventTable[eventType] + handler;
        }

        public static void RemoveListener(string eventType, CallBack handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (CallBack)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener(string eventType)
        {
            OnListenerRemoving(eventType);
            eventTable.Remove(eventType);
        }

        public static void Broadcast(string eventType)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    #endregion

    #region One parameter
    public class MessengerMgr<T> : EventMessenger
    {
        public static void AddListener(string eventType, CallBack<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CallBack<T>)eventTable[eventType] + handler;
        }

        public static void RemoveListener(string eventType, CallBack<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (CallBack<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void RemoveListener(string eventType)
        {
            OnListenerRemoving(eventType);
            eventTable.Remove(eventType);
        }

        public static void Broadcast(string eventType, T arg1)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg1);
                }
            }
            else
            {
                throw CreateBroadcastSignatureException(eventType);
            }
        }
    }
    #endregion

    #region Two parameters
    public class MessengerMgr<T, U> : EventMessenger
    {
        public static void AddListener(string eventType, CallBack<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] + handler;
        }

        public static void RemoveListener(string eventType, CallBack<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (CallBack<T, U>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void Broadcast(string eventType, T arg1, U arg2)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, U> callback = d as CallBack<T, U>;

                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
    }
    #endregion

    #region Three parameters
    public class MessengerMgr<T, U, V> : EventMessenger
    {
        public static void AddListener(string eventType, CallBack<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] + handler;
        }

        public static void RemoveListener(string eventType, CallBack<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (CallBack<T, U, V>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public static void Broadcast(string eventType, T arg1, U arg2, V arg3)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, U, V> callback = d as CallBack<T, U, V>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
    }
    #endregion
}
