// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	public class MakeEqualsProcessor
	{
		private sealed class _MakeEqualsMethod_c__AnonStorey0
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
			internal Expression equals;

			internal UnaryExpression pCastThis;

			internal UnaryExpression pCastThat;

			internal Func<Expression> getLast;

			internal Action<Expression> setLast;

			internal void __m__0(FieldInfo field, Type type)
			{
				this.equals = null;
			}

			internal void __m__1(FieldInfo field, Type type)
			{
				this.equals = Expression.Constant(true, typeof(bool));
			}

			internal void __m__2(FieldInfo field, Type type)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Unhandled dictionary ",
					field.Name,
					" in ",
					type,
					"; tag with IgnoreRollbackValidation or remove"
				}));
				this.equals = Expression.Call(MakeEqualsProcessor.equalsMethod, Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__3(FieldInfo field, Type type)
			{
				this.equals = Expression.Equal(Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__4(FieldInfo field, Type type)
			{
				this.equals = Expression.Equal(Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__5(FieldInfo field, Type type)
			{
				Type[] genericArguments = field.FieldType.GetGenericArguments();
				MethodInfo method = typeof(DictionaryUtil).GetMethod("DictionariesAreEqual");
				MethodInfo method2 = method.MakeGenericMethod(genericArguments);
				this.equals = Expression.Call(method2, Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__6(FieldInfo field, Type type)
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
				this.equals = Expression.Call(method2, Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__7(FieldInfo field, Type type)
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
				this.equals = Expression.Call(method2, Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__8(FieldInfo field, Type type)
			{
				this.equals = Expression.Call(Expression.Field(this.pCastThis, field), MakeEqualsProcessor.memberwiseEqualsMethod, new Expression[]
				{
					Expression.Field(this.pCastThat, field)
				});
			}

			internal void __m__9(FieldInfo field, Type type)
			{
				this.equals = Expression.Call(MakeEqualsProcessor.equalsMethod, Expression.Field(this.pCastThis, field), Expression.Field(this.pCastThat, field));
			}

			internal void __m__A(FieldInfo field, Type type)
			{
				this.equals = Expression.Condition(Expression.NotEqual(Expression.Field(this.pCastThis, field), Expression.Constant(null)), this.equals, Expression.Equal(Expression.Field(this.pCastThat, field), Expression.Constant(null)));
			}

			internal void __m__B(FieldInfo field, Type type)
			{
				if (this.getLast() == null)
				{
					this.setLast(this.equals);
				}
				else
				{
					this.setLast(Expression.AndAlso(this.getLast(), this.equals));
				}
			}
		}

		private static MethodInfo equalsMethod = typeof(object).GetMethod("Equals", BindingFlags.Static | BindingFlags.Public);

		private static MethodInfo memberwiseEqualsMethod = typeof(MemberwiseEqualityObject).GetMethod("MemberwiseEquals", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

		private static Action<FieldInfo, Type> __f__am_cache0;

		public static Func<object, object, bool> MakeEqualsMethod(Type type)
		{
			MakeEqualsProcessor._MakeEqualsMethod_c__AnonStorey0 _MakeEqualsMethod_c__AnonStorey = new MakeEqualsProcessor._MakeEqualsMethod_c__AnonStorey0();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "x");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "y");
			UnaryExpression pCastThis = Expression.Convert(parameterExpression, type);
			UnaryExpression pCastThat = Expression.Convert(parameterExpression2, type);
			_MakeEqualsMethod_c__AnonStorey.last = null;
			Action<Expression> setLast = new Action<Expression>(_MakeEqualsMethod_c__AnonStorey.__m__0);
			Func<Expression> getLast = new Func<Expression>(_MakeEqualsMethod_c__AnonStorey.__m__1);
			ProcessContext context = MakeEqualsProcessor.createProcessContext(type, parameterExpression, pCastThis, pCastThat, setLast, getLast);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			if (_MakeEqualsMethod_c__AnonStorey.last == null)
			{
				_MakeEqualsMethod_c__AnonStorey.last = Expression.Constant(true, typeof(bool));
			}
			_MakeEqualsMethod_c__AnonStorey.last = Expression.Condition(Expression.AndAlso(Expression.NotEqual(parameterExpression2, Expression.Constant(null)), Expression.TypeIs(parameterExpression2, type)), _MakeEqualsMethod_c__AnonStorey.last, Expression.Constant(false, typeof(bool)));
			return Expression.Lambda<Func<object, object, bool>>(_MakeEqualsMethod_c__AnonStorey.last, new ParameterExpression[]
			{
				parameterExpression,
				parameterExpression2
			}).Compile();
		}

		private static ProcessContext createProcessContext(Type objType, ParameterExpression pThis, UnaryExpression pCastThis, UnaryExpression pCastThat, Action<Expression> setLast, Func<Expression> getLast)
		{
			MakeEqualsProcessor._createProcessContext_c__AnonStorey1 _createProcessContext_c__AnonStorey = new MakeEqualsProcessor._createProcessContext_c__AnonStorey1();
			_createProcessContext_c__AnonStorey.pCastThis = pCastThis;
			_createProcessContext_c__AnonStorey.pCastThat = pCastThat;
			_createProcessContext_c__AnonStorey.getLast = getLast;
			_createProcessContext_c__AnonStorey.setLast = setLast;
			_createProcessContext_c__AnonStorey.equals = null;
			ProcessContext processContext = new ProcessContext();
			processContext.OnBeginProcessField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__0);
			processContext.IsIgnoreEqualityComparison = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__1);
			processContext.IsUnhandledDictionary = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__2);
			processContext.IsEnum = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__3);
			processContext.IsValueType = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__4);
			ProcessContext arg_A9_0 = processContext;
			if (MakeEqualsProcessor.__f__am_cache0 == null)
			{
				MakeEqualsProcessor.__f__am_cache0 = new Action<FieldInfo, Type>(MakeEqualsProcessor._createProcessContext_m__0);
			}
			arg_A9_0.IsInterface = MakeEqualsProcessor.__f__am_cache0;
			processContext.IsDictionary = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__5);
			processContext.IsEnumerable = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__6);
			processContext.IsFixedCapacityList = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__7);
			processContext.IsMemberwiseEqualityObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__8);
			processContext.IsGenericObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__9);
			processContext.OnProcessedReferenceField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__A);
			processContext.OnProcessedField = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__B);
			return processContext;
		}

		private static void _createProcessContext_m__0(FieldInfo field, Type type)
		{
			throw new Exception("Unhandled case");
		}
	}
}
