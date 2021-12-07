// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	public static class AssignObjectProcessor
	{
		public enum AssignMethod
		{
			DeepAssign,
			ShallowAssign
		}

		private sealed class _createProcessContext_c__AnonStorey0
		{
			internal ILGenerator generator;

			internal AssignObjectProcessor.AssignMethod assignMethod;

			internal void __m__0(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
			}

			internal void __m__1(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
			}

			internal void __m__2(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
			}

			internal void __m__3(FieldInfo field, Type type)
			{
				if (this.assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneDictionary(field, this.generator, type);
				}
				else
				{
					Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
				}
			}

			internal void __m__4(FieldInfo field, Type type)
			{
				if (this.assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneEnumerable(field, this.generator, type);
				}
				else
				{
					Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
				}
			}

			internal void __m__5(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
			}

			internal void __m__6(FieldInfo field, Type type)
			{
				if (this.assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneCloneableObject(field, this.generator, type);
				}
				else
				{
					Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
				}
			}

			internal void __m__7(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, this.generator, type, Assigner.AssignMode.Assign);
			}
		}

		private static Action<FieldInfo, Type> __f__mg_cache0;

		private static Action<FieldInfo, Type> __f__mg_cache1;

		private static Action<FieldInfo, Type> __f__mg_cache2;

		private static Action<FieldInfo, Type> __f__am_cache0;

		private static Action<FieldInfo, Type> __f__am_cache1;

		public static Action<object, object> MakeAssignMethod(Type type, AssignObjectProcessor.AssignMethod assignMethod = AssignObjectProcessor.AssignMethod.DeepAssign)
		{
			DynamicMethod dynamicMethod;
			ILGenerator iLGenerator;
			Assigner.StartMakeAssignFunction(out dynamicMethod, out iLGenerator, type);
			ProcessContext context = AssignObjectProcessor.createProcessContext(type, iLGenerator, dynamicMethod, assignMethod);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			return Assigner.EndMakeAssignFunction(type, iLGenerator, dynamicMethod);
		}

		private static ProcessContext createProcessContext(Type objType, ILGenerator generator, DynamicMethod dynamicMethod, AssignObjectProcessor.AssignMethod assignMethod)
		{
			AssignObjectProcessor._createProcessContext_c__AnonStorey0 _createProcessContext_c__AnonStorey = new AssignObjectProcessor._createProcessContext_c__AnonStorey0();
			_createProcessContext_c__AnonStorey.generator = generator;
			_createProcessContext_c__AnonStorey.assignMethod = assignMethod;
			ProcessContext processContext = new ProcessContext();
			ProcessContext arg_38_0 = processContext;
			if (AssignObjectProcessor.__f__mg_cache0 == null)
			{
				AssignObjectProcessor.__f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_38_0.OnBeginProcessField = AssignObjectProcessor.__f__mg_cache0;
			processContext.IsIgnoreEqualityComparison = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__0);
			ProcessContext arg_6D_0 = processContext;
			if (AssignObjectProcessor.__f__am_cache0 == null)
			{
				AssignObjectProcessor.__f__am_cache0 = new Action<FieldInfo, Type>(AssignObjectProcessor._createProcessContext_m__0);
			}
			arg_6D_0.IsUnhandledDictionary = AssignObjectProcessor.__f__am_cache0;
			processContext.IsEnum = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__1);
			processContext.IsValueType = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__2);
			ProcessContext arg_B4_0 = processContext;
			if (AssignObjectProcessor.__f__am_cache1 == null)
			{
				AssignObjectProcessor.__f__am_cache1 = new Action<FieldInfo, Type>(AssignObjectProcessor._createProcessContext_m__1);
			}
			arg_B4_0.IsInterface = AssignObjectProcessor.__f__am_cache1;
			processContext.IsDictionary = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__3);
			processContext.IsEnumerable = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__4);
			processContext.IsFixedCapacityList = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__5);
			processContext.IsMemberwiseEqualityObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__6);
			processContext.IsGenericObject = new Action<FieldInfo, Type>(_createProcessContext_c__AnonStorey.__m__7);
			ProcessContext arg_131_0 = processContext;
			if (AssignObjectProcessor.__f__mg_cache1 == null)
			{
				AssignObjectProcessor.__f__mg_cache1 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_131_0.OnProcessedReferenceField = AssignObjectProcessor.__f__mg_cache1;
			ProcessContext arg_154_0 = processContext;
			if (AssignObjectProcessor.__f__mg_cache2 == null)
			{
				AssignObjectProcessor.__f__mg_cache2 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			arg_154_0.OnProcessedField = AssignObjectProcessor.__f__mg_cache2;
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

		private static void _createProcessContext_m__1(FieldInfo field, Type type)
		{
			throw new Exception("Unhandled case");
		}
	}
}
