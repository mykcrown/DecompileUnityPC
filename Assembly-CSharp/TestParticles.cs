using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class TestParticles : MonoBehaviour
{
	// Token: 0x06000065 RID: 101 RVA: 0x00005E7C File Offset: 0x0000427C
	private void Start()
	{
		if (this.m_PrefabListFire.Length > 0 || this.m_PrefabListWind.Length > 0 || this.m_PrefabListWater.Length > 0 || this.m_PrefabListEarth.Length > 0 || this.m_PrefabListIce.Length > 0 || this.m_PrefabListThunder.Length > 0 || this.m_PrefabListLight.Length > 0 || this.m_PrefabListDarkness.Length > 0)
		{
			this.m_CurrentElementIndex = 0;
			this.m_CurrentParticleIndex = 0;
			this.ShowParticle();
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00005F10 File Offset: 0x00004310
	private void Update()
	{
		if (this.m_CurrentElementIndex != -1 && this.m_CurrentParticleIndex != -1)
		{
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				this.m_CurrentElementIndex++;
				this.m_CurrentParticleIndex = 0;
				this.ShowParticle();
			}
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				this.m_CurrentElementIndex--;
				this.m_CurrentParticleIndex = 0;
				this.ShowParticle();
			}
			else if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				this.m_CurrentParticleIndex--;
				this.ShowParticle();
			}
			else if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				this.m_CurrentParticleIndex++;
				this.ShowParticle();
			}
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00005FE0 File Offset: 0x000043E0
	private void OnGUI()
	{
		GUI.Window(1, new Rect((float)(Screen.width - 260), 5f, 250f, 80f), new GUI.WindowFunction(this.InfoWindow), "Info");
		GUI.Window(2, new Rect((float)(Screen.width - 260), (float)(Screen.height - 85), 250f, 80f), new GUI.WindowFunction(this.ParticleInformationWindow), "Help");
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00006064 File Offset: 0x00004464
	private void ShowParticle()
	{
		if (this.m_CurrentElementIndex > 7)
		{
			this.m_CurrentElementIndex = 0;
		}
		else if (this.m_CurrentElementIndex < 0)
		{
			this.m_CurrentElementIndex = 7;
		}
		if (this.m_CurrentElementIndex == 0)
		{
			this.m_CurrentElementList = this.m_PrefabListFire;
			this.m_ElementName = "FIRE";
		}
		else if (this.m_CurrentElementIndex == 1)
		{
			this.m_CurrentElementList = this.m_PrefabListWater;
			this.m_ElementName = "WATER";
		}
		else if (this.m_CurrentElementIndex == 2)
		{
			this.m_CurrentElementList = this.m_PrefabListWind;
			this.m_ElementName = "WIND";
		}
		else if (this.m_CurrentElementIndex == 3)
		{
			this.m_CurrentElementList = this.m_PrefabListEarth;
			this.m_ElementName = "EARTH";
		}
		else if (this.m_CurrentElementIndex == 4)
		{
			this.m_CurrentElementList = this.m_PrefabListThunder;
			this.m_ElementName = "THUNDER";
		}
		else if (this.m_CurrentElementIndex == 5)
		{
			this.m_CurrentElementList = this.m_PrefabListIce;
			this.m_ElementName = "ICE";
		}
		else if (this.m_CurrentElementIndex == 6)
		{
			this.m_CurrentElementList = this.m_PrefabListLight;
			this.m_ElementName = "LIGHT";
		}
		else if (this.m_CurrentElementIndex == 7)
		{
			this.m_CurrentElementList = this.m_PrefabListDarkness;
			this.m_ElementName = "DARKNESS";
		}
		if (this.m_CurrentParticleIndex >= this.m_CurrentElementList.Length)
		{
			this.m_CurrentParticleIndex = 0;
		}
		else if (this.m_CurrentParticleIndex < 0)
		{
			this.m_CurrentParticleIndex = this.m_CurrentElementList.Length - 1;
		}
		this.m_ParticleName = this.m_CurrentElementList[this.m_CurrentParticleIndex].name;
		if (this.m_CurrentParticle != null)
		{
			UnityEngine.Object.Destroy(this.m_CurrentParticle);
		}
		this.m_CurrentParticle = UnityEngine.Object.Instantiate<GameObject>(this.m_CurrentElementList[this.m_CurrentParticleIndex]);
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00006260 File Offset: 0x00004660
	private void ParticleInformationWindow(int id)
	{
		GUI.Label(new Rect(12f, 25f, 280f, 20f), string.Concat(new object[]
		{
			"Up/Down: ",
			this.m_ElementName,
			" (",
			this.m_CurrentParticleIndex + 1,
			"/",
			this.m_CurrentElementList.Length,
			")"
		}));
		GUI.Label(new Rect(12f, 50f, 280f, 20f), "Left/Right: " + this.m_ParticleName.ToUpper());
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00006314 File Offset: 0x00004714
	private void InfoWindow(int id)
	{
		GUI.Label(new Rect(15f, 25f, 240f, 20f), "Elementals 1.1.1");
		GUI.Label(new Rect(15f, 50f, 240f, 20f), "www.ge-team.com/pages");
	}

	// Token: 0x040000B6 RID: 182
	public GameObject[] m_PrefabListFire;

	// Token: 0x040000B7 RID: 183
	public GameObject[] m_PrefabListWind;

	// Token: 0x040000B8 RID: 184
	public GameObject[] m_PrefabListWater;

	// Token: 0x040000B9 RID: 185
	public GameObject[] m_PrefabListEarth;

	// Token: 0x040000BA RID: 186
	public GameObject[] m_PrefabListIce;

	// Token: 0x040000BB RID: 187
	public GameObject[] m_PrefabListThunder;

	// Token: 0x040000BC RID: 188
	public GameObject[] m_PrefabListLight;

	// Token: 0x040000BD RID: 189
	public GameObject[] m_PrefabListDarkness;

	// Token: 0x040000BE RID: 190
	private int m_CurrentElementIndex = -1;

	// Token: 0x040000BF RID: 191
	private int m_CurrentParticleIndex = -1;

	// Token: 0x040000C0 RID: 192
	private string m_ElementName = string.Empty;

	// Token: 0x040000C1 RID: 193
	private string m_ParticleName = string.Empty;

	// Token: 0x040000C2 RID: 194
	private GameObject[] m_CurrentElementList;

	// Token: 0x040000C3 RID: 195
	private GameObject m_CurrentParticle;
}
