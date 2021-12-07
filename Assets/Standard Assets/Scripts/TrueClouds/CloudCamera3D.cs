// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace TrueClouds
{
	internal class CloudCamera3D : CloudCamera
	{
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.RenderClouds(source, destination);
		}
	}
}
