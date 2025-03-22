using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000023 RID: 35
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public sealed class BzThirdPersonRigid : BzThirdPersonBase
	{
		// Token: 0x060000AD RID: 173 RVA: 0x0000522B File Offset: 0x0000342B
		protected override void Awake()
		{
			base.Awake();
			this._capsuleCollider = base.GetComponent<CapsuleCollider>();
			this._rigidbody = base.GetComponent<Rigidbody>();
			if (base.GetComponent<CharacterController>() != null)
			{
				Debug.LogWarning("You do not needed to attach 'CharacterController' to controller with 'Rigidbody'");
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005263 File Offset: 0x00003463
		public override void CharacterEnable(bool enable)
		{
			base.CharacterEnable(enable);
			this._capsuleCollider.enabled = enable;
			this._rigidbody.isKinematic = !enable;
			if (enable)
			{
				this._firstAnimatorFrame = true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005291 File Offset: 0x00003491
		protected override Vector3 PlayerVelocity
		{
			get
			{
				return this._rigidbody.velocity;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000052A0 File Offset: 0x000034A0
		protected override void ApplyCapsuleHeight()
		{
			float @float = this._animator.GetFloat(this._animatorCapsuleY);
			this._capsuleCollider.height = @float;
			Vector3 center = this._capsuleCollider.center;
			center.y = @float / 2f;
			this._capsuleCollider.center = center;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000052F4 File Offset: 0x000034F4
		private void ProccessOnCollisionOccured(Collision collision)
		{
			float num = base.transform.position.y + this._capsuleCollider.center.y - this._capsuleCollider.height / 2f + this._capsuleCollider.radius * 0.8f;
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				if (contactPoint.point.y < num && !contactPoint.otherCollider.transform.IsChildOf(base.transform))
				{
					this._groundChecker = true;
					Debug.DrawRay(contactPoint.point, contactPoint.normal, Color.blue);
					return;
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000053AC File Offset: 0x000035AC
		private void OnCollisionStay(Collision collision)
		{
			this.ProccessOnCollisionOccured(collision);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000053B5 File Offset: 0x000035B5
		private void OnCollisionEnter(Collision collision)
		{
			this.ProccessOnCollisionOccured(collision);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000053BE File Offset: 0x000035BE
		protected override bool PlayerTouchGound()
		{
			bool groundChecker = this._groundChecker;
			this._groundChecker = false;
			return groundChecker & this._jumpStartedTime + 0.5f < Time.time;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000053E4 File Offset: 0x000035E4
		protected override void UpdatePlayerPosition(Vector3 deltaPos)
		{
			Vector3 vector = deltaPos / Time.deltaTime;
			if (!this._jumpPressed)
			{
				vector.y = this._rigidbody.velocity.y;
			}
			else
			{
				this._jumpStartedTime = Time.time;
			}
			this._airVelocity = vector;
			this._rigidbody.velocity = vector;
		}

		// Token: 0x040000DB RID: 219
		private CapsuleCollider _capsuleCollider;

		// Token: 0x040000DC RID: 220
		private Rigidbody _rigidbody;

		// Token: 0x040000DD RID: 221
		private bool _groundChecker;

		// Token: 0x040000DE RID: 222
		private float _jumpStartedTime;
	}
}
