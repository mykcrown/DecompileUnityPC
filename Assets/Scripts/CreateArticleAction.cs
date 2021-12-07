// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CreateArticleAction : IPreloadedGameAsset
{
	public int fireFrame;

	public BodyPart fireBodyPart;

	public int fireAngle;

	[FormerlySerializedAs("projectile")]
	public ArticleData data;

	public float speed;

	public MovementType movementType;

	public Vector2 inheritSpeedAmount;

	public Vector3 worldOffset;

	public void RegisterPreload(PreloadContext context)
	{
		if (this.data != null)
		{
			this.data.RegisterPreload(context);
		}
	}
}
