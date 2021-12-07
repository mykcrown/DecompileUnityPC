// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class PlayerCameraBoxController : IPlayerCameraBoxController
{
	private IPlayerDelegate player;

	public static float LEDGE_GRAB_BUFFER = 1f;

	public PlayerCameraBoxController(IPlayerDelegate player)
	{
		this.player = player;
	}

	public FixedRect GetCameraBox(HorizontalDirection facing)
	{
		FixedRect cameraBoxData = this.player.CharacterMenusData.bounds.CameraBoxData;
		if (facing == HorizontalDirection.Left)
		{
			cameraBoxData.position.x = -cameraBoxData.position.x - cameraBoxData.Width;
		}
		return cameraBoxData;
	}
}
