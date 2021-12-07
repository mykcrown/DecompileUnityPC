using System;
using System.Collections.Generic;
using FixedPoint;

namespace AI
{
	// Token: 0x02000324 RID: 804
	public class BehaviorTree
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0006470E File Offset: 0x00062B0E
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x00064716 File Offset: 0x00062B16
		[Inject]
		public IAICalculator calculator { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x0006471F File Offset: 0x00062B1F
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00064727 File Offset: 0x00062B27
		[Inject]
		public IInputConverter inputConverter { private get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x00064730 File Offset: 0x00062B30
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x00064738 File Offset: 0x00062B38
		[Inject]
		public GameController gameController { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x00064741 File Offset: 0x00062B41
		// (set) Token: 0x06001137 RID: 4407 RVA: 0x00064749 File Offset: 0x00062B49
		[Inject]
		public ITranslateTreeData translator { private get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00064752 File Offset: 0x00062B52
		public GameManager gameManager
		{
			get
			{
				return this.gameController.currentGame;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0006475F File Offset: 0x00062B5F
		public void Init(PlayerReference playerRef, CompositeNodeData rootNode)
		{
			this.playerRef = playerRef;
			this.root = this.translator.Translate(rootNode);
			this.root.Init(this);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00064786 File Offset: 0x00062B86
		public void TickFrame()
		{
			this.inputList.Clear();
			this.root.TickFrame();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x000647A0 File Offset: 0x00062BA0
		public void ReadInput(InputData inputReference, InputValue values)
		{
			foreach (Input input in this.inputList)
			{
				if (input.inputType == inputReference.inputType)
				{
					if (inputReference.inputType == InputType.HorizontalAxis || inputReference.inputType == InputType.VerticalAxis)
					{
						values.Set(input.value, false);
					}
					else if (inputReference.inputType == InputType.Button && inputReference.button == input.button)
					{
						values.Set(0f, true);
					}
				}
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0006485C File Offset: 0x00062C5C
		public void AddInput(InputType inputType, float value)
		{
			Input item = default(Input);
			item.inputType = inputType;
			item.value = value;
			this.inputList.Add(item);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00064890 File Offset: 0x00062C90
		public void AddInput(InputType inputType, ButtonPress button)
		{
			Input item = default(Input);
			item.inputType = inputType;
			item.value = 0f;
			item.button = button;
			this.inputList.Add(item);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x000648CD File Offset: 0x00062CCD
		public void AddInput(Macro macro)
		{
			this.inputConverter.AddMacro(ref this.inputList, macro);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x000648E1 File Offset: 0x00062CE1
		public Fixed GenerateRandomNumber()
		{
			return this.calculator.GenerateRandomNumber();
		}

		// Token: 0x04000AEA RID: 2794
		private INode root;

		// Token: 0x04000AEB RID: 2795
		public PlayerReference playerRef;

		// Token: 0x04000AEC RID: 2796
		private List<Input> inputList = new List<Input>(8);
	}
}
