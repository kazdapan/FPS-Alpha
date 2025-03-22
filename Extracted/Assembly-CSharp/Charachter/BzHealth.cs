using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x0200001E RID: 30
	public sealed class BzHealth : MonoBehaviour, IBzDamageable
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003FE1 File Offset: 0x000021E1
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003FEC File Offset: 0x000021EC
		public float Health
		{
			get
			{
				return this._health;
			}
			set
			{
				if (this._health > Mathf.Epsilon && value <= Mathf.Epsilon)
				{
					if (this._bzRagdoll != null)
					{
						this._bzRagdoll.IsRagdolled = true;
					}
				}
				else if (this._health <= Mathf.Epsilon && value > Mathf.Epsilon && this._bzRagdoll != null)
				{
					this._bzRagdoll.IsRagdolled = false;
				}
				this._health = Mathf.Clamp01(value);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004059 File Offset: 0x00002259
		private void Awake()
		{
			this._bzRagdoll = base.GetComponent<IBzRagdoll>();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004068 File Offset: 0x00002268
		public void Shot(Ray ray, float force, float distance)
		{
			Debug.DrawRay(ray.origin, ray.origin + ray.direction * distance, Color.red, 2f);
			RaycastHit raycastHit;
			if (this._bzRagdoll == null)
			{
				if (!Physics.Raycast(ray, out raycastHit, distance))
				{
					return;
				}
			}
			else if (!this._bzRagdoll.Raycast(ray, out raycastHit, distance))
			{
				return;
			}
			this.Health -= force;
			Rigidbody rigidbody = raycastHit.rigidbody;
			if (rigidbody == null || raycastHit.transform.root != base.transform.root)
			{
				return;
			}
			if (!this.IsDead())
			{
				return;
			}
			this._impactTarget = rigidbody;
			this._impactDirection = ray.direction;
			this._impactEndTime = Time.time + 0.25f;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004136 File Offset: 0x00002336
		public bool IsDead()
		{
			return this.Health <= Mathf.Epsilon;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004148 File Offset: 0x00002348
		public bool IsFullHealth()
		{
			return this.Health >= 1f - Mathf.Epsilon;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004160 File Offset: 0x00002360
		private void FixedUpdate()
		{
			if (Time.time < this._impactEndTime)
			{
				this._impactTarget.AddForce(this._impactDirection * Time.deltaTime * 80f, ForceMode.VelocityChange);
			}
		}

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		private IBzRagdoll _bzRagdoll;

		// Token: 0x040000A7 RID: 167
		private float _health = 1f;

		// Token: 0x040000A8 RID: 168
		private float _impactEndTime;

		// Token: 0x040000A9 RID: 169
		private Rigidbody _impactTarget;

		// Token: 0x040000AA RID: 170
		private Vector3 _impactDirection;
	}
}
