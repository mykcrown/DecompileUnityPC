// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.Serialization;

[Serializable]
public class AnnouncementData
{
	public bool isEmpty;

	public string subtitle;

	[FormerlySerializedAs("_sound")]
	public AudioData sound;

	public int weight;

	public Priority priority = Priority.Low;
}
