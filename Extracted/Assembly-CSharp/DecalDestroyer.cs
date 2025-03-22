using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class DecalDestroyer : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x0000340C File Offset: 0x0000160C
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.lifeTime);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000064 RID: 100
	public float lifeTime = 5f;
}
