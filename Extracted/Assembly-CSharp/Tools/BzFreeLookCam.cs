using System;
using BzKovSoft.RagdollTemplate.Scripts.Charachter;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Tools
{
	// Token: 0x0200001B RID: 27
	public sealed class BzFreeLookCam : MonoBehaviour
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003B14 File Offset: 0x00001D14
		private void Start()
		{
			this._camera = Camera.main.transform;
			if (this._pivot == null)
			{
				Debug.LogError("CameraFree Missing pivot");
				return;
			}
			this._cameraPivot = this._camera.parent;
			this._ragdoll = this._pivot.GetComponentInParent<IBzRagdoll>();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003B6C File Offset: 0x00001D6C
		private void Update()
		{
			if (this.UpdateCameraPos(false))
			{
				this._callCountChecker++;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003B88 File Offset: 0x00001D88
		private void LateUpdate()
		{
			if (this.UpdateCameraPos(true))
			{
				this._callCountChecker++;
			}
			if (this._callCountChecker != 1)
			{
				throw new InvalidOperationException("There are invalid count of 'setting camera' calls. Count = " + this._callCountChecker.ToString());
			}
			this._callCountChecker = 0;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BD8 File Offset: 0x00001DD8
		private bool UpdateCameraPos(bool lateUpdate)
		{
			if (this._ragdoll != null && this._ragdoll.IsRagdolled)
			{
				if (lateUpdate)
				{
					return false;
				}
			}
			else if (!lateUpdate)
			{
				return false;
			}
			if (this._pivot == null || Time.deltaTime < Mathf.Epsilon)
			{
				return true;
			}
			base.transform.position = this._pivot.transform.position;
			this.HandleRotationMovement();
			this.HandleWalls();
			return true;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003C48 File Offset: 0x00001E48
		private void HandleRotationMovement()
		{
			float axis = Input.GetAxis("Mouse X");
			float axis2 = Input.GetAxis("Mouse Y");
			if (this._turnSmoothing > 0f)
			{
				this._smoothX = Mathf.SmoothDamp(this._smoothX, axis, ref this._smoothXvelocity, this._turnSmoothing);
				this._smoothY = Mathf.SmoothDamp(this._smoothY, axis2, ref this._smoothYvelocity, this._turnSmoothing);
			}
			else
			{
				this._smoothX = axis;
				this._smoothY = axis2;
			}
			this._yawAngle += this._smoothX * this._mouseSensitive;
			this._pitchAngle -= this._smoothY * this._mouseSensitive;
			this._pitchAngle = Mathf.Clamp(this._pitchAngle, -this._tiltMin, this._tiltMax);
			this._cameraPivot.localRotation = Quaternion.Euler(this._pitchAngle, this._yawAngle, 0f);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003D38 File Offset: 0x00001F38
		private void HandleWalls()
		{
			Vector3 vector = this._camera.position - this._cameraPivot.position;
			vector.Normalize();
			RaycastHit[] array = Physics.SphereCastAll(this._cameraPivot.position, 0.3f, vector, this._maxDistanse);
			float num = this._maxDistanse;
			foreach (RaycastHit raycastHit in array)
			{
				if (raycastHit.transform.root != this._pivot.transform.root && num > raycastHit.distance)
				{
					num = raycastHit.distance;
				}
			}
			if (num < this._minDistanse)
			{
				num = this._minDistanse;
			}
			Debug.DrawRay(this._cameraPivot.position, vector * num);
			this._camera.position = this._cameraPivot.position + vector * num;
		}

		// Token: 0x04000091 RID: 145
		[SerializeField]
		private Transform _pivot;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		private float _turnSmoothing = 0.1f;

		// Token: 0x04000093 RID: 147
		[SerializeField]
		private float _tiltMax = 75f;

		// Token: 0x04000094 RID: 148
		[SerializeField]
		private float _tiltMin = 45f;

		// Token: 0x04000095 RID: 149
		[SerializeField]
		private float _maxDistanse = 3f;

		// Token: 0x04000096 RID: 150
		[SerializeField]
		private float _minDistanse = 0.7f;

		// Token: 0x04000097 RID: 151
		[Range(0f, 10f)]
		[SerializeField]
		private float _mouseSensitive = 1.5f;

		// Token: 0x04000098 RID: 152
		private Transform _cameraPivot;

		// Token: 0x04000099 RID: 153
		private Transform _camera;

		// Token: 0x0400009A RID: 154
		private IBzRagdoll _ragdoll;

		// Token: 0x0400009B RID: 155
		private float _yawAngle;

		// Token: 0x0400009C RID: 156
		private float _pitchAngle;

		// Token: 0x0400009D RID: 157
		private float _smoothX;

		// Token: 0x0400009E RID: 158
		private float _smoothY;

		// Token: 0x0400009F RID: 159
		private float _smoothXvelocity;

		// Token: 0x040000A0 RID: 160
		private float _smoothYvelocity;

		// Token: 0x040000A1 RID: 161
		private int _callCountChecker;
	}
}
