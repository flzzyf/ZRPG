using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abil")]
public class Abil : ScriptableObject
{
    public Sprite icon;

    [Header("属性设置")]
    //目标类型
    public TargetType targetType;

    public Effect effect;

    //范围显示效果
    public Effect rangeDisplayEffect;

    //释放技能
    public void Cast(Actor caster)
    {
        if(targetType == TargetType.Null)
            effect.Trigger(caster, caster);
    }
}
