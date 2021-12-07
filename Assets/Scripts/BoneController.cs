// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BoneController : IBoneController, ITickable, IBodyOwner, IRollbackStateOwner, IHurtBoxOwner, IDestroyable, IInversionOwner
{
	private struct ParticleTransformData
	{
		public Transform particleTransform;

		public Vector3 particleOffset;

		public ParticleOffsetSpace particleOffsetSpace;

		public bool invertHorizontal;
	}

	public static readonly Fixed MIN_ROOT_MOTION = (Fixed)0.0005;

	private Dictionary<BodyPart, HurtBoxState> hurtBoxMap = new Dictionary<BodyPart, HurtBoxState>(default(BodyPartComparer));

	private Dictionary<BodyPart, int> hurtBoxIndexMap = new Dictionary<BodyPart, int>(default(BodyPartComparer));

	private Dictionary<HurtBox, CapsuleShape> hurtBoxCapsules = new Dictionary<HurtBox, CapsuleShape>();

	private Dictionary<BodyPart, BodyPart> invertMap = new Dictionary<BodyPart, BodyPart>(default(BodyPartComparer));

	private Dictionary<BodyPart, List<BoneController.ParticleTransformData>> particleTrackerMap = new Dictionary<BodyPart, List<BoneController.ParticleTransformData>>(default(BodyPartComparer));

	private Vector3F centerOffset;

	private IPositionOwner positionOwner;

	private BoneData boneData;

	private IAnimationDataSource animationDataSource;

	private IPlayerOrientation orientation;

	private IAnimatorDataOwner animator;

	private Transform playerTransform;

	private IPlayerDelegate playerDelegate;

	private BoneControllerState state;

	private static Comparison<HurtBoxState> __f__am_cache0;

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

	bool IBodyOwner.HasRootMotion
	{
		get
		{
			return this.animationDataSource.HasRootDeltaData(this.animator.CurrentAnimationName);
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	public bool IsInverted
	{
		get
		{
			return this.state.invert;
		}
	}

	public List<HurtBoxState> HurtBoxes
	{
		get;
		private set;
	}

	public string AnimationName
	{
		get
		{
			return this.animator.CurrentAnimationName;
		}
	}

	public int AnimationFrame
	{
		get
		{
			return this.animator.CurrentFrame;
		}
	}

	public FixedRect CurrentMaxBounds
	{
		get
		{
			FixedRect maxBounds = this.animationDataSource.GetMaxBounds(this.animator.CurrentAnimationName);
			maxBounds.position += this.positionOwner.Position;
			return maxBounds;
		}
	}

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
		List<HurtBoxState> arg_139_0 = this.HurtBoxes;
		if (BoneController.__f__am_cache0 == null)
		{
			BoneController.__f__am_cache0 = new Comparison<HurtBoxState>(BoneController._Init_m__0);
		}
		arg_139_0.Sort(BoneController.__f__am_cache0);
		if (this.boneData != null && this.boneData.Bones != null)
		{
			foreach (Bone current in boneData.Bones)
			{
				foreach (Bone current2 in boneData.Bones)
				{
					if ((current.bodyPart == BodyPart.leftCalf && current2.bodyPart == BodyPart.rightCalf) || (current.bodyPart == BodyPart.leftFoot && current2.bodyPart == BodyPart.rightFoot) || (current.bodyPart == BodyPart.leftForearm && current2.bodyPart == BodyPart.rightForearm) || (current.bodyPart == BodyPart.leftHand && current2.bodyPart == BodyPart.rightHand) || (current.bodyPart == BodyPart.leftThigh && current2.bodyPart == BodyPart.rightThigh) || (current.bodyPart == BodyPart.leftUpperArm && current2.bodyPart == BodyPart.rightUpperArm) || (current.bodyPart == BodyPart.leftThumbTip && current2.bodyPart == BodyPart.rightThumbTip) || (current.bodyPart == BodyPart.leftIndexTip && current2.bodyPart == BodyPart.rightIndexTip) || (current.bodyPart == BodyPart.leftMiddleTip && current2.bodyPart == BodyPart.rightMiddleTip) || (current.bodyPart == BodyPart.leftRingTip && current2.bodyPart == BodyPart.rightRingTip) || (current.bodyPart == BodyPart.leftLittleTip && current2.bodyPart == BodyPart.rightLittleTip))
					{
						this.invertMap[current.bodyPart] = current2.bodyPart;
						this.invertMap[current2.bodyPart] = current.bodyPart;
					}
				}
				this.particleTrackerMap.Add(current.bodyPart, new List<BoneController.ParticleTransformData>());
			}
			this.particleTrackerMap.Add(BodyPart.shield, new List<BoneController.ParticleTransformData>());
		}
		animator.SetHiddenProps(boneData.HiddenProps);
		if (animationDataSource == null)
		{
			UnityEngine.Debug.LogError("Missing baked animation data!");
			return;
		}
	}

	public void Destroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugDrawChannel));
		}
		foreach (CapsuleShape current in this.hurtBoxCapsules.Values)
		{
			current.Clear();
		}
	}

	private void onToggleDebugDrawChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		bool enabled = toggleDebugDrawChannelCommand.enabled;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HurtBoxes)
		{
			this.toggleHurtBoxCapsules(enabled);
		}
	}

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
			foreach (CapsuleShape current in this.hurtBoxCapsules.Values)
			{
				current.Clear();
			}
			this.hurtBoxCapsules.Clear();
		}
	}

	public void TickFrame()
	{
		Quaternion rootRotation = (Quaternion)this.GetRotation(BodyPart.root, false);
		foreach (KeyValuePair<BodyPart, List<BoneController.ParticleTransformData>> current in this.particleTrackerMap)
		{
			BodyPart key = current.Key;
			List<BoneController.ParticleTransformData> value = current.Value;
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
		foreach (KeyValuePair<HurtBox, CapsuleShape> current2 in this.hurtBoxCapsules)
		{
			HurtBox key2 = current2.Key;
			CapsuleShape value2 = current2.Value;
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

	public void ToggleHurtBoxes(HurtBoxVisibilityState visState)
	{
		for (int i = 0; i < this.state.hurtBoxVisibility.Count; i++)
		{
			this.state.hurtBoxVisibility[i] = visState;
		}
	}

	public void ToggleBodyParts(BodyPart[] bodyParts, HurtBoxVisibilityState visState)
	{
		for (int i = 0; i < bodyParts.Length; i++)
		{
			BodyPart bodyPart = bodyParts[i];
			int index;
			if (!this.hurtBoxIndexMap.TryGetValue(bodyPart, out index))
			{
				string message = "Tried to toggle bodypart " + bodyPart + " when BoneController has no record of it";
				UnityEngine.Debug.LogError(message);
				throw new KeyNotFoundException(message);
			}
			this.state.hurtBoxVisibility[index] = visState;
		}
	}

	public bool MatchesVisibilityState(BodyPart bodyPart, HurtBoxVisibilityState visState)
	{
		int index;
		if (!this.hurtBoxIndexMap.TryGetValue(bodyPart, out index))
		{
			string message = "Tried to access bodypart " + bodyPart + " when BoneController has no record of it";
			UnityEngine.Debug.LogError(message);
			throw new KeyNotFoundException(message);
		}
		return this.state.hurtBoxVisibility[index] == visState;
	}

	public HurtBoxState GetHurtBox(BodyPart bodyPart)
	{
		HurtBoxState result;
		if (!this.hurtBoxMap.TryGetValue(bodyPart, out result))
		{
			string message = "Tried to access bone " + bodyPart + " when this character doesn't have it assigned";
			UnityEngine.Debug.LogError(message);
			throw new KeyNotFoundException(message);
		}
		return result;
	}

	private BodyPart getInvertedBodyPart(BodyPart bodyPart)
	{
		BodyPart result;
		if (this.invertMap.TryGetValue(bodyPart, out result))
		{
			return result;
		}
		return bodyPart;
	}

	private static bool requiresRotation(IPlayerDelegate player, BodyPart bodyPart)
	{
		return player == null || player.IsRotationRolled || bodyPart == BodyPart.throwBone;
	}

	public Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false)
	{
		if (this.animationDataSource == null)
		{
			UnityEngine.Debug.LogWarning("Missing baked animation data");
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

	public Vector3 GetOffsetParticlePosition(BodyPart bodyPart, ParticleOffsetSpace offsetSpace, Vector3 offset, bool forceInvert = false)
	{
		Quaternion rootRotation = (Quaternion)this.GetRotation(BodyPart.root, false);
		Vector3 bodyPartPosition = (Vector3)this.GetBonePosition(bodyPart, forceInvert);
		Quaternion bodyPartRotation = (Quaternion)this.GetRotation(bodyPart, forceInvert);
		return this.UpdateParticlePosition(bodyPartPosition, rootRotation, bodyPartRotation, offsetSpace, offset);
	}

	public QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false)
	{
		if (this.animationDataSource == null)
		{
			UnityEngine.Debug.LogWarning("Missing baked animation data");
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

	public void DrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
		}
	}

	public void InvertHurtBoxes(bool isInverted)
	{
		this.state.invert = isInverted;
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<BoneControllerState>(this.state));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<BoneControllerState>(ref this.state);
		return true;
	}

	public IBoneTransform GetBoneTransform(BodyPart bone)
	{
		return new BoneTransform(bone, this);
	}

	public IBoneTransform GetBoneOffsetTransform(BodyPart bone, BodyPart secondaryBone, Fixed offset)
	{
		return new BoneOffsetTransform(bone, secondaryBone, offset, this);
	}

	private static int _Init_m__0(HurtBoxState a, HurtBoxState b)
	{
		return b.Priority() - a.Priority();
	}
}
