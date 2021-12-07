using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009A2 RID: 2466
public class LoginScreenPanel : MonoBehaviour
{
	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x06004362 RID: 17250 RVA: 0x001284D5 File Offset: 0x001268D5
	// (set) Token: 0x06004363 RID: 17251 RVA: 0x001284DD File Offset: 0x001268DD
	[Inject]
	public ILoginScreenAPI api { get; set; }

	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x06004364 RID: 17252 RVA: 0x001284E6 File Offset: 0x001268E6
	// (set) Token: 0x06004365 RID: 17253 RVA: 0x001284EE File Offset: 0x001268EE
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x06004366 RID: 17254 RVA: 0x001284F7 File Offset: 0x001268F7
	// (set) Token: 0x06004367 RID: 17255 RVA: 0x001284FF File Offset: 0x001268FF
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x06004368 RID: 17256 RVA: 0x00128508 File Offset: 0x00126908
	// (set) Token: 0x06004369 RID: 17257 RVA: 0x00128510 File Offset: 0x00126910
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x0600436A RID: 17258 RVA: 0x00128519 File Offset: 0x00126919
	// (set) Token: 0x0600436B RID: 17259 RVA: 0x00128521 File Offset: 0x00126921
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x0600436C RID: 17260 RVA: 0x0012852A File Offset: 0x0012692A
	// (set) Token: 0x0600436D RID: 17261 RVA: 0x00128532 File Offset: 0x00126932
	[Inject]
	public ILoginValidator validator { get; set; }

	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x0600436E RID: 17262 RVA: 0x0012853B File Offset: 0x0012693B
	// (set) Token: 0x0600436F RID: 17263 RVA: 0x00128543 File Offset: 0x00126943
	[Inject]
	public IWindowDisplay windowManager { get; set; }

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x06004370 RID: 17264 RVA: 0x0012854C File Offset: 0x0012694C
	// (set) Token: 0x06004371 RID: 17265 RVA: 0x00128554 File Offset: 0x00126954
	public bool IsCurrentPanel { get; set; }

	// Token: 0x06004372 RID: 17266 RVA: 0x0012855D File Offset: 0x0012695D
	private void Awake()
	{
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x0012856B File Offset: 0x0012696B
	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x0012858C File Offset: 0x0012698C
	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x001285F0 File Offset: 0x001269F0
	protected LoginTextInputField createTextEntry(GameObject stub, string titleKey, LoginEntryType type)
	{
		while (stub.transform.childCount > 0)
		{
			Transform child = stub.transform.GetChild(0);
			child.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		LoginTextInputField loginTextInputField = UnityEngine.Object.Instantiate<LoginTextInputField>(this.LoginTextInputFieldPrefab);
		this.injector.Inject(loginTextInputField);
		loginTextInputField.Type = type;
		loginTextInputField.transform.SetParent(stub.transform, false);
		loginTextInputField.Title.text = this.localization.GetText(titleKey);
		return loginTextInputField;
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x0012867B File Offset: 0x00126A7B
	protected void selectTextField(WavedashTMProInput field)
	{
		if (this.uiManager.CurrentInputModule is CursorInputModule)
		{
			(this.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(field);
		}
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x001286A8 File Offset: 0x00126AA8
	public virtual void InitSelection()
	{
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x001286AA File Offset: 0x00126AAA
	public virtual void OnHide()
	{
	}

	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x06004379 RID: 17273 RVA: 0x001286AC File Offset: 0x00126AAC
	// (set) Token: 0x0600437A RID: 17274 RVA: 0x001286B9 File Offset: 0x00126AB9
	public float alpha
	{
		get
		{
			return this.canvasGroup.alpha;
		}
		set
		{
			this.canvasGroup.alpha = value;
		}
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x001286C7 File Offset: 0x00126AC7
	public virtual void OnDestroy()
	{
		this.removeAllListeners();
	}

	// Token: 0x04002CF3 RID: 11507
	public LoginTextInputField LoginTextInputFieldPrefab;

	// Token: 0x04002CF4 RID: 11508
	protected CanvasGroup canvasGroup;

	// Token: 0x04002CF5 RID: 11509
	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();
}
