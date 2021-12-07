// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileTools
{
	public static List<FileInfo> FindAllFilesInTreeAtRoot(string root)
	{
		List<FileInfo> list = new List<FileInfo>();
		Stack<string> stack = new Stack<string>();
		if (!Directory.Exists(root))
		{
			UnityEngine.Debug.LogErrorFormat("Root directory {0} does not exist.", new object[]
			{
				root
			});
			return list;
		}
		stack.Push(root);
		while (stack.Count > 0)
		{
			string text = stack.Pop();
			string[] directories;
			try
			{
				directories = Directory.GetDirectories(text);
			}
			catch (UnauthorizedAccessException)
			{
				UnityEngine.Debug.LogErrorFormat("Unauthorized access when attempting to get subdirectories of directory {0}", new object[]
				{
					text
				});
				continue;
			}
			catch (DirectoryNotFoundException)
			{
				UnityEngine.Debug.LogErrorFormat("Directory {0} not found when attempting to get subdirectories of it.", new object[]
				{
					text
				});
				continue;
			}
			string[] files;
			try
			{
				files = Directory.GetFiles(text);
			}
			catch (UnauthorizedAccessException)
			{
				UnityEngine.Debug.LogErrorFormat("Unauthorized access when attempting to get files of directory {0}", new object[]
				{
					text
				});
				continue;
			}
			catch (DirectoryNotFoundException)
			{
				UnityEngine.Debug.LogErrorFormat("Directory {0} not found when attempting to get files from it.", new object[]
				{
					text
				});
				continue;
			}
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				try
				{
					FileInfo item = new FileInfo(text2);
					list.Add(item);
				}
				catch (FileNotFoundException)
				{
					UnityEngine.Debug.LogErrorFormat("File {0} not found when attempting to get file info from it.", new object[]
					{
						text2
					});
				}
			}
			string[] array2 = directories;
			for (int j = 0; j < array2.Length; j++)
			{
				string item2 = array2[j];
				stack.Push(item2);
			}
		}
		return list;
	}

	public static List<FileInfo> FilterFileList(List<FileInfo> list, Func<FileInfo, bool> shouldKeepFn)
	{
		List<FileInfo> list2 = new List<FileInfo>();
		foreach (FileInfo current in list)
		{
			if (shouldKeepFn != null && shouldKeepFn(current))
			{
				list2.Add(current);
			}
		}
		return list2;
	}

	public static string AbsolutePathToAssetPath(string absolutePath)
	{
		return "Assets" + absolutePath.Replace(Path.DirectorySeparatorChar, '/').Substring(Application.dataPath.Length);
	}

	public static void GetEquipmentAndCharacterFromPath(string path, ref EquipmentTypes equipTypeOut, ref CharacterID charIdOut)
	{
		equipTypeOut = EquipmentTypes.NONE;
		charIdOut = CharacterID.None;
		FileTools.TryGetEquipmentTypeFromPath(path, ref equipTypeOut, Path.DirectorySeparatorChar);
		FileTools.TryGetCharacterIdFromPath(path, ref charIdOut, Path.DirectorySeparatorChar);
	}

	public static bool TryGetEquipmentTypeFromPath(string path, ref EquipmentTypes equipTypeOut, char dirSeparator = '/')
	{
		string[] array = path.Split(new char[]
		{
			dirSeparator
		});
		equipTypeOut = EquipmentTypes.NONE;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (FileTools.IsEquipmentTypeDirectoryName(array[i], ref equipTypeOut))
			{
				return true;
			}
		}
		return false;
	}

	public static bool TryGetCharacterIdFromPath(string path, ref CharacterID charIdOut, char dirSeparator = '/')
	{
		string[] array = path.Split(new char[]
		{
			dirSeparator
		});
		charIdOut = CharacterID.None;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (FileTools.IsCharacterNameDirectory(array[i], ref charIdOut))
			{
				return true;
			}
		}
		return false;
	}

	public static bool TryGetCharacterDefinitionFromPath(string path, ref CharacterDefinition charDefOut, char dirSeparator = '/')
	{
		string[] array = path.Split(new char[]
		{
			dirSeparator
		});
		charDefOut = null;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (FileTools.IsCharacterNameDirectory(array[i], ref charDefOut))
			{
				break;
			}
		}
		if (charDefOut != null && charDefOut.characterName == "AfiGalu")
		{
			string[] array2 = path.Split(new char[]
			{
				'/'
			});
			if (array2[array2.Length - 1].ToLower().Contains("galu"))
			{
				GameEnvironmentData gameEnvironmentData = GameEnvironmentData.Load();
				foreach (CharacterDefinition current in gameEnvironmentData.characters)
				{
					if (current.characterName == "GaluAfi")
					{
						charDefOut = current;
						break;
					}
				}
			}
		}
		return charDefOut != null;
	}

	public static bool IsEquipmentTypeDirectoryName(string name, ref EquipmentTypes equipTypeOut)
	{
		string str = name + "/";
		foreach (KeyValuePair<EquipmentTypes, string> current in ItemLoader.ResourceDirectories)
		{
			EquipmentTypes key = current.Key;
			string value = current.Value;
			if (str.EqualsIgnoreCase(value))
			{
				equipTypeOut = key;
				return true;
			}
		}
		return false;
	}

	public static bool IsCharacterNameDirectory(string name, ref CharacterDefinition charDefOut)
	{
		GameEnvironmentData gameEnvironmentData = GameEnvironmentData.Load();
		foreach (CharacterDefinition current in gameEnvironmentData.characters)
		{
			if (current.characterName.EqualsIgnoreCase(name))
			{
				charDefOut = current;
				return true;
			}
		}
		return false;
	}

	public static bool IsCharacterNameDirectory(string name, ref CharacterID charIdOut)
	{
		CharacterID[] values = EnumUtil.GetValues<CharacterID>();
		CharacterID[] array = values;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterID characterID = array[i];
			if (characterID.ToString().EqualsIgnoreCase(name))
			{
				charIdOut = characterID;
				return true;
			}
		}
		if (name.EqualsIgnoreCase("ezzie"))
		{
			charIdOut = CharacterID.CHARACTER_8;
			return true;
		}
		return false;
	}

	public static string CharacterIdToCharacterName(CharacterID characterId)
	{
		if (characterId != CharacterID.CHARACTER_8)
		{
			return characterId.ToString();
		}
		return "Ezzie";
	}
}
