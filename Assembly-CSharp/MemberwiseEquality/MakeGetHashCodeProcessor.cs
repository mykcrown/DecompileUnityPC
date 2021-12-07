using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	// Token: 0x02000B2B RID: 2859
	public static class MakeGetHashCodeProcessor
	{
		// Token: 0x060052FA RID: 21242 RVA: 0x001ACEA4 File Offset: 0x001AB2A4
		public static Func<object, int> MakeGetHashCodeMethod(Type type)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "x");
			Expression last = null;
			Action<Expression> setLast = delegate(Expression value)
			{
				last = value;
			};
			Func<Expression> getLast = () => last;
			ProcessContext context = MakeGetHashCodeProcessor.createProcessContext(type, parameterExpression, setLast, getLast);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			if (last == null)
			{
				last = Expression.Constant(0, typeof(int));
			}
			return Expression.Lambda<Func<object, int>>(last, new ParameterExpression[]
			{
				parameterExpression
			}).Compile();
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001ACF40 File Offset: 0x001AB340
		private static ProcessContext createProcessContext(Type objType, ParameterExpression pThis, Action<Expression> setLast, Func<Expression> getLast)
		{
			UnaryExpression pCastThis = Expression.Convert(pThis, objType);
			Expression hash = null;
			ProcessContext processContext = new ProcessContext();
			processContext.OnBeginProcessField = delegate(FieldInfo field, Type type)
			{
				hash = null;
			};
			ProcessContext processContext2 = processContext;
			if (MakeGetHashCodeProcessor.f__mg_cache0 == null)
			{
				MakeGetHashCodeProcessor.f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext2.IsIgnoreEqualityComparison = MakeGetHashCodeProcessor.f__mg_cache0;
			processContext.IsUnhandledDictionary = delegate(FieldInfo field, Type type)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unhandled dictionary ",
					field.Name,
					" in ",
					type,
					"; tag with IgnoreRollbackValidation or remove"
				}));
			};
			processContext.IsEnum = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Call(Expression.Convert(Expression.Field(pCastThis, field), typeof(int)), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			};
			processContext.IsValueType = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Call(Expression.Field(pCastThis, field), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			};
			processContext.IsInterface = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Call(Expression.Convert(Expression.Field(pCastThis, field), typeof(object)), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			};
			processContext.IsDictionary = delegate(FieldInfo field, Type type)
			{
				Type[] genericArguments = field.FieldType.GetGenericArguments();
				MethodInfo method = typeof(DictionaryUtil).GetMethod("GetDictionaryHashCode");
				MethodInfo method2 = method.MakeGenericMethod(genericArguments);
				hash = Expression.Call(method2, Expression.Field(pCastThis, field));
			};
			processContext.IsEnumerable = delegate(FieldInfo field, Type type)
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
				hash = Expression.Call(method2, Expression.Field(pCastThis, field));
			};
			processContext.IsFixedCapacityList = delegate(FieldInfo field, Type type)
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
				hash = Expression.Call(method2, Expression.Field(pCastThis, field));
			};
			processContext.IsMemberwiseEqualityObject = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Call(Expression.Field(pCastThis, field), "GetMemberwiseHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			};
			processContext.IsGenericObject = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Call(Expression.Field(pCastThis, field), "GetHashCode", Type.EmptyTypes, Array.Empty<Expression>());
			};
			processContext.OnProcessedReferenceField = delegate(FieldInfo field, Type type)
			{
				hash = Expression.Condition(Expression.NotEqual(Expression.Field(pCastThis, field), Expression.Constant(null)), hash, Expression.Constant(0, typeof(int)));
			};
			processContext.OnProcessedField = delegate(FieldInfo field, Type type)
			{
				if (getLast() == null)
				{
					setLast(hash);
				}
				else
				{
					setLast(Expression.ExclusiveOr(getLast(), hash));
				}
			};
			return processContext;
		}

		// Token: 0x040034B6 RID: 13494
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache0;
	}
}
