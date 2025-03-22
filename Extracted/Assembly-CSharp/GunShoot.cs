using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class GunShoot : MonoBehaviour
{
	// Token: 0x06000057 RID: 87 RVA: 0x00003551 File Offset: 0x00001751
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.gunAim = base.GetComponentInParent<GunAim>();
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0000356C File Offset: 0x0000176C
	private void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time > this.nextFire && !this.gunAim.GetIsOutOfBounds())
		{
			this.nextFire = Time.time + this.fireRate;
			this.muzzleFlash.Play();
			this.cartridgeEjection.Play();
			this.anim.SetTrigger("Fire");
			RaycastHit hit;
			if (Physics.Raycast(this.gunEnd.position, this.gunEnd.forward, out hit, this.weaponRange))
			{
				this.HandleHit(hit);
			}
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00003604 File Offset: 0x00001804
	private void HandleHit(RaycastHit hit)
	{
		if (hit.collider.sharedMaterial != null)
		{
			string name = hit.collider.sharedMaterial.name;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1044434307U)
			{
				if (num <= 329707512U)
				{
					if (num != 81868168U)
					{
						if (num != 329707512U)
						{
							return;
						}
						if (!(name == "WaterFilledExtinguish"))
						{
							return;
						}
						this.SpawnDecal(hit, this.waterLeakExtinguishEffect);
						this.SpawnDecal(hit, this.metalHitEffect);
					}
					else
					{
						if (!(name == "Wood"))
						{
							return;
						}
						this.SpawnDecal(hit, this.woodHitEffect);
						return;
					}
				}
				else if (num != 970575400U)
				{
					if (num != 1044434307U)
					{
						return;
					}
					if (!(name == "Sand"))
					{
						return;
					}
					this.SpawnDecal(hit, this.sandHitEffect);
					return;
				}
				else
				{
					if (!(name == "WaterFilled"))
					{
						return;
					}
					this.SpawnDecal(hit, this.waterLeakEffect);
					this.SpawnDecal(hit, this.metalHitEffect);
					return;
				}
			}
			else if (num <= 2840670588U)
			{
				if (num != 1842662042U)
				{
					if (num != 2840670588U)
					{
						return;
					}
					if (!(name == "Metal"))
					{
						return;
					}
					this.SpawnDecal(hit, this.metalHitEffect);
					return;
				}
				else
				{
					if (!(name == "Stone"))
					{
						return;
					}
					this.SpawnDecal(hit, this.stoneHitEffect);
					return;
				}
			}
			else if (num != 3966976176U)
			{
				if (num != 4022181330U)
				{
					return;
				}
				if (!(name == "Meat"))
				{
					return;
				}
				this.SpawnDecal(hit, this.fleshHitEffects[Random.Range(0, this.fleshHitEffects.Length)]);
				return;
			}
			else
			{
				if (!(name == "Character"))
				{
					return;
				}
				this.SpawnDecal(hit, this.fleshHitEffects[Random.Range(0, this.fleshHitEffects.Length)]);
				return;
			}
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000037BD File Offset: 0x000019BD
	private void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		Object.Instantiate<GameObject>(prefab, hit.point, Quaternion.LookRotation(hit.normal)).transform.SetParent(hit.collider.transform);
	}

	// Token: 0x0400006F RID: 111
	public float fireRate = 0.25f;

	// Token: 0x04000070 RID: 112
	public float weaponRange = 20f;

	// Token: 0x04000071 RID: 113
	public Transform gunEnd;

	// Token: 0x04000072 RID: 114
	public ParticleSystem muzzleFlash;

	// Token: 0x04000073 RID: 115
	public ParticleSystem cartridgeEjection;

	// Token: 0x04000074 RID: 116
	public GameObject metalHitEffect;

	// Token: 0x04000075 RID: 117
	public GameObject sandHitEffect;

	// Token: 0x04000076 RID: 118
	public GameObject stoneHitEffect;

	// Token: 0x04000077 RID: 119
	public GameObject waterLeakEffect;

	// Token: 0x04000078 RID: 120
	public GameObject waterLeakExtinguishEffect;

	// Token: 0x04000079 RID: 121
	public GameObject[] fleshHitEffects;

	// Token: 0x0400007A RID: 122
	public GameObject woodHitEffect;

	// Token: 0x0400007B RID: 123
	private float nextFire;

	// Token: 0x0400007C RID: 124
	private Animator anim;

	// Token: 0x0400007D RID: 125
	private GunAim gunAim;
}
