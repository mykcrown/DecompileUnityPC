// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MemberwiseEquality
{
	public class ReflectionEqualityProcessor
	{
		private sealed class _CheckReflectionEquality_c__AnonStorey0
		{
			internal bool foundMismatch;

			internal string internalError;

			internal HashSet<int> mismatchedFieldHashes;

			internal void __m__0(FieldInfo field, string val)
			{
				this.foundMismatch = true;
				this.internalError += val;
				if (this.mismatchedFieldHashes != null)
				{
					this.mismatchedFieldHashes.Add(field.GetHashCode());
				}
			}
		}

		private sealed class _createProcessContext_c__AnonStorey1
		{
			internal Action<FieldInfo, string> appendError;

			internal object obj1;

			internal object obj2;

			internal void __m__0(FieldInfo field, Type type)
			{
				this.appendError(field, "Unhandled dictionary: " + field.Name + ", ");
			}

			internal void __m__1(FieldInfo field, Type type)
			{
				if (field.GetValue(this.obj1) == null)
				{
					if (field.GetValue(this.obj2) != null)
					{
						this.appendError(field, field.Name + ": (mbw) lhs is null but rhs is not");
					}
				}
				else if (!((MemberwiseEqualityObject)field.GetValue(this.obj1)).MemberwiseEquals(field.GetValue(this.obj2)))
				{
					this.appendError(field, string.Concat(new object[]
					{
						field.Name,
						": ",
						field.GetValue(this.obj1),
						" !=(mbw) ",
						field.GetValue(this.obj2),
						", "
					}));
				}
			}
		}

		private sealed class _GenerateBasicEqualityCompare_c__AnonStorey2
		{
			internal object obj1;

			internal object obj2;

			internal Action<FieldInfo, string> appendError;

			internal void __m__0(FieldInfo field, Type type)
			{
				if (!field.FieldType.IsValueType && field.GetValue(this.obj1) == null)
				{
					if (field.GetValue(this.obj2) != null)
					{
						this.appendError(field, field.Name + ": lhs is null but rhs is not");
					}
				}
				else if (!field.GetValue(this.obj1).Equals(field.GetValue(this.obj2)))
				{
					this.appendError(field, string.Concat(new object[]
					{
						field.Name,
						": ",
						field.GetValue(this.obj1),
						" != ",
						field.GetValue(this.obj2),
						", "
					}));
				}
			}
		}

		private sealed class _GenerateEnumerableCompare_c__AnonStorey3
		{
			internal object obj1;

			internal object obj2;

			internal Action<FieldInfo, string> appendError;

			internal void __m__0(FieldInfo field, Type type)
			{
				if (field.GetValue(this.obj1) == null != (field.GetValue(this.obj2) == null))
				{
					this.appendError(field, field.Name + ": Null mismatch");
					return;
				}
				if (field.GetValue(this.obj1) == null)
				{
					return;
				}
				IEnumerator enumerator = ((IEnumerable)field.GetValue(this.obj1)).GetEnumerator();
				bool flag = enumerator.MoveNext();
				IEnumerator enumerator2 = ((IEnumerable)field.GetValue(this.obj2)).GetEnumerator();
				bool flag2 = enumerator2.MoveNext();
				if (flag != flag2)
				{
					this.appendError(field, field.Name + ": Length mismatch");
				}
				int num = 0;
				while (flag && flag2)
				{
					if (enumerator.Current is MemberwiseEqualityObject)
					{
						if (!((MemberwiseEqualityObject)enumerator.Current).MemberwiseEquals((MemberwiseEqualityObject)enumerator2.Current))
						{
							this.appendError(field, string.Concat(new object[]
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
						this.appendError(field, string.Concat(new object[]
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
						this.appendError(field, field.Name + ": Length mismatch");
					}
					if (!flag || !flag2)
					{
						break;
					}
					num++;
				}
			}
		}

		private sealed class _GenerateFixedCapacityListCompare_c__AnonStorey4
		{
			internal object obj1;

			internal object obj2;

			internal Action<FieldInfo, string> appendError;

			internal void __m__0(FieldInfo field, Type type)
			{
				if (field.GetValue(this.obj1) == null != (field.GetValue(this.obj2) == null))
				{
					this.appendError(field, field.Name + ": Null mismatch");
					return;
				}
				if (field.GetValue(this.obj1) == null)
				{
					return;
				}
				IEnumerator enumerator = ((ICustomList)field.GetValue(this.obj1)).ManualGetEnumerator();
				bool flag = enumerator.MoveNext();
				IEnumerator enumerator2 = ((ICustomList)field.GetValue(this.obj2)).ManualGetEnumerator();
				bool flag2 = enumerator2.MoveNext();
				if (flag != flag2)
				{
					this.appendError(field, field.Name + ": Length mismatch");
				}
				int num = 0;
				while (flag && flag2)
				{
					if (enumerator.Current is MemberwiseEqualityObject)
					{
						if (!((MemberwiseEqualityObject)enumerator.Current).MemberwiseEquals((MemberwiseEqualityObject)enumerator2.Current))
						{
							this.appendError(field, string.Concat(new object[]
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
						this.appendError(field, string.Concat(new object[]
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
						this.appendError(field, field.Name + ": Length mismatch");
					}
					if (!flag || !flag2)
					{
						break;
					}
					num++;
				}
			}
		}

		private static Action<FieldInfo, Type> __f__mg_cache0;

		private static Action<FieldInfo, Type> __f__mg_cache1;

		private static Action<FieldInfo, Type> __f__mg_cache2;

		private static Action<FieldInfo, Type> __f__mg_cache3;

		private static Action<FieldInfo, Type> __f__am_cache0;

		public static bool CheckReflectionEquality(object obj1, object obj2, ref string error, HashSet<int> mismatchedFieldHashes = null)
		{
			ReflectionEqualityProcessor._CheckReflectionEquality_c__AnonStorey0 _CheckReflectionEquality_c__AnonStorey = new ReflectionEqualityProcessor._CheckReflectionEquality_c__AnonStorey0();
			_CheckReflectionEquality_c__AnonStorey.mismatchedFieldHashes = mismatchedFieldHashes;
			_CheckReflectionEquality_c__AnonStorey.internalError = string.Empty;
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
				ReflectionEqualityProcessor._CheckReflectionEquality_c__AnonStorey0 arg_7F_0 = _CheckReflectionEquality_c__AnonStorey;
				string internalError = _CheckReflectionEquality_c__AnonStorey.internalError;
				arg_7F_0.internalError = string.Concat(new object[]
				{
					internalError,
					"Type mismatch: ",
					obj1.GetType(),
					" != ",
					obj2.GetType()
				});
				return false;
			}
			_CheckReflectionEquality_c__AnonStorey.foundMismatch = false;
			Action<FieldInfo, string> appendError = new Action<FieldInfo, string>(_CheckReflectionEquality_c__AnonStorey.__m__0);
			ProcessContext context = ReflectionEqualityProcessor.createProcessContext(obj1, obj2, appendError);
			MemberwiseEqualityObjectProcessor.ProcessType(obj1.GetType(), context);
			if (_CheckReflectionEquality_c__AnonStorey.internalError.Length > 0)
			{
				_CheckReflectionEquality_c__AnonStorey.internalError = string.Concat(new object[]
				{
					"[",
					obj1.GetType(),
					"]",
					_CheckReflectionEquality_c__AnonStorey.internalError
				});
			}
			error = _CheckReflectionEquality_c__AnonStorey.internalError;
			return !_CheckReflectionEquality_c__AnonStorey.foundMismatch;
		}

		private static ProcessContext createProcessContext(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			ReflectionEqualityProcessor._createProcessContext_c__AnonStorey1 _createProcessContext_c__AnonStorey = new ReflectionEqualityProcessor._createProcessContext_c__AnonStorey1();
			_createProcessContext_c__AnonStorey.appendError = appendError;
			_createProcessContext_c__AnonStorey.obj1 = obj1;
			_createProcessContext_c__AnonStorey.obj2 = obj2;
			ProcessContext processContext = new ProcessContext();
			ProcessContext arg_3F_0 = processContext;
			if (ReflectionEqualityProcessor.__f__mg_cache0 == null)
			{
				ReflectionEqualityProcessor.__f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_3F_0.OnBeginProcessField = ReflectionEqualityProcessor.__f__mg_cache0;
			ProcessContext arg_62_0 = processContext;
			if (ReflectionEqualityProcessor.__f__mg_cache1 == null)
			{
				ReflectionEqualityProcessor.__f__mg_cache1 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_62_0.IsIgnoreEqualityComparison = ReflectionEqualityProcessor.__f__mg_cache1;
			processContext.IsUnhandledDictionary = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__0);
			processContext.IsEnum = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			processContext.IsValueType = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			ProcessContext arg_D1_0 = processContext;
			if (ReflectionEqualityProcessor.__f__am_cache0 == null)
			{
				ReflectionEqualityProcessor.__f__am_cache0 = new Action<FieldInfo, Type>(ReflectionEqualityProcessor._createProcessContext_m__0);
			}
			arg_D1_0.IsInterface = ReflectionEqualityProcessor.__f__am_cache0;
			processContext.IsDictionary = ReflectionEqualityProcessor.GenerateEnumerableCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			processContext.IsEnumerable = ReflectionEqualityProcessor.GenerateEnumerableCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			processContext.IsFixedCapacityList = ReflectionEqualityProcessor.GenerateFixedCapacityListCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			processContext.IsMemberwiseEqualityObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__1);
			processContext.IsGenericObject = ReflectionEqualityProcessor.GenerateBasicEqualityCompare(_createProcessContext_c__AnonStorey.obj1, _createProcessContext_c__AnonStorey.obj2, _createProcessContext_c__AnonStorey.appendError);
			ProcessContext arg_17A_0 = processContext;
			if (ReflectionEqualityProcessor.__f__mg_cache2 == null)
			{
				ReflectionEqualityProcessor.__f__mg_cache2 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_17A_0.OnProcessedReferenceField = ReflectionEqualityProcessor.__f__mg_cache2;
			ProcessContext arg_19D_0 = processContext;
			if (ReflectionEqualityProcessor.__f__mg_cache3 == null)
			{
				ReflectionEqualityProcessor.__f__mg_cache3 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_19D_0.OnProcessedField = ReflectionEqualityProcessor.__f__mg_cache3;
			return processContext;
		}

		private static Action<FieldInfo, Type> GenerateBasicEqualityCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			ReflectionEqualityProcessor._GenerateBasicEqualityCompare_c__AnonStorey2 _GenerateBasicEqualityCompare_c__AnonStorey = new ReflectionEqualityProcessor._GenerateBasicEqualityCompare_c__AnonStorey2();
			_GenerateBasicEqualityCompare_c__AnonStorey.obj1 = obj1;
			_GenerateBasicEqualityCompare_c__AnonStorey.obj2 = obj2;
			_GenerateBasicEqualityCompare_c__AnonStorey.appendError = appendError;
			return new Action<FieldInfo, Type>(_GenerateBasicEqualityCompare_c__AnonStorey.__m__0);
		}

		private static Action<FieldInfo, Type> GenerateEnumerableCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			ReflectionEqualityProcessor._GenerateEnumerableCompare_c__AnonStorey3 _GenerateEnumerableCompare_c__AnonStorey = new ReflectionEqualityProcessor._GenerateEnumerableCompare_c__AnonStorey3();
			_GenerateEnumerableCompare_c__AnonStorey.obj1 = obj1;
			_GenerateEnumerableCompare_c__AnonStorey.obj2 = obj2;
			_GenerateEnumerableCompare_c__AnonStorey.appendError = appendError;
			return new Action<FieldInfo, Type>(_GenerateEnumerableCompare_c__AnonStorey.__m__0);
		}

		private static Action<FieldInfo, Type> GenerateFixedCapacityListCompare(object obj1, object obj2, Action<FieldInfo, string> appendError)
		{
			ReflectionEqualityProcessor._GenerateFixedCapacityListCompare_c__AnonStorey4 _GenerateFixedCapacityListCompare_c__AnonStorey = new ReflectionEqualityProcessor._GenerateFixedCapacityListCompare_c__AnonStorey4();
			_GenerateFixedCapacityListCompare_c__AnonStorey.obj1 = obj1;
			_GenerateFixedCapacityListCompare_c__AnonStorey.obj2 = obj2;
			_GenerateFixedCapacityListCompare_c__AnonStorey.appendError = appendError;
			return new Action<FieldInfo, Type>(_GenerateFixedCapacityListCompare_c__AnonStorey.__m__0);
		}

		private static void _createProcessContext_m__0(FieldInfo field, Type type)
		{
			throw new Exception("Unhandled type");
		}
	}
}
