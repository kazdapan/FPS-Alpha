using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class WaveSpawner : MonoBehaviour
{
	// Token: 0x0600003F RID: 63 RVA: 0x000030EF File Offset: 0x000012EF
	private void Start()
	{
		base.StartCoroutine(this.SpawnGameObject());
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000030FE File Offset: 0x000012FE
	private void Update()
	{
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003100 File Offset: 0x00001300
	private IEnumerator SpawnGameObject()
	{
		int num;
		for (int x = 0; x < this.spawnTotal; x = num + 1)
		{
			Object.Instantiate<Transform>(this.spawnObject, this.spawnPoint.position, this.spawnPoint.rotation);
			yield return new WaitForSeconds(this.timeBetweenSpawns);
			num = x;
		}
		yield break;
	}

	// Token: 0x0400004B RID: 75
	public Transform spawnPoint;

	// Token: 0x0400004C RID: 76
	public Transform spawnObject;

	// Token: 0x0400004D RID: 77
	private int spawnTotal = 10;

	// Token: 0x0400004E RID: 78
	public float timeBetweenSpawns;
}
