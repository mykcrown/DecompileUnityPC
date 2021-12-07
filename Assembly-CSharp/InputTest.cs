using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020006B1 RID: 1713
public class InputTest : MonoBehaviour
{
	// Token: 0x06002ABA RID: 10938 RVA: 0x000E1648 File Offset: 0x000DFA48
	private void Start()
	{
		this.image = base.GetComponent<Image>();
		this.color = default(Color);
		this.color.b = 1f;
		this.color.a = 1f;
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000E1690 File Offset: 0x000DFA90
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y) && !Input.GetKey(KeyCode.Y))
		{
			Debug.Log("Input mismatch!!");
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			this.image.color = this.color;
		}
	}

	// Token: 0x04001EBD RID: 7869
	private Image image;

	// Token: 0x04001EBE RID: 7870
	private Color color;
}
