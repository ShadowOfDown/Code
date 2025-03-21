//Author : _SourceCode
//CreateTime : 2025-03-03-12:48:02
//Version : 1.0
//UnityVersion : 2022.3.53f1c1


using MyFrame.Event;
public class EventManager
{
    private EventManager() { }

    public static void AddListener(string eventType, CallBack handler)
    {
        MessengerMgr.AddListener(eventType, handler);
    }

    public static void AddListener<T>(string eventType, CallBack<T> handler)
    {
        MessengerMgr<T>.AddListener(eventType, handler);
    }

    public static void AddListener<T, U>(string eventType, CallBack<T, U> handler)
    {
        MessengerMgr<T, U>.AddListener(eventType, handler);
    }

    public static void AddListener<T, U, V>(string eventType, CallBack<T, U, V> handler)
    {
        MessengerMgr<T, U, V>.AddListener(eventType, handler);
    }

    public static void RemoveListener(string eventType)  
    { 
        MessengerMgr.RemoveListener(eventType);
    }

    public static void RemoveListener(string eventType,CallBack handler)
    {
        MessengerMgr.RemoveListener(eventType,handler);
    }

    public static void RemoveListener<T>(string eventType, CallBack<T> handler)
    {
        MessengerMgr<T>.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<T, U>(string eventType, CallBack<T, U> handler)
    {
        MessengerMgr<T, U>.RemoveListener(eventType, handler);
    }

    public static void RemoveListener<T, U, V>(string eventType, CallBack<T, U, V> handler)
    {
        MessengerMgr<T, U, V>.RemoveListener(eventType, handler);
    }

    public static void BroadCast(string eventType)
    {
        MessengerMgr.Broadcast(eventType);
    }

    public static void BroadCast<T>(string eventType, T arg)
    {
        MessengerMgr<T>.Broadcast(eventType,arg);
    }

    public static void BroadCast<T, U>(string eventType, T arg1, U arg2)
    {
        MessengerMgr<T, U>.Broadcast(eventType, arg1, arg2);
    }

    public static void BroadCast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
    {
        MessengerMgr<T, U, V>.Broadcast(eventType, arg1, arg2, arg3);
    }
}
