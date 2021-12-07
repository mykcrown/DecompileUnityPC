// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
	private Image image;

	private Color color;

	private void Start()
	{
		this.image = base.GetComponent<Image>();
		this.color = default(Color);
		this.color.b = 1f;
		this.color.a = 1f;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y) && !Input.GetKey(KeyCode.Y))
		{
			UnityEngine.Debug.Log("Input mismatch!!");
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.image.color = this.color;
		}
	}
}
