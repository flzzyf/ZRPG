using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Panel_Abils panel_Abils;

    public Actor actor;

    void Start()
    {
        panel_Abils.Init(actor);
    }

    void Update()
    {
        
    }
}
