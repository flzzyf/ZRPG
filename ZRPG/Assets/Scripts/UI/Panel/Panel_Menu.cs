using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Menu : MonoBehaviour
{
    public void Restart()
    {
        SceneFader.instance.ReloadScene();
    }
}
