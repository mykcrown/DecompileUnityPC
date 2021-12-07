// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CameraInfluencerBehaviour : StageBehaviour
{
	public StageCameraInfluencer CameraInfluencer;

	public bool SetActive = true;

	public bool hasToggleDuration;

	public int duration;

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
}
