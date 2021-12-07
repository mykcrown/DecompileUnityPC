// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IOptionProfileSaver
{
	void Load(Action<LoadOptionsProfileListResult> callback);

	void Save(OptionsProfileSet data, Action<SaveOptionsProfileResult> callback);
}
