using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200039B RID: 923
public class BoneController : IBoneController, ITickable, IBodyOwner, IRollbackStateOwner, IHurtBoxOwner, IDestroyable, IInversionOwner
{
	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060013BD RID: 5053 RVA: 0x000705B6 File Offset: 0x0006E9B6
	// (set) Token: 0x060013BE RID: 5054 RVA: 0x000705BE File Offset: 0x0006E9BE
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060013BF RID: 5055 RVA: 0x000705C7 File Offset: 0x0006E9C7
	// (set) Token: 0x060013C0 RID: 5056 RVA: 0x000705CF File Offset: 0x0006E9CF
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060013C1 RID: 5057 RVA: 0x000705D8 File Offset: 0x0006E9D8
	public bool IsInverted
	{
		get
		{
			return this.state.invert;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060013C2 RID: 5058 RVA: 0x000705E5 File Offset: 0x0006E9E5
	// (set) Token: 0x060013C3 RID: 5059 RVA: 0x000705ED File Offset: 0x0006E9ED
	public List<HurtBoxState> HurtBoxes { get; private set; }

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060013C4 RID: 5060 RVA: 0x000705F6 File Offset: 0x0006E9F6
	public string AnimationName
	{
		get
		{
			return this.animator.CurrentAnimationName;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00070603 File Offset: 0x0006EA03
	public int AnimationFrame
	{
		get
		{
			return this.animator.CurrentFrame;
		}
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x00070610 File Offset: 0x0006EA10
	public void Init(CharacterData characterData, BoneData boneData, IAnimationDataSource animationDataSource, IPlayerOrientation orientation, IAnimatorDataOwner animator, IPositionOwner positionOwner, Vector3F centerOffset, Transform playerTransform, IPlayerDelegate playerDelegate)
	{
		this.boneData = boneData;
		this.animationDataSource = animationDataSource;
		this.orientation = orientation;
		this.animator = animator;
		this.centerOffset = centerOffset;
		this.positionOwner = positionOwner;
		this.playerTransform = playerTransform;
		this.playerDelegate = playerDelegate;
		this.state = new BoneControllerState();
		this.HurtBoxes = new List<HurtBoxState>();
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HurtBoxes))
		{
			this.toggleHurtBoxCapsules(true);
		}
		if (this.events != null)
		{
			this.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugDrawChannel));
		}
		if (characterData != null)
		{
			for (int i = 0; i < characterData.HurtBoxes.Count; i++)
			{
				HurtBox hurtBox = characterData.HurtBoxes[i];
				HurtBoxState hurtBoxState = new HurtBoxState(hurtBox, this, this);
				this.hurtBoxMap.Add(hurtBox.startBone, hurtBoxState);
				this.HurtBoxes.Add(hurtBoxState);
				this.state.hurtBoxVisibility.Add(HurtBoxVisibilityState.VISIBLE);
				this.hurtBoxIndexMap[hurtBox.startBone] = i;
			}
		}
		this.HurtBoxes.Sort((HurtBoxState a, HurtBoxState b) => b.Priority() - a.Priority());
		if (this.boneData != null && this.boneData.Bones != null)
		{
			foreach (Bone bone in boneData.Bones)
			{
				foreach (Bone bone2 in boneData.Bones)
				{
					if ((bone.bodyPart == BodyPart.leftCalf && bone2.bodyPart == BodyPart.rightCalf) || (bone.bodyPart == BodyPart.leftFoot && bone2.bodyPart == BodyPart.rightFoot) || (bone.bodyPart == BodyPart.leftForearm && bone2.bodyPart == BodyPart.rightForearm) || (bone.bodyPart == BodyPart.leftHand && bone2.bodyPart == BodyPart.rightHand) || (bone.bodyPart == BodyPart.leftThigh && bone2.bodyPart == BodyPart.rightThigh) || (bone.bodyPart == BodyPart.leftUpperArm && bone2.bodyPart == BodyPart.rightUpperArm) || (bone.bodyPart == BodyPart.leftThumbTip && bone2.bodyPart == BodyPart.rightThumbTip) || (bone.bodyPart == BodyPart.leftIndexTip && bone2.bodyPart == BodyPart.rightIndexTip) || (bone.bodyPart == BodyPart.leftMiddleTip && bone2.bodyPart == BodyPart.rightMiddleTip) || (bone.bodyPart == BodyPart.leftRingTip && bone2.bodyPart == BodyPart.rightRingTip) || (bone.bodyPart == BodyPart.leftLittleTip && bone2.bodyPart == BodyPart.rightLittleTip))
					{
						this.invertMap[bone.bodyPart] = bone2.bodyPart;
						this.invertMap[bone2.bodyPart] = bone.bodyPart;
					}
				}
				this.particleTrackerMap.Add(bone.bodyPart, new List<BoneController.ParticleTransformData>());
			}
			this.particleTrackerMap.Add(BodyPart.shield, new List<BoneController.ParticleTransformData>());
		}
		animator.SetHiddenProps(boneData.HiddenProps);
		if (animationDataSource == null)
		{
			Debug.LogError("Missing baked animation data!");
			return;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060013C7 RID: 5063 RVA: 0x000709C0 File Offset: 0x0006EDC0
	public FixedRect CurrentMaxBounds
	{
		get
		{
			FixedRect maxBounds = this.animationDataSource.GetMaxBounds(this.animator.CurrentAnimationName);
			maxBounds.position += this.positionOwner.Position;
			return maxBounds;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00070A08 File Offset: 0x0006EE08
	Vector3F IBodyOwner.DeltaPosition
	{
		get
		{
			Vector3F vector3F = this.animationDataSource.GetRootDelta(this.animator.CurrentAnimationName, this.animator.CurrentFrame);
			if (!this.animationDataSource.IsBoneDataAbsolute)
			{
				vector3F = this.orientation.Rotation * vector3F;
			}
			return vector3F;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00070A5A File Offset: 0x0006EE5A
	bool IBodyOwner.HasRootMotion
	{
		get
		{
			return this.animationDataSource.HasRootDeltaData(this.animator.CurrentAnimationName);
		}
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00070A74 File Offset: 0x0006EE74
	public void Destroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugDrawChannel));
		}
		foreach (CapsuleShape capsuleShape in this.hurtBoxCapsules.Values)
		{
			capsuleShape.Clear();
		}
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00070B00 File Offset: 0x0006EF00
	private void onToggleDebugDrawChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		bool enabled = toggleDebugDrawChannelCommand.enabled;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HurtBoxes)
		{
			this.toggleHurtBoxCapsules(enabled);
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x00070B30 File Offset: 0x0006EF30
	private void toggleHurtBoxCapsules(bool enabled)
	{
		if (enabled)
		{
			for (int i = 0; i < this.HurtBoxes.Count; i++)
			{
				HurtBox hurtBox = this.HurtBoxes[i].hurtBox;
				CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.playerTransform);
				Vector3 position = (Vector3)hurtBox.GetPoint1(this);
				Vector3 position2 = (Vector3)hurtBox.GetPoint2(this);
				capsule.Load(position, position2, hurtBox.radius, WColor.DebugHurtboxColor, hurtBox.isCircle);
				this.hurtBoxCapsules[hurtBox] = capsule;
			}
		}
		else
		{
			foreach (CapsuleShape capsuleShape in this.hurtBoxCapsules.Values)
			{
				capsuleShape.Clear();
			}
			this.hurtBoxCapsules.Clear();
		}
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00070C2C File Offset: 0x0006F02C
	public void TickFrame()
	{
		Quaternion rootRotation = (Quaternion)this.GetRotation(BodyPart.root, false);
		foreach (KeyValuePair<BodyPart, List<BoneController.ParticleTransformData>> keyValuePair in this.particleTrackerMap)
		{
			BodyPart key = keyValuePair.Key;
			List<BoneController.ParticleTransformData> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				for (int i = value.Count - 1; i >= 0; i--)
				{
					if (value[i].particleTransform == null || value[i].particleTransform.gameObject == null || !value[i].particleTransform.gameObject.activeInHierarchy)
					{
						value.RemoveAt(i);
					}
					else
					{
						bool invertHorizontal = value[i].invertHorizontal;
						Vector3 bodyPartPosition = (Vector3)this.GetBonePosition(key, invertHorizontal);
						Quaternion quaternion = (Quaternion)this.GetRotation(key, invertHorizontal);
						value[i].particleTransform.position = this.UpdateParticlePosition(bodyPartPosition, rootRotation, quaternion, value[i].particleOffsetSpace, value[i].particleOffset);
						value[i].particleTransform.rotation = quaternion;
					}
				}
			}
		}
		foreach (KeyValuePair<HurtBox, CapsuleShape> keyValuePair2 in this.hurtBoxCapsules)
		{
			HurtBox key2 = keyValuePair2.Key;
			CapsuleShape value2 = keyValuePair2.Value;
			Vector3 position = (Vector3)key2.GetPoint1(this);
			Vector3 position2 = (Vector3)key2.GetPoint2(this);
			value2.SetPositions(position, position2, key2.isCircle);
			value2.Radius = key2.radius;
			Color color = WColor.DebugHurtboxColor;
			if (this.MatchesVisibilityState(key2.startBone, HurtBoxVisibilityState.HIDDEN_FROM_PROJECTILES))
			{
				color = WColor.DebugHurtboxProjectileImmuneColor;
			}
			value2.SetColor(color);
			value2.Visible = !this.MatchesVisibilityState(key2.startBone, HurtBoxVisibilityState.HIDDEN);
		}
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00070EC0 File Offset: 0x0006F2C0
	public void ToggleHurtBoxes(HurtBoxVisibilityState visState)
	{
		for (int i = 0; i < this.state.hurtBoxVisibility.Count; i++)
		{
			this.state.hurtBoxVisibility[i] = visState;
		}
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x00070F00 File Offset: 0x0006F300
	public void ToggleBodyParts(BodyPart[] bodyParts, HurtBoxVisibilityState visState)
	{
		foreach (BodyPart bodyPart in bodyParts)
		{
			int index;
			if (!this.hurtBoxIndexMap.TryGetValue(bodyPart, out index))
			{
				string message = "Tried to toggle bodypart " + bodyPart + " when BoneController has no record of it";
				Debug.LogError(message);
				throw new KeyNotFoundException(message);
			}
			this.state.hurtBoxVisibility[index] = visState;
		}
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00070F74 File Offset: 0x0006F374
	public bool MatchesVisibilityState(BodyPart bodyPart, HurtBoxVisibilityState visState)
	{
		int index;
		if (!this.hurtBoxIndexMap.TryGetValue(bodyPart, out index))
		{
			string message = "Tried to access bodypart " + bodyPart + " when BoneController has no record of it";
			Debug.LogError(message);
			throw new KeyNotFoundException(message);
		}
		return this.state.hurtBoxVisibility[index] == visState;
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00070FCC File Offset: 0x0006F3CC
	public HurtBoxState GetHurtBox(BodyPart bodyPart)
	{
		HurtBoxState result;
		if (!this.hurtBoxMap.TryGetValue(bodyPart, out result))
		{
			string message = "Tried to access bone " + bodyPart + " when this character doesn't have it assigned";
			Debug.LogError(message);
			throw new KeyNotFoundException(message);
		}
		return result;
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x00071010 File Offset: 0x0006F410
	private BodyPart getInvertedBodyPart(BodyPart bodyPart)
	{
		BodyPart result;
		if (this.invertMap.TryGetValue(bodyPart, out result))
		{
			return result;
		}
		return bodyPart;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00071033 File Offset: 0x0006F433
	private static bool requiresRotation(IPlayerDelegate player, BodyPart bodyPart)
	{
		return player == null || player.IsRotationRolled || bodyPart == BodyPart.throwBone;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00071050 File Offset: 0x0006F450
	public Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false)
	{
		if (this.animationDataSource == null)
		{
			Debug.LogWarning("Missing baked animation data");
			return Vector3F.zero;
		}
		if (this.animator.CurrentAnimationName == null)
		{
			return Vector3F.zero;
		}
		if (bodyPart == BodyPart.shield)
		{
			return this.playerDelegate.Shield.ShieldPosition;
		}
		if (forceInvert)
		{
			bodyPart = this.getInvertedBodyPart(bodyPart);
		}
		else if (this.IsInverted && this.animationDataSource.IsBoneDataAbsolute)
		{
			bodyPart = this.getInvertedBodyPart(bodyPart);
		}
		HorizontalDirection horizontalDirection = (!BoneController.requiresRotation(this.playerDelegate, bodyPart)) ? this.playerDelegate.Facing : HorizontalDirection.None;
		Vector3F vector3F = this.animationDataSource.GetBoneFrameData(this.animator.CurrentAnimationName, bodyPart, this.animator.CurrentFrame, horizontalDirection).position;
		if (!this.animationDataSource.IsBoneDataAbsolute)
		{
			if (this.IsInverted && !forceInvert)
			{
				if (horizontalDirection == HorizontalDirection.None)
				{
					vector3F.x = -vector3F.x;
				}
				else
				{
					vector3F.z = -vector3F.z;
				}
			}
			if (BoneController.requiresRotation(this.playerDelegate, bodyPart))
			{
				vector3F = this.orientation.Rotation * vector3F;
			}
			vector3F = this.centerOffset + this.positionOwner.Position + vector3F;
		}
		else if (this.playerDelegate != null)
		{
			vector3F -= this.playerDelegate.Model.displayOffset;
			if (this.IsInverted && bodyPart >= BodyPart.weaponGun)
			{
				vector3F.z *= -1;
			}
		}
		return vector3F;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x00071210 File Offset: 0x0006F610
	public Vector3 GetOffsetParticlePosition(BodyPart bodyPart, ParticleOffsetSpace offsetSpace, Vector3 offset, bool forceInvert = false)
	{
		Quaternion rootRotation = (Quaternion)this.GetRotation(BodyPart.root, false);
		Vector3 bodyPartPosition = (Vector3)this.GetBonePosition(bodyPart, forceInvert);
		Quaternion bodyPartRotation = (Quaternion)this.GetRotation(bodyPart, forceInvert);
		return this.UpdateParticlePosition(bodyPartPosition, rootRotation, bodyPartRotation, offsetSpace, offset);
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x00071258 File Offset: 0x0006F658
	public QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false)
	{
		if (this.animationDataSource == null)
		{
			Debug.LogWarning("Missing baked animation data");
			return default(QuaternionF);
		}
		if (forceInvert)
		{
			bodyPart = this.getInvertedBodyPart(bodyPart);
		}
		else if (this.IsInverted && this.animationDataSource.IsBoneDataAbsolute)
		{
			bodyPart = this.getInvertedBodyPart(bodyPart);
		}
		if (bodyPart == BodyPart.shield)
		{
			return QuaternionF.identity;
		}
		HorizontalDirection facing = (!BoneController.requiresRotation(this.playerDelegate, bodyPart)) ? this.playerDelegate.Facing : HorizontalDirection.None;
		QuaternionF quaternionF = this.animationDataSource.GetBoneFrameData(this.animator.CurrentAnimationName, bodyPart, this.animator.CurrentFrame, facing).rotation;
		if (!this.animationDataSource.IsBoneDataAbsolute)
		{
			if (this.IsInverted)
			{
			}
			if (BoneController.requiresRotation(this.playerDelegate, bodyPart))
			{
				quaternionF = this.orientation.Rotation * quaternionF;
			}
		}
		return quaternionF;
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00071358 File Offset: 0x0006F758
	public void DrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
		}
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x0007136A File Offset: 0x0006F76A
	public void InvertHurtBoxes(bool isInverted)
	{
		this.state.invert = isInverted;
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00071378 File Offset: 0x0006F778
	private Vector3 UpdateParticlePosition(Vector3 bodyPartPosition, Quaternion rootRotation, Quaternion bodyPartRotation, ParticleOffsetSpace particleOffsetSpace, Vector3 particleOffset)
	{
		Vector3 b;
		switch (particleOffsetSpace)
		{
		case ParticleOffsetSpace.ParticleOffsetRootRotation:
			b = rootRotation * particleOffset;
			goto IL_3C;
		case ParticleOffsetSpace.ParticleOffsetBodyPartRotation:
			b = bodyPartRotation * particleOffset;
			goto IL_3C;
		}
		b = particleOffset;
		IL_3C:
		return bodyPartPosition + b;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x000713C8 File Offset: 0x0006F7C8
	public void AttachVFX(BodyPart bodyPart, Transform vfxTransform, Vector3 vfxOffset, ParticleOffsetSpace particleOffsetSpace, bool invertHorizontal)
	{
		BoneController.ParticleTransformData item;
		item.particleTransform = vfxTransform;
		item.particleOffset = vfxOffset;
		item.particleOffsetSpace = particleOffsetSpace;
		Vector3 bodyPartPosition = (Vector3)this.GetBonePosition(bodyPart, invertHorizontal);
		Quaternion quaternion = (Quaternion)this.GetRotation(bodyPart, invertHorizontal);
		item.particleTransform.position = this.UpdateParticlePosition(bodyPartPosition, (Quaternion)this.GetRotation(BodyPart.root, false), quaternion, particleOffsetSpace, vfxOffset);
		item.particleTransform.rotation = quaternion;
		item.invertHorizontal = invertHorizontal;
		this.particleTrackerMap[bodyPart].Add(item);
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00071459 File Offset: 0x0006F859
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<BoneControllerState>(this.state));
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x00071473 File Offset: 0x0006F873
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<BoneControllerState>(ref this.state);
		return true;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x00071483 File Offset: 0x0006F883
	public IBoneTransform GetBoneTransform(BodyPart bone)
	{
		return new BoneTransform(bone, this);
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x00071491 File Offset: 0x0006F891
	public IBoneTransform GetBoneOffsetTransform(BodyPart bone, BodyPart secondaryBone, Fixed offset)
	{
		return new BoneOffsetTransform(bone, secondaryBone, offset, this);
	}

	// Token: 0x04000D3F RID: 3391
	public static readonly Fixed MIN_ROOT_MOTION = (Fixed)0.0005;

	// Token: 0x04000D40 RID: 3392
	private Dictionary<BodyPart, HurtBoxState> hurtBoxMap = new Dictionary<BodyPart, HurtBoxState>(default(BodyPartComparer));

	// Token: 0x04000D42 RID: 3394
	private Dictionary<BodyPart, int> hurtBoxIndexMap = new Dictionary<BodyPart, int>(default(BodyPartComparer));

	// Token: 0x04000D43 RID: 3395
	private Dictionary<HurtBox, CapsuleShape> hurtBoxCapsules = new Dictionary<HurtBox, CapsuleShape>();

	// Token: 0x04000D44 RID: 3396
	private Dictionary<BodyPart, BodyPart> invertMap = new Dictionary<BodyPart, BodyPart>(default(BodyPartComparer));

	// Token: 0x04000D45 RID: 3397
	private Dictionary<BodyPart, List<BoneController.ParticleTransformData>> particleTrackerMap = new Dictionary<BodyPart, List<BoneController.ParticleTransformData>>(default(BodyPartComparer));

	// Token: 0x04000D46 RID: 3398
	private Vector3F centerOffset;

	// Token: 0x04000D47 RID: 3399
	private IPositionOwner positionOwner;

	// Token: 0x04000D48 RID: 3400
	private BoneData boneData;

	// Token: 0x04000D49 RID: 3401
	private IAnimationDataSource animationDataSource;

	// Token: 0x04000D4A RID: 3402
	private IPlayerOrientation orientation;

	// Token: 0x04000D4B RID: 3403
	private IAnimatorDataOwner animator;

	// Token: 0x04000D4C RID: 3404
	private Transform playerTransform;

	// Token: 0x04000D4D RID: 3405
	private IPlayerDelegate playerDelegate;

	// Token: 0x04000D4E RID: 3406
	private BoneControllerState state;

	// Token: 0x0200039C RID: 924
	private struct ParticleTransformData
	{
		// Token: 0x04000D50 RID: 3408
		public Transform particleTransform;

		// Token: 0x04000D51 RID: 3409
		public Vector3 particleOffset;

		// Token: 0x04000D52 RID: 3410
		public ParticleOffsetSpace particleOffsetSpace;

		// Token: 0x04000D53 RID: 3411
		public bool invertHorizontal;
	}
}
