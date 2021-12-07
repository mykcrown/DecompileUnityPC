// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class UnityFileLink<T> : UnityFileLink where T : UnityEngine.Object
{
	[SerializeField]
	private UnityEngine.Object reference;

	[SerializeField]
	private string _FILE_PATH;

	public T obj
	{
		get
		{
			return (!(this.reference == null)) ? ((T)((object)this.reference)) : ((T)((object)null));
		}
	}

	public void RuntimeOverrideWithMemoryObject(T obj)
	{
		this.reference = obj;
	}
}
