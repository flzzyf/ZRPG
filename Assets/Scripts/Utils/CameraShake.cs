using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
	public void Shake(float duration, float magnitude)
	{
		StartCoroutine(ShakeCor(duration, magnitude));
	}

    public IEnumerator ShakeCor(float duration, float magnitude)
	{
		Vector3 originalPos = transform.localPosition;

		for (float timer = 0; timer < duration; timer += Time.unscaledDeltaTime)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = new Vector3(x, y, originalPos.z);

			yield return null;
		}

		transform.localPosition = originalPos;
	}
}
