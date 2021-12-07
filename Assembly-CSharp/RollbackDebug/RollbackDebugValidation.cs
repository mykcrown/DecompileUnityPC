using System;
using System.Reflection;
using UnityEngine;

namespace RollbackDebug
{
	// Token: 0x02000858 RID: 2136
	public class RollbackDebugValidation
	{
		// Token: 0x06003554 RID: 13652 RVA: 0x000FCEBA File Offset: 0x000FB2BA
		public void Initialize(bool enabled)
		{
			this.enabled = enabled;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x000FCEC3 File Offset: 0x000FB2C3
		public void StartupValidation()
		{
			if (!this.enabled)
			{
				return;
			}
			RollbackDebugValidation.detectRollbackStateCaching();
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000FCED8 File Offset: 0x000FB2D8
		private static void detectRollbackStateCaching()
		{
			Type[] types = typeof(RollbackDebugValidation).Assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!typeof(IRollbackStateOwner).IsAssignableFrom(type) && !typeof(RollbackState).IsAssignableFrom(type))
				{
					if (!typeof(ScriptableObject).IsAssignableFrom(type))
					{
						if (!type.IsInterface)
						{
							if (!type.Name.StartsWith("<"))
							{
								foreach (FieldInfo memberInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
								{
									RollbackDebugValidation.evaluateType(type, memberInfo);
								}
								foreach (PropertyInfo memberInfo2 in type.GetProperties())
								{
									RollbackDebugValidation.evaluateType(type, memberInfo2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000FCFEC File Offset: 0x000FB3EC
		private static bool evaluateType(Type classType, MemberInfo memberInfo)
		{
			Type type = (!(memberInfo is PropertyInfo)) ? (memberInfo as FieldInfo).FieldType : (memberInfo as PropertyInfo).PropertyType;
			if (typeof(RollbackState).IsAssignableFrom(classType) && memberInfo.GetCustomAttributes(typeof(AllowCachedState), true).Length == 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Detected RollbackState member ",
					memberInfo.Name,
					"(",
					type,
					") in non-rollback-state-owner class ",
					classType.ToString(),
					". Add [AllowCachedState] if you are handling it properly, or implement IRollbackStateOwner."
				}));
				return false;
			}
			return true;
		}

		// Token: 0x040024B1 RID: 9393
		public static RollbackDebugValidation instance = new RollbackDebugValidation();

		// Token: 0x040024B2 RID: 9394
		private bool enabled;
	}
}
