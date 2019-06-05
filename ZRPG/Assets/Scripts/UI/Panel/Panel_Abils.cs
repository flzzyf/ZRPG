using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Abils : MonoBehaviour
{
    public Panel_Abil panelPrefab;

    public List<Panel_Abil> panels;

    public void Init(Actor actor)
    {
        for (int i = 0; i < actor.abils.Count; i++)
        {
            panels[i].Init(actor.abils[i]);
        }
    }
}
