// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollbackDebug
{
	public class DummyRollbackStateContainer : RollbackStateContainer
	{
		public enum Mode
		{
			Base,
			StateCountMismatch,
			ValueTypeMismatch,
			StructMismatch,
			EnumerableMismatch,
			NestedObjectMismatch
		}

		public DummyRollbackStateContainer.Mode MyMode
		{
			get;
			private set;
		}

		public DummyRollbackStateContainer(DummyRollbackStateContainer.Mode mode) : base(true)
		{
			this.MyMode = mode;
			switch (mode)
			{
			case DummyRollbackStateContainer.Mode.Base:
				IL_2D:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(1f, 2f, 3f)));
				return;
			case DummyRollbackStateContainer.Mode.StateCountMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				return;
			case DummyRollbackStateContainer.Mode.ValueTypeMismatch:
				base.WriteState(new DummyRollbackStateA(1, true, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(2f, 2f, 3f)));
				return;
			case DummyRollbackStateContainer.Mode.StructMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1,
					2
				}, new string[]
				{
					"a",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(2f, 2f, 3f)));
				return;
			case DummyRollbackStateContainer.Mode.EnumerableMismatch:
				base.WriteState(new DummyRollbackStateA(100, false, "C"));
				base.WriteState(new DummyRollbackStateB(new List<int>
				{
					1
				}, new string[]
				{
					"c",
					"b"
				}, null));
				base.WriteState(new DummyRollbackStateC(new Vector3(1f, 2f, 3f)));
				return;
			}
			goto IL_2D;
		}
	}
}
