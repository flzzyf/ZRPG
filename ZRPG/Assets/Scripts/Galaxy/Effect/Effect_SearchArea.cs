using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_SearchArea")]
public class Effect_SearchArea : Effect
{
    public Vector2 offset;

    public float radius;

    public Effect effect;

    public Filter filter;

    public override void Trigger()
    {
        Vector2 newOffset = offset;
        newOffset.x *= caster.transform.localScale.x;
        Vector2 searchPos = (Vector2)caster.transform.position + newOffset;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(searchPos, radius);

        //遍历判断每一个目标
        for (int i = 0; i < colliders.Length; i++)
        {
            //判断目标过滤器

            //触发效果
            effect.Trigger(caster, colliders[i].GetComponent<Actor>());
        }
    }
}
