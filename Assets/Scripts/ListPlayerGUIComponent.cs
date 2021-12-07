// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPlayerGUIComponent : PlayerGUIComponent
{
	public ListPlayerGUIComponentData Data;

	protected float count;

	protected List<GameObject> items = new List<GameObject>();

	protected GameObject maxDisplay;

	public Transform MaxDisplayAnchor
	{
		get;
		set;
	}

	public override void Initialize(PlayerNum player, Transform anchor)
	{
		base.Initialize(player, anchor);
		if (this.Data == null)
		{
			UnityEngine.Debug.LogWarning("No data exists for list player gui component");
			return;
		}
		this.count = (float)this.Data.defaultCount;
		HorizontalLayoutGroup component = anchor.GetComponent<HorizontalLayoutGroup>();
		if (component != null)
		{
			component.spacing = (float)this.Data.spacing;
		}
		for (int i = 0; i < this.Data.totalCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Data.itemPrefab.gameObject);
			gameObject.transform.SetParent(anchor, false);
			this.items.Add(gameObject);
			IChargeListSprite chargeListSprite = this.items[i].GetComponent(typeof(IChargeListSprite)) as IChargeListSprite;
			if (i >= this.Data.defaultCount)
			{
				if (chargeListSprite != null)
				{
					chargeListSprite.SetActive(false);
				}
				else
				{
					gameObject.gameObject.SetActive(false);
				}
			}
		}
		if (this.Data.maxPrefab != null)
		{
			this.maxDisplay = UnityEngine.Object.Instantiate<GameObject>(this.Data.maxPrefab);
			this.maxDisplay.transform.SetParent(this.MaxDisplayAnchor, false);
			this.maxDisplay.SetActive(this.Data.defaultCount == this.Data.totalCount);
		}
		Type type = Type.GetType(this.Data.itemCountChangedEventType);
		if (type == null)
		{
			UnityEngine.Debug.LogWarning("Item count changed event " + this.Data.itemCountChangedEventType + " could not be found");
			return;
		}
		if (type.IsAssignableFrom(typeof(IPlayerOwnedQuantity)))
		{
			UnityEngine.Debug.LogWarning("Item count changed event " + this.Data.itemCountChangedEventType + " does not implement IPlayerOwned");
			return;
		}
		base.events.Subscribe(typeof(ChargeLossWarningEvent), new Events.EventHandler(this.warnItem));
		base.events.Subscribe(Type.GetType(this.Data.itemCountChangedEventType), new Events.EventHandler(this.changeItemCount));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(ChargeLossWarningEvent), new Events.EventHandler(this.warnItem));
		base.events.Unsubscribe(Type.GetType(this.Data.itemCountChangedEventType), new Events.EventHandler(this.changeItemCount));
	}

	protected virtual void warnItem(GameEvent message = null)
	{
		ChargeLossWarningEvent chargeLossWarningEvent = message as ChargeLossWarningEvent;
		if (chargeLossWarningEvent.Player != this.player)
		{
			return;
		}
		int num = Mathf.FloorToInt(Math.Max(0f, Math.Min((float)chargeLossWarningEvent.Count, (float)this.Data.totalCount))) - 1;
		if (num >= 0 && this.items.Count > num)
		{
			IChargeListSprite chargeListSprite = this.items[num].GetComponent(typeof(IChargeListSprite)) as IChargeListSprite;
			if (chargeListSprite != null)
			{
				chargeListSprite.WarnImminentLoss((float)chargeLossWarningEvent.TimeTilLoss);
			}
		}
	}

	protected virtual void changeItemCount(GameEvent message = null)
	{
		IPlayerOwnedQuantity playerOwnedQuantity = message as IPlayerOwnedQuantity;
		if (playerOwnedQuantity.Player != this.player)
		{
			return;
		}
		this.count = Math.Max(0f, Math.Min((float)playerOwnedQuantity.Count, (float)this.Data.totalCount));
		for (int i = 0; i < this.Data.totalCount; i++)
		{
			bool flag = this.items[i].gameObject.activeInHierarchy;
			GameObject gameObject = this.items[i].gameObject;
			IChargeListSprite chargeListSprite = this.items[i].GetComponent(typeof(IChargeListSprite)) as IChargeListSprite;
			if (chargeListSprite != null)
			{
				flag = chargeListSprite.IsActive;
			}
			int num = Mathf.FloorToInt(this.count);
			if (!flag && i < num)
			{
				if (chargeListSprite != null)
				{
					chargeListSprite.SetActive(true);
				}
				else
				{
					gameObject.SetActive(true);
				}
			}
			else if (flag && i >= num)
			{
				if (chargeListSprite != null)
				{
					chargeListSprite.SetActive(false);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			if (chargeListSprite != null)
			{
				float partialValue = 0f;
				if ((float)(i + 1) > this.count && (float)i < this.count)
				{
					partialValue = this.count - (float)i;
				}
				chargeListSprite.SetPartialValue(partialValue);
			}
		}
		bool flag2 = this.count == (float)this.Data.totalCount;
		if (this.maxDisplay != null)
		{
			this.maxDisplay.SetActive(flag2);
		}
		for (int j = 0; j < this.items.Count; j++)
		{
			IChargeListSprite chargeListSprite2 = this.items[j].GetComponent(typeof(IChargeListSprite)) as IChargeListSprite;
			if (chargeListSprite2 != null)
			{
				chargeListSprite2.SetMaxCharge(flag2);
			}
		}
	}
}
