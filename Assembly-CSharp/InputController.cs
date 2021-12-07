using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FixedPoint;
using UnityEngine;

// Token: 0x02000699 RID: 1689
public abstract class InputController : MonoBehaviour, ITickable, IMoveInput, IRollbackInputController, IRollbackStateOwner, IGameInput
{
	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x060029AD RID: 10669 RVA: 0x0006138D File Offset: 0x0005F78D
	// (set) Token: 0x060029AE RID: 10670 RVA: 0x00061395 File Offset: 0x0005F795
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x060029AF RID: 10671 RVA: 0x0006139E File Offset: 0x0005F79E
	// (set) Token: 0x060029B0 RID: 10672 RVA: 0x000613A6 File Offset: 0x0005F7A6
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000613AF File Offset: 0x0005F7AF
	// (set) Token: 0x060029B2 RID: 10674 RVA: 0x000613B7 File Offset: 0x0005F7B7
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x060029B3 RID: 10675 RVA: 0x000613C0 File Offset: 0x0005F7C0
	// (set) Token: 0x060029B4 RID: 10676 RVA: 0x000613C8 File Offset: 0x0005F7C8
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000613D1 File Offset: 0x0005F7D1
	// (set) Token: 0x060029B6 RID: 10678 RVA: 0x000613D9 File Offset: 0x0005F7D9
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000613E2 File Offset: 0x0005F7E2
	// (set) Token: 0x060029B8 RID: 10680 RVA: 0x000613EA File Offset: 0x0005F7EA
	public ReadOnlyCollection<InputData> allInputData { get; protected set; }

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060029B9 RID: 10681 RVA: 0x000613F3 File Offset: 0x0005F7F3
	// (set) Token: 0x060029BA RID: 10682 RVA: 0x000613FB File Offset: 0x0005F7FB
	public InputData horizontalAxis { get; protected set; }

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x060029BB RID: 10683 RVA: 0x00061404 File Offset: 0x0005F804
	// (set) Token: 0x060029BC RID: 10684 RVA: 0x0006140C File Offset: 0x0005F80C
	public InputData verticalAxis { get; protected set; }

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x060029BD RID: 10685
	public abstract bool allowTapJumping { get; }

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x060029BE RID: 10686
	public abstract bool allowTapStrike { get; }

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x060029BF RID: 10687
	public abstract bool allowRecoveryJumping { get; }

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x060029C0 RID: 10688 RVA: 0x00061415 File Offset: 0x0005F815
	public bool autoFastFallEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x060029C1 RID: 10689 RVA: 0x00061418 File Offset: 0x0005F818
	public bool attackAssistEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x060029C2 RID: 10690
	public abstract bool requireDoubleTapToRun { get; }

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x060029C3 RID: 10691 RVA: 0x0006141B File Offset: 0x0005F81B
	// (set) Token: 0x060029C4 RID: 10692 RVA: 0x00061423 File Offset: 0x0005F823
	public bool AllowTapJumpThisFrame { get; private set; }

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x060029C5 RID: 10693 RVA: 0x0006142C File Offset: 0x0005F82C
	// (set) Token: 0x060029C6 RID: 10694 RVA: 0x00061434 File Offset: 0x0005F834
	public bool AllowTapStrikeThisFrame { get; private set; }

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0006143D File Offset: 0x0005F83D
	// (set) Token: 0x060029C8 RID: 10696 RVA: 0x00061445 File Offset: 0x0005F845
	public bool AllowRecoveryJumpingThisFrame { get; private set; }

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x060029C9 RID: 10697 RVA: 0x0006144E File Offset: 0x0005F84E
	public bool AutoFastFallThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x060029CA RID: 10698 RVA: 0x00061451 File Offset: 0x0005F851
	public bool AttackAssistThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x060029CB RID: 10699 RVA: 0x00061454 File Offset: 0x0005F854
	// (set) Token: 0x060029CC RID: 10700 RVA: 0x0006145C File Offset: 0x0005F85C
	public bool RequireDoubleTapToRunThisFrame { get; private set; }

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x060029CD RID: 10701 RVA: 0x00061465 File Offset: 0x0005F865
	private bool useAsyncInputs
	{
		get
		{
			return this.gameManager != null && this.gameManager.FrameController.IsRollback;
		}
	}

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x060029CE RID: 10702 RVA: 0x0006148B File Offset: 0x0005F88B
	// (set) Token: 0x060029CF RID: 10703 RVA: 0x00061493 File Offset: 0x0005F893
	public bool isLocal { get; set; }

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x060029D0 RID: 10704 RVA: 0x0006149C File Offset: 0x0005F89C
	public int InputCount
	{
		get
		{
			return this.inputStateSnapshot.Inputs.Length;
		}
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000614AC File Offset: 0x0005F8AC
	[PostConstruct]
	public virtual void Init()
	{
		this.isLocal = true;
		this.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x00061500 File Offset: 0x0005F900
	private void onGameInit(GameEvent message)
	{
		GameInitEvent gameInitEvent = message as GameInitEvent;
		this.gameManager = gameInitEvent.gameManager;
		this.inputStateSnapshot.Clear();
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x0006152B File Offset: 0x0005F92B
	private void onEndGame(VictoryScreenPayload payload)
	{
		this.gameManager = null;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x00061534 File Offset: 0x0005F934
	private void OnDestroy()
	{
		this.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		this.signalBus.GetSignal<EndGameSignal>().RemoveListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x00061574 File Offset: 0x0005F974
	public Vector2F GetAxisValue()
	{
		Fixed axis = this.GetAxis(this.horizontalAxis);
		Fixed axis2 = this.GetAxis(this.verticalAxis);
		return new Vector2F(axis, axis2);
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000615A4 File Offset: 0x0005F9A4
	public Fixed GetAxis(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.axis;
		}
		return 0;
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x000615CC File Offset: 0x0005F9CC
	public IntegerAxis GetIntegerAxis(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.IntAxis;
		}
		return null;
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000615F0 File Offset: 0x0005F9F0
	public int GetFramesHeldDown(ButtonPress press)
	{
		for (int i = 0; i < this.allInputData.Count; i++)
		{
			InputData inputData = this.allInputData[i];
			if (inputData != null && inputData.button == press)
			{
				return this.GetFramesHeldDown(inputData);
			}
		}
		return 0;
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x00061644 File Offset: 0x0005FA44
	public int GetFramesHeldDown(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.framesHeldDown;
		}
		return 0;
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x00061668 File Offset: 0x0005FA68
	public bool GetTapped(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.tapped;
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x0006168C File Offset: 0x0005FA8C
	public bool GetJustTapped(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.justTapped;
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x000616B0 File Offset: 0x0005FAB0
	public void UpdateTapped(InputData inputData, bool tapped, int currentFrame)
	{
		InputState currentState = this.GetCurrentState(inputData);
		if (currentState != null)
		{
			currentState.UpdateTapped(tapped, currentFrame);
		}
		else
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Attempted to call UpdateTapped on ",
				inputData.button,
				" with a value of ",
				tapped,
				" which has no state"
			}));
		}
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x00061718 File Offset: 0x0005FB18
	public Fixed GetPreviousAxis(InputData data)
	{
		InputState previousState = this.getPreviousState(data);
		if (previousState != null)
		{
			return previousState.axis;
		}
		return 0;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x00061740 File Offset: 0x0005FB40
	public bool GetAxisPressed(InputData data)
	{
		return this.GetAxis(data) != this.GetPreviousAxis(data);
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x00061758 File Offset: 0x0005FB58
	public bool IsHorizontalDirectionHeld(HorizontalDirection direction)
	{
		Fixed axis = this.GetAxis(this.horizontalAxis);
		if (direction == HorizontalDirection.Left)
		{
			return axis < 0;
		}
		if (direction != HorizontalDirection.Right)
		{
			return direction == HorizontalDirection.Any && FixedMath.Abs(axis) > 0;
		}
		return axis > 0;
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x000617AC File Offset: 0x0005FBAC
	public bool GetButton(ButtonPress buttonPress)
	{
		InputData inputData = null;
		if (this.buttonInputDataMap.ContainsKey(buttonPress))
		{
			inputData = this.buttonInputDataMap[buttonPress];
		}
		return inputData != null && this.GetButton(inputData);
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000617E8 File Offset: 0x0005FBE8
	public bool GetButton(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.button;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x0006180C File Offset: 0x0005FC0C
	public bool GetButtonUp(ButtonPress buttonPress)
	{
		InputData inputData = null;
		this.buttonInputDataMap.TryGetValue(buttonPress, out inputData);
		return inputData != null && this.GetButtonUp(inputData);
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x0006183C File Offset: 0x0005FC3C
	public bool GetButtonUp(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		InputState previousState = this.getPreviousState(data);
		return currentState != null && previousState != null && !currentState.button && previousState.button;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x0006187C File Offset: 0x0005FC7C
	public bool GetButtonDown(ButtonPress buttonPress)
	{
		InputData inputData = null;
		this.buttonInputDataMap.TryGetValue(buttonPress, out inputData);
		return inputData != null && this.GetButtonDown(inputData);
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000618AC File Offset: 0x0005FCAC
	public bool GetButtonDown(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		InputState previousState = this.getPreviousState(data);
		return currentState != null && previousState != null && currentState.button && !previousState.button;
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000618EE File Offset: 0x0005FCEE
	private InputState getPreviousState(InputData data)
	{
		return this.inputStateSnapshot.PreviousInputs[this.getInputDataIndex(data)];
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x00061903 File Offset: 0x0005FD03
	public InputState GetCurrentState(InputData data)
	{
		return this.inputStateSnapshot.Inputs[this.getInputDataIndex(data)];
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x00061918 File Offset: 0x0005FD18
	protected int getInputDataIndex(InputData data)
	{
		if (data == null)
		{
			Debug.LogWarning("Tried to get index from null data");
		}
		return this.inputDataIndexMap[data];
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x00061938 File Offset: 0x0005FD38
	public virtual void Initialize(InputConfigData options)
	{
		this.inputConfig = options;
		List<InputData> list = new List<InputData>();
		List<InputData> list2 = new List<InputData>();
		this.horizontalAxis = new InputData();
		this.horizontalAxis.inputType = InputType.HorizontalAxis;
		this.verticalAxis = new InputData();
		this.verticalAxis.inputType = InputType.VerticalAxis;
		list2.Add(this.horizontalAxis);
		list2.Add(this.verticalAxis);
		this.inputDataIndexMap.Add(this.horizontalAxis, 0);
		this.inputDataIndexMap.Add(this.verticalAxis, 1);
		int num = list2.Count;
		List<DefaultInputBinding> defaultBindings = options.defaultBindings;
		if (defaultBindings != null)
		{
			foreach (DefaultInputBinding defaultInputBinding in defaultBindings)
			{
				if (defaultInputBinding != null && this.usesBinding(defaultInputBinding, list2))
				{
					if (!PlayerInputActions.IsAxis(defaultInputBinding.button))
					{
						InputData inputData = new InputData();
						inputData.inputType = InputType.Button;
						inputData.button = defaultInputBinding.button;
						inputData.controlType = defaultInputBinding.controlType;
						list2.Add(inputData);
						this.inputDataIndexMap[inputData] = num;
						num++;
						list.Add(inputData);
					}
				}
			}
			int bufferSize = 2;
			if (list2.Count > InputStateSnapshot.INPUT_COUNT)
			{
				throw new Exception("Input list length exceeds buffer of " + InputStateSnapshot.INPUT_COUNT);
			}
			this.inputStateSnapshot = new InputStateSnapshot(bufferSize, InputStateSnapshot.INPUT_COUNT);
			for (int i = 0; i < list2.Count; i++)
			{
				this.inputStateSnapshot.Inputs[i] = new InputState();
			}
		}
		this.allInputData = new ReadOnlyCollection<InputData>(list2);
		this.buttonInputDataMap = new Dictionary<ButtonPress, InputData>(default(ButtonPressComparer));
		for (int j = 0; j < list.Count; j++)
		{
			InputData inputData2 = list[j];
			this.buttonInputDataMap[inputData2.button] = inputData2;
		}
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x00061B60 File Offset: 0x0005FF60
	public virtual bool usesBinding(DefaultInputBinding binding, List<InputData> inputList)
	{
		return true;
	}

	// Token: 0x060029EB RID: 10731
	public abstract void ReadAllInputs(ref InputValuesSnapshot values, InputValue inputValue, bool tauntsOnly);

	// Token: 0x060029EC RID: 10732
	public abstract void ReadInputValue(InputData inputReference, InputValue values);

	// Token: 0x060029ED RID: 10733 RVA: 0x00061B63 File Offset: 0x0005FF63
	protected virtual void beginReadingValues()
	{
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x00061B65 File Offset: 0x0005FF65
	protected virtual void endReadingValues()
	{
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x00061B68 File Offset: 0x0005FF68
	public void ReadPlayerInputValues(ref InputValuesSnapshot values, bool tauntsOnly)
	{
		if (values == null)
		{
			throw new Exception("Null input values");
		}
		this.beginReadingValues();
		this.ReadAllInputs(ref values, this.valueBuffer, tauntsOnly);
		values.SetFlag(InputValuesSnapshot.InputFlag.TapJump, this.allowTapJumping);
		values.SetFlag(InputValuesSnapshot.InputFlag.TapStrike, this.allowTapStrike);
		values.SetFlag(InputValuesSnapshot.InputFlag.RecoveryJumping, this.allowRecoveryJumping);
		values.SetFlag(InputValuesSnapshot.InputFlag.RequireDoubleTapToRun, this.requireDoubleTapToRun);
		this.endReadingValues();
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x00061BDC File Offset: 0x0005FFDC
	public void LoadInputValues(InputValuesSnapshot values)
	{
		if (values == null)
		{
			Debug.LogWarning("InputController tried to load null values... bailing out");
			return;
		}
		for (int i = 0; i < this.allInputData.Count; i++)
		{
			InputData inputData = this.allInputData[i];
			if (!InputController.IsMetagameInput(inputData.button, false))
			{
				int inputDataIndex = this.getInputDataIndex(inputData);
				this.valueBuffer.Clear();
				values.GetValue(inputData, this.valueBuffer);
				this.recordInputValue(inputDataIndex, this.valueBuffer);
			}
		}
		this.AllowTapJumpThisFrame = values.GetFlag(InputValuesSnapshot.InputFlag.TapJump);
		this.AllowTapStrikeThisFrame = values.GetFlag(InputValuesSnapshot.InputFlag.TapStrike);
		this.AllowRecoveryJumpingThisFrame = values.GetFlag(InputValuesSnapshot.InputFlag.RecoveryJumping);
		this.RequireDoubleTapToRunThisFrame = values.GetFlag(InputValuesSnapshot.InputFlag.RequireDoubleTapToRun);
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x00061C9A File Offset: 0x0006009A
	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<InputStateSnapshot>(this.inputStateSnapshot));
		return true;
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x00061CB6 File Offset: 0x000600B6
	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<InputStateSnapshot>(ref this.inputStateSnapshot);
		return true;
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x00061CC8 File Offset: 0x000600C8
	public virtual void TickFrame()
	{
		if (this.gameManager == null && this.allInputData != null)
		{
			for (int i = 0; i < this.allInputData.Count; i++)
			{
				if (!InputController.IsMetagameInput(this.allInputData[i].button, false))
				{
					this.reusableInputValue.Clear();
					this.ReadInputValue(this.allInputData[i], this.reusableInputValue);
					this.recordInputValue(i, this.reusableInputValue);
				}
			}
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x00061D59 File Offset: 0x00060159
	public virtual void DoUpdate()
	{
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x00061D5B File Offset: 0x0006015B
	protected virtual void recordInputValue(int index, InputValue value)
	{
		this.inputStateSnapshot.RecordInput(value, index);
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x00061D6A File Offset: 0x0006016A
	public static bool IsMetagameInput(ButtonPress button, bool includeAsync = false)
	{
		return button == ButtonPress.Start && includeAsync;
	}

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x060029F7 RID: 10743 RVA: 0x00061D7B File Offset: 0x0006017B
	bool IGameInput.IsJumpInputPressed
	{
		get
		{
			return this.isTapJumpInputPressed || this.GetButton(ButtonPress.Jump);
		}
	}

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x060029F8 RID: 10744 RVA: 0x00061D93 File Offset: 0x00060193
	bool IGameInput.IsTapJumpInputPressed
	{
		get
		{
			return this.isTapJumpInputPressed;
		}
	}

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x060029F9 RID: 10745 RVA: 0x00061D9B File Offset: 0x0006019B
	public bool isTapJumpInputPressed
	{
		get
		{
			return this.GetFramesHeldDown(this.verticalAxis) > 1 && this.GetAxis(this.verticalAxis) > 0;
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x00061DC4 File Offset: 0x000601C4
	bool IGameInput.IsHorizontalDirectionHeld(HorizontalDirection dir)
	{
		return this.IsHorizontalDirectionHeld(dir);
	}

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x060029FB RID: 10747 RVA: 0x00061DCD File Offset: 0x000601CD
	Fixed IGameInput.HorizontalAxisValue
	{
		get
		{
			return this.GetAxis(this.horizontalAxis);
		}
	}

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x060029FC RID: 10748 RVA: 0x00061DDB File Offset: 0x000601DB
	Fixed IGameInput.VerticalAxisValue
	{
		get
		{
			return this.GetAxis(this.verticalAxis);
		}
	}

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x060029FD RID: 10749 RVA: 0x00061DE9 File Offset: 0x000601E9
	public bool IsShieldInputPressed
	{
		get
		{
			return this.GetButton(ButtonPress.Shield1) || this.GetButton(ButtonPress.Shield2);
		}
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x060029FE RID: 10750 RVA: 0x00061E02 File Offset: 0x00060202
	public bool IsCrouchingInputPressed
	{
		get
		{
			return InputUtils.IsCrouchInput(this.GetAxis(this.verticalAxis), this.GetAxis(this.horizontalAxis), this.inputConfig);
		}
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x00061E27 File Offset: 0x00060227
	public virtual void Vibrate(float leftMotor, float rightMotor, float duration)
	{
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x00061E29 File Offset: 0x00060229
	public Fixed GetHorizontalAxis()
	{
		return this.GetAxis(this.horizontalAxis);
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x00061E37 File Offset: 0x00060237
	public Fixed GetVerticalAxis()
	{
		return this.GetAxis(this.verticalAxis);
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x00061E48 File Offset: 0x00060248
	public string GenerateDebugString()
	{
		bool flag = false;
		this.stringBuilderBuffer.Length = 0;
		this.stringBuilderBuffer.AppendFormat("Move Inputs[{0}]:", this.gameManager.Frame);
		for (int i = 0; i < this.allInputData.Count; i++)
		{
			InputData inputData = this.allInputData[i];
			if (inputData != null)
			{
				InputState currentState = this.GetCurrentState(inputData);
				bool flag2 = currentState.tapped || currentState.justTapped;
				if (currentState.axis != 0)
				{
					flag = true;
					this.stringBuilderBuffer.AppendFormat("{0} axis:{1}[{2}]{3}{4}", new object[]
					{
						inputData.button,
						currentState.axis,
						currentState.framesHeldDown,
						(!currentState.justTapped) ? string.Empty : "!",
						(!currentState.tapped) ? string.Empty : "!"
					});
				}
				else if (flag2)
				{
					flag = true;
					this.stringBuilderBuffer.AppendFormat("{0}(tapped),", inputData.button);
				}
				else if (currentState.framesHeldDown > 0)
				{
					flag = true;
					this.stringBuilderBuffer.AppendFormat("{0} held:{1},", inputData.button, currentState.framesHeldDown);
				}
			}
		}
		if (!flag)
		{
			this.stringBuilderBuffer.Append("--");
		}
		this.stringBuilderBuffer.AppendFormat("\n", Array.Empty<object>());
		return this.stringBuilderBuffer.ToString();
	}

	// Token: 0x04001DFD RID: 7677
	private Dictionary<ButtonPress, InputData> buttonInputDataMap;

	// Token: 0x04001E04 RID: 7684
	protected InputConfigData inputConfig;

	// Token: 0x04001E05 RID: 7685
	protected GameManager gameManager;

	// Token: 0x04001E06 RID: 7686
	protected InputStateSnapshot inputStateSnapshot;

	// Token: 0x04001E07 RID: 7687
	public Dictionary<InputData, int> inputDataIndexMap = new Dictionary<InputData, int>();

	// Token: 0x04001E08 RID: 7688
	private StringBuilder stringBuilderBuffer = new StringBuilder();

	// Token: 0x04001E09 RID: 7689
	private InputValue reusableInputValue = new InputValue();

	// Token: 0x04001E0B RID: 7691
	private InputValue valueBuffer = new InputValue();
}
