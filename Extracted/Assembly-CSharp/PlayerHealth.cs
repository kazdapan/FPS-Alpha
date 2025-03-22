using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200000B RID: 11
public class PlayerHealth : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x00002E63 File Offset: 0x00001063
	public void Update()
	{
		this.Health.text = "Health : " + this.currentHealth.ToString();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002E85 File Offset: 0x00001085
	public void TakeDamage(int damage)
	{
		this.currentHealth -= damage;
		if (this.currentHealth <= 0 && !this.isPlayerDead)
		{
			SceneManager.LoadSceneAsync("Death Screen");
			Cursor.lockState = CursorLockMode.None;
			this.isPlayerDead = true;
		}
	}

	// Token: 0x0400003D RID: 61
	public int currentHealth = 30;

	// Token: 0x0400003E RID: 62
	public bool isPlayerDead;

	// Token: 0x0400003F RID: 63
	public TMP_Text Health;
}
