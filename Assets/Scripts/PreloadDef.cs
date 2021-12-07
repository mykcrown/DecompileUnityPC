// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PreloadDef : IEquatable<PreloadDef>
{
	public PreloadType type
	{
		get;
		private set;
	}

	public GameObject obj
	{
		get;
		private set;
	}

	public PreloadDef(GameObject obj, PreloadType type = PreloadType.EFFECT)
	{
		this.obj = obj;
		this.type = type;
	}

	public bool Equals(PreloadDef compare)
	{
		return compare != null && compare.type == this.type && compare.obj == this.obj;
	}

	public override bool Equals(object compare)
	{
		return compare is PreloadDef && this.Equals(compare as PreloadDef);
	}

	public override int GetHashCode()
	{
		return HashCode.Of<PreloadType>(this.type).And<GameObject>(this.obj);
	}
}
