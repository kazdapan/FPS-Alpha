using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000022 RID: 34
	[RequireComponent(typeof(IBzThirdPerson))]
	public sealed class BzThirdPersonControl : MonoBehaviour
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00005030 File Offset: 0x00003230
		private void Start()
		{
			if (Camera.main == null)
			{
				Debug.LogError("Error: no main camera found.");
			}
			else
			{
				this._camTransform = Camera.main.transform;
			}
			this._character = base.GetComponent<IBzThirdPerson>();
			this._health = base.GetComponent<IBzDamageable>();
			this._ragdoll = base.GetComponent<IBzRagdoll>();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000508A File Offset: 0x0000328A
		private void Update()
		{
			if (!this._jumpPressed)
			{
				this._jumpPressed = Input.GetButtonDown("Jump");
			}
			if (!this._fire)
			{
				this._fire = Input.GetMouseButtonDown(0);
			}
			this._crouch = Input.GetKey(KeyCode.C);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000050C8 File Offset: 0x000032C8
		private void FixedUpdate()
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			Vector3 normalized = new Vector3(this._camTransform.forward.x, 0f, this._camTransform.forward.z).normalized;
			Vector3 vector = axis2 * normalized + axis * this._camTransform.right;
			if (vector.magnitude > 1f)
			{
				vector.Normalize();
			}
			this.ProcessDamage();
			this._character.Move(vector, this._crouch, this._jumpPressed);
			this._jumpPressed = false;
			if (this._ragdoll != null && this._ragdoll.IsRagdolled)
			{
				this._ragdoll.AddExtraMove(vector * 100f * Time.deltaTime);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000051AC File Offset: 0x000033AC
		private void ProcessDamage()
		{
			if (this._health == null)
			{
				return;
			}
			if (this._fire)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				this._health.Shot(ray, 0.4f, 10000f);
				this._fire = false;
			}
			if (this._jumpPressed && this._health.IsDead())
			{
				this._health.Health = 1f;
				this._jumpPressed = false;
			}
		}

		// Token: 0x040000D4 RID: 212
		private IBzThirdPerson _character;

		// Token: 0x040000D5 RID: 213
		private IBzRagdoll _ragdoll;

		// Token: 0x040000D6 RID: 214
		private IBzDamageable _health;

		// Token: 0x040000D7 RID: 215
		private Transform _camTransform;

		// Token: 0x040000D8 RID: 216
		private bool _jumpPressed;

		// Token: 0x040000D9 RID: 217
		private bool _fire;

		// Token: 0x040000DA RID: 218
		private bool _crouch;
	}
}
