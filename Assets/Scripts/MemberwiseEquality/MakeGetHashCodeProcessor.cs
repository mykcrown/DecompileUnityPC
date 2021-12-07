// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	public static class MakeGetHashCodeProcessor
	{
		private sealed class _MakeGetHashCodeMethod_c__AnonStorey0
		{
			internal Expression last;

			internal void __m__0(Expression value)
			{
				this.last = value;
			}

			internal Expression __m__1()
			{
				return this.last;
			}
		}

		private sealed class _createProcessContext_c__AnonStorey1
		{
			internal Expression hash;

			internal UnaryExpression pCastThis;

			internal Func<Expression> getLast;

			internal Action<Expression> setLast;

			internal void __m__0(FieldInfo field, Type type)
			{
				this.hash = null;
			}

			internal void __m__1(FieldInfo field, Type type)
			{
				this.hash = Expression.Call(Expression.Convert(Expression.Field(this.pCastThis, field), typeof(int)), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			}

			internal void __m__2(FieldInfo field, Type type)
			{
				this.hash = Expression.Call(Expression.Field(this.pCastThis, field), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			}

			internal void __m__3(FieldInfo field, Type type)
			{
				this.hash = Expression.Call(Expression.Convert(Expression.Field(this.pCastThis, field), typeof(object)), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			}

			internal void __m__4(FieldInfo field, Type type)
			{
				Type[] genericArguments = field.FieldType.GetGenericArguments();
				MethodInfo method = typeof(DictionaryUtil).GetMethod("GetDictionaryHashCode");
				MethodInfo method2 = method.MakeGenericMethod(genericArguments);
				this.hash = Expression.Call(method2, Expression.Field(this.pCastThis, field));
			}

			internal void __m__5(FieldInfo field, Type type)
			{
				Type type2 = (!field.FieldType.IsArray) ? field.FieldType.GetGenericArguments()[0] : field.FieldType.GetElementType();
				MethodInfo method = typeof(ListUtil).GetMethod("GetEnumerableHashCode");
				if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type2))
				{
					if (field.FieldType.IsArray)
					{
						method = typeof(ListUtil).GetMethod("GetMemberwiseArrayHashCode");
					}
					else if (typeof(IList).IsAssignableFrom(field.FieldType))
					{
						method = typeof(ListUtil).GetMethod("GetMemberwiseListHashCode");
					}
					else
					{
						method = typeof(ListUtil).GetMethod("GetMemberwiseEnumerableHashCode");
					}
				}
				else if (field.FieldType.IsArray)
				{
					if (type2.IsEnum)
					{
						method = typeof(ListUtil).GetMethod("GetEnumArrayHashCode");
					}
					else
					{
						method = typeof(ListUtil).GetMethod("GetArrayHashCode");
					}
				}
				else if (typeof(IList).IsAssignableFrom(field.FieldType))
				{
					if (type2.IsEnum)
					{
						method = typeof(ListUtil).GetMethod("GetEnumListHashCode");
					}
					else
					{
						method = typeof(ListUtil).GetMethod("GetListHashCode");
					}
				}
				MethodInfo method2 = method.MakeGenericMethod(new Type[]
				{
					type2
				});
				this.hash = Expression.Call(method2, Expression.Field(this.pCastThis, field));
			}

			internal void __m__6(FieldInfo field, Type type)
			{
				Type type2 = (!field.FieldType.IsArray) ? field.FieldType.GetGenericArguments()[0] : field.FieldType.GetElementType();
				MethodInfo method = typeof(ListUtil).GetMethod("GetFixedCapacityListHashCode");
				if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type2))
				{
					method = typeof(ListUtil).GetMethod("GetFixedCapacityMemberwiseListHashCode");
				}
				MethodInfo method2 = method.MakeGenericMethod(new Type[]
				{
					type2
				});
				this.hash = Expression.Call(method2, Expression.Field(this.pCastThis, field));
			}

			internal void __m__7(FieldInfo field, Type type)
			{
				this.hash = Expression.Call(Expression.Field(this.pCastThis, field), "GetMemberwiseHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			}

			internal void __m__8(FieldInfo field, Type type)
			{
				this.hash = Expression.Call(Expression.Field(this.pCastThis, field), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			}

			internal void __m__9(FieldInfo field, Type type)
			{
				this.hash = Expression.Condition(Expression.NotEqual(Expression.Field(this.pCastThis, field), Expression.Constant(null)), this.hash, Expression.Constant(0, typeof(int)));
			}

			internal void __m__A(FieldInfo field, Type type)
			{
				if (this.getLast() == null)
				{
					this.setLast(this.hash);
				}
				else
				{
					this.setLast(Expression.ExclusiveOr(this.getLast(), this.hash));
				}
			}
		}

		private static Action<FieldInfo, Type> __f__mg_cache0;

		private static Action<FieldInfo, Type> __f__am_cache0;

		public static Func<object, int> MakeGetHashCodeMethod(Type type)
		{
			MakeGetHashCodeProcessor._MakeGetHashCodeMethod_c__AnonStorey0 _MakeGetHashCodeMethod_c__AnonStorey = new MakeGetHashCodeProcessor._MakeGetHashCodeMethod_c__AnonStorey0();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "x");
			_MakeGetHashCodeMethod_c__AnonStorey.last = null;
			Action<Expression> setLast = new Action<Expression>(_MakeGetHashCodeMethod_c__AnonStorey.__m__0);
			Func<Expression> getLast = new Func<Expression>(_MakeGetHashCodeMethod_c__AnonStorey.__m__1);
			ProcessContext context = MakeGetHashCodeProcessor.createProcessContext(type, parameterExpression, setLast, getLast);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			if (_MakeGetHashCodeMethod_c__AnonStorey.last == null)
			{
				_MakeGetHashCodeMethod_c__AnonStorey.last = Expression.Constant(0, typeof(int));
			}
			return Expression.Lambda<Func<object, int>>(_MakeGetHashCodeMethod_c__AnonStorey.last, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		private static ProcessContext createProcessContext(Type objType, ParameterExpression pThis, Action<Expression> setLast, Func<Expression> getLast)
		{
			MakeGetHashCodeProcessor._createProcessContext_c__AnonStorey1 _createProcessContext_c__AnonStorey = new MakeGetHashCodeProcessor._createProcessContext_c__AnonStorey1();
			_createProcessContext_c__AnonStorey.getLast = getLast;
			_createProcessContext_c__AnonStorey.setLast = setLast;
			_createProcessContext_c__AnonStorey.pCastThis = Expression.Convert(pThis, objType);
			_createProcessContext_c__AnonStorey.hash = null;
			ProcessContext processContext = new ProcessContext();
			processContext.OnBeginProcessField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__0);
			ProcessContext arg_5E_0 = processContext;
			if (MakeGetHashCodeProcessor.__f__mg_cache0 == null)
			{
				MakeGetHashCodeProcessor.__f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_5E_0.IsIgnoreEqualityComparison = MakeGetHashCodeProcessor.__f__mg_cache0;
			ProcessContext arg_81_0 = processContext;
			if (MakeGetHashCodeProcessor.__f__am_cache0 == null)
			{
				MakeGetHashCodeProcessor.__f__am_cache0 = new Action<FieldInfo, Type>(MakeGetHashCodeProcessor._createProcessContext_m__0);
			}
			arg_81_0.IsUnhandledDictionary = MakeGetHashCodeProcessor.__f__am_cache0;
			processContext.IsEnum = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__1);
			processContext.IsValueType = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__2);
			processContext.IsInterface = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__3);
			processContext.IsDictionary = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__4);
			processContext.IsEnumerable = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__5);
			processContext.IsFixedCapacityList = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__6);
			processContext.IsMemberwiseEqualityObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__7);
			processContext.IsGenericObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__8);
			processContext.OnProcessedReferenceField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__9);
			processContext.OnProcessedField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__A);
			return processContext;
		}

		private static void _createProcessContext_m__0(FieldInfo field, Type type)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Unhandled dictionary ",
				field.Name,
				" in ",
				type,
				"; tag with IgnoreRollbackValidation or remove"
			}));
		}
	}
}
