// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Reflection;
using UnityEngine;

namespace RollbackDebug
{
	public class RollbackDebugValidation
	{
		public static RollbackDebugValidation instance = new RollbackDebugValidation();

		private bool enabled;

		public void Initialize(bool enabled)
		{
			this.enabled = enabled;
		}

		public void StartupValidation()
		{
			if (!this.enabled)
			{
				return;
			}
			RollbackDebugValidation.detectRollbackStateCaching();
		}

		private static void detectRollbackStateCaching()
		{
			Type[] types = typeof(RollbackDebugValidation).Assembly.GetTypes();
			Type[] array = types;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				if (!typeof(IRollbackStateOwner).IsAssignableFrom(type) && !typeof(RollbackState).IsAssignableFrom(type))
				{
					if (!typeof(ScriptableObject).IsAssignableFrom(type))
					{
						if (!type.IsInterface)
						{
							if (!type.Name.StartsWith("<"))
							{
								FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								for (int j = 0; j < fields.Length; j++)
								{
									FieldInfo memberInfo = fields[j];
									RollbackDebugValidation.evaluateType(type, memberInfo);
								}
								PropertyInfo[] properties = type.GetProperties();
								for (int k = 0; k < properties.Length; k++)
								{
									PropertyInfo memberInfo2 = properties[k];
									RollbackDebugValidation.evaluateType(type, memberInfo2);
								}
							}
						}
					}
				}
			}
		}

		private static bool evaluateType(Type classType, MemberInfo memberInfo)
		{
			Type type = (!(memberInfo is PropertyInfo)) ? (memberInfo as FieldInfo).FieldType : (memberInfo as PropertyInfo).PropertyType;
			if (typeof(RollbackState).IsAssignableFrom(classType) && memberInfo.GetCustomAttributes(typeof(AllowCachedState), true).Length == 0)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
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
	}
}
