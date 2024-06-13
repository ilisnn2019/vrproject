using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EventManager : MonoBehaviour
{
    #region C# property

    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }
    #endregion

    #region varients

    private static EventManager instance;

    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new();


    #endregion

    #region Method

    // Awake is called for init
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            DestroyImmediate(this);
    }

    /// <summary>
    /// 리스너 추가
    /// </summary>
    /// <param name="Event_Type">수신할 이벤트</param>
    /// <param name="Listener">이벤트를 수신할 리스너</param>
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {
        List<IListener> ListenList = new();
        //상황에 따른 리스너 리스트가 존재하는지, 있으면 추가
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        //없으면 새로 추가
        ListenList = new();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);

    }
    /// <summary>
    /// 이벤트를 리스너에게 전달
    /// </summary>
    /// <param name="Event_Type">전달할 이벤트</param>
    /// <param name="Sender">이벤트를 부르는 오브젝트</param>
    /// <param name="Param">파라미터</param>
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        List<IListener> ListenList;
        if (!Listeners.TryGetValue(Event_Type, out ListenList)) return;

        for (int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_Type, Sender, Param);
        }
    }

    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);
    }

    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> TmpListeners = new();

        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i++)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        Listeners = TmpListeners;
    }

    private void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
    #endregion

}