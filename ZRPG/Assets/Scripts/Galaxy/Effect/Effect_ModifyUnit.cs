using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/ModifyUnit")]
public class Effect_ModifyUnit : Effect
{
    public int hp;

    public override void Trigger()
    {
        base.Trigger();

        if(hp != 0)
            target.ModifyHp(hp);
    }

}
