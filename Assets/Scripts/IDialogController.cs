// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IDialogController
{
	void Init(GameClient client);

	GenericDialog ShowOneButtonDialog(string title, string body, string buttonText, WindowTransition transition = WindowTransition.STANDARD_FADE, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData));

	GenericDialog ShowTwoButtonDialog(string title, string body, string confirmText, string cancelText);

	GenericDialog ShowOneButtonSpinnyDialog(string title, string buttonText);

	GenericDialog ShowSpinnyDialog(string title, WindowTransition transition = WindowTransition.STANDARD_FADE);

	UnlockFlowDialog ShowUnlockFlowDialog();

	NuxPopup ShowNuxNewPlayerDialog();

	NuxPopup ShowNuxFirstTokenDialog();

	NuxPopup ShowNuxSecondTokenDialog();

	RankedFriendlyPopup ShowNuxRankChoiceDialog();

	void QueueNuxRankChoiceDialog(List<Func<BaseWindow>> windowQueue);

	NuxPopup ShowNuxRankConfirmedDialog();

	DetailedUnlockFlowDialog ShowDetailedUnlockFlowDialog();

	EquipTauntDialog ShowEquipDialog();

	AdvancedControllerDialog ShowAdvancedControllerDialog();

	AdvancedKeyboardDialog ShowAdvancedKeyboardDialog();

	FeedbackDialog ShowFeedbackDialog();

	CreateLobbyDialog ShowCreateLobbyDialog(bool isCreate);

	JoinLobbyDialog ShowJoinLobbyDialog();

	LootBoxBuyPopup ShowLootBoxBuyDialog();
}
