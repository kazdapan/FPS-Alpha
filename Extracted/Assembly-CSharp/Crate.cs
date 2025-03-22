using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class Crate : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002210 File Offset: 0x00000410
	public void Shatter()
	{
		this.box.enabled = false;
		foreach (Rigidbody rigidbody in this.allParts)
		{
			rigidbody.isKinematic = false;
		}
	}

	// Token: 0x04000001 RID: 1
	public List<Rigidbody> allParts = new List<Rigidbody>();

	// Token: 0x04000002 RID: 2
	public Collider box;
}
