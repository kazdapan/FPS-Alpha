using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000021 RID: 33
	[RequireComponent(typeof(CharacterController))]
	public sealed class BzThirdPersonChCtrler : BzThirdPersonBase
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00004F04 File Offset: 0x00003104
		protected override void Awake()
		{
			base.Awake();
			this._characterController = base.GetComponent<CharacterController>();
			if (base.GetComponent<CapsuleCollider>() != null)
			{
				Debug.LogWarning("You do not needed to attach 'CapsuleCollider' to controller with 'CharacterController'");
			}
			if (base.GetComponent<Rigidbody>() != null)
			{
				Debug.LogWarning("You do not needed to attach 'rigidbody' to controller with 'CharacterController'");
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004F53 File Offset: 0x00003153
		public override void CharacterEnable(bool enable)
		{
			base.CharacterEnable(enable);
			this._characterController.enabled = enable;
			if (enable)
			{
				this._firstAnimatorFrame = true;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004F72 File Offset: 0x00003172
		protected override Vector3 PlayerVelocity
		{
			get
			{
				return this._characterController.velocity;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004F80 File Offset: 0x00003180
		protected override void ApplyCapsuleHeight()
		{
			float @float = this._animator.GetFloat(this._animatorCapsuleY);
			this._characterController.height = @float;
			Vector3 center = this._characterController.center;
			center.y = @float / 2f;
			center.y += 0.03f;
			this._characterController.center = center;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004FE1 File Offset: 0x000031E1
		protected override bool PlayerTouchGound()
		{
			return this._characterController.isGrounded;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004FEE File Offset: 0x000031EE
		protected override void UpdatePlayerPosition(Vector3 deltaPos)
		{
			if (this._characterController.enabled)
			{
				this._characterController.Move(deltaPos);
				if (!this._characterController.isGrounded)
				{
					return;
				}
			}
			this._airVelocity = Vector3.zero;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005023 File Offset: 0x00003223
		private void asd()
		{
		}

		// Token: 0x040000D3 RID: 211
		private CharacterController _characterController;
	}
}
