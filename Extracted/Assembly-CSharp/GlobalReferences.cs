using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class GlobalReferences : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000026 RID: 38 RVA: 0x00002C1C File Offset: 0x00000E1C
	// (set) Token: 0x06000027 RID: 39 RVA: 0x00002C23 File Offset: 0x00000E23
	public static GlobalReferences Instance { get; set; }

	// Token: 0x06000028 RID: 40 RVA: 0x00002C2B File Offset: 0x00000E2B
	private void Awake()
	{
		if (GlobalReferences.Instance != null && GlobalReferences.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GlobalReferences.Instance = this;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002C59 File Offset: 0x00000E59
	public void Update()
	{
		this.enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (this.enemies.Length == 0)
		{
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene("WinScreen");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.PauseGame();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002C94 File Offset: 0x00000E94
	private void PauseGame()
	{
		this.Resume.gameObject.SetActive(true);
		this.LowSens.gameObject.SetActive(true);
		this.MediumSens.gameObject.SetActive(true);
		this.HighSens.gameObject.SetActive(true);
		Time.timeScale = 0f;
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002CF8 File Offset: 0x00000EF8
	public void ResumeGame()
	{
		Time.timeScale = 1f;
		this.Resume.gameObject.SetActive(false);
		this.LowSens.gameObject.SetActive(false);
		this.MediumSens.gameObject.SetActive(false);
		this.HighSens.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Token: 0x04000034 RID: 52
	public Button Resume;

	// Token: 0x04000035 RID: 53
	public Button LowSens;

	// Token: 0x04000036 RID: 54
	public Button MediumSens;

	// Token: 0x04000037 RID: 55
	public Button HighSens;

	// Token: 0x04000038 RID: 56
	public GameObject bulletImpactEffectPrefab;

	// Token: 0x04000039 RID: 57
	public GameObject[] enemies;
}
