using System;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class DummyDevConsole : MonoBehaviour, IDevConsole
{
	// Token: 0x06001055 RID: 4181 RVA: 0x0006077F File Offset: 0x0005EB7F
	void IDevConsole.RunAutoExec()
	{
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x00060781 File Offset: 0x0005EB81
	void IDevConsole.PrintLn(string text)
	{
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x00060783 File Offset: 0x0005EB83
	void IDevConsole.AddCommand(Action callback, string category, string text, string help)
	{
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x00060785 File Offset: 0x0005EB85
	void IDevConsole.AddCommand<T>(Action<T> callback, string category, string text, string help)
	{
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x00060787 File Offset: 0x0005EB87
	void IDevConsole.AddAdminCommand(Action callback, string text, string help)
	{
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x00060789 File Offset: 0x0005EB89
	void IDevConsole.AddAdminCommand<T>(Action<T> callback, string text, string help)
	{
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0006078B File Offset: 0x0005EB8B
	void IDevConsole.AddConsoleVariable<T>(string context, string name, string label, string help, Func<T> getCallback, Action<T> setCallback)
	{
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0006078D File Offset: 0x0005EB8D
	void IDevConsole.AddPlayerConsoleVariable<T>(string name, string label, string help, Func<PlayerNum, T> getCallback, Action<PlayerNum, T> setCallback)
	{
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0006078F File Offset: 0x0005EB8F
	void IDevConsole.AddPlayerCommand(Action<PlayerNum> callback, string text, string help)
	{
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x00060791 File Offset: 0x0005EB91
	void IDevConsole.AddPlayerCommand<T>(Action<PlayerNum, T> callback, string text, string help)
	{
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x00060793 File Offset: 0x0005EB93
	void IDevConsole.ExecuteCommand(string command)
	{
	}
}
