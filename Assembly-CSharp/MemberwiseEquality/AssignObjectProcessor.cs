using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MemberwiseEquality
{
	// Token: 0x02000B25 RID: 2853
	public static class AssignObjectProcessor
	{
		// Token: 0x060052E1 RID: 21217 RVA: 0x001AC154 File Offset: 0x001AA554
		public static Action<object, object> MakeAssignMethod(Type type, AssignObjectProcessor.AssignMethod assignMethod = AssignObjectProcessor.AssignMethod.DeepAssign)
		{
			DynamicMethod dynamicMethod;
			ILGenerator ilgenerator;
			Assigner.StartMakeAssignFunction(out dynamicMethod, out ilgenerator, type);
			ProcessContext context = AssignObjectProcessor.createProcessContext(type, ilgenerator, dynamicMethod, assignMethod);
			MemberwiseEqualityObjectProcessor.ProcessType(type, context);
			return Assigner.EndMakeAssignFunction(type, ilgenerator, dynamicMethod);
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x001AC188 File Offset: 0x001AA588
		private static ProcessContext createProcessContext(Type objType, ILGenerator generator, DynamicMethod dynamicMethod, AssignObjectProcessor.AssignMethod assignMethod)
		{
			ProcessContext processContext = new ProcessContext();
			ProcessContext processContext2 = processContext;
			if (AssignObjectProcessor.f__mg_cache0 == null)
			{
				AssignObjectProcessor.f__mg_cache0 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext2.OnBeginProcessField = AssignObjectProcessor.f__mg_cache0;
			processContext.IsIgnoreEqualityComparison = delegate(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
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
			};
			processContext.IsEnum = delegate(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
			};
			processContext.IsValueType = delegate(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
			};
			processContext.IsInterface = delegate(FieldInfo field, Type type)
			{
				throw new Exception("Unhandled case");
			};
			processContext.IsDictionary = delegate(FieldInfo field, Type type)
			{
				if (assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneDictionary(field, generator, type);
				}
				else
				{
					Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
				}
			};
			processContext.IsEnumerable = delegate(FieldInfo field, Type type)
			{
				if (assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneEnumerable(field, generator, type);
				}
				else
				{
					Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
				}
			};
			processContext.IsFixedCapacityList = delegate(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
			};
			processContext.IsMemberwiseEqualityObject = delegate(FieldInfo field, Type type)
			{
				if (assignMethod != AssignObjectProcessor.AssignMethod.ShallowAssign)
				{
					Assigner.CloneCloneableObject(field, generator, type);
				}
				else
				{
					Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
				}
			};
			processContext.IsGenericObject = delegate(FieldInfo field, Type type)
			{
				Assigner.AssignField(field, generator, type, Assigner.AssignMode.Assign);
			};
			ProcessContext processContext3 = processContext;
			if (AssignObjectProcessor.f__mg_cache1 == null)
			{
				AssignObjectProcessor.f__mg_cache1 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext3.OnProcessedReferenceField = AssignObjectProcessor.f__mg_cache1;
			ProcessContext processContext4 = processContext;
			if (AssignObjectProcessor.f__mg_cache2 == null)
			{
				AssignObjectProcessor.f__mg_cache2 = new Action<FieldInfo, Type>(ProcessContext.NoOp);
			}
			processContext4.OnProcessedField = AssignObjectProcessor.f__mg_cache2;
			return processContext;
		}

		// Token: 0x040034A5 RID: 13477
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache0;

		// Token: 0x040034A6 RID: 13478
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache1;

		// Token: 0x040034A7 RID: 13479
		[CompilerGenerated]
		private static Action<FieldInfo, Type> f__mg_cache2;

		// Token: 0x02000B26 RID: 2854
		public enum AssignMethod
		{
			// Token: 0x040034AB RID: 13483
			DeepAssign,
			// Token: 0x040034AC RID: 13484
			ShallowAssign
		}
	}
}
