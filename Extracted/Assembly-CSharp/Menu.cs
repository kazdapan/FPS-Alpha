using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000009 RID: 9
public class Menu : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002D61 File Offset: 0x00000F61
	public void Play()
	{
		SceneManager.LoadSceneAsync("Level 01");
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002D6E File Offset: 0x00000F6E
	public void Quit()
	{
		Application.Quit();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002D75 File Offset: 0x00000F75
	public void MainMenu()
	{
		SceneManager.LoadSceneAsync("Menu");
	}
}
