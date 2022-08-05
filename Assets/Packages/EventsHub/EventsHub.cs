using IntrovertStudios.Messaging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class EventLog
{
    // An Event without a generic type has an Action handler and no content
    public string id;
    public string eventType;
    private Action handler;
    public float time;

    public EventLog()
    {
        this.time = Time.time;
    }

    public EventLog(string id, string eventType, Action handler = null)
    {
        this.id = id;
        this.eventType = eventType;
        this.handler = handler;
        this.time = Time.time;
    }

    public EventLog(string id, string eventType)
    {
        this.id = id;
        this.eventType = eventType;
        this.time = Time.time;
    }

    public static string GetActionString(Action action)
    {
        var text = "";
        if (action != null)
        {
            text = action.Target.ToString() + " - " + action.Method.Name;
        }
        return text;
    }

    public EventLogFormatted Format()
    {
        return new EventLogFormatted(id, eventType, GetActionString(handler), "", time);
    }
}

public class EventLog<T> : EventLog
{
    // An Event with a generic type has an Action<T> handlerT and a T content
    private T content;
    public Action<T> handlerT;

    public EventLog(string id, string eventType, Action<T> handler = null)
    {
        this.id = id;
        this.eventType = eventType;
        this.handlerT = handler;
        this.time = Time.time;
    }

    public EventLog(string id, string eventType, T content)
    {
        this.id = id;
        this.eventType = eventType;
        this.content = content;
        this.time = Time.time;
    }

    public static string GetContentString(T content)
    {
        var text = "";
        if (content != null)
            text = content.ToString();
        return text;
    }

    public static string GetActionTString(Action<T> action)
    {
        var text = "";
        if (action != null)
        {
            text = action.Target.ToString() + " - " + typeof(T).ToString() + " - " + action.Method.Name;
        }
        return text;
    }

    public new EventLogFormatted Format()
    {
        return new EventLogFormatted(id, eventType, GetActionTString(handlerT), GetContentString(content), time);
    }
}

public class EventLogFormatted {
    public string id;
    public string eventType;
    public string handler;
    public string content;
    public float time;
    public StackFrame[] stack;

    public EventLogFormatted(string id = null, string eventType = null, string handler = null, string content = null, float time = 0)
    {
        this.id = id;
        this.eventType = eventType;
        this.handler = handler;
        this.content = content;
        this.time = time;

        stack = new StackTrace(true).GetFrames();
    }
}

public class EventsHub : MonoBehaviour
{
    //private static readonly MessageHub<EventType> hub;
    private static readonly MessageHub<string> hub;
    private static readonly List<Action<EventLogFormatted>> handlers = new List<Action<EventLogFormatted>>();

    static EventsHub()
    {
        //hub = new MessageHub<EventType>();
        hub = new MessageHub<string>();
    }

    public static void Connect(string id, Action handler)
    {
        LogEvent(new EventLog(id, "connect", handler));
        hub.Connect(id, handler);
    }

    public static void Connect<T>(string id, Action<T> handler) //where T : class
    {
        LogEvent<T>(new EventLog<T>(id, "connect", handler));
        hub.Connect<T>(id, handler);
    }

    public static void Disconnect(string id, Action handler)
    {
        LogEvent(new EventLog(id, "disconnect", handler));
        hub.Disconnect(id, handler);
    }

    public static void Disconnect<T>(string id, Action<T> handler) //where T : class
    {
        LogEvent<T>(new EventLog<T>(id, "disconnect", handler));
        hub.Disconnect<T>(id, handler);
    }

    public static void DisconnectAll()
    {
        LogEvent(new EventLog("", "disconnect", null));
        hub.DisconnectAll();
    }

    public static void Post(string id)
    {
        LogEvent(new EventLog(id, "post", null));
        hub.Post(id);
    }

    public static void Post<T>(string id, T content) /*where T : class*/
    {
        LogEvent<T>(new EventLog<T>(id, "post", content));
        hub.Post<T>(id, content);
    }

    public static MessageHub<string> GetHub()
    {
        return hub;
    }

    // logging code
    public delegate void handleEventDelegate(EventLogFormatted eventLog);
    public static handleEventDelegate handleEvent = (eventLog) => { };

    private static void LogEvent(EventLog eventLog)
    {
        handleEvent(eventLog.Format());
    }

    private static void LogEvent<T>(EventLog<T> eventLog)
    {
        handleEvent(eventLog.Format());
    }

    public static void AddHandler(Action<EventLogFormatted> handler)
    { // id eventType content handler
        handleEvent += handler.Invoke;
    }

    public static void RemoveHandler(Action<EventLogFormatted> handler)
    {
        handleEvent -= handler.Invoke;
    }
}
