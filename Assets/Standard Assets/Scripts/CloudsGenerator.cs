// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

public class CloudsGenerator : MonoBehaviour
{
	public float Density = 2f;

	public GameObject[] Prefabs = new GameObject[0];

	public Vector3 StartPos = Vector3.zero;

	public Vector3 EndPos = new Vector3(100f, 0f, 100f);

	public Texture2D HeightMap;

	private void Start()
	{
		int num = 0;
		Vector3 startPos = this.StartPos;
		while (startPos.z < this.EndPos.z)
		{
			startPos.z += UnityEngine.Random.Range(this.Density / 2f, this.Density * 1.5f);
			startPos.x = this.StartPos.x;
			while (startPos.x < this.EndPos.x)
			{
				startPos.x += UnityEngine.Random.Range(this.Density / 5f, this.Density * 5f);
				int x = (int)((float)this.HeightMap.width * startPos.x / (this.EndPos - this.StartPos).x);
				int y = (int)((float)this.HeightMap.height * startPos.z / (this.EndPos - this.StartPos).x);
				if (this.HeightMap.GetPixel(x, y).g >= 0.75f)
				{
					float num2 = this.HeightMap.GetPixel(x, y).g * 46f - 30f;
					float num3 = this.HeightMap.GetPixel(x, y).b * 40f;
					num2 *= 5f;
					num3 *= 5f;
					startPos.y = 150f;
					int num4 = UnityEngine.Random.Range(0, this.Prefabs.Length);
					num++;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[num4], startPos, Quaternion.identity);
					gameObject.transform.localScale = new Vector3(num3, 10f, num3);
					gameObject.transform.parent = base.transform;
				}
			}
		}
		UnityEngine.Debug.Log(num);
	}

	private void Update()
	{
		base.transform.position += Vector3.back * Time.deltaTime * 100f;
	}
}
