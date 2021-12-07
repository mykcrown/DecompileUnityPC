using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace MemberwiseEquality
{
	// Token: 0x02000B2A RID: 2858
	public class MakeEqualsProcessor
	{
		// Token: 0x060052F6 RID: 21238 RVA: 0x001AC864 File Offset: 0x001AAC64
		public static Func<object, object, bool> MakeEqualsMethod(Type type)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "y");
			UnaryExpression pCastThis = Expression.Convert(parameterExpression, type);
			UnaryExpression pCastThat = Expression.Convert(parameterExpression2, type);
			Expression last = null;
			Action<Expression> setLast = delegate(Expression value)
			{
				last = value;
			};
			Func<Expression> getLast = () => last;
			ProcessContext context = MakeEqualsProcessor.createProcessContext(type, parameterExpression, pCastThis, pCastThat, setLast, getLast);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			if (last == null)
			{
				last = Expression.Constant(true, typeof(bool));
			}
			last = Expression.Condition(Expression.AndAlso(Expression.NotEqual(parameterExpression2, Expression.Constant(null)), Expression.TypeIs(parameterExpression2, type)), last, Expression.Constant(false, typeof(bool)));
			return Expression.Lambda<Func<object, object, bool>>(last, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001AC970 File Offset: 0x001AAD70
		private static ProcessContext createProcessContext(Type objType, ParameterExpression pThis, UnaryExpression pCastThis, UnaryExpression pCastThat, Action<Expression> setLast, Func<Expression> getLast)
		{
			Expression equals = null;
			ProcessContext processContext = new ProcessContext();
			processContext.OnBeginProcessField = delegate(FieldInfo field, Type type)
			{
				equals = null;
			};
			processContext.IsIgnoreEqualityComparison = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Constant(true, typeof(bool));
			};
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
				equals = Expression.Call(MakeEqualsProcessor.equalsMethod, Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsEnum = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Equal(Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsValueType = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Equal(Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsInterface = delegate(FieldInfo field, Type type)
			{
				throw new Exception("Unhandled case");
			};
			processContext.IsDictionary = delegate(FieldInfo field, Type type)
			{
				Type[] genericArguments = field.FieldType.GetGenericArguments();
				MethodInfo method = typeof(DictionaryUtil).GetMethod("DictionariesAreEqual");
				MethodInfo method2 = method.MakeGenericMethod(genericArguments);
				equals = Expression.Call(method2, Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsEnumerable = delegate(FieldInfo field, Type type)
			{
				Type type2 = (!field.FieldType.IsArray) ? field.FieldType.GetGenericArguments()[0] : field.FieldType.GetElementType();
				MethodInfo method = typeof(ListUtil).GetMethod("EnumerablesAreEqual");
				if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type2))
				{
					method = typeof(ListUtil).GetMethod("MemberwiseEnumerablesAreEqual");
				}
				MethodInfo method2 = method.MakeGenericMethod(new Type[]
				{
					type2
				});
				equals = Expression.Call(method2, Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsFixedCapacityList = delegate(FieldInfo field, Type type)
			{
				Type type2 = (!field.FieldType.IsArray) ? field.FieldType.GetGenericArguments()[0] : field.FieldType.GetElementType();
				MethodInfo method = typeof(ListUtil).GetMethod("FixedCapacityListAreEqual");
				if (typeof(MemberwiseEqualityObject).IsAssignableFrom(type2))
				{
					method = typeof(ListUtil).GetMethod("MemberwiseFixedCapacityListAreEqual");
				}
				MethodInfo method2 = method.MakeGenericMethod(new Type[]
				{
					type2
				});
				equals = Expression.Call(method2, Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.IsMemberwiseEqualityObject = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Call(Expression.Field(pCastThis, field), MakeEqualsProcessor.memberwiseEqualsMethod, new Expression[]
				{
					Expression.Field(pCastThat, field)
				});
			};
			processContext.IsGenericObject = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Call(MakeEqualsProcessor.equalsMethod, Expression.Field(pCastThis, field), Expression.Field(pCastThat, field));
			};
			processContext.OnProcessedReferenceField = delegate(FieldInfo field, Type type)
			{
				equals = Expression.Condition(Expression.NotEqual(Expression.Field(pCastThis, field), Expression.Constant(null)), equals, Expression.Equal(Expression.Field(pCastThat, field), Expression.Constant(null)));
			};
			processContext.OnProcessedField = delegate(FieldInfo field, Type type)
			{
				if (getLast() == null)
				{
					setLast(equals);
				}
				else
				{
					setLast(Expression.AndAlso(getLast(), equals));
				}
			};
			return processContext;
		}

		// Token: 0x040034B3 RID: 13491
		private static MethodInfo equalsMethod = typeof(object).GetMethod("Equals", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x040034B4 RID: 13492
		private static MethodInfo memberwiseEqualsMethod = typeof(MemberwiseEqualityObject).GetMethod("MemberwiseEquals", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
	}
}
