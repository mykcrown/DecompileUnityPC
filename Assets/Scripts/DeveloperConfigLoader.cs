// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class DeveloperConfigLoader
{
	public DeveloperConfig config;

	public static readonly string DEV_CONFIG_PATH = "Assets/Editor/Config/";

	public static readonly string DEV_MULTIPLAYER_CONFIG_PATH = "Editor/Config/";

	public DeveloperConfigLoader()
	{
		if (this.config == null)
		{
			this.config = new DeveloperConfig();
		}
	}

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
				UnityEngine.Debug.LogError("failure");
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
					UnityEngine.Debug.LogWarning("Unknown user config option " + text2);
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
}
