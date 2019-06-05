using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent[] animEvent;

    public void Event1()
    {
        animEvent[0].Invoke();
    }

    public void Event2()
    {
        animEvent[1].Invoke();
    }

}
