using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;


public class ShakerSocket : XRSocketInteractor
{

    /// <summary>
    /// Initiates socket snapping for a specified <see cref="XRGrabInteractable"/> object.
    /// </summary>
    /// <param name="grabInteractable">The <see cref="XRGrabInteractable"/> object to initiate socket snapping for.</param>
    /// <returns>Returns <see langword="true"/> if the operation is successful; false if the socket snapping has already started for the interactable or if the number of interactables with socket transformer exceeds the socket limit.</returns>
    /// <remarks>
    /// If the socket snapping has already started for the interactable, or if the number of interactables with socket transformer exceeds the socket limit, the method does nothing.
    /// Otherwise, it adds the specified grab interactable to the socket grab transformer and adds it to the global and local interactables with socket transformer lists.
    /// </remarks>
    /// <seealso cref="EndSocketSnapping"/>
    protected override bool StartSocketSnapping(XRGrabInteractable grabInteractable)
    {
        if (base.StartSocketSnapping(grabInteractable))
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.SHAKER_HEADON, grabInteractable.transform);
            return true;
        }
        else
        {
            return false;
        }
        
    }

}
