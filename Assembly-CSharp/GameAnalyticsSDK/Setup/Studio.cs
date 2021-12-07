using System;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Setup
{
	// Token: 0x02000036 RID: 54
	public class Studio
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x0000E1E7 File Offset: 0x0000C5E7
		public Studio(string name, string id, List<Game> games)
		{
			this.Name = name;
			this.ID = id;
			this.Games = games;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000E204 File Offset: 0x0000C604
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0000E20C File Offset: 0x0000C60C
		public string Name { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000E215 File Offset: 0x0000C615
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0000E21D File Offset: 0x0000C61D
		public string ID { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000E226 File Offset: 0x0000C626
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000E22E File Offset: 0x0000C62E
		public List<Game> Games { get; private set; }

		// Token: 0x060001C7 RID: 455 RVA: 0x0000E238 File Offset: 0x0000C638
		public static string[] GetStudioNames(List<Studio> studios, bool addFirstEmpty = true)
		{
			if (studios == null)
			{
				return new string[]
				{
					"-"
				};
			}
			if (addFirstEmpty)
			{
				string[] array = new string[studios.Count + 1];
				array[0] = "-";
				string text = string.Empty;
				for (int i = 0; i < studios.Count; i++)
				{
					array[i + 1] = studios[i].Name + text;
					text += " ";
				}
				return array;
			}
			string[] array2 = new string[studios.Count];
			string text2 = string.Empty;
			for (int j = 0; j < studios.Count; j++)
			{
				array2[j] = studios[j].Name + text2;
				text2 += " ";
			}
			return array2;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000E310 File Offset: 0x0000C710
		public static string[] GetGameNames(int index, List<Studio> studios)
		{
			if (studios == null || studios[index].Games == null)
			{
				return new string[]
				{
					"-"
				};
			}
			string[] array = new string[studios[index].Games.Count + 1];
			array[0] = "-";
			string text = string.Empty;
			for (int i = 0; i < studios[index].Games.Count; i++)
			{
				array[i + 1] = studios[index].Games[i].Name + text;
				text += " ";
			}
			return array;
		}
	}
}
