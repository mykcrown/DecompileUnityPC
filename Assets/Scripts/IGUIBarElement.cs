// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IGUIBarElement : ITickable
{
	bool Visible
	{
		get;
	}

	Transform transform
	{
		get;
	}

	void setPosition(float x, float y);
}
