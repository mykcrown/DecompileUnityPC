// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterActionData
{
	[FormerlySerializedAs("clip1")]
	public AnimationClip animation;

	public AnimationClip leftAnimation;

	public WrapMode wrapMode;

	public float animationSpeed = 1f;

	public int skipFrames;

	public SoundEffect[] soundEffects = new SoundEffect[0];

	[FormerlySerializedAs("reference")]
	public ActionState characterActionState;

	public string name;

	public bool useRootTranslation;

	public int interruptibleFrames;

	public bool overrideBlendIn;

	public bool overrideBlendOut;

	public float blendInDuration;

	public float blendOutDuration;

	public bool readAnyBufferedInput;

	public bool triggerHeldInputAsTap;

	public MoveLabel[] validBufferedMoveLabels = new MoveLabel[0];

	public ButtonPress[] maskedBufferButtons = new ButtonPress[0];

	public string LeftAnimationName
	{
		get
		{
			return this.name + "_left";
		}
	}

	public float animationDuration
	{
		get
		{
			if (this.animationSpeed == 0f || this.animation == null)
			{
				return 0f;
			}
			return this.animation.length / this.animationSpeed;
		}
	}

	public int frameDuration
	{
		get
		{
			return (int)(this.animationDuration * 60f);
		}
	}

	public CharacterActionData(ActionState actionState, string name)
	{
		this.characterActionState = actionState;
		this.name = name;
	}

	public CharacterActionData()
	{
	}
}
