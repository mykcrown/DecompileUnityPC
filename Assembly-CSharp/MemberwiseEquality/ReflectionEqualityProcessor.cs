using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MemberwiseEquality
{
	// Token: 0x02000B31 RID: 2865
	public class ReflectionEqualityProcessor
	{
		// Token: 0x0600531F RID: 21279 RVA: 0x001AD970 File Offset: 0x001ABD70
		public static bool CheckReflectionEquality(object obj1, object obj2, ref string error, HashSet<int> mismatchedFieldHashes = null)
		{
			ReflectionEqualityProcessor.<CheckReflectionEquality>c__AnonStorey0 <CheckReflectionEquality>c__AnonStorey = new ReflectionEqualityProcessor.<CheckReflectionEquality>c__AnonStorey0();
			<CheckReflectionEquality>c__AnonStorey.mismatchedFieldHashes = mismatchedFieldHashes;
			<CheckReflectionEquality>c__AnonStorey.internalError = string.Empty;
			if (obj1 == obj2)
			{
				return true;
			}
			if (obj1 == null != (obj2 == null))
			{
				return false;
			}
			if (obj1.GetType() != obj2.GetType())
			{
				ReflectionEqualityProcessor.<CheckReflectionEquality>c__AnonStorey0 <CheckReflectionEquality>c__AnonStorey2 = <CheckReflectionEquality>c__AnonStorey;
				string internalError = <CheckReflectionEquality>c__AnonStorey.internalError;
				<CheckReflectionEquality>c__AnonStorey2.internalError = string.Concat(new object[]
				{
					internalError,
					"Type mismatch: ",
					obj1.GetType(),
					" != ",
					obj2.GetType()
				});
				return false;
			}
			<CheckReflectionEquality>c__AnonStorey.foundMismatch = false;
			Action<FieldInfo, string> appendError = delegate(FieldInfo field, string val)
			{
				<CheckReflectionEquality>c__AnonStorey.foundMismatch = true;
				<CheckReflectionEquality>c__AnonStorey.internalError += val;
				if (<CheckReflectionEquality>c__AnonStorey.mismatchedFieldHashes != null)
				{
					<CheckReflectionEquality>c__AnonStorey.mismatchedFieldHashes.Add(field.GetHashCode());
				}
			};
			ProcessContext context = ReflectionEqualityProcessor.createProcessContext(obj1, obj2, appendError);
			MemberwiseEqualityObjectProcessor.ProcessType(obj1.GetType(), context);
			if (<CheckReflectionEquality>c__AnonStorey.internalError.Length > 0)
			{
				<CheckReflectionEquality>c__AnonStorey.internalError = string.Concat(new object[]
				{
					"[",
					obj1.GetType(),
					"]",
					<CheckReflectionEquality>c__AnonStorey.internalError
				});
			}
			error = <CheckReflectionEquality>c__AnonStorey.internalError;
			return !<CheckReflectionEquality>c__AnonStorey.foundMismatch;
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x001ADA84 File Offset: 0x001ABE84
		private static ProcessContext createProcessContext(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			ProcessContext processContext = new ProcessContext();
			ProcessContext processContext2 = processContext;
			if (ReflectionEqualityProcessor.f__mg_cache0 == null)
			{
				ReflectionEqualityProcessor.f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext2.OnBeginProcessField = ReflectionEqualityProcessor.f__mg_cache0;
			ProcessContext processContext3 = processContext;
			if (ReflectionEqualityProcessor.f__mg_cache1 == null)
			{
				ReflectionEqualityProcessor.f__mg_cache1 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext3.IsIgnoreEqualityComparison = ReflectionEqualityProcessor.f__mg_cache1;
			processContext.IsUnhandledDictionary = delegate(FieldInfo field, Type type)
			{
				appendError(field, "Unhandled dictionary: " + field.Name + ", ");
			};
			processContext.IsEnum = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(obj1, obj2, appendError);
			processContext.IsValueType = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(obj1, obj2, appendError);
			processContext.IsInterface = delegate(FieldInfo field, Type type)
			{
				throw new Exception("Unhandled type");
			};
			processContext.IsDictionary = ReflectionEqualityProcessor.GenerateEnumerableCompare(obj1, obj2, appendError);
			processContext.IsEnumerable = ReflectionEqualityProcessor.GenerateEnumerableCompare(obj1, obj2, appendError);
			processContext.IsFixedCapacityList = ReflectionEqualityProcessor.GenerateFixedCapacityListCompare(obj1, obj2, appendError);
			processContext.IsMemberwiseEqualityObject = delegate(FieldInfo field, Type type)
			{
				if (field.GetValue(obj1) == null)
				{
					if (field.GetValue(obj2) != null)
					{
						appendError(field, field.Name + ": (mbw) lhs is null but rhs is not");
					}
				}
				else if (!((MemberwiseEqualityObject)field.GetValue(obj1)).MemberwiseEquals(field.GetValue(obj2)))
				{
					appendError(field, string.Concat(new object[]
					{
						field.Name,
						": ",
						field.GetValue(obj1),
						" !=(mbw) ",
						field.GetValue(obj2),
						", "
					}));
				}
			};
			processContext.IsGenericObject = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(obj1, obj2, appendError);
			ProcessContext processContext4 = processContext;
			if (ReflectionEqualityProcessor.f__mg_cache2 == null)
			{
				ReflectionEqualityProcessor.f__mg_cache2 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext4.OnProcessedReferenceField = ReflectionEqualityProcessor.f__mg_cache2;
			ProcessContext processContext5 = processContext;
			if (ReflectionEqualityProcessor.f__mg_cache3 == null)
			{
				ReflectionEqualityProcessor.f__mg_cache3 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext5.OnProcessedField = ReflectionEqualityProcessor.f__mg_cache3;
			return processContext;
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x001ADC34 File Offset: 0x001AC034
		private static Action<FieldInfo, Type> GenerateBasicEqualityCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			return delegate(FieldInfo field, Type type)
			{
				if (!field.FieldType.IsValueType && field.GetValue(obj1) == null)
				{
					if (field.GetValue(obj2) != null)
					{
						appendError(field, field.Name + ": lhs is null but rhs is not");
					}
				}
				else if (!field.GetValue(obj1).Equals(field.GetValue(obj2)))
				{
					appendError(field, string.Concat(new object[]
					{
						field.Name,
						": ",
						field.GetValue(obj1),
						" != ",
						field.GetValue(obj2),
						", "
					}));
				}
			};
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x001ADC68 File Offset: 0x001AC068
		private static Action<FieldInfo, Type> GenerateEnumerableCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			return delegate(FieldInfo field, Type type)
			{
				if (field.GetValue(obj1) == null != (field.GetValue(obj2) == null))
				{
					appendError(field, field.Name + ": Null mismatch");
					return;
				}
				if (field.GetValue(obj1) == null)
				{
					return;
				}
				IEnumerator enumerator = ((IEnumerable)field.GetValue(obj1)).GetEnumerator();
				bool flag = enumerator.MoveNext();
				IEnumerator enumerator2 = ((IEnumerable)field.GetValue(obj2)).GetEnumerator();
				bool flag2 = enumerator2.MoveNext();
				if (flag != flag2)
				{
					appendError(field, field.Name + ": Length mismatch");
				}
				int num = 0;
				while (flag && flag2)
				{
					if (enumerator.Current is MemberwiseEqualityObject)
					{
						if (!((MemberwiseEqualityObject)enumerator.Current).MemberwiseEquals((MemberwiseEqualityObject)enumerator2.Current))
						{
							appendError(field, string.Concat(new object[]
							{
								field.Name,
								": ",
								field.GetValue(enumerator.Current),
								" !=(mbw) ",
								field.GetValue(enumerator2.Current),
								", "
							}));
						}
					}
					else if (!enumerator.Current.Equals(enumerator2.Current))
					{
						appendError(field, string.Concat(new object[]
						{
							field.Name,
							"[",
							num,
							"] mismatch: ",
							enumerator,
							" != ",
							enumerator2,
							", "
						}));
					}
					flag = enumerator.MoveNext();
					flag2 = enumerator2.MoveNext();
					if (flag != flag2)
					{
						appendError(field, field.Name + ": Length mismatch");
					}
					if (!flag || !flag2)
					{
						break;
					}
					num++;
				}
			};
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x001ADC9C File Offset: 0x001AC09C
		private static Action<FieldInfo, Type> GenerateFixedCapacityListCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			return delegate(FieldInfo field, Type type)
			{
				if (field.GetValue(obj1) == null != (field.GetValue(obj2) == null))
				{
					appendError(field, field.Name + ": Null mismatch");
					return;
				}
				if (field.GetValue(obj1) == null)
				{
					return;
				}
				IEnumerator enumerator = ((ICustomList)field.GetValue(obj1)).ManualGetEnumerator();
				bool flag = enumerator.MoveNext();
				IEnumerator enumerator2 = ((ICustomList)field.GetValue(obj2)).ManualGetEnumerator();
				bool flag2 = enumerator2.MoveNext();
				if (flag != flag2)
				{
					appendError(field, field.Name + ": Length mismatch");
				}
				int num = 0;
				while (flag && flag2)
				{
					if (enumerator.Current is MemberwiseEqualityObject)
					{
						if (!((MemberwiseEqualityObject)enumerator.Current).MemberwiseEquals((MemberwiseEqualityObject)enumerator2.Current))
						{
							appendError(field, string.Concat(new object[]
							{
								field.Name,
								": ",
								field.GetValue(enumerator.Current),
								" !=(mbw) ",
								field.GetValue(enumerator2.Current),
								", "
							}));
						}
					}
					else if (!enumerator.Current.Equals(enumerator2.Current))
					{
						appendError(field, string.Concat(new object[]
						{
							field.Name,
							"[",
							num,
							"] mismatch: ",
							enumerator,
							" != ",
							enumerator2,
							", "
						}));
					}
					flag = enumerator.MoveNext();
					flag2 = enumerator2.MoveNext();
					if (flag != flag2)
					{
						appendError(field, field.Name + ": Length mismatch");
					}
					if (!flag || !flag2)
					{
						break;
					}
					num++;
				}
			};
		}

		// Token: 0x040034CF RID: 13519
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache0;

		// Token: 0x040034D0 RID: 13520
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache1;

		// Token: 0x040034D1 RID: 13521
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache2;

		// Token: 0x040034D2 RID: 13522
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache3;
	}
}
