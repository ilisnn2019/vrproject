using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EVENT_TYPE
{
    SHAKER_HEADON,
    SHAKER_LOCKING
};

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}