using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200057F RID: 1407
[Serializable]
public class CharacterData : ScriptableObject, IPreloadedGameAsset, IDefaultCharacterPhysicsDataOwner
{
	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06001F97 RID: 8087 RVA: 0x000A1494 File Offset: 0x0009F894
	public bool isRandom
	{
		get
		{
			return this.characterDefinition.isRandom;
		}
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06001F98 RID: 8088 RVA: 0x000A14A1 File Offset: 0x0009F8A1
	public string characterName
	{
		get
		{
			return this.characterDefinition.characterName;
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000A14AE File Offset: 0x0009F8AE
	public CharacterID characterID
	{
		get
		{
			return this.characterDefinition.characterID;
		}
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06001F9A RID: 8090 RVA: 0x000A14BB File Offset: 0x0009F8BB
	public bool isPartner
	{
		get
		{
			return this.characterDefinition.isPartner;
		}
	}

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000A14C8 File Offset: 0x0009F8C8
	// (set) Token: 0x06001F9C RID: 8092 RVA: 0x000A14D0 File Offset: 0x0009F8D0
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

	// Token: 0x06001F9D RID: 8093 RVA: 0x000A14D9 File Offset: 0x0009F8D9
	public GameObject GetRawPrefab(SkinData skin)
	{
		if (this.characterDefinition.isPartner)
		{
			return skin.partnerPrefab;
		}
		return skin.characterPrefab;
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x000A14F8 File Offset: 0x0009F8F8
	public GameObject GetCombinedPrefab(SkinData skin)
	{
		if (this.characterDefinition.isPartner)
		{
			return skin.combinedPartnerPrefab;
		}
		return skin.combinedPrefab;
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x000A1518 File Offset: 0x0009F918
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
		foreach (BodyPart bodyPart in dictionary.Keys)
		{
			HurtBox item = new HurtBox(bodyPart, dictionary[bodyPart], 0.17f, Vector3.zero, true);
			this.HurtBoxes.Add(item);
		}
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000A1624 File Offset: 0x0009FA24
	public void RegisterPreload(PreloadContext context)
	{
		if (!context.AlreadyChecked(this))
		{
			foreach (CharacterComponent characterComponent in this.components)
			{
				characterComponent.RegisterPreload(context);
			}
			foreach (CharacterMoveSetData characterMoveSetData in this.moveSets)
			{
				characterMoveSetData.RegisterPreload(context);
			}
		}
	}

	// Token: 0x040018FB RID: 6395
	public ShaderVariantFile shaderVariantFile = new ShaderVariantFile();

	// Token: 0x040018FC RID: 6396
	public Sprite smallPortrait;

	// Token: 0x040018FD RID: 6397
	public Sprite miniPortrait;

	// Token: 0x040018FE RID: 6398
	public Vector3 textureCameraOffset = new Vector3(0f, 0f, -3.5f);

	// Token: 0x040018FF RID: 6399
	public CompositeNodeFile overrideAIBehavior = new CompositeNodeFile();

	// Token: 0x04001900 RID: 6400
	public CharacterDefinition characterDefinition;

	// Token: 0x04001901 RID: 6401
	public Fixed airECBExtend = 0;

	// Token: 0x04001902 RID: 6402
	public bool useDashPivotState = true;

	// Token: 0x04001903 RID: 6403
	public int dashDanceAnimationFrame = 3;

	// Token: 0x04001904 RID: 6404
	public int landingDashAnimationFrame = 3;

	// Token: 0x04001905 RID: 6405
	public int dashPivotAnimationFrame = 1;

	// Token: 0x04001906 RID: 6406
	public Fixed dashPivotOffset = 0;

	// Token: 0x04001907 RID: 6407
	public Fixed runPivotJumpOffset = 0;

	// Token: 0x04001908 RID: 6408
	public int dashDashInputLeeway = 1;

	// Token: 0x04001909 RID: 6409
	public Fixed attackAssistJabRange = (Fixed)2.125;

	// Token: 0x0400190A RID: 6410
	public Fixed attackAssistUpTiltMinimumRange = (Fixed)1.5;

	// Token: 0x0400190B RID: 6411
	public Fixed attackAssistUpTiltHorizontalMaximum = (Fixed)2.5;

	// Token: 0x0400190C RID: 6412
	public Fixed attackAssistUpTiltHorizontalMinimum = (Fixed)0.0;

	// Token: 0x0400190D RID: 6413
	public Fixed attackAssistUpStrikeMinimumRange = (Fixed)1.5;

	// Token: 0x0400190E RID: 6414
	public Fixed attackAssistUpStrikeHorizontalMaximum = (Fixed)2.5;

	// Token: 0x0400190F RID: 6415
	public Fixed attackAssistUpStrikeHorizontalMinimum = (Fixed)0.0;

	// Token: 0x04001910 RID: 6416
	public Fixed attackAssistDStrikeMinimumRange = (Fixed)(-3.0);

	// Token: 0x04001911 RID: 6417
	public Fixed attackAssistDStrikeHorizontalMaximum = (Fixed)1.0;

	// Token: 0x04001912 RID: 6418
	public Fixed attackAssistDStrikeHorizontalMinimum = (Fixed)0.0;

	// Token: 0x04001913 RID: 6419
	public Fixed attackAssistDStrikeHorizontalOffset = (Fixed)0.0;

	// Token: 0x04001914 RID: 6420
	public Fixed attackAssistUpAirMinimumRange = (Fixed)1.0;

	// Token: 0x04001915 RID: 6421
	public Fixed attackAssistUpAirHorizontalMaximum = (Fixed)2.5;

	// Token: 0x04001916 RID: 6422
	public Fixed attackAssistUpAirHorizontalMinimum = (Fixed)0.0;

	// Token: 0x04001917 RID: 6423
	public Fixed attackAssistDairMinimumRange = (Fixed)(-1.5);

	// Token: 0x04001918 RID: 6424
	public Fixed attackAssistDairHorizontalMaximum = (Fixed)2.0;

	// Token: 0x04001919 RID: 6425
	public Fixed attackAssistDairHorizontalMinimum = (Fixed)0.0;

	// Token: 0x0400191A RID: 6426
	public Fixed attackAssistDairHorizontalOffset = (Fixed)0.0;

	// Token: 0x0400191B RID: 6427
	public Fixed attackAssistBairHorizontalOffset = (Fixed)0.0;

	// Token: 0x0400191C RID: 6428
	public Fixed attackAssistBairMinimumHeight = (Fixed)(-1.5);

	// Token: 0x0400191D RID: 6429
	[NonSerialized]
	public Fixed attackAssistVelocityOverlapMultiplier = (Fixed)0.375;

	// Token: 0x0400191E RID: 6430
	public Fixed attackAssistAerialBelowOverlapThreshold = (Fixed)1.2999999523162842;

	// Token: 0x0400191F RID: 6431
	public Fixed attackAssistAerialBelowOverlapOffset = (Fixed)0.0;

	// Token: 0x04001920 RID: 6432
	public Fixed attackAssistGroundNudgeMultiplier = (Fixed)2.5;

	// Token: 0x04001921 RID: 6433
	public Fixed attackAssistAirNudgeMultiplier = (Fixed)1.350000023841858;

	// Token: 0x04001922 RID: 6434
	public Fixed attackAssistGroundNudgeBase = (Fixed)3.0;

	// Token: 0x04001923 RID: 6435
	public Fixed attackAssistAirNudgeBase = (Fixed)1.25;

	// Token: 0x04001924 RID: 6436
	public Fixed attackAssistNudgeMinDistance = (Fixed)1.399999976158142;

	// Token: 0x04001925 RID: 6437
	public Fixed attackAssistNudgeMaxDistance = (Fixed)3.0;

	// Token: 0x04001926 RID: 6438
	public AudioData mockingVoiceLine;

	// Token: 0x04001927 RID: 6439
	public FixedRect shoveBounds = new FixedRect(-(Fixed)0.6000000238418579, (Fixed)0.800000011920929, 1, (Fixed)1.7999999523162842);

	// Token: 0x04001928 RID: 6440
	public bool reversesStance = true;

	// Token: 0x04001929 RID: 6441
	[SerializeField]
	private CharacterPhysicsData physics;

	// Token: 0x0400192A RID: 6442
	public CharacterShieldData shield;

	// Token: 0x0400192B RID: 6443
	public PlayerGUIComponent playerGUIComponent;

	// Token: 0x0400192C RID: 6444
	public CharacterGrabData grab;

	// Token: 0x0400192D RID: 6445
	public CharacterParticleData particles;

	// Token: 0x0400192E RID: 6446
	[FormerlySerializedAs("moves")]
	public CharacterMoveSetData[] moveSets = new CharacterMoveSetData[1];

	// Token: 0x0400192F RID: 6447
	public CharacterComponent[] components = new CharacterComponent[0];

	// Token: 0x04001930 RID: 6448
	public List<HurtBox> HurtBoxes = new List<HurtBox>();

	// Token: 0x04001931 RID: 6449
	public bool useLandingCameraShake;

	// Token: 0x04001932 RID: 6450
	public MoveCameraShakeData landingCameraShake;
}
