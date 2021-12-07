using System;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000899 RID: 2201
public class DeveloperConfigLoader
{
	// Token: 0x06003743 RID: 14147 RVA: 0x00101AA4 File Offset: 0x000FFEA4
	public DeveloperConfigLoader()
	{
		if (this.config == null)
		{
			this.config = new DeveloperConfig();
		}
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x00101AC4 File Offset: 0x000FFEC4
	private DeveloperConfig getConfig(string GamePath)
	{
		StreamReader streamReader;
		try
		{
			streamReader = new StreamReader(GamePath);
		}
		catch (Exception)
		{
			return null;
		}
		DeveloperConfig developerConfig = new DeveloperConfig();
		Type typeFromHandle = typeof(DeveloperConfig);
		int num = 0;
		string text;
		while ((text = streamReader.ReadLine()) != string.Empty)
		{
			num++;
			if (text == null)
			{
				break;
			}
			if (num > 1000)
			{
				Debug.LogError("failure");
				break;
			}
			string[] array = text.Split(new char[]
			{
				'='
			});
			string text2 = array[0].Trim();
			FieldInfo field = typeFromHandle.GetField(text2);
			if (field == null)
			{
				if (text2[0] != '#')
				{
					Debug.LogWarning("Unknown user config option " + text2);
				}
			}
			else
			{
				string value = array[1].Trim();
				if (field.FieldType == typeof(bool))
				{
					field.SetValue(developerConfig, ObjectUtil.Convert<bool>(value));
				}
				else if (field.FieldType == typeof(int))
				{
					field.SetValue(developerConfig, ObjectUtil.Convert<int>(value));
				}
				else if (field.FieldType == typeof(float))
				{
					field.SetValue(developerConfig, ObjectUtil.Convert<float>(value));
				}
				else
				{
					field.SetValue(developerConfig, value);
				}
			}
		}
		streamReader.Close();
		return developerConfig;
	}

	// Token: 0x04002581 RID: 9601
	public DeveloperConfig config;

	// Token: 0x04002582 RID: 9602
	public static readonly string DEV_CONFIG_PATH = "Assets/Editor/Config/";

	// Token: 0x04002583 RID: 9603
	public static readonly string DEV_MULTIPLAYER_CONFIG_PATH = "Editor/Config/";
}
