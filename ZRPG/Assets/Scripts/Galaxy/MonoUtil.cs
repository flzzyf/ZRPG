using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoUtil : MonoBehaviour
{
    public static MonoUtil instance;

    void Awake()
    {
        instance = this;
    }
}
