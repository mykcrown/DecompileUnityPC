// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveSet : IMoveSet
{
	private enum MoveSearchResult
	{
		Found,
		ContinueSearch,
		BreakSearch
	}

	public static Fixed NORMAL_Y_TO_X_RATIO = 1;

	public static Fixed RECOVERY_Y_TO_X_RATIO = (Fixed)0.5;

	private CharacterMoveSetData moveSetData;

	private MoveData[] moves;

	private Dictionary<ButtonPress, MoveData> tauntMapping = new Dictionary<ButtonPress, MoveData>();

	private EqualityUtil.ButtonPressEqualityComparer ButtonPressComparer = new EqualityUtil.ButtonPressEqualityComparer();

	private List<ButtonPress> buttonPressBuffer = new List<ButtonPress>();

	private List<ButtonPress> buttonHeldBuffer = new List<ButtonPress>();

	private List<ButtonPress> buttonScanBuffer = new List<ButtonPress>();

	private Dictionary<string, MoveData> cacheMovesByName = new Dictionary<string, MoveData>();

	private Dictionary<MoveLabel, MoveData> cacheMovesByLabel = new Dictionary<MoveLabel, MoveData>(default(MoveLabelComparer));

	private Dictionary<MoveLabel, MoveData[]> cacheAllMovesWithLabel = new Dictionary<MoveLabel, MoveData[]>(default(MoveLabelComparer));

	private MoveData[] cacheEmptyList = new MoveData[0];

	private Dictionary<ButtonPress, MoveData[]> cacheMovesForButtonPress = new Dictionary<ButtonPress, MoveData[]>(default(ButtonPressComparer));

	CharacterMoveSetData IMoveSet.MoveSetData
	{
		get
		{
			return this.moveSetData;
		}
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public CharacterActionSet Actions
	{
		get
		{
			return this.moveSetData.characterActions;
		}
	}

	public void Init(CharacterMoveSetData moveSetData, Dictionary<ButtonPress, MoveData> tauntOverrides)
	{
		this.moveSetData = moveSetData;
		this.moves = (MoveData[])moveSetData.moves.Clone();
		if (tauntOverrides != null)
		{
			this.tauntMapping[ButtonPress.TauntLeft] = ((!tauntOverrides.ContainsKey(ButtonPress.TauntLeft)) ? null : tauntOverrides[ButtonPress.TauntLeft]);
			this.tauntMapping[ButtonPress.TauntRight] = ((!tauntOverrides.ContainsKey(ButtonPress.TauntRight)) ? null : tauntOverrides[ButtonPress.TauntRight]);
			this.tauntMapping[ButtonPress.TauntUp] = ((!tauntOverrides.ContainsKey(ButtonPress.TauntUp)) ? null : tauntOverrides[ButtonPress.TauntUp]);
			this.tauntMapping[ButtonPress.TauntDown] = ((!tauntOverrides.ContainsKey(ButtonPress.TauntDown)) ? null : tauntOverrides[ButtonPress.TauntDown]);
		}
		else
		{
			this.tauntMapping[ButtonPress.TauntLeft] = null;
			this.tauntMapping[ButtonPress.TauntRight] = null;
			this.tauntMapping[ButtonPress.TauntUp] = null;
			this.tauntMapping[ButtonPress.TauntDown] = null;
		}
		if (this.injector != null)
		{
			MoveData[] array = this.moves;
			for (int i = 0; i < array.Length; i++)
			{
				MoveData moveData = array[i];
				MoveComponent[] components = moveData.components;
				for (int j = 0; j < components.Length; j++)
				{
					MoveComponent moveComponent = components[j];
					if (moveComponent != null)
					{
						this.injector.Inject(moveComponent);
					}
				}
			}
		}
		for (int k = 0; k < this.moves.Length; k++)
		{
			MoveData moveData2 = this.moves[k];
			for (int l = 0; l < this.moves.Length; l++)
			{
				MoveData moveData3 = this.moves[l];
				if (moveData2.name != moveData3.name && moveData2.moveName == moveData3.moveName)
				{
					UnityEngine.Debug.LogWarning(string.Concat(new string[]
					{
						"Warning: ",
						moveData2.name,
						" (",
						moveData2.moveName,
						") has the same name as ",
						moveData3.name
					}));
				}
			}
		}
		Array.Sort<MoveData>(this.moves, new Comparison<MoveData>(this.compareMoves));
		this.buildCaches();
	}

	private void buildCaches()
	{
		this.buildMoveLookupCaches();
		this.buildInputScanCaches();
	}

	private void buildInputScanCaches()
	{
		Dictionary<ButtonPress, List<MoveData>> dictionary = new Dictionary<ButtonPress, List<MoveData>>();
		Array values = Enum.GetValues(typeof(ButtonPress));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ButtonPress button = (ButtonPress)enumerator.Current;
				this.buildInputScanCacheWithButton(button, dictionary);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		foreach (ButtonPress current in dictionary.Keys)
		{
			this.cacheMovesForButtonPress[current] = dictionary[current].ToArray();
		}
	}

	private void buildInputScanCacheWithButton(ButtonPress button, Dictionary<ButtonPress, List<MoveData>> tempMovesForButtonPress)
	{
		tempMovesForButtonPress[button] = new List<MoveData>();
		if (this.tauntMapping.ContainsKey(button))
		{
			if (this.tauntMapping[button] != null)
			{
				tempMovesForButtonPress[button].Add(this.tauntMapping[button]);
			}
		}
		else
		{
			for (int i = 0; i < this.moves.Length; i++)
			{
				MoveData moveData = this.moves[i];
				if (button == ButtonPress.None)
				{
					if (this.moveUsesNonPressInput(moveData))
					{
						tempMovesForButtonPress[button].Add(moveData);
					}
				}
				else if (this.moveUsesButtonPress(button, moveData))
				{
					tempMovesForButtonPress[button].Add(moveData);
				}
			}
		}
	}

	private bool moveUsesButtonPress(ButtonPress button, MoveData move)
	{
		if (move.activeInputProfile != null)
		{
			InputProfileEntry[] entries = move.activeInputProfile.entries;
			for (int i = 0; i < entries.Length; i++)
			{
				InputProfileEntry inputProfileEntry = entries[i];
				if (inputProfileEntry.buttonsPressed.Length != 0)
				{
					ButtonPress[] buttonsPressed = inputProfileEntry.buttonsPressed;
					for (int j = 0; j < buttonsPressed.Length; j++)
					{
						ButtonPress buttonPress = buttonsPressed[j];
						if (buttonPress == button)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private bool moveUsesNonPressInput(MoveData move)
	{
		if (move.activeInputProfile != null)
		{
			InputProfileEntry[] entries = move.activeInputProfile.entries;
			for (int i = 0; i < entries.Length; i++)
			{
				InputProfileEntry inputProfileEntry = entries[i];
				if (inputProfileEntry.buttonsPressed.Length == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void buildMoveLookupCaches()
	{
		Dictionary<MoveLabel, List<MoveData>> dictionary = new Dictionary<MoveLabel, List<MoveData>>();
		for (int i = 0; i < this.moves.Length; i++)
		{
			MoveData moveData = this.moves[i];
			if (!this.cacheMovesByName.ContainsKey(moveData.moveName))
			{
				this.cacheMovesByName[moveData.moveName] = moveData;
			}
			if (!this.cacheMovesByLabel.ContainsKey(moveData.label))
			{
				this.cacheMovesByLabel[moveData.label] = moveData;
			}
			if (!dictionary.ContainsKey(moveData.label))
			{
				dictionary[moveData.label] = new List<MoveData>();
			}
			dictionary[moveData.label].Add(moveData);
		}
		foreach (MoveLabel current in dictionary.Keys)
		{
			this.cacheAllMovesWithLabel[current] = dictionary[current].ToArray();
		}
	}

	void IMoveSet.LoadMoveSet(CharacterMoveSetData moveSetData)
	{
		this.moveSetData = moveSetData;
	}

	private bool isAerial(MoveData move)
	{
		return move.label == MoveLabel.UpAir || move.label == MoveLabel.DownAir || move.label == MoveLabel.ForwardAir || move.label == MoveLabel.BackwardAir || move.label == MoveLabel.NeutralAir;
	}

	private int compareMoves(MoveData move1, MoveData move2)
	{
		if (move1 == move2)
		{
			return 0;
		}
		if (move1.isRecovery != move2.isRecovery)
		{
			return (!move1.isRecovery) ? 1000000000 : (-1000000000);
		}
		if (move1.activeInputProfile == null != (move2.activeInputProfile == null))
		{
			return (!(move1.activeInputProfile == null)) ? 100000000 : (-100000000);
		}
		if (move1.activeInputProfile == null && move2.activeInputProfile == null)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		if (move1.activeInputProfile.entries.Length > 0)
		{
			num = move1.activeInputProfile.entries[0].buttonsPressed.Length;
		}
		if (move2.activeInputProfile.entries.Length > 0)
		{
			num2 = move2.activeInputProfile.entries[0].buttonsPressed.Length;
		}
		if (num != num2)
		{
			return (num <= num2) ? 10000000 : (-10000000);
		}
		if (move1.IsGrab != move2.IsGrab)
		{
			return (!move1.IsGrab) ? 1000000 : (-1000000);
		}
		if (!this.isAerial(move1) || this.isAerial(move2))
		{
		}
		if (num > 0 && num2 > 0 && InputUtils.IsDirection(move1.activeInputProfile.entries[0].buttonsPressed[0]) != InputUtils.IsDirection(move2.activeInputProfile.entries[0].buttonsPressed[0]))
		{
			return (!InputUtils.IsDirection(move1.activeInputProfile.entries[0].buttonsPressed[0])) ? 10000 : (-10000);
		}
		int num3 = 0;
		int num4 = 0;
		if (move1.activeInputProfile.entries.Length > 0)
		{
			num3 = move1.activeInputProfile.entries[0].buttonsHeld.Length;
		}
		if (move2.activeInputProfile.entries.Length > 0)
		{
			num4 = move2.activeInputProfile.entries[0].buttonsHeld.Length;
		}
		if (num3 != num4)
		{
			return (num3 <= num4) ? 1000 : (-1000);
		}
		if (num3 > 0 && num4 > 0)
		{
			bool flag = InputUtils.IsTap(move1.activeInputProfile.entries[0].buttonsHeld[0]);
			bool flag2 = InputUtils.IsTap(move2.activeInputProfile.entries[0].buttonsHeld[0]);
			if (flag != flag2)
			{
				return (!flag) ? 100 : (-100);
			}
			bool flag3 = InputUtils.IsVertical(move1.activeInputProfile.entries[0].buttonsHeld[0]);
			bool flag4 = InputUtils.IsVertical(move2.activeInputProfile.entries[0].buttonsHeld[0]);
			if (flag3 != flag4)
			{
				return (!flag3) ? 10 : (-10);
			}
		}
		for (int i = 0; i < move1.components.Length; i++)
		{
			MoveComponent moveComponent = move1.components[i];
			if (moveComponent is IMoveCompareComponent)
			{
				int num5 = ((IMoveCompareComponent)moveComponent).CompareMove(move2);
				if (num5 != 0)
				{
					return num5;
				}
			}
		}
		if (move1.label == MoveLabel.DashAttack != (move2.label == MoveLabel.DashAttack))
		{
			return (move1.label != MoveLabel.DashAttack) ? (-1) : 1;
		}
		return 0;
	}

	public bool CanInputBufferMove(MoveData move, List<ButtonPress> moveButtonsPressed)
	{
		InputProfileEntry[] entries = move.activeInputProfile.entries;
		for (int i = 0; i < entries.Length; i++)
		{
			InputProfileEntry inputProfileEntry = entries[i];
			int num = this.listSubset<ButtonPress>(inputProfileEntry.buttonsPressed, moveButtonsPressed, ref this.buttonPressBuffer, this.ButtonPressComparer, true);
			if (num > 0)
			{
				return true;
			}
		}
		return false;
	}

	public MoveData GetMove(string moveName)
	{
		if (this.cacheMovesByName.ContainsKey(moveName))
		{
			return this.cacheMovesByName[moveName];
		}
		return null;
	}

	public MoveData GetMove(MoveLabel label)
	{
		if (this.cacheMovesByLabel.ContainsKey(label))
		{
			return this.cacheMovesByLabel[label];
		}
		UnityEngine.Debug.LogError("Move " + label + " not found, this shouldn't happen.");
		return null;
	}

	public MoveData[] GetMoves(MoveLabel label)
	{
		if (this.cacheAllMovesWithLabel.ContainsKey(label))
		{
			return this.cacheAllMovesWithLabel[label];
		}
		return this.cacheEmptyList;
	}

	public MoveData GetMove(InputButtonsData inputButtonsData, IPlayerDelegate player, out InterruptData interruptData, ref ButtonPress buttonUsed)
	{
		interruptData = null;
		if (inputButtonsData.moveButtonsPressed.Count > 0 || inputButtonsData.buttonsHeld.Count > 0)
		{
			this.buttonScanBuffer.Clear();
			this.buttonScanBuffer.Add(ButtonPress.None);
			foreach (ButtonPress current in inputButtonsData.moveButtonsPressed)
			{
				this.buttonScanBuffer.Add(current);
				switch (current)
				{
				case ButtonPress.ForwardTap:
					this.buttonScanBuffer.Add(ButtonPress.Forward);
					this.buttonScanBuffer.Add(ButtonPress.SideTap);
					break;
				case ButtonPress.BackwardTap:
					this.buttonScanBuffer.Add(ButtonPress.Backward);
					this.buttonScanBuffer.Add(ButtonPress.SideTap);
					break;
				case ButtonPress.UpTap:
					this.buttonScanBuffer.Add(ButtonPress.Up);
					break;
				case ButtonPress.DownTap:
					this.buttonScanBuffer.Add(ButtonPress.Down);
					break;
				}
			}
			foreach (ButtonPress current2 in this.buttonScanBuffer)
			{
				MoveData[] array = this.cacheMovesForButtonPress[current2];
				MoveData[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					MoveData moveData = array2[i];
					MoveSet.MoveSearchResult moveSearchResult = this.canActivateMove(moveData, player, inputButtonsData, out interruptData, ref buttonUsed);
					if (buttonUsed == ButtonPress.Taunt)
					{
						buttonUsed = current2;
					}
					if (moveSearchResult == MoveSet.MoveSearchResult.Found)
					{
						MoveData result = moveData;
						return result;
					}
					if (moveSearchResult == MoveSet.MoveSearchResult.BreakSearch)
					{
						MoveData result = null;
						return result;
					}
				}
			}
		}
		return null;
	}

	private bool listContainsMove(MoveData move, MoveData[] moveList)
	{
		for (int i = 0; i < moveList.Length; i++)
		{
			if (moveList[i].Equals(move))
			{
				return true;
			}
		}
		return false;
	}

	private bool canInterrupt(MoveData move, InterruptData[] interrupts, int currentFrame, out InterruptData interruptData)
	{
		interruptData = null;
		for (int i = 0; i < interrupts.Length; i++)
		{
			InterruptData interruptData2 = interrupts[i];
			if (interruptData2.interruptType == InterruptType.Move)
			{
				if (interruptData2.triggerType == InterruptTriggerType.None)
				{
					if (currentFrame >= interruptData2.startFrame && currentFrame <= interruptData2.endFrame)
					{
						for (int j = 0; j < interruptData2.linkableMoves.Length; j++)
						{
							MoveData moveData = interruptData2.linkableMoves[j];
							if (!(moveData == null))
							{
								if (moveData.Equals(move))
								{
									interruptData = interruptData2;
									return true;
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	private bool validateRequirements(List<ICharacterComponent> validators, MoveData move, IPlayerDelegate player, InputButtonsData input, bool requireAllTrue)
	{
		bool result = validators.Count == 0;
		for (int i = 0; i < validators.Count; i++)
		{
			if (!validators[i].ValidateRequirements(move, player, input))
			{
				if (requireAllTrue)
				{
					return false;
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	private bool validateRequirements(IMoveRequirementValidator[] validators, MoveData move, IPlayerDelegate player, InputButtonsData input, bool requireAllTrue)
	{
		bool result = validators.Length == 0;
		for (int i = 0; i < validators.Length; i++)
		{
			if (!validators[i].ValidateRequirements(move, player, input))
			{
				if (requireAllTrue)
				{
					return false;
				}
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	private MoveSet.MoveSearchResult canActivateMove(MoveData move, IPlayerDelegate player, InputButtonsData inputButtons, out InterruptData interruptData, ref ButtonPress buttonUsed)
	{
		interruptData = null;
		if (player.InputController == null)
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		if (move.activeInputProfile == null || move.label == MoveLabel.AllyAssist)
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.AttackAssist) && player.InputController.AttackAssistThisFrame && move.label == MoveLabel.DashAttack)
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		MoveModel moveModel = (!player.ActiveMove.IsActive) ? null : player.ActiveMove.Model;
		if (move.requiredMoves.Length > 0 && (moveModel == null || !this.listContainsMove(moveModel.data, move.requiredMoves)))
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		if (moveModel != null && !moveModel.IsInterruptibleByAnything(player) && !this.canInterrupt(move, moveModel.data.interrupts, moveModel.internalFrame, out interruptData))
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		if (!this.validateRequirements(move.components, move, player, inputButtons, true) || !this.validateRequirements(player.Components, move, player, inputButtons, true))
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		bool flag = false;
		InputProfileEntry[] entries = move.activeInputProfile.entries;
		for (int i = 0; i < entries.Length; i++)
		{
			InputProfileEntry inputProfileEntry = entries[i];
			if (player.InputController.AllowTapStrikeThisFrame || !this.isTapStrike(move.label, inputProfileEntry))
			{
				if (inputProfileEntry.buttonsPressed.Length != 0)
				{
					int num = this.listSubset<ButtonPress>(inputProfileEntry.buttonsPressed, inputButtons.moveButtonsPressed, ref this.buttonPressBuffer, this.ButtonPressComparer, true);
					if (num <= 0)
					{
						goto IL_2A5;
					}
					if (this.buttonPressBuffer.Count > 0)
					{
						buttonUsed = this.buttonPressBuffer[0];
					}
				}
				if (this.validateRequirements(inputProfileEntry.stateRequirements.legalStates, move, player, inputButtons, false))
				{
					this.buttonPressBuffer.Clear();
					this.buttonHeldBuffer.Clear();
					this.filterHeldButtons(inputButtons, ref this.buttonHeldBuffer, move.activeInputProfile, player);
					if (inputProfileEntry.buttonsHeld.Length > 0)
					{
						int num2 = this.listSubset<ButtonPress>(inputProfileEntry.buttonsHeld, this.buttonHeldBuffer, ref this.buttonPressBuffer, this.ButtonPressComparer, false);
						if (num2 < inputProfileEntry.buttonsHeld.Length)
						{
							goto IL_2A5;
						}
					}
					if (inputProfileEntry.buttonsNotHeld.Length > 0)
					{
						bool flag2 = false;
						ButtonPress[] buttonsNotHeld = inputProfileEntry.buttonsNotHeld;
						for (int j = 0; j < buttonsNotHeld.Length; j++)
						{
							ButtonPress item = buttonsNotHeld[j];
							if (this.buttonHeldBuffer.Contains(item))
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							goto IL_2A5;
						}
					}
					flag = true;
					break;
				}
			}
			IL_2A5:;
		}
		if (!flag)
		{
			return MoveSet.MoveSearchResult.ContinueSearch;
		}
		if (!player.MoveUseTracker.HasMoveUsesLeft(move))
		{
			return MoveSet.MoveSearchResult.BreakSearch;
		}
		return MoveSet.MoveSearchResult.Found;
	}

	private bool isTapStrike(MoveLabel moveLabel, InputProfileEntry entry)
	{
		return (moveLabel == MoveLabel.SideStrikeAttack || moveLabel == MoveLabel.UpStrikeAttack || moveLabel == MoveLabel.DownStrikeAttack) && entry.buttonsHeld.Length != 0 && InputUtils.IsDirection(entry.buttonsHeld[0]) && entry.buttonsPressed.Length == 1 && entry.buttonsPressed[0] == ButtonPress.Attack;
	}

	private bool isTapDirection(ButtonPress button)
	{
		return button == ButtonPress.UpTap || button == ButtonPress.DownTap || button == ButtonPress.BackwardTap || button == ButtonPress.ForwardTap || button == ButtonPress.SideTap;
	}

	private void filterHeldButtons(InputButtonsData inputButtons, ref List<ButtonPress> outButtons, InputProfile inputProfile, IPlayerDelegate player)
	{
		Fixed other = MoveSet.NORMAL_Y_TO_X_RATIO;
		if (!player.State.IsGrounded && inputProfile.entries[0].buttonsPressed.Contains(ButtonPress.Special))
		{
			other = MoveSet.RECOVERY_Y_TO_X_RATIO;
		}
		Fixed one = 0;
		if (inputButtons.horizontalAxisValue == 0)
		{
			one = 10000;
		}
		else
		{
			one = FixedMath.Abs(inputButtons.verticalAxisValue / inputButtons.horizontalAxisValue);
		}
		for (int i = 0; i < inputButtons.buttonsHeld.Count; i++)
		{
			ButtonPress buttonPress = inputButtons.buttonsHeld[i];
			if ((!InputUtils.IsVertical(buttonPress) && !InputUtils.IsHorizontalLeftStick(buttonPress)) || (InputUtils.IsVertical(buttonPress) && one >= other) || (InputUtils.IsHorizontalLeftStick(buttonPress) && one < other))
			{
				outButtons.Add(buttonPress);
			}
		}
	}

	private int listSubset<T>(T[] subset, List<T> superset, ref List<T> outSet, EqualityComparer<T> comparer = null, bool allowIncomplete = false)
	{
		if (subset == null || superset == null)
		{
			return 0;
		}
		if (!allowIncomplete && subset.Length > superset.Count)
		{
			return 0;
		}
		if (comparer == null)
		{
			comparer = EqualityComparer<T>.Default;
		}
		outSet.Clear();
		for (int i = 0; i < subset.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < superset.Count; j++)
			{
				if (comparer.Equals(subset[i], superset[j]))
				{
					flag = true;
					outSet.Add(subset[i]);
					break;
				}
			}
			if (!flag && !allowIncomplete)
			{
				return 0;
			}
		}
		return outSet.Count;
	}

	void IMoveSet.LoadMoveInfo(MoveData newMove)
	{
		for (int i = 0; i < this.moves.Length; i++)
		{
			if (this.moves[i] != null && this.moves[i].moveName == newMove.moveName)
			{
				UnityEngine.Debug.Log("Move " + newMove.name + " loaded");
				this.moves[i] = newMove;
			}
		}
	}
}
