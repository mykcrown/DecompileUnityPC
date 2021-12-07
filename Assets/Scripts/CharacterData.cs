// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterData : ScriptableObject, IPreloadedGameAsset, IDefaultCharacterPhysicsDataOwner
{
	public ShaderVariantFile shaderVariantFile = new ShaderVariantFile();

	public Sprite smallPortrait;

	public Sprite miniPortrait;

	public Vector3 textureCameraOffset = new Vector3(0f, 0f, -3.5f);

	public CompositeNodeFile overrideAIBehavior = new CompositeNodeFile();

	public CharacterDefinition characterDefinition;

	public Fixed airECBExtend = 0;

	public bool useDashPivotState = true;

	public int dashDanceAnimationFrame = 3;

	public int landingDashAnimationFrame = 3;

	public int dashPivotAnimationFrame = 1;

	public Fixed dashPivotOffset = 0;

	public Fixed runPivotJumpOffset = 0;

	public int dashDashInputLeeway = 1;

	public Fixed attackAssistJabRange = (Fixed)2.125;

	public Fixed attackAssistUpTiltMinimumRange = (Fixed)1.5;

	public Fixed attackAssistUpTiltHorizontalMaximum = (Fixed)2.5;

	public Fixed attackAssistUpTiltHorizontalMinimum = (Fixed)0.0;

	public Fixed attackAssistUpStrikeMinimumRange = (Fixed)1.5;

	public Fixed attackAssistUpStrikeHorizontalMaximum = (Fixed)2.5;

	public Fixed attackAssistUpStrikeHorizontalMinimum = (Fixed)0.0;

	public Fixed attackAssistDStrikeMinimumRange = (Fixed)(-3.0);

	public Fixed attackAssistDStrikeHorizontalMaximum = (Fixed)1.0;

	public Fixed attackAssistDStrikeHorizontalMinimum = (Fixed)0.0;

	public Fixed attackAssistDStrikeHorizontalOffset = (Fixed)0.0;

	public Fixed attackAssistUpAirMinimumRange = (Fixed)1.0;

	public Fixed attackAssistUpAirHorizontalMaximum = (Fixed)2.5;

	public Fixed attackAssistUpAirHorizontalMinimum = (Fixed)0.0;

	public Fixed attackAssistDairMinimumRange = (Fixed)(-1.5);

	public Fixed attackAssistDairHorizontalMaximum = (Fixed)2.0;

	public Fixed attackAssistDairHorizontalMinimum = (Fixed)0.0;

	public Fixed attackAssistDairHorizontalOffset = (Fixed)0.0;

	public Fixed attackAssistBairHorizontalOffset = (Fixed)0.0;

	public Fixed attackAssistBairMinimumHeight = (Fixed)(-1.5);

	[NonSerialized]
	public Fixed attackAssistVelocityOverlapMultiplier = (Fixed)0.375;

	public Fixed attackAssistAerialBelowOverlapThreshold = (Fixed)1.2999999523162842;

	public Fixed attackAssistAerialBelowOverlapOffset = (Fixed)0.0;

	public Fixed attackAssistGroundNudgeMultiplier = (Fixed)2.5;

	public Fixed attackAssistAirNudgeMultiplier = (Fixed)1.3500000238418579;

	public Fixed attackAssistGroundNudgeBase = (Fixed)3.0;

	public Fixed attackAssistAirNudgeBase = (Fixed)1.25;

	public Fixed attackAssistNudgeMinDistance = (Fixed)1.3999999761581421;

	public Fixed attackAssistNudgeMaxDistance = (Fixed)3.0;

	public AudioData mockingVoiceLine;

	public FixedRect shoveBounds = new FixedRect(-(Fixed)0.60000002384185791, (Fixed)0.800000011920929, 1, (Fixed)1.7999999523162842);

	public bool reversesStance = true;

	[SerializeField]
	private CharacterPhysicsData physics;

	public CharacterShieldData shield;

	public PlayerGUIComponent playerGUIComponent;

	public CharacterGrabData grab;

	public CharacterParticleData particles;

	[FormerlySerializedAs("moves")]
	public CharacterMoveSetData[] moveSets = new CharacterMoveSetData[1];

	public CharacterComponent[] components = new CharacterComponent[0];

	public List<HurtBox> HurtBoxes = new List<HurtBox>();

	public bool useLandingCameraShake;

	public MoveCameraShakeData landingCameraShake;

	CharacterPhysicsData IDefaultCharacterPhysicsDataOwner.DefaultPhysicsData
	{
		get
		{
			return this.physics;
		}
		set
		{
			this.physics = value;
		}
	}

	public bool isRandom
	{
		get
		{
			return this.characterDefinition.isRandom;
		}
	}

	public string characterName
	{
		get
		{
			return this.characterDefinition.characterName;
		}
	}

	public CharacterID characterID
	{
		get
		{
			return this.characterDefinition.characterID;
		}
	}

	public bool isPartner
	{
		get
		{
			return this.characterDefinition.isPartner;
		}
	}

	public GameObject GetRawPrefab(SkinData skin)
	{
		if (this.characterDefinition.isPartner)
		{
			return skin.partnerPrefab;
		}
		return skin.characterPrefab;
	}

	public GameObject GetCombinedPrefab(SkinData skin)
	{
		if (this.characterDefinition.isPartner)
		{
			return skin.combinedPartnerPrefab;
		}
		return skin.combinedPrefab;
	}

	public void AutoConfigureHurtBoxes()
	{
		Dictionary<BodyPart, BodyPart> dictionary = new Dictionary<BodyPart, BodyPart>
		{
			{
				BodyPart.head,
				BodyPart.upperTorso
			},
			{
				BodyPart.upperTorso,
				BodyPart.lowerTorso
			},
			{
				BodyPart.leftUpperArm,
				BodyPart.leftForearm
			},
			{
				BodyPart.rightUpperArm,
				BodyPart.rightForearm
			},
			{
				BodyPart.leftForearm,
				BodyPart.leftHand
			},
			{
				BodyPart.rightForearm,
				BodyPart.rightHand
			},
			{
				BodyPart.leftHand,
				BodyPart.none
			},
			{
				BodyPart.rightHand,
				BodyPart.none
			},
			{
				BodyPart.leftThigh,
				BodyPart.leftCalf
			},
			{
				BodyPart.rightThigh,
				BodyPart.rightCalf
			},
			{
				BodyPart.leftCalf,
				BodyPart.leftFoot
			},
			{
				BodyPart.rightCalf,
				BodyPart.rightFoot
			},
			{
				BodyPart.leftFoot,
				BodyPart.none
			},
			{
				BodyPart.rightFoot,
				BodyPart.none
			}
		};
		this.HurtBoxes.Clear();
		foreach (BodyPart current in dictionary.Keys)
		{
			HurtBox item = new HurtBox(current, dictionary[current], 0.17f, Vector3.zero, true);
			this.HurtBoxes.Add(item);
		}
	}

	public void RegisterPreload(PreloadContext context)
	{
		if (!context.AlreadyChecked(this))
		{
			CharacterComponent[] array = this.components;
			for (int i = 0; i < array.Length; i++)
			{
				CharacterComponent characterComponent = array[i];
				characterComponent.RegisterPreload(context);
			}
			CharacterMoveSetData[] array2 = this.moveSets;
			for (int j = 0; j < array2.Length; j++)
			{
				CharacterMoveSetData characterMoveSetData = array2[j];
				characterMoveSetData.RegisterPreload(context);
			}
		}
	}
}
