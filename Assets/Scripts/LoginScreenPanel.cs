// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreenPanel : MonoBehaviour
{
	public LoginTextInputField LoginTextInputFieldPrefab;

	protected CanvasGroup canvasGroup;

	private List<SignalListenerRecord> signalListeners = new List<SignalListenerRecord>();

	[Inject]
	public ILoginScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
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
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	[Inject]
	public ILoginValidator validator
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowManager
	{
		get;
		set;
	}

	public bool IsCurrentPanel
	{
		get;
		set;
	}

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

	private void Awake()
	{
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	protected void listen(string signalName, Action theFunction)
	{
		this.signalBus.AddListener(signalName, theFunction);
		this.signalListeners.Add(new SignalListenerRecord(signalName, theFunction));
	}

	protected void removeAllListeners()
	{
		for (int i = this.signalListeners.Count - 1; i >= 0; i--)
		{
			this.signalBus.RemoveListener(this.signalListeners[i].name, this.signalListeners[i].theFunction);
		}
		this.signalListeners.Clear();
	}

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

	protected void selectTextField(WavedashTMProInput field)
	{
		if (this.uiManager.CurrentInputModule is CursorInputModule)
		{
			(this.uiManager.CurrentInputModule as CursorInputModule).SetSelectedInputField(field);
		}
	}

	public virtual void InitSelection()
	{
	}

	public virtual void OnHide()
	{
	}

	public virtual void OnDestroy()
	{
		this.removeAllListeners();
	}
}
