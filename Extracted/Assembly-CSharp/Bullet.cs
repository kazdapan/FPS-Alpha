using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Bullet : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void OnCollisionEnter(Collision objectWeHit)
	{
		if (objectWeHit.gameObject.CompareTag("Target"))
		{
			MonoBehaviour.print("hit " + objectWeHit.gameObject.name);
			Object.Destroy(base.gameObject);
			Bullet.<OnCollisionEnter>g__CreateBulletImpactEffect|0_0(objectWeHit);
		}
		if (objectWeHit.gameObject.CompareTag("Wall"))
		{
			MonoBehaviour.print("Hit a Wall, Noob");
			Bullet.<OnCollisionEnter>g__CreateBulletImpactEffect|0_0(objectWeHit);
			Object.Destroy(base.gameObject);
		}
		if (objectWeHit.gameObject.CompareTag("Floor"))
		{
			MonoBehaviour.print("Hit The Floor");
			Bullet.<OnCollisionEnter>g__CreateBulletImpactEffect|0_0(objectWeHit);
			Object.Destroy(base.gameObject);
		}
		if (objectWeHit.gameObject.CompareTag("Enemy"))
		{
			MonoBehaviour.print("Hit An Enemy !!");
			Bullet.<OnCollisionEnter>g__CreateBulletImpactEffect|0_0(objectWeHit);
			Object.Destroy(base.gameObject);
		}
		if (objectWeHit.gameObject.CompareTag("Crate"))
		{
			MonoBehaviour.print("Hit a Crate");
			objectWeHit.gameObject.GetComponent<Crate>().Shatter();
			Object.Destroy(base.gameObject);
		}
		if (objectWeHit.gameObject.CompareTag("Player"))
		{
			MonoBehaviour.print("Hit The Player !!");
			Object.Destroy(base.gameObject);
		}
		if (objectWeHit.gameObject.CompareTag("CratePiece"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021A4 File Offset: 0x000003A4
	[CompilerGenerated]
	internal static void <OnCollisionEnter>g__CreateBulletImpactEffect|0_0(Collision objectWeHit)
	{
		ContactPoint contactPoint = objectWeHit.contacts[0];
		GameObject gameObject = Object.Instantiate<GameObject>(GlobalReferences.Instance.bulletImpactEffectPrefab, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
		if (objectWeHit.gameObject.CompareTag("Player"))
		{
			Object.Destroy(gameObject);
		}
		gameObject.transform.SetParent(objectWeHit.gameObject.transform);
	}
}
