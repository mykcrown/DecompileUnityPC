using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B33 RID: 2867
public class SceneLoaderMono : MonoBehaviour
{
	// Token: 0x0600532E RID: 21294 RVA: 0x001AE3E4 File Offset: 0x001AC7E4
	public static SceneLoaderMono CreateNew(string name)
	{
		SceneLoaderMono sceneLoaderMono = new GameObject(name).AddComponent<SceneLoaderMono>();
		UnityEngine.Object.DontDestroyOnLoad(sceneLoaderMono.gameObject);
		sceneLoaderMono.gameObject.SetActive(false);
		return sceneLoaderMono;
	}

	// Token: 0x0600532F RID: 21295 RVA: 0x001AE415 File Offset: 0x001AC815
	private void updateActiveState()
	{
		if (this.unloadCount > 0 || this.loadCount > 0)
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005330 RID: 21296 RVA: 0x001AE44C File Offset: 0x001AC84C
	public void UnloadScene(string sceneName, Action callback)
	{
		this.unloadCount++;
		this.updateActiveState();
		base.StartCoroutine(this.unloadScene(sceneName, callback));
	}

	// Token: 0x06005331 RID: 21297 RVA: 0x001AE474 File Offset: 0x001AC874
	private IEnumerator unloadScene(string sceneName, Action callback)
	{
		AsyncOperation async = null;
		try
		{
			async = SceneManager.UnloadSceneAsync(sceneName);
		}
		catch
		{
			Debug.Log("Failed to unload " + sceneName);
		}
		if (async == null)
		{
			Debug.Log("Unload failed just proceed");
			callback();
		}
		else
		{
			while (!async.isDone)
			{
				yield return null;
			}
			this.unloadCount--;
			this.updateActiveState();
			callback();
		}
		yield break;
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x001AE49D File Offset: 0x001AC89D
	public void LoadSceneSync(string sceneName, LoadSceneMode mode)
	{
		SceneManager.LoadScene(sceneName, mode);
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x001AE4A6 File Offset: 0x001AC8A6
	public void LoadScene(string sceneName, LoadSceneMode mode, Action callback)
	{
		this.loadCount++;
		this.updateActiveState();
		base.StartCoroutine(this.loadScene(sceneName, mode, callback));
	}

	// Token: 0x06005334 RID: 21300 RVA: 0x001AE4CC File Offset: 0x001AC8CC
	private IEnumerator loadScene(string sceneName, LoadSceneMode mode, Action callback)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, mode);
		while (!async.isDone)
		{
			yield return null;
		}
		this.loadCount--;
		this.updateActiveState();
		callback();
		yield break;
	}

	// Token: 0x040034D8 RID: 13528
	private int unloadCount;

	// Token: 0x040034D9 RID: 13529
	private int loadCount;
}
