// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenArrow : GameBehavior
{
	public Sprite BlueArrow;

	public Sprite RedArrow;

	public Sprite GreenArrow;

	public Sprite YellowArrow;

	public Sprite PurpleArrow;

	public Sprite PinkArrow;

	public Sprite GreyArrow;

	public RawImage RawImage;

	public Image Circle;

	public Transform AttachPoint;

	public Transform RotateContainer;

	private PlayerController playerController;

	private RectTransform rectTransform;

	private bool isVisible = true;

	private float boundaryY;

	private Camera playerCamera;

	private RenderTexture renderTexture;

	private bool initialized;

	private Dictionary<Color, Sprite> colorMap = new Dictionary<Color, Sprite>();

	private bool shouldBeHidden
	{
		get
		{
			return this.playerController.State.IsDead || !this.playerController.IsInBattle || this.playerController.State.IsRespawning;
		}
	}

	private void Start()
	{
		this.boundaryY = this.Circle.rectTransform.sizeDelta.y / 2f;
		this.colorMap[WColor.UIBlue] = this.BlueArrow;
		this.colorMap[WColor.UIRed] = this.RedArrow;
		this.colorMap[WColor.UIGreen] = this.GreenArrow;
		this.colorMap[WColor.UIYellow] = this.YellowArrow;
		this.colorMap[WColor.UIPurple] = this.PurpleArrow;
		this.colorMap[WColor.UIPink] = this.PinkArrow;
		this.colorMap[WColor.UIGrey] = this.GreyArrow;
		Color iconColor = this.playerController.iconColor;
		this.Circle.sprite = this.colorMap[iconColor];
		this.setVisible(false);
	}

	private void CreateTextureCamera(PlayerController playerController, Camera prefab)
	{
		this.playerCamera = UnityEngine.Object.Instantiate<Camera>(prefab);
		this.playerCamera.transform.SetParent(playerController.transform, false);
	}

	public void Init(PlayerController playerController, Camera playerTextureCameraPrefab)
	{
		this.playerController = playerController;
		this.CreateTextureCamera(playerController, playerTextureCameraPrefab);
		int width = (int)this.RawImage.rectTransform.sizeDelta.x;
		int height = (int)this.RawImage.rectTransform.sizeDelta.y;
		this.renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
		this.RawImage.texture = this.renderTexture;
		this.playerCamera.targetTexture = this.renderTexture;
		this.playerCamera.backgroundColor = base.gameController.currentGame.Stage.SimulationData.offscreenUIColor;
		this.initialized = true;
	}

	private void setPlayerTextureEnabled(bool enabled)
	{
		this.RawImage.enabled = enabled;
		if (this.playerCamera.gameObject != null)
		{
			this.playerCamera.gameObject.SetActive(enabled);
		}
	}

	private void setVisible(bool visible)
	{
		this.setPlayerTextureEnabled(visible);
		this.isVisible = visible;
		this.Circle.enabled = visible;
	}

	private void Update()
	{
		if (base.config == null || !this.initialized)
		{
			return;
		}
		if (base.gameManager == null || base.gameManager.Camera.current == null)
		{
			return;
		}
		Vector2 vector = base.gameManager.Camera.current.WorldToScreenPoint((Vector3)this.playerController.Center);
		bool flag = this.checkOnScreen(vector, base.config.uiuxSettings.offscreenDetectionPadding);
		if (this.isVisible && (flag || this.shouldBeHidden))
		{
			this.setVisible(false);
		}
		else if (!flag && !this.shouldBeHidden)
		{
			this.setVisible(true);
			this.playerCamera.transform.localPosition = this.playerController.GetCharacterTextureCameraPosition();
			Vector2 screenCenter = this.getScreenCenter();
			Vector2 vector2 = vector - screenCenter;
			float offset = this.getOffset();
			float num = Vector2.SignedAngle(Vector2.right, vector2);
			this.RotateContainer.rotation = Quaternion.Euler(0f, 0f, num - 90f);
			Vector2 a = this.findScreenIntersectionPoint(screenCenter, vector2, offset);
			Vector2 b = a - this.AttachPoint.position;
			Vector2 vector3 = base.transform.position;
			vector3 += b;
			float num2 = this.boundaryY * ((float)Screen.width / GameClient.NATIVE_RESOLUTION.x);
			if (vector3.y + num2 > (float)Screen.height - offset)
			{
				vector3.y = (float)Screen.height - offset - num2;
			}
			else if (vector3.y - num2 < offset)
			{
				vector3.y = offset + num2;
			}
			base.transform.position = vector3;
			vector2 = vector - this.AttachPoint.position;
			num = Vector2.SignedAngle(Vector2.right, vector2);
			this.RotateContainer.rotation = Quaternion.Euler(0f, 0f, num - 90f);
		}
	}

	private bool checkOnScreen(Vector2 vec, float padding)
	{
		return vec.x > -padding && vec.x < (float)Screen.width + padding && vec.y > -padding && vec.y < (float)Screen.height + padding;
	}

	private float getOffset()
	{
		float offscreenUIOffset = base.config.uiuxSettings.offscreenUIOffset;
		return offscreenUIOffset * ((float)Screen.width / GameClient.NATIVE_RESOLUTION.x);
	}

	private Vector2 findScreenIntersectionPoint(Vector2 center, Vector2 slopeVect, float offset)
	{
		float num = (float)Screen.height;
		float num2 = (float)Screen.width;
		if (slopeVect.x != 0f)
		{
			float num3 = slopeVect.y / slopeVect.x;
			if (slopeVect.y > 0f)
			{
				float num4 = center.x + (center.y - offset) / num3;
				if (num4 > offset && num4 < num2 - offset)
				{
					return new Vector2(num4, num - offset);
				}
			}
			else if (slopeVect.y < 0f)
			{
				float num5 = center.x - (center.y - offset) / num3;
				if (num5 > offset && num5 < num2 - offset)
				{
					return new Vector2(num5, offset);
				}
			}
			if (slopeVect.x > 0f)
			{
				float num6 = center.y + (center.x - offset) * num3;
				if (num6 > offset && num6 < num - offset)
				{
					return new Vector2(num2 - offset, num6);
				}
			}
			else if (slopeVect.x < 0f)
			{
				float num7 = center.y - (center.x - offset) * num3;
				if (num7 > offset && num7 < num - offset)
				{
					return new Vector2(offset, num7);
				}
			}
			return default(Vector2);
		}
		if (slopeVect.y > 0f)
		{
			return new Vector2(center.x, num - offset);
		}
		return new Vector2(center.x, offset);
	}

	private Vector2 getScreenCenter()
	{
		return new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
	}
}
