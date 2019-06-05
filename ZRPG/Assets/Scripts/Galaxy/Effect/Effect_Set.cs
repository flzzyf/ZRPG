using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Effect_Set")]
public class Effect_Set : Effect
{
    public TargetType targetPosTyp;

    public Effect[] effects = new Effect[1];

    public override void Trigger()
    {
        //判断是否延迟触发
        if (delayTime <= 0)
        {
            TriggerSet();
        }
        else
        {
            MonoUtil.instance.StartCoroutine(Delay());
        }
    }

    //触发集效果
    void TriggerSet()
    {
        foreach (Effect item in effects)
        {
            item.Trigger(this);
        }
    }

    #region 延迟触发

    public float delayTime;
    float timer;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);

        TriggerSet();
    }
    #endregion

}
