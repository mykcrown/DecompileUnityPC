// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RenderTracker : ClientBehavior
{
	public Action PreRenderCallback;

	[Inject]
	public IMatchDeepLogging deepLogging
	{
		get;
		set;
	}

	private void OnPreRender()
	{
		this.deepLogging.RecordPreRender();
		if (this.PreRenderCallback != null)
		{
			this.PreRenderCallback();
		}
	}

	private void OnPostRender()
	{
		this.deepLogging.RecordPostRender();
	}
}
