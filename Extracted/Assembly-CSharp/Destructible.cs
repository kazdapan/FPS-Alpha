using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Destructible : MonoBehaviour
{
	// Token: 0x06000063 RID: 99 RVA: 0x000039CC File Offset: 0x00001BCC
	private void OnMouseDown()
	{
		Object.Instantiate<GameObject>(this.destroyedVersion, base.transform.position, base.transform.rotation);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400008E RID: 142
	public GameObject destroyedVersion;
}
