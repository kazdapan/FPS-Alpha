using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class Weapon : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x0000311F File Offset: 0x0000131F
	private void Awake()
	{
		this.shootSound = base.GetComponent<AudioSource>();
		this.readyToShoot = true;
		this.burstBulletsLeft = this.bulletsPerBurst;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003140 File Offset: 0x00001340
	public void Update()
	{
		if (this.currentShootingMode == Weapon.ShootingMode.Auto)
		{
			this.isShooting = Input.GetKey(KeyCode.Mouse0);
		}
		else if (this.currentShootingMode == Weapon.ShootingMode.Single || this.currentShootingMode == Weapon.ShootingMode.Burst)
		{
			this.isShooting = Input.GetKeyDown(KeyCode.Mouse0);
		}
		if (this.readyToShoot && this.isShooting)
		{
			this.burstBulletsLeft = this.bulletsPerBurst;
			this.FireWeapon();
			this.shootSound.Play();
			this.GunshotHitCheck();
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000031BC File Offset: 0x000013BC
	private void FireWeapon()
	{
		this.readyToShoot = false;
		Vector3 normalized = this.CalculateDirectionAndSpread().normalized;
		GameObject gameObject = Object.Instantiate<GameObject>(this.bulletPrefab, this.bulletSpawn.position, Quaternion.identity);
		gameObject.transform.forward = normalized;
		gameObject.GetComponent<Rigidbody>().AddForce(normalized.normalized * this.bulletVelocity, ForceMode.Impulse);
		base.StartCoroutine(this.DestroyBulletAfterTime(gameObject, this.bulletPrefabLifeTime));
		if (this.allowReset)
		{
			base.Invoke("ResetShot", this.shootDelay);
			this.allowReset = false;
		}
		if (this.currentShootingMode == Weapon.ShootingMode.Burst && this.burstBulletsLeft > 1)
		{
			this.burstBulletsLeft--;
			base.Invoke("FireWeapon", this.shootDelay);
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000328C File Offset: 0x0000148C
	private void GunshotHitCheck()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(this.mainCamera.position, this.mainCamera.forward), out raycastHit, this.range) && raycastHit.collider.CompareTag("Enemy"))
		{
			raycastHit.collider.GetComponent<EnemyHealth>().TakeDamage(this.damage);
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000032ED File Offset: 0x000014ED
	private void ResetShot()
	{
		this.readyToShoot = true;
		this.allowReset = true;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003300 File Offset: 0x00001500
	public Vector3 CalculateDirectionAndSpread()
	{
		Ray ray = this.playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit raycastHit;
		Vector3 point;
		if (Physics.Raycast(ray, out raycastHit))
		{
			point = raycastHit.point;
		}
		else
		{
			point = ray.GetPoint(100f);
		}
		Vector3 a = point - this.bulletSpawn.position;
		float x = Random.Range(-this.spreadIntensity, this.spreadIntensity);
		float y = Random.Range(-this.spreadIntensity, this.spreadIntensity);
		return a + new Vector3(x, y, 0f);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003398 File Offset: 0x00001598
	private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
	{
		yield return new WaitForSeconds(delay);
		Object.Destroy(bullet);
		yield break;
	}

	// Token: 0x0400004F RID: 79
	public Camera playerCamera;

	// Token: 0x04000050 RID: 80
	public bool isShooting;

	// Token: 0x04000051 RID: 81
	public bool readyToShoot;

	// Token: 0x04000052 RID: 82
	private bool allowReset = true;

	// Token: 0x04000053 RID: 83
	public float shootDelay = 2f;

	// Token: 0x04000054 RID: 84
	public int bulletsPerBurst = 3;

	// Token: 0x04000055 RID: 85
	public int burstBulletsLeft;

	// Token: 0x04000056 RID: 86
	public float spreadIntensity;

	// Token: 0x04000057 RID: 87
	public GameObject bulletPrefab;

	// Token: 0x04000058 RID: 88
	public Transform bulletSpawn;

	// Token: 0x04000059 RID: 89
	public float bulletVelocity = 30f;

	// Token: 0x0400005A RID: 90
	public float bulletPrefabLifeTime = 3f;

	// Token: 0x0400005B RID: 91
	public Transform mainCamera;

	// Token: 0x0400005C RID: 92
	[SerializeField]
	private float range = 250f;

	// Token: 0x0400005D RID: 93
	private int damage = 3;

	// Token: 0x0400005E RID: 94
	public AudioSource shootSound;

	// Token: 0x0400005F RID: 95
	public Weapon.ShootingMode currentShootingMode;

	// Token: 0x0200002B RID: 43
	public enum ShootingMode
	{
		// Token: 0x040000E9 RID: 233
		Single,
		// Token: 0x040000EA RID: 234
		Burst,
		// Token: 0x040000EB RID: 235
		Auto
	}
}
