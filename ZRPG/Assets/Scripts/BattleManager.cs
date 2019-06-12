using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public Panel_Abils panel_Abils;

    public Actor player;
    public Actor enemy;

    void Start()
    {
        panel_Abils.Init(player);
    }

    void Update()
    {
        
    }

    public IEnumerator Respawn(Actor actor)
    {
        yield return new WaitForSeconds(1f);

        actor.gameObject.SetActive(true);

        actor.transform.position = new Vector2(0, 10);
        actor.SetHp(player.hpMax);
    }

}
