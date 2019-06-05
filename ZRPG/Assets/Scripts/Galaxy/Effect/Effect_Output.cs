using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_Output")]
public class Effect_Output : Effect
{
    public string text;

    public override void Trigger()
    {
        Debug.Log("Output：" + text);

    }
}
