// Decompile from assembly: Assembly-CSharp.dll

using System;

[Flags]
public enum DebugDrawChannel
{
	None = 0,
	Physics = 1,
	HitBoxes = 2,
	HurtBoxes = 4,
	Bounds = 8,
	Camera = 16,
	Input = 32,
	Impact = 64,
	Grid = 128,
	All = -1
}
