// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.UI;

public class HitboxWidget : TrainingWidget
{
	public Text EnabledText;

	public bool IsEnabled
	{
		get
		{
			return DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes);
		}
		set
		{
			DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, value);
		}
	}

	protected override void Start()
	{
		base.Start();
		this.IsEnabled = false;
	}

	public override void OnLeft()
	{
		this.IsEnabled = false;
	}

	public override void OnRight()
	{
		this.IsEnabled = true;
	}

	private void Update()
	{
		if (this.EnabledText != null)
		{
			this.EnabledText.text = ((!this.IsEnabled) ? "Off" : "On");
		}
	}
}
