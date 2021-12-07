// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class DummyPositionOwner : IPositionOwner
{
	public Vector3F Position
	{
		get;
		private set;
	}

	public Vector3F Center
	{
		get;
		private set;
	}

	public DummyPositionOwner(Vector3F position)
	{
		this.Center = position;
		this.Position = position;
	}
}
