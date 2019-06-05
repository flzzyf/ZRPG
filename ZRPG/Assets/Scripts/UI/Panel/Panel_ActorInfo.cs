using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_ActorInfo : MonoBehaviour
{
    public Image portrait;
    public Slider slider_HP;
    public Text text_HP;

    public void Set(Actor actor)
    {
        //slider_HP.value = (float)actor.hpCurrent / actor.hpMax;
        StopAllCoroutines();
        StartCoroutine(SetSliderValue((float)actor.hpCurrent / actor.hpMax));

        //text_HP.text = string.Format("{0}/{1}", actor.hpCurrent, actor.hpMax);
        text_HP.text = actor.hpCurrent.ToString();
    }

    public float sliderValueChangeSpeed = 0.5f;

    IEnumerator SetSliderValue(float value)
    {
        while(Mathf.Abs(value - slider_HP.value) > 0.01f)
        {
            slider_HP.value = Mathf.Lerp(slider_HP.value, value, sliderValueChangeSpeed);

            yield return null;
        }
    }
}
