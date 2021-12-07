using System;
using InControl;
using UnityEngine;

namespace BindingsExample
{
	// Token: 0x02000040 RID: 64
	public class BindingsExample : MonoBehaviour
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000F6BB File Offset: 0x0000DABB
		private void OnEnable()
		{
			this.playerActions = PlayerActions.CreateWithDefaultBindings();
			this.LoadBindings();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F6CE File Offset: 0x0000DACE
		private void OnDisable()
		{
			this.playerActions.Destroy();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F6DB File Offset: 0x0000DADB
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000F6EC File Offset: 0x0000DAEC
		private void Update()
		{
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.playerActions.Move.X, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.playerActions.Move.Y, Space.World);
			Color a = (!this.playerActions.Fire.IsPressed) ? Color.white : Color.red;
			Color b = (!this.playerActions.Jump.IsPressed) ? Color.white : Color.green;
			this.cachedRenderer.material.color = Color.Lerp(a, b, 0.5f);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000F7B9 File Offset: 0x0000DBB9
		private void SaveBindings()
		{
			this.saveData = this.playerActions.Save();
			PlayerPrefs.SetString("Bindings", this.saveData);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000F7DC File Offset: 0x0000DBDC
		private void LoadBindings()
		{
			if (PlayerPrefs.HasKey("Bindings"))
			{
				this.saveData = PlayerPrefs.GetString("Bindings");
				this.playerActions.Load(this.saveData);
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000F80E File Offset: 0x0000DC0E
		private void OnApplicationQuit()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000F818 File Offset: 0x0000DC18
		private void OnGUI()
		{
			float num = 10f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Input Type: " + this.playerActions.LastInputType);
			num += 22f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Device Class: " + this.playerActions.LastDeviceClass);
			num += 22f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), "Last Device Style: " + this.playerActions.LastDeviceStyle);
			num += 22f;
			int count = this.playerActions.Actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.playerActions.Actions[i];
				string text = playerAction.Name;
				if (playerAction.IsListeningForBinding)
				{
					text += " (Listening)";
				}
				text = text + " = " + playerAction.Value;
				GUI.Label(new Rect(10f, num, 500f, num + 22f), text);
				num += 22f;
				int count2 = playerAction.Bindings.Count;
				for (int j = 0; j < count2; j++)
				{
					BindingSource bindingSource = playerAction.Bindings[j];
					GUI.Label(new Rect(75f, num, 300f, num + 22f), bindingSource.DeviceName + ": " + bindingSource.Name);
					if (GUI.Button(new Rect(20f, num + 3f, 20f, 17f), "-"))
					{
						playerAction.RemoveBinding(bindingSource);
					}
					if (GUI.Button(new Rect(45f, num + 3f, 20f, 17f), "+"))
					{
						playerAction.ListenForBindingReplacing(bindingSource);
					}
					num += 22f;
				}
				if (GUI.Button(new Rect(20f, num + 3f, 20f, 17f), "+"))
				{
					playerAction.ListenForBinding();
				}
				if (GUI.Button(new Rect(50f, num + 3f, 50f, 17f), "Reset"))
				{
					playerAction.ResetBindings();
				}
				num += 25f;
			}
			if (GUI.Button(new Rect(20f, num + 3f, 50f, 22f), "Load"))
			{
				this.LoadBindings();
			}
			if (GUI.Button(new Rect(80f, num + 3f, 50f, 22f), "Save"))
			{
				this.SaveBindings();
			}
			if (GUI.Button(new Rect(140f, num + 3f, 50f, 22f), "Reset"))
			{
				this.playerActions.Reset();
			}
		}

		// Token: 0x040001AC RID: 428
		private Renderer cachedRenderer;

		// Token: 0x040001AD RID: 429
		private PlayerActions playerActions;

		// Token: 0x040001AE RID: 430
		private string saveData;
	}
}
