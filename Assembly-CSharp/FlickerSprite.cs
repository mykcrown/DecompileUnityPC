using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C9 RID: 2249
public class FlickerSprite : MonoBehaviour
{
	// Token: 0x060038B1 RID: 14513 RVA: 0x0010A465 File Offset: 0x00108865
	public void Flicker(float flickerDuration, float flickerInterval)
	{
		this.beginFlicker(flickerDuration, flickerInterval);
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x0010A46F File Offset: 0x0010886F
	public void Flicker()
	{
		this.beginFlicker(this.FlickerDuration, this.FlickerInterval);
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x0010A483 File Offset: 0x00108883
	public void StopFlicker()
	{
		if (this.flickerRoutine != null)
		{
			base.StopCoroutine(this.flickerRoutine);
			this.Image.enabled = true;
			this.flickerRoutine = null;
		}
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x0010A4AF File Offset: 0x001088AF
	private void beginFlicker(float flickerDuration, float flickerInterval)
	{
		this.flickerRoutine = base.StartCoroutine(this.flicker(flickerDuration, flickerInterval));
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x0010A4C8 File Offset: 0x001088C8
	private IEnumerator flicker(float flickerDuration, float flickerInterval)
	{
		float timeElapsed = 0f;
		bool visible = true;
		while (timeElapsed < flickerDuration)
		{
			visible = !visible;
			this.Image.enabled = visible;
			timeElapsed += WTime.deltaTime;
			yield return new WaitForSeconds(flickerInterval);
		}
		this.Image.enabled = true;
		yield break;
	}

	// Token: 0x04002704 RID: 9988
	public float FlickerDuration = 0.3f;

	// Token: 0x04002705 RID: 9989
	public float FlickerInterval = 0.1f;

	// Token: 0x04002706 RID: 9990
	private Coroutine flickerRoutine;

	// Token: 0x04002707 RID: 9991
	public Image Image;
}
