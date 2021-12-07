using System;
using System.Collections.Generic;

// Token: 0x02000936 RID: 2358
public interface IDialogController
{
	// Token: 0x06003E37 RID: 15927
	void Init(GameClient client);

	// Token: 0x06003E38 RID: 15928
	GenericDialog ShowOneButtonDialog(string title, string body, string buttonText, WindowTransition transition = WindowTransition.STANDARD_FADE, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData));

	// Token: 0x06003E39 RID: 15929
	GenericDialog ShowTwoButtonDialog(string title, string body, string confirmText, string cancelText);

	// Token: 0x06003E3A RID: 15930
	GenericDialog ShowOneButtonSpinnyDialog(string title, string buttonText);

	// Token: 0x06003E3B RID: 15931
	GenericDialog ShowSpinnyDialog(string title, WindowTransition transition = WindowTransition.STANDARD_FADE);

	// Token: 0x06003E3C RID: 15932
	UnlockFlowDialog ShowUnlockFlowDialog();

	// Token: 0x06003E3D RID: 15933
	NuxPopup ShowNuxNewPlayerDialog();

	// Token: 0x06003E3E RID: 15934
	NuxPopup ShowNuxFirstTokenDialog();

	// Token: 0x06003E3F RID: 15935
	NuxPopup ShowNuxSecondTokenDialog();

	// Token: 0x06003E40 RID: 15936
	RankedFriendlyPopup ShowNuxRankChoiceDialog();

	// Token: 0x06003E41 RID: 15937
	void QueueNuxRankChoiceDialog(List<Func<BaseWindow>> windowQueue);

	// Token: 0x06003E42 RID: 15938
	NuxPopup ShowNuxRankConfirmedDialog();

	// Token: 0x06003E43 RID: 15939
	DetailedUnlockFlowDialog ShowDetailedUnlockFlowDialog();

	// Token: 0x06003E44 RID: 15940
	EquipTauntDialog ShowEquipDialog();

	// Token: 0x06003E45 RID: 15941
	AdvancedControllerDialog ShowAdvancedControllerDialog();

	// Token: 0x06003E46 RID: 15942
	AdvancedKeyboardDialog ShowAdvancedKeyboardDialog();

	// Token: 0x06003E47 RID: 15943
	FeedbackDialog ShowFeedbackDialog();

	// Token: 0x06003E48 RID: 15944
	CreateLobbyDialog ShowCreateLobbyDialog(bool isCreate);

	// Token: 0x06003E49 RID: 15945
	JoinLobbyDialog ShowJoinLobbyDialog();

	// Token: 0x06003E4A RID: 15946
	LootBoxBuyPopup ShowLootBoxBuyDialog();
}
