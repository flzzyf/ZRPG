using UnityEngine;

public class Effect : ScriptableObject
{
    [HideInInspector] public Actor caster;
    [HideInInspector] public Actor target;

    public virtual void Trigger()
    {

    }

    public void Trigger(Effect parent)
    {
        caster = parent.caster;
        target = parent.target;

        Trigger();
    }

    public void Trigger(Actor caster, Actor target)
    {
        this.caster = caster;
        this.target = target;

        Trigger();
    }
}
