using System;

// Token: 0x0200062B RID: 1579
public class CameraInfluencerBehaviour : StageBehaviour
{
	// Token: 0x060026DF RID: 9951 RVA: 0x000BE320 File Offset: 0x000BC720
	public override void Play(object context)
	{
		if (this.CameraInfluencer != null)
		{
			if (this.hasToggleDuration)
			{
				this.CameraInfluencer.SetIsActive(this.SetActive, this.duration);
			}
			else
			{
				this.CameraInfluencer.SetIsActive(this.SetActive, -1);
			}
		}
	}

	// Token: 0x04001C65 RID: 7269
	public StageCameraInfluencer CameraInfluencer;

	// Token: 0x04001C66 RID: 7270
	public bool SetActive = true;

	// Token: 0x04001C67 RID: 7271
	public bool hasToggleDuration;

	// Token: 0x04001C68 RID: 7272
	public int duration;
}
