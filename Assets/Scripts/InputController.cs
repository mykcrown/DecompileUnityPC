// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

public abstract class InputController : MonoBehaviour, ITickable, IMoveInput, IRollbackInputController, IRollbackStateOwner, IGameInput
{
	private Dictionary<ButtonPress, InputData> buttonInputDataMap;

	protected InputConfigData inputConfig;

	protected GameManager gameManager;

	protected InputStateSnapshot inputStateSnapshot;

	public Dictionary<InputData, int> inputDataIndexMap = new Dictionary<InputData, int>();

	private StringBuilder stringBuilderBuffer = new StringBuilder();

	private InputValue reusableInputValue = new InputValue();

	private InputValue valueBuffer = new InputValue();

	bool IGameInput.IsJumpInputPressed
	{
		get
		{
			return this.isTapJumpInputPressed || this.GetButton(ButtonPress.Jump);
		}
	}

	bool IGameInput.IsTapJumpInputPressed
	{
		get
		{
			return this.isTapJumpInputPressed;
		}
	}

	Fixed IGameInput.HorizontalAxisValue
	{
		get
		{
			return this.GetAxis(this.horizontalAxis);
		}
	}

	Fixed IGameInput.VerticalAxisValue
	{
		get
		{
			return this.GetAxis(this.verticalAxis);
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	public ReadOnlyCollection<InputData> allInputData
	{
		get;
		protected set;
	}

	public InputData horizontalAxis
	{
		get;
		protected set;
	}

	public InputData verticalAxis
	{
		get;
		protected set;
	}

	public abstract bool allowTapJumping
	{
		get;
	}

	public abstract bool allowTapStrike
	{
		get;
	}

	public abstract bool allowRecoveryJumping
	{
		get;
	}

	public bool autoFastFallEnabled
	{
		get
		{
			return false;
		}
	}

	public bool attackAssistEnabled
	{
		get
		{
			return false;
		}
	}

	public abstract bool requireDoubleTapToRun
	{
		get;
	}

	public bool AllowTapJumpThisFrame
	{
		get;
		private set;
	}

	public bool AllowTapStrikeThisFrame
	{
		get;
		private set;
	}

	public bool AllowRecoveryJumpingThisFrame
	{
		get;
		private set;
	}

	public bool AutoFastFallThisFrame
	{
		get
		{
			return false;
		}
	}

	public bool AttackAssistThisFrame
	{
		get
		{
			return false;
		}
	}

	public bool RequireDoubleTapToRunThisFrame
	{
		get;
		private set;
	}

	private bool useAsyncInputs
	{
		get
		{
			return this.gameManager != null && this.gameManager.FrameController.IsRollback;
		}
	}

	public bool isLocal
	{
		get;
		set;
	}

	public int InputCount
	{
		get
		{
			return this.inputStateSnapshot.Inputs.Length;
		}
	}

	public bool isTapJumpInputPressed
	{
		get
		{
			return this.GetFramesHeldDown(this.verticalAxis) > 1 && this.GetAxis(this.verticalAxis) > 0;
		}
	}

	public bool IsShieldInputPressed
	{
		get
		{
			return this.GetButton(ButtonPress.Shield1) || this.GetButton(ButtonPress.Shield2);
		}
	}

	public bool IsCrouchingInputPressed
	{
		get
		{
			return InputUtils.IsCrouchInput(this.GetAxis(this.verticalAxis), this.GetAxis(this.horizontalAxis), this.inputConfig);
		}
	}

	[PostConstruct]
	public virtual void Init()
	{
		this.isLocal = true;
		this.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	private void onGameInit(GameEvent message)
	{
		GameInitEvent gameInitEvent = message as GameInitEvent;
		this.gameManager = gameInitEvent.gameManager;
		this.inputStateSnapshot.Clear();
	}

	private void onEndGame(VictoryScreenPayload payload)
	{
		this.gameManager = null;
	}

	private void OnDestroy()
	{
		this.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		this.signalBus.GetSignal<EndGameSignal>().RemoveListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	public Vector2F GetAxisValue()
	{
		Fixed axis = this.GetAxis(this.horizontalAxis);
		Fixed axis2 = this.GetAxis(this.verticalAxis);
		return new Vector2F(axis, axis2);
	}

	public Fixed GetAxis(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.axis;
		}
		return 0;
	}

	public IntegerAxis GetIntegerAxis(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.IntAxis;
		}
		return null;
	}

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

	public int GetFramesHeldDown(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		if (currentState != null)
		{
			return currentState.framesHeldDown;
		}
		return 0;
	}

	public bool GetTapped(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.tapped;
	}

	public bool GetJustTapped(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.justTapped;
	}

	public void UpdateTapped(InputData inputData, bool tapped, int currentFrame)
	{
		InputState currentState = this.GetCurrentState(inputData);
		if (currentState != null)
		{
			currentState.UpdateTapped(tapped, currentFrame);
		}
		else
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Attempted to call UpdateTapped on ",
				inputData.button,
				" with a value of ",
				tapped,
				" which has no state"
			}));
		}
	}

	public Fixed GetPreviousAxis(InputData data)
	{
		InputState previousState = this.getPreviousState(data);
		if (previousState != null)
		{
			return previousState.axis;
		}
		return 0;
	}

	public bool GetAxisPressed(InputData data)
	{
		return this.GetAxis(data) != this.GetPreviousAxis(data);
	}

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

	public bool GetButton(ButtonPress buttonPress)
	{
		InputData inputData = null;
		if (this.buttonInputDataMap.ContainsKey(buttonPress))
		{
			inputData = this.buttonInputDataMap[buttonPress];
		}
		return inputData != null && this.GetButton(inputData);
	}

	public bool GetButton(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		return currentState != null && currentState.button;
	}

	public bool GetButtonUp(ButtonPress buttonPress)
	{
		InputData inputData = null;
		this.buttonInputDataMap.TryGetValue(buttonPress, out inputData);
		return inputData != null && this.GetButtonUp(inputData);
	}

	public bool GetButtonUp(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		InputState previousState = this.getPreviousState(data);
		return currentState != null && previousState != null && !currentState.button && previousState.button;
	}

	public bool GetButtonDown(ButtonPress buttonPress)
	{
		InputData inputData = null;
		this.buttonInputDataMap.TryGetValue(buttonPress, out inputData);
		return inputData != null && this.GetButtonDown(inputData);
	}

	public bool GetButtonDown(InputData data)
	{
		InputState currentState = this.GetCurrentState(data);
		InputState previousState = this.getPreviousState(data);
		return currentState != null && previousState != null && currentState.button && !previousState.button;
	}

	private InputState getPreviousState(InputData data)
	{
		return this.inputStateSnapshot.PreviousInputs[this.getInputDataIndex(data)];
	}

	public InputState GetCurrentState(InputData data)
	{
		return this.inputStateSnapshot.Inputs[this.getInputDataIndex(data)];
	}

	protected int getInputDataIndex(InputData data)
	{
		if (data == null)
		{
			UnityEngine.Debug.LogWarning("Tried to get index from null data");
		}
		return this.inputDataIndexMap[data];
	}

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
			foreach (DefaultInputBinding current in defaultBindings)
			{
				if (current != null && this.usesBinding(current, list2))
				{
					if (!PlayerInputActions.IsAxis(current.button))
					{
						InputData inputData = new InputData();
						inputData.inputType = InputType.Button;
						inputData.button = current.button;
						inputData.controlType = current.controlType;
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

	public virtual bool usesBinding(DefaultInputBinding binding, List<InputData> inputList)
	{
		return true;
	}

	public abstract void ReadAllInputs(ref InputValuesSnapshot values, InputValue inputValue, bool tauntsOnly);

	public abstract void ReadInputValue(InputData inputReference, InputValue values);

	protected virtual void beginReadingValues()
	{
	}

	protected virtual void endReadingValues()
	{
	}

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

	public void LoadInputValues(InputValuesSnapshot values)
	{
		if (values == null)
		{
			UnityEngine.Debug.LogWarning("InputController tried to load null values... bailing out");
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

	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<InputStateSnapshot>(this.inputStateSnapshot));
		return true;
	}

	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<InputStateSnapshot>(ref this.inputStateSnapshot);
		return true;
	}

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

	public virtual void DoUpdate()
	{
	}

	protected virtual void recordInputValue(int index, InputValue value)
	{
		this.inputStateSnapshot.RecordInput(value, index);
	}

	public static bool IsMetagameInput(ButtonPress button, bool includeAsync = false)
	{
		return button == ButtonPress.Start && includeAsync;
	}

	bool IGameInput.IsHorizontalDirectionHeld(HorizontalDirection dir)
	{
		return this.IsHorizontalDirectionHeld(dir);
	}

	public virtual void Vibrate(float leftMotor, float rightMotor, float duration)
	{
	}

	public Fixed GetHorizontalAxis()
	{
		return this.GetAxis(this.horizontalAxis);
	}

	public Fixed GetVerticalAxis()
	{
		return this.GetAxis(this.verticalAxis);
	}

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
}
