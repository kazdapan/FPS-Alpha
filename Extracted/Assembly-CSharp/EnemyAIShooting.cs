using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class EnemyAIShooting : MonoBehaviour
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002650 File Offset: 0x00000850
	private void Update()
	{
		if (this.EnemyHealth < 0 || this.player == null)
		{
			return;
		}
		this.DisBtw = Vector2.Distance(this.player.gameObject.transform.position, base.gameObject.transform.position);
		this.Gun.LookAt(this.player.transform);
		if (this.DisBtw > this.MinDis && this.DisBtw < this.MaxDis)
		{
			base.transform.position = Vector2.MoveTowards(base.transform.position, this.player.gameObject.transform.position, this.EnemySpeed * Time.deltaTime);
		}
		this.LookAtPlayer();
		if (this.IsShoot && this.DisBtw <= this.MinDis)
		{
			base.StartCoroutine(this.Fire());
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002758 File Offset: 0x00000958
	private void LookAtPlayer()
	{
		Vector3 vector = this.player.transform.position - base.transform.position;
		float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		this.Gun.localRotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000027B9 File Offset: 0x000009B9
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Object.Destroy(collision.gameObject);
			this.TakeDamage();
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000027E3 File Offset: 0x000009E3
	private void TakeDamage()
	{
		this.EnemyHealth--;
		if (this.EnemyHealth <= 0)
		{
			Object.Destroy(base.gameObject, this.DestroyTime);
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000280D File Offset: 0x00000A0D
	private IEnumerator Fire()
	{
		this.IsShoot = false;
		Object.Destroy(Object.Instantiate<GameObject>(this.Bullets, this.Gun.position, this.Gun.rotation, this.Gun.transform), this.BulletLifeTime);
		yield return new WaitForSeconds(this.TimeBtwShots);
		this.IsShoot = true;
		yield break;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000281C File Offset: 0x00000A1C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(this.Gun.position, this.player.transform.position);
	}

	// Token: 0x04000016 RID: 22
	[Header("GameObjects")]
	[SerializeField]
	private GameObject player;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	private GameObject Bullets;

	// Token: 0x04000018 RID: 24
	[Header("Distance Between Player")]
	[SerializeField]
	private float MaxDis = 5f;

	// Token: 0x04000019 RID: 25
	[SerializeField]
	private float MinDis = 1f;

	// Token: 0x0400001A RID: 26
	private float DisBtw;

	// Token: 0x0400001B RID: 27
	[Header("Enemy Constraints")]
	[SerializeField]
	private int EnemyHealth = 10;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private float EnemySpeed = 10f;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	private float DestroyTime = 3f;

	// Token: 0x0400001E RID: 30
	[SerializeField]
	private Transform Gun;

	// Token: 0x0400001F RID: 31
	[Header("Bullet Constraints")]
	[SerializeField]
	private float BulletLifeTime = 5f;

	// Token: 0x04000020 RID: 32
	[SerializeField]
	private float TimeBtwShots = 3f;

	// Token: 0x04000021 RID: 33
	private bool IsShoot = true;
}
