using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class ExtinguishableFire : MonoBehaviour
{
	// Token: 0x0600004E RID: 78 RVA: 0x0000342E File Offset: 0x0000162E
	private void Start()
	{
		this.m_isExtinguished = true;
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.Stop();
		base.StartCoroutine(this.StartingFire());
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000345A File Offset: 0x0000165A
	public void Extinguish()
	{
		if (this.m_isExtinguished)
		{
			return;
		}
		this.m_isExtinguished = true;
		base.StartCoroutine(this.Extinguishing());
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003479 File Offset: 0x00001679
	private IEnumerator Extinguishing()
	{
		this.fireParticleSystem.Stop();
		this.smokeParticleSystem.time = 0f;
		this.smokeParticleSystem.Play();
		for (float elapsedTime = 0f; elapsedTime < 2f; elapsedTime += Time.deltaTime)
		{
			float d = Mathf.Max(0f, 1f - elapsedTime / 2f);
			this.fireParticleSystem.transform.localScale = Vector3.one * d;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.transform.localScale = Vector3.one;
		yield return new WaitForSeconds(4f);
		base.StartCoroutine(this.StartingFire());
		yield break;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003488 File Offset: 0x00001688
	private IEnumerator StartingFire()
	{
		this.smokeParticleSystem.Stop();
		this.fireParticleSystem.time = 0f;
		this.fireParticleSystem.Play();
		for (float elapsedTime = 0f; elapsedTime < 2f; elapsedTime += Time.deltaTime)
		{
			float d = Mathf.Min(1f, elapsedTime / 2f);
			this.fireParticleSystem.transform.localScale = Vector3.one * d;
			yield return null;
		}
		this.fireParticleSystem.transform.localScale = Vector3.one;
		this.m_isExtinguished = false;
		yield break;
	}

	// Token: 0x04000065 RID: 101
	public ParticleSystem fireParticleSystem;

	// Token: 0x04000066 RID: 102
	public ParticleSystem smokeParticleSystem;

	// Token: 0x04000067 RID: 103
	protected bool m_isExtinguished;

	// Token: 0x04000068 RID: 104
	private const float m_FireStartingTime = 2f;
}
