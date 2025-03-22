using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class EnemyHealth : MonoBehaviour
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002BD9 File Offset: 0x00000DD9
	public void TakeDamage(int damage)
	{
		this.currentHealth -= damage;
		if (this.currentHealth <= 0 && !this.isEnemyDead)
		{
			Object.Destroy(base.gameObject);
			this.isEnemyDead = true;
		}
	}

	// Token: 0x04000031 RID: 49
	public int currentHealth = 20;

	// Token: 0x04000032 RID: 50
	public bool isEnemyDead;
}
