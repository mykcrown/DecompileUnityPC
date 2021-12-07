using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPoint;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public class MoveSet : IMoveSet
{
	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00093030 File Offset: 0x00091430
	// (set) Token: 0x06001C9A RID: 7322 RVA: 0x00093038 File Offset: 0x00091438
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06001C9B RID: 7323 RVA: 0x00093041 File Offset: 0x00091441
	// (set) Token: 0x06001C9C RID: 7324 RVA: 0x00093049 File Offset: 0x00091449
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x06001C9D RID: 7325 RVA: 0x00093052 File Offset: 0x00091452
	CharacterMoveSetData IMoveSet.MoveSetData
	{
		get
		{
			return this.moveSetData;
		}
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x0009305C File Offset: 0x0009145C
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
			foreach (MoveData moveData in this.moves)
			{
				foreach (MoveComponent moveComponent in moveData.components)
				{
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
					Debug.LogWarning(string.Concat(new string[]
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

	// Token: 0x06001C9F RID: 7327 RVA: 0x000932BA File Offset: 0x000916BA
	private void buildCaches()
	{
		this.buildMoveLookupCaches();
		this.buildInputScanCaches();
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000932C8 File Offset: 0x000916C8
	private void buildInputScanCaches()
	{
		Dictionary<ButtonPress, List<MoveData>> dictionary = new Dictionary<ButtonPress, List<MoveData>>();
		Array values = Enum.GetValues(typeof(ButtonPress));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ButtonPress button = (ButtonPress)obj;
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
		foreach (ButtonPress key in dictionary.Keys)
		{
			this.cacheMovesForButtonPress[key] = dictionary[key].ToArray();
		}
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000933A4 File Offset: 0x000917A4
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

	// Token: 0x06001CA2 RID: 7330 RVA: 0x00093464 File Offset: 0x00091864
	private bool moveUsesButtonPress(ButtonPress button, MoveData move)
	{
		if (move.activeInputProfile != null)
		{
			foreach (InputProfileEntry inputProfileEntry in move.activeInputProfile.entries)
			{
				if (inputProfileEntry.buttonsPressed.Length != 0)
				{
					foreach (ButtonPress buttonPress in inputProfileEntry.buttonsPressed)
					{
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

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000934E4 File Offset: 0x000918E4
	private bool moveUsesNonPressInput(MoveData move)
	{
		if (move.activeInputProfile != null)
		{
			foreach (InputProfileEntry inputProfileEntry in move.activeInputProfile.entries)
			{
				if (inputProfileEntry.buttonsPressed.Length == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x00093538 File Offset: 0x00091938
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
		foreach (MoveLabel key in dictionary.Keys)
		{
			this.cacheAllMovesWithLabel[key] = dictionary[key].ToArray();
		}
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x00093654 File Offset: 0x00091A54
	void IMoveSet.LoadMoveSet(CharacterMoveSetData moveSetData)
	{
		this.moveSetData = moveSetData;
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x0009365D File Offset: 0x00091A5D
	public CharacterActionSet Actions
	{
		get
		{
			return this.moveSetData.characterActions;
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x0009366C File Offset: 0x00091A6C
	private bool isAerial(MoveData move)
	{
		return move.label == MoveLabel.UpAir || move.label == MoveLabel.DownAir || move.label == MoveLabel.ForwardAir || move.label == MoveLabel.BackwardAir || move.label == MoveLabel.NeutralAir;
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000936BC File Offset: 0x00091ABC
	private int compareMoves(MoveData move1, MoveData move2)
	{
		if (move1 == move2)
		{
			return 0;
		}
		if (move1.isRecovery != move2.isRecovery)
		{
			return (!move1.isRecovery) ? 1000000000 : -1000000000;
		}
		if (move1.activeInputProfile == null != (move2.activeInputProfile == null))
		{
			return (!(move1.activeInputProfile == null)) ? 100000000 : -100000000;
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
			return (num <= num2) ? 10000000 : -10000000;
		}
		if (move1.IsGrab != move2.IsGrab)
		{
			return (!move1.IsGrab) ? 1000000 : -1000000;
		}
		if (!this.isAerial(move1) || this.isAerial(move2))
		{
		}
		if (num > 0 && num2 > 0 && InputUtils.IsDirection(move1.activeInputProfile.entries[0].buttonsPressed[0]) != InputUtils.IsDirection(move2.activeInputProfile.entries[0].buttonsPressed[0]))
		{
			return (!InputUtils.IsDirection(move1.activeInputProfile.entries[0].buttonsPressed[0])) ? 10000 : -10000;
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
			return (num3 <= num4) ? 1000 : -1000;
		}
		if (num3 > 0 && num4 > 0)
		{
			bool flag = InputUtils.IsTap(move1.activeInputProfile.entries[0].buttonsHeld[0]);
			bool flag2 = InputUtils.IsTap(move2.activeInputProfile.entries[0].buttonsHeld[0]);
			if (flag != flag2)
			{
				return (!flag) ? 100 : -100;
			}
			bool flag3 = InputUtils.IsVertical(move1.activeInputProfile.entries[0].buttonsHeld[0]);
			bool flag4 = InputUtils.IsVertical(move2.activeInputProfile.entries[0].buttonsHeld[0]);
			if (flag3 != flag4)
			{
				return (!flag3) ? 10 : -10;
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
			return (move1.label != MoveLabel.DashAttack) ? -1 : 1;
		}
		return 0;
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x00093A2C File Offset: 0x00091E2C
	public bool CanInputBufferMove(MoveData move, List<ButtonPress> moveButtonsPressed)
	{
		foreach (InputProfileEntry inputProfileEntry in move.activeInputProfile.entries)
		{
			int num = this.listSubset<ButtonPress>(inputProfileEntry.buttonsPressed, moveButtonsPressed, ref this.buttonPressBuffer, this.ButtonPressComparer, true);
			if (num > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x00093A82 File Offset: 0x00091E82
	public MoveData GetMove(string moveName)
	{
		if (this.cacheMovesByName.ContainsKey(moveName))
		{
			return this.cacheMovesByName[moveName];
		}
		return null;
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x00093AA3 File Offset: 0x00091EA3
	public MoveData GetMove(MoveLabel label)
	{
		if (this.cacheMovesByLabel.ContainsKey(label))
		{
			return this.cacheMovesByLabel[label];
		}
		Debug.LogError("Move " + label + " not found, this shouldn't happen.");
		return null;
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00093ADE File Offset: 0x00091EDE
	public MoveData[] GetMoves(MoveLabel label)
	{
		if (this.cacheAllMovesWithLabel.ContainsKey(label))
		{
			return this.cacheAllMovesWithLabel[label];
		}
		return this.cacheEmptyList;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x00093B04 File Offset: 0x00091F04
	public MoveData GetMove(InputButtonsData inputButtonsData, IPlayerDelegate player, out InterruptData interruptData, ref ButtonPress buttonUsed)
	{
		interruptData = null;
		if (inputButtonsData.moveButtonsPressed.Count > 0 || inputButtonsData.buttonsHeld.Count > 0)
		{
			this.buttonScanBuffer.Clear();
			this.buttonScanBuffer.Add(ButtonPress.None);
			foreach (ButtonPress item in inputButtonsData.moveButtonsPressed)
			{
				this.buttonScanBuffer.Add(item);
				switch (item)
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
			foreach (ButtonPress buttonPress in this.buttonScanBuffer)
			{
				MoveData[] array = this.cacheMovesForButtonPress[buttonPress];
				foreach (MoveData moveData in array)
				{
					MoveSet.MoveSearchResult moveSearchResult = this.canActivateMove(moveData, player, inputButtonsData, out interruptData, ref buttonUsed);
					if (buttonUsed == ButtonPress.Taunt)
					{
						buttonUsed = buttonPress;
					}
					if (moveSearchResult == MoveSet.MoveSearchResult.Found)
					{
						return moveData;
					}
					if (moveSearchResult == MoveSet.MoveSearchResult.BreakSearch)
					{
						return null;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x00093CD8 File Offset: 0x000920D8
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

	// Token: 0x06001CAF RID: 7343 RVA: 0x00093D0C File Offset: 0x0009210C
	private bool canInterrupt(MoveData move, InterruptData[] interrupts, int currentFrame, out InterruptData interruptData)
	{
		interruptData = null;
		foreach (InterruptData interruptData2 in interrupts)
		{
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

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00093DB8 File Offset: 0x000921B8
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

	// Token: 0x06001CB1 RID: 7345 RVA: 0x00093E0C File Offset: 0x0009220C
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

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00093E58 File Offset: 0x00092258
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
		foreach (InputProfileEntry inputProfileEntry in move.activeInputProfile.entries)
		{
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
						foreach (ButtonPress item in inputProfileEntry.buttonsNotHeld)
						{
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

	// Token: 0x06001CB3 RID: 7347 RVA: 0x00094138 File Offset: 0x00092538
	private bool isTapStrike(MoveLabel moveLabel, InputProfileEntry entry)
	{
		return (moveLabel == MoveLabel.SideStrikeAttack || moveLabel == MoveLabel.UpStrikeAttack || moveLabel == MoveLabel.DownStrikeAttack) && entry.buttonsHeld.Length != 0 && InputUtils.IsDirection(entry.buttonsHeld[0]) && entry.buttonsPressed.Length == 1 && entry.buttonsPressed[0] == ButtonPress.Attack;
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x0009419E File Offset: 0x0009259E
	private bool isTapDirection(ButtonPress button)
	{
		return button == ButtonPress.UpTap || button == ButtonPress.DownTap || button == ButtonPress.BackwardTap || button == ButtonPress.ForwardTap || button == ButtonPress.SideTap;
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000941C8 File Offset: 0x000925C8
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

	// Token: 0x06001CB6 RID: 7350 RVA: 0x000942C0 File Offset: 0x000926C0
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

	// Token: 0x06001CB7 RID: 7351 RVA: 0x0009437C File Offset: 0x0009277C
	void IMoveSet.LoadMoveInfo(MoveData newMove)
	{
		for (int i = 0; i < this.moves.Length; i++)
		{
			if (this.moves[i] != null && this.moves[i].moveName == newMove.moveName)
			{
				Debug.Log("Move " + newMove.name + " loaded");
				this.moves[i] = newMove;
			}
		}
	}

	// Token: 0x040017A3 RID: 6051
	public static Fixed NORMAL_Y_TO_X_RATIO = 1;

	// Token: 0x040017A4 RID: 6052
	public static Fixed RECOVERY_Y_TO_X_RATIO = (Fixed)0.5;

	// Token: 0x040017A5 RID: 6053
	private CharacterMoveSetData moveSetData;

	// Token: 0x040017A6 RID: 6054
	private MoveData[] moves;

	// Token: 0x040017A7 RID: 6055
	private Dictionary<ButtonPress, MoveData> tauntMapping = new Dictionary<ButtonPress, MoveData>();

	// Token: 0x040017A8 RID: 6056
	private EqualityUtil.ButtonPressEqualityComparer ButtonPressComparer = new EqualityUtil.ButtonPressEqualityComparer();

	// Token: 0x040017A9 RID: 6057
	private List<ButtonPress> buttonPressBuffer = new List<ButtonPress>();

	// Token: 0x040017AA RID: 6058
	private List<ButtonPress> buttonHeldBuffer = new List<ButtonPress>();

	// Token: 0x040017AB RID: 6059
	private List<ButtonPress> buttonScanBuffer = new List<ButtonPress>();

	// Token: 0x040017AC RID: 6060
	private Dictionary<string, MoveData> cacheMovesByName = new Dictionary<string, MoveData>();

	// Token: 0x040017AD RID: 6061
	private Dictionary<MoveLabel, MoveData> cacheMovesByLabel = new Dictionary<MoveLabel, MoveData>(default(MoveLabelComparer));

	// Token: 0x040017AE RID: 6062
	private Dictionary<MoveLabel, MoveData[]> cacheAllMovesWithLabel = new Dictionary<MoveLabel, MoveData[]>(default(MoveLabelComparer));

	// Token: 0x040017AF RID: 6063
	private MoveData[] cacheEmptyList = new MoveData[0];

	// Token: 0x040017B0 RID: 6064
	private Dictionary<ButtonPress, MoveData[]> cacheMovesForButtonPress = new Dictionary<ButtonPress, MoveData[]>(default(ButtonPressComparer));

	// Token: 0x0200052D RID: 1325
	private enum MoveSearchResult
	{
		// Token: 0x040017B2 RID: 6066
		Found,
		// Token: 0x040017B3 RID: 6067
		ContinueSearch,
		// Token: 0x040017B4 RID: 6068
		BreakSearch
	}
}
