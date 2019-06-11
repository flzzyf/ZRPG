using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_Force")]
public class Effect_Force : Effect
{
    public enum TargetType { Target, Caster }
    public TargetType targetType = TargetType.Target;

    public float amount = 3;
    public float time;

    public override void Trigger()
    {

        //if (targetType == TargetType.Target)    //对目标释放
        //{
        //    Debug.Log(caster.name + "对" + target.name + "施力");
        //    Controller2D controller = target.GetComponent<Controller2D>();

        //    Vector3 dir = target.transform.position - caster.transform.position;
        //    dir.Normalize();
        //    Debug.Log(dir);

        //    //dir.y = 0;

        //    if (controller != null)
        //    {
        //        Debug.Log("qwe");

        //        controller.AddForce(dir, amount);

        //    }
        //    else
        //    {
        //        //target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //        target.GetComponent<Rigidbody2D>().AddForce(dir * amount, ForceMode2D.Impulse);   //没有controller就直接rb施力 

        //    }
        //}
        //else
        //{
        //    Controller2D controller = caster.GetComponent<Controller2D>();
        //    controller.AddForce(caster.transform.right * caster.transform.localScale.x, amount);

        //}
    }
}
