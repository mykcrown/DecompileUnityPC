using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000ABC RID: 2748
public static class FileTools
{
	// Token: 0x0600507B RID: 20603 RVA: 0x0014FC70 File Offset: 0x0014E070
	public static List<FileInfo> FindAllFilesInTreeAtRoot(string root)
	{
		List<FileInfo> list = new List<FileInfo>();
		Stack<string> stack = new Stack<string>();
		if (!Directory.Exists(root))
		{
			Debug.LogErrorFormat("Root directory {0} does not exist.", new object[]
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
				Debug.LogErrorFormat("Unauthorized access when attempting to get subdirectories of directory {0}", new object[]
				{
					text
				});
				continue;
			}
			catch (DirectoryNotFoundException)
			{
				Debug.LogErrorFormat("Directory {0} not found when attempting to get subdirectories of it.", new object[]
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
				Debug.LogErrorFormat("Unauthorized access when attempting to get files of directory {0}", new object[]
				{
					text
				});
				continue;
			}
			catch (DirectoryNotFoundException)
			{
				Debug.LogErrorFormat("Directory {0} not found when attempting to get files from it.", new object[]
				{
					text
				});
				continue;
			}
			foreach (string text2 in files)
			{
				try
				{
					FileInfo item = new FileInfo(text2);
					list.Add(item);
				}
				catch (FileNotFoundException)
				{
					Debug.LogErrorFormat("File {0} not found when attempting to get file info from it.", new object[]
					{
						text2
					});
				}
			}
			foreach (string item2 in directories)
			{
				stack.Push(item2);
			}
		}
		return list;
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x0014FE0C File Offset: 0x0014E20C
	public static List<FileInfo> FilterFileList(List<FileInfo> list, Func<FileInfo, bool> shouldKeepFn)
	{
		List<FileInfo> list2 = new List<FileInfo>();
		foreach (FileInfo fileInfo in list)
		{
			if (shouldKeepFn != null && shouldKeepFn(fileInfo))
			{
				list2.Add(fileInfo);
			}
		}
		return list2;
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x0014FE7C File Offset: 0x0014E27C
	public static string AbsolutePathToAssetPath(string absolutePath)
	{
		return "Assets" + absolutePath.Replace(Path.DirectorySeparatorChar, '/').Substring(Application.dataPath.Length);
	}

	// Token: 0x0600507E RID: 20606 RVA: 0x0014FEA4 File Offset: 0x0014E2A4
	public static void GetEquipmentAndCharacterFromPath(string path, ref EquipmentTypes equipTypeOut, ref CharacterID charIdOut)
	{
		equipTypeOut = EquipmentTypes.NONE;
		charIdOut = CharacterID.None;
		FileTools.TryGetEquipmentTypeFromPath(path, ref equipTypeOut, Path.DirectorySeparatorChar);
		FileTools.TryGetCharacterIdFromPath(path, ref charIdOut, Path.DirectorySeparatorChar);
	}

	// Token: 0x0600507F RID: 20607 RVA: 0x0014FEC8 File Offset: 0x0014E2C8
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

	// Token: 0x06005080 RID: 20608 RVA: 0x0014FF10 File Offset: 0x0014E310
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

	// Token: 0x06005081 RID: 20609 RVA: 0x0014FF58 File Offset: 0x0014E358
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
				foreach (CharacterDefinition characterDefinition in gameEnvironmentData.characters)
				{
					if (characterDefinition.characterName == "GaluAfi")
					{
						charDefOut = characterDefinition;
						break;
					}
				}
			}
		}
		return charDefOut != null;
	}

	// Token: 0x06005082 RID: 20610 RVA: 0x0015006C File Offset: 0x0014E46C
	public static bool IsEquipmentTypeDirectoryName(string name, ref EquipmentTypes equipTypeOut)
	{
		string str = name + "/";
		foreach (KeyValuePair<EquipmentTypes, string> keyValuePair in ItemLoader.ResourceDirectories)
		{
			EquipmentTypes key = keyValuePair.Key;
			string value = keyValuePair.Value;
			if (str.EqualsIgnoreCase(value))
			{
				equipTypeOut = key;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005083 RID: 20611 RVA: 0x001500FC File Offset: 0x0014E4FC
	public static bool IsCharacterNameDirectory(string name, ref CharacterDefinition charDefOut)
	{
		GameEnvironmentData gameEnvironmentData = GameEnvironmentData.Load();
		foreach (CharacterDefinition characterDefinition in gameEnvironmentData.characters)
		{
			if (characterDefinition.characterName.EqualsIgnoreCase(name))
			{
				charDefOut = characterDefinition;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005084 RID: 20612 RVA: 0x00150178 File Offset: 0x0014E578
	public static bool IsCharacterNameDirectory(string name, ref CharacterID charIdOut)
	{
		CharacterID[] values = EnumUtil.GetValues<CharacterID>();
		foreach (CharacterID characterID in values)
		{
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

	// Token: 0x06005085 RID: 20613 RVA: 0x001501D8 File Offset: 0x0014E5D8
	public static string CharacterIdToCharacterName(CharacterID characterId)
	{
		if (characterId != CharacterID.CHARACTER_8)
		{
			return characterId.ToString();
		}
		return "Ezzie";
	}
}
