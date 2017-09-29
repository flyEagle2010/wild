﻿/// <summary>
/// 网络消息处理中心
/// 
/// create at 2014.8.26 by sun
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Util;

public struct NetMsgEvt:type,data
{
    public int type;
    public byte[] data;
}

public struct CustomEvent
{
    public string type;
    public object data;
}

public delegate void EventHandle(byte[] data);

public class MessageCenter : SingletonMonoBehaviour<MessageCenter>
{
    private Dictionary<int, EventHandle> evtHandleDic = new Dictionary<int, EventHandle>();
    public Queue<NetMsgEvt> RecvMessageQueue = new Queue<NetMsgEvt>();
    
    //添加普通事件观察者
	public void AddEventListener(int eventType, EventHandle handle)
    {
        if (evtHandleDic.ContainsKey(eventType))
        {
            evtHandleDic[eventType] += handle;
        }
        else
        {
            evtHandleDic.Add(eventType, handle);
        }
    }

    //删除普通事件观察者
	public void RemoveEventListener(int eventType, GameLogicHandle callback)
    {
        if (evtHandleDic.ContainsKey(eventType))
        {
            evtHandleDic[eventType] -= callback;
            if (evtHandleDic[eventType] == null)
            {
                evtHandleDic.Remove(eventType);
            }
        }
    }
    
    public bool DispatchEvent(CustomEvent evt)
    {
        if (evtHandleDic.ContainsKey(evt.type))
        {
            evtHandleDic[evt.type](evt.data);
        }
    }
    
    void Update()
    {
        while (RecvMessageQueue.Count > 0)
        {
            lock (RecvMessageQueue)
            {
				NetMessageEvent netMessageEvt = RecvMessageQueue.Dequeue();
                if (evtDic.ContainsKey(netMessageEvt.eventType))
                {
                    evtDic[netMessageEvt.eventType](netMessageEvt.eventData);
                }
            }
        }
    }
}