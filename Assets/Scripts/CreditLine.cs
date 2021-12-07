// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class CreditLine : MonoBehaviour
{
	public TextMeshPro nameText;

	public TextMeshPro titleText;

	public void Set(string name, string title)
	{
		this.nameText.text = name;
		this.titleText.text = title;
	}
}
