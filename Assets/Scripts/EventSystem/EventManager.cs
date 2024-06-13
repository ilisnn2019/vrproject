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
    /// ������ �߰�
    /// </summary>
    /// <param name="Event_Type">������ �̺�Ʈ</param>
    /// <param name="Listener">�̺�Ʈ�� ������ ������</param>
    public void AddListener(EVENT_TYPE Event_Type, IListener Listener)
    {
        List<IListener> ListenList = new();
        //��Ȳ�� ���� ������ ����Ʈ�� �����ϴ���, ������ �߰�
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        //������ ���� �߰�
        ListenList = new();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);

    }
    /// <summary>
    /// �̺�Ʈ�� �����ʿ��� ����
    /// </summary>
    /// <param name="Event_Type">������ �̺�Ʈ</param>
    /// <param name="Sender">�̺�Ʈ�� �θ��� ������Ʈ</param>
    /// <param name="Param">�Ķ����</param>
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