using System;
using FixedPoint;

// Token: 0x020005DB RID: 1499
public class PlayerCameraBoxController : IPlayerCameraBoxController
{
	// Token: 0x060021F4 RID: 8692 RVA: 0x000A7F1B File Offset: 0x000A631B
	public PlayerCameraBoxController(IPlayerDelegate player)
	{
		this.player = player;
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000A7F2C File Offset: 0x000A632C
	public FixedRect GetCameraBox(HorizontalDirection facing)
	{
		FixedRect cameraBoxData = this.player.CharacterMenusData.bounds.CameraBoxData;
		if (facing == HorizontalDirection.Left)
		{
			cameraBoxData.position.x = -cameraBoxData.position.x - cameraBoxData.Width;
		}
		return cameraBoxData;
	}

	// Token: 0x04001A71 RID: 6769
	private IPlayerDelegate player;

	// Token: 0x04001A72 RID: 6770
	public static float LEDGE_GRAB_BUFFER = 1f;
}
