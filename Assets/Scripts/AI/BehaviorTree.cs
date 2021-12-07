// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

namespace AI
{
	public class BehaviorTree
	{
		private INode root;

		public PlayerReference playerRef;

		private List<Input> inputList = new List<Input>(8);

		[Inject]
		public IAICalculator calculator
		{
			get;
			set;
		}

		[Inject]
		public IInputConverter inputConverter
		{
			private get;
			set;
		}

		[Inject]
		public GameController gameController
		{
			get;
			set;
		}

		[Inject]
		public ITranslateTreeData translator
		{
			private get;
			set;
		}

		public GameManager gameManager
		{
			get
			{
				return this.gameController.currentGame;
			}
		}

		public void Init(PlayerReference playerRef, CompositeNodeData rootNode)
		{
			this.playerRef = playerRef;
			this.root = this.translator.Translate(rootNode);
			this.root.Init(this);
		}

		public void TickFrame()
		{
			this.inputList.Clear();
			this.root.TickFrame();
		}

		public void ReadInput(InputData inputReference, InputValue values)
		{
			foreach (Input current in this.inputList)
			{
				if (current.inputType == inputReference.inputType)
				{
					if (inputReference.inputType == InputType.HorizontalAxis || inputReference.inputType == InputType.VerticalAxis)
					{
						values.Set(current.value, false);
					}
					else if (inputReference.inputType == InputType.Button && inputReference.button == current.button)
					{
						values.Set(0f, true);
					}
				}
			}
		}

		public void AddInput(InputType inputType, float value)
		{
			Input item = default(Input);
			item.inputType = inputType;
			item.value = value;
			this.inputList.Add(item);
		}

		public void AddInput(InputType inputType, ButtonPress button)
		{
			Input item = default(Input);
			item.inputType = inputType;
			item.value = 0f;
			item.button = button;
			this.inputList.Add(item);
		}

		public void AddInput(Macro macro)
		{
			this.inputConverter.AddMacro(ref this.inputList, macro);
		}

		public Fixed GenerateRandomNumber()
		{
			return this.calculator.GenerateRandomNumber();
		}
	}
}
