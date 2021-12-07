using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C3 RID: 2243
public class OffscreenArrow : GameBehavior
{
	// Token: 0x0600388A RID: 14474 RVA: 0x00109314 File Offset: 0x00107714
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

	// Token: 0x0600388B RID: 14475 RVA: 0x00109409 File Offset: 0x00107809
	private void CreateTextureCamera(PlayerController playerController, Camera prefab)
	{
		this.playerCamera = UnityEngine.Object.Instantiate<Camera>(prefab);
		this.playerCamera.transform.SetParent(playerController.transform, false);
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x00109430 File Offset: 0x00107830
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

	// Token: 0x0600388D RID: 14477 RVA: 0x001094DF File Offset: 0x001078DF
	private void setPlayerTextureEnabled(bool enabled)
	{
		this.RawImage.enabled = enabled;
		if (this.playerCamera.gameObject != null)
		{
			this.playerCamera.gameObject.SetActive(enabled);
		}
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x00109514 File Offset: 0x00107914
	private void setVisible(bool visible)
	{
		this.setPlayerTextureEnabled(visible);
		this.isVisible = visible;
		this.Circle.enabled = visible;
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x00109530 File Offset: 0x00107930
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

	// Token: 0x06003890 RID: 14480 RVA: 0x0010976C File Offset: 0x00107B6C
	private bool checkOnScreen(Vector2 vec, float padding)
	{
		return vec.x > -padding && vec.x < (float)Screen.width + padding && vec.y > -padding && vec.y < (float)Screen.height + padding;
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x001097C0 File Offset: 0x00107BC0
	private float getOffset()
	{
		float offscreenUIOffset = base.config.uiuxSettings.offscreenUIOffset;
		return offscreenUIOffset * ((float)Screen.width / GameClient.NATIVE_RESOLUTION.x);
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x001097F4 File Offset: 0x00107BF4
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

	// Token: 0x06003893 RID: 14483 RVA: 0x00109977 File Offset: 0x00107D77
	private Vector2 getScreenCenter()
	{
		return new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06003894 RID: 14484 RVA: 0x0010998E File Offset: 0x00107D8E
	private bool shouldBeHidden
	{
		get
		{
			return this.playerController.State.IsDead || !this.playerController.IsInBattle || this.playerController.State.IsRespawning;
		}
	}

	// Token: 0x040026E6 RID: 9958
	public Sprite BlueArrow;

	// Token: 0x040026E7 RID: 9959
	public Sprite RedArrow;

	// Token: 0x040026E8 RID: 9960
	public Sprite GreenArrow;

	// Token: 0x040026E9 RID: 9961
	public Sprite YellowArrow;

	// Token: 0x040026EA RID: 9962
	public Sprite PurpleArrow;

	// Token: 0x040026EB RID: 9963
	public Sprite PinkArrow;

	// Token: 0x040026EC RID: 9964
	public Sprite GreyArrow;

	// Token: 0x040026ED RID: 9965
	public RawImage RawImage;

	// Token: 0x040026EE RID: 9966
	public Image Circle;

	// Token: 0x040026EF RID: 9967
	public Transform AttachPoint;

	// Token: 0x040026F0 RID: 9968
	public Transform RotateContainer;

	// Token: 0x040026F1 RID: 9969
	private PlayerController playerController;

	// Token: 0x040026F2 RID: 9970
	private RectTransform rectTransform;

	// Token: 0x040026F3 RID: 9971
	private bool isVisible = true;

	// Token: 0x040026F4 RID: 9972
	private float boundaryY;

	// Token: 0x040026F5 RID: 9973
	private Camera playerCamera;

	// Token: 0x040026F6 RID: 9974
	private RenderTexture renderTexture;

	// Token: 0x040026F7 RID: 9975
	private bool initialized;

	// Token: 0x040026F8 RID: 9976
	private Dictionary<Color, Sprite> colorMap = new Dictionary<Color, Sprite>();
}
