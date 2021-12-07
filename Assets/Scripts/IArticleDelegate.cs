// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IArticleDelegate
{
	ArticleData Data
	{
		get;
	}

	ArticleModel Model
	{
		get;
	}

	Vector3F Position
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	IEvents getEvents();

	bool PerformBoundCast(AbsoluteDirection boundPoint, Fixed castDist, Vector2F castDirection, int castMask, out RaycastHitData hit);
}
