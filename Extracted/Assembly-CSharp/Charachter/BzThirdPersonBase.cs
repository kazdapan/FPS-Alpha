using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000020 RID: 32
	[RequireComponent(typeof(Animator))]
	public abstract class BzThirdPersonBase : MonoBehaviour, IBzRagdollCharacter, IBzThirdPerson
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00004A5C File Offset: 0x00002C5C
		protected virtual void Awake()
		{
			this._animator = base.GetComponent<Animator>();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004A6A File Offset: 0x00002C6A
		public Vector3 CharacterVelocity
		{
			get
			{
				if (!this._onGround)
				{
					return this._airVelocity;
				}
				return this.PlayerVelocity;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004A81 File Offset: 0x00002C81
		public virtual void CharacterEnable(bool enable)
		{
			this._enabled = enable;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004A8A File Offset: 0x00002C8A
		public void Move(Vector3 move, bool crouch, bool jump)
		{
			this._moveInput = move;
			this._crouch = crouch;
			this._jump = jump;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000094 RID: 148
		protected abstract Vector3 PlayerVelocity { get; }

		// Token: 0x06000095 RID: 149
		protected abstract bool PlayerTouchGound();

		// Token: 0x06000096 RID: 150
		protected abstract void ApplyCapsuleHeight();

		// Token: 0x06000097 RID: 151
		protected abstract void UpdatePlayerPosition(Vector3 deltaPos);

		// Token: 0x06000098 RID: 152 RVA: 0x00004AA4 File Offset: 0x00002CA4
		private void HandleGroundedVelocities(int currentAnimation)
		{
			bool flag = currentAnimation == this._animatorGrounded;
			if (!(this._jump & !this._crouch) || !flag)
			{
				return;
			}
			Vector3 characterVelocity = this.CharacterVelocity;
			characterVelocity.y += 5f;
			this._airVelocity = characterVelocity;
			this._jump = false;
			this._onGround = false;
			this._jumpPressed = true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004B04 File Offset: 0x00002D04
		private void UpdateAnimator()
		{
			this._animator.SetFloat(this._animatorForward, this._forwardAmount, 0.1f, Time.deltaTime);
			this._animator.SetFloat(this._animatorTurn, this._turnAmount, 0.1f, Time.deltaTime);
			this._animator.SetBool(this._animatorOnGround, this._onGround);
			if (!this._onGround)
			{
				this._animator.SetFloat(this._animatorJump, this.CharacterVelocity.y);
				return;
			}
			float value = (float)((Mathf.Repeat(this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1f) < 0.5f) ? 1 : -1) * this._forwardAmount;
			this._animator.SetFloat(this._animatorJumpLeg, value);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004BDC File Offset: 0x00002DDC
		private void ConvertMoveInput()
		{
			Vector3 vector = base.transform.InverseTransformDirection(this._moveInput);
			if (Math.Abs(vector.x) > 1.401298E-45f & Math.Abs(vector.z) > 1.401298E-45f)
			{
				this._turnAmount = Mathf.Atan2(vector.x, vector.z);
			}
			else
			{
				this._turnAmount = 0f;
			}
			this._forwardAmount = vector.z;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004C54 File Offset: 0x00002E54
		private void ApplyExtraTurnRotation(int currentAnimation)
		{
			if (currentAnimation != this._animatorGrounded)
			{
				return;
			}
			float num = Mathf.Lerp(180f, 360f, this._forwardAmount);
			base.transform.Rotate(0f, this._turnAmount * num * Time.deltaTime, 0f);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004CA4 File Offset: 0x00002EA4
		private void HandleAirborneVelocities()
		{
			Vector3 b = new Vector3(this._moveInput.x * 5f, this._airVelocity.y, this._moveInput.z * 5f);
			this._airVelocity = Vector3.Lerp(this._airVelocity, b, Time.deltaTime * 2f);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004D04 File Offset: 0x00002F04
		private void FixedUpdate()
		{
			if (!this._enabled)
			{
				return;
			}
			this._onGround = (!this._jumpPressed && this.PlayerTouchGound());
			this._animator.SetBool(this._animatorCrouch, this._crouch);
			int fullPathHash = this._animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
			this.ApplyCapsuleHeight();
			this.ApplyExtraTurnRotation(fullPathHash);
			this.ConvertMoveInput();
			if (this._onGround)
			{
				this.HandleGroundedVelocities(fullPathHash);
			}
			else
			{
				this.HandleAirborneVelocities();
			}
			this.UpdateAnimator();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004D90 File Offset: 0x00002F90
		private void OnAnimatorMove()
		{
			if (Time.deltaTime < Mathf.Epsilon)
			{
				return;
			}
			Vector3 b = Physics.gravity * Time.deltaTime;
			this._airVelocity += b;
			Vector3 vector;
			if (this._onGround)
			{
				vector = this._animator.deltaPosition;
				vector.y -= 5f * Time.deltaTime;
			}
			else
			{
				vector = this._airVelocity * Time.deltaTime;
			}
			if (this._firstAnimatorFrame)
			{
				vector = new Vector3(0f, vector.y, 0f);
				this._firstAnimatorFrame = false;
			}
			this.UpdatePlayerPosition(vector);
			base.transform.rotation *= this._animator.deltaRotation;
			this._jumpPressed = false;
		}

		// Token: 0x040000BA RID: 186
		private readonly int _animatorForward = Animator.StringToHash("Forward");

		// Token: 0x040000BB RID: 187
		private readonly int _animatorTurn = Animator.StringToHash("Turn");

		// Token: 0x040000BC RID: 188
		private readonly int _animatorCrouch = Animator.StringToHash("Crouch");

		// Token: 0x040000BD RID: 189
		private readonly int _animatorOnGround = Animator.StringToHash("OnGround");

		// Token: 0x040000BE RID: 190
		private readonly int _animatorJump = Animator.StringToHash("Jump");

		// Token: 0x040000BF RID: 191
		private readonly int _animatorJumpLeg = Animator.StringToHash("JumpLeg");

		// Token: 0x040000C0 RID: 192
		protected readonly int _animatorCapsuleY = Animator.StringToHash("CapsuleY");

		// Token: 0x040000C1 RID: 193
		private readonly int _animatorGrounded = Animator.StringToHash("Base Layer.Grounded.Grounded");

		// Token: 0x040000C2 RID: 194
		private const float JumpPower = 5f;

		// Token: 0x040000C3 RID: 195
		private const float AirSpeed = 5f;

		// Token: 0x040000C4 RID: 196
		private const float AirControl = 2f;

		// Token: 0x040000C5 RID: 197
		private const float StationaryTurnSpeed = 180f;

		// Token: 0x040000C6 RID: 198
		private const float MovingTurnSpeed = 360f;

		// Token: 0x040000C7 RID: 199
		private const float RunCycleLegOffset = 0.2f;

		// Token: 0x040000C8 RID: 200
		protected Animator _animator;

		// Token: 0x040000C9 RID: 201
		private bool _onGround;

		// Token: 0x040000CA RID: 202
		private Vector3 _moveInput;

		// Token: 0x040000CB RID: 203
		private bool _crouch;

		// Token: 0x040000CC RID: 204
		private bool _jump;

		// Token: 0x040000CD RID: 205
		private float _turnAmount;

		// Token: 0x040000CE RID: 206
		private float _forwardAmount;

		// Token: 0x040000CF RID: 207
		private bool _enabled = true;

		// Token: 0x040000D0 RID: 208
		protected Vector3 _airVelocity;

		// Token: 0x040000D1 RID: 209
		protected bool _jumpPressed;

		// Token: 0x040000D2 RID: 210
		protected bool _firstAnimatorFrame = true;
	}
}
