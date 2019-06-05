using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Abil : MonoBehaviour
{
    public Text text_Hotkey;
    public Image icon;
    public Text text_AbilName;

    Abil abil;

    public void Init(Abil abil)
    {
        icon.sprite = abil.icon;
        text_AbilName.text = abil.name;

        this.abil = abil;
    }
}
