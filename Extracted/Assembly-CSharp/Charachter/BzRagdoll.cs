using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x0200001F RID: 31
	[RequireComponent(typeof(IBzRagdollCharacter))]
	public sealed class BzRagdoll : MonoBehaviour, IBzRagdoll
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000041A8 File Offset: 0x000023A8
		public bool Raycast(Ray ray, out RaycastHit hit, float distance)
		{
			foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, distance))
			{
				if (raycastHit.transform != base.transform && raycastHit.transform.root == base.transform.root)
				{
					hit = raycastHit;
					return true;
				}
			}
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00004216 File Offset: 0x00002416
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000422C File Offset: 0x0000242C
		public bool IsRagdolled
		{
			get
			{
				return this._state == BzRagdoll.RagdollState.Ragdolled || this._state == BzRagdoll.RagdollState.WaitStablePosition;
			}
			set
			{
				if (value)
				{
					this.RagdollIn();
					return;
				}
				this.RagdollOut();
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004240 File Offset: 0x00002440
		public void AddExtraMove(Vector3 move)
		{
			if (this.IsRagdolled)
			{
				Vector3 a = new Vector3(move.x * 5f, 0f, move.z * 5f);
				foreach (BzRagdoll.RigidComponent rigidComponent in this._rigids)
				{
					rigidComponent.RigidBody.AddForce(a / 100f, ForceMode.VelocityChange);
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000042D0 File Offset: 0x000024D0
		private void Start()
		{
			this._anim = base.GetComponent<Animator>();
			this._hipsTransform = this._anim.GetBoneTransform(HumanBodyBones.Hips);
			this._hipsTransformRigid = this._hipsTransform.GetComponent<Rigidbody>();
			this._bzRagdollCharacter = base.GetComponent<IBzRagdollCharacter>();
			foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>())
			{
				if (!(rigidbody.transform == base.transform))
				{
					BzRagdoll.RigidComponent item = new BzRagdoll.RigidComponent(rigidbody);
					this._rigids.Add(item);
				}
			}
			this.ActivateRagdollParts(false);
			Transform[] componentsInChildren2 = base.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				BzRagdoll.TransformComponent item2 = new BzRagdoll.TransformComponent(componentsInChildren2[i]);
				this._transforms.Add(item2);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004390 File Offset: 0x00002590
		private void FixedUpdate()
		{
			if (this._state == BzRagdoll.RagdollState.WaitStablePosition && this._hipsTransformRigid.velocity.magnitude < 0.1f)
			{
				this.GetUp();
			}
			if (this._state == BzRagdoll.RagdollState.Animated && this._bzRagdollCharacter.CharacterVelocity.y < -10f)
			{
				this.RagdollIn();
				this.RagdollOut();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000043F4 File Offset: 0x000025F4
		private void LateUpdate()
		{
			if (this._state != BzRagdoll.RagdollState.BlendToAnim)
			{
				return;
			}
			float num = 1f - Mathf.InverseLerp(this._ragdollingEndTime, this._ragdollingEndTime + 0.5f, Time.time);
			if (this._storedHipsPositionPrivBlend != this._hipsTransform.position)
			{
				this._storedHipsPositionPrivAnim = this._hipsTransform.position;
			}
			this._storedHipsPositionPrivBlend = Vector3.Lerp(this._storedHipsPositionPrivAnim, this._storedHipsPosition, num);
			this._hipsTransform.position = this._storedHipsPositionPrivBlend;
			foreach (BzRagdoll.TransformComponent transformComponent in this._transforms)
			{
				if (transformComponent.PrivRotation != transformComponent.Transform.localRotation)
				{
					transformComponent.PrivRotation = Quaternion.Slerp(transformComponent.Transform.localRotation, transformComponent.StoredRotation, num);
					transformComponent.Transform.localRotation = transformComponent.PrivRotation;
				}
				if (transformComponent.PrivPosition != transformComponent.Transform.localPosition)
				{
					transformComponent.PrivPosition = Vector3.Slerp(transformComponent.Transform.localPosition, transformComponent.StoredPosition, num);
					transformComponent.Transform.localPosition = transformComponent.PrivPosition;
				}
			}
			if (Mathf.Abs(num) < Mathf.Epsilon)
			{
				this._state = BzRagdoll.RagdollState.Animated;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004568 File Offset: 0x00002768
		private static IEnumerator FixTransformAndEnableJoint(BzRagdoll.RigidComponent joint)
		{
			if (joint.Joint == null || !joint.Joint.autoConfigureConnectedAnchor)
			{
				yield break;
			}
			SoftJointLimit highTwistLimit = default(SoftJointLimit);
			SoftJointLimit lowTwistLimit = default(SoftJointLimit);
			SoftJointLimit swing1Limit = default(SoftJointLimit);
			SoftJointLimit swing2Limit = default(SoftJointLimit);
			SoftJointLimit curHighTwistLimit = highTwistLimit = joint.Joint.highTwistLimit;
			SoftJointLimit curLowTwistLimit = lowTwistLimit = joint.Joint.lowTwistLimit;
			SoftJointLimit curSwing1Limit = swing1Limit = joint.Joint.swing1Limit;
			SoftJointLimit curSwing2Limit = swing2Limit = joint.Joint.swing2Limit;
			float aTime = 0.3f;
			Vector3 startConPosition = joint.Joint.connectedBody.transform.InverseTransformVector(joint.Joint.transform.position - joint.Joint.connectedBody.transform.position);
			joint.Joint.autoConfigureConnectedAnchor = false;
			for (float t = 0f; t < 1f; t += Time.deltaTime / aTime)
			{
				Vector3 connectedAnchor = Vector3.Lerp(startConPosition, joint.ConnectedAnchorDefault, t);
				joint.Joint.connectedAnchor = connectedAnchor;
				curHighTwistLimit.limit = Mathf.Lerp(177f, highTwistLimit.limit, t);
				curLowTwistLimit.limit = Mathf.Lerp(-177f, lowTwistLimit.limit, t);
				curSwing1Limit.limit = Mathf.Lerp(177f, swing1Limit.limit, t);
				curSwing2Limit.limit = Mathf.Lerp(177f, swing2Limit.limit, t);
				joint.Joint.highTwistLimit = curHighTwistLimit;
				joint.Joint.lowTwistLimit = curLowTwistLimit;
				joint.Joint.swing1Limit = curSwing1Limit;
				joint.Joint.swing2Limit = curSwing2Limit;
				yield return null;
			}
			joint.Joint.connectedAnchor = joint.ConnectedAnchorDefault;
			yield return new WaitForFixedUpdate();
			joint.Joint.autoConfigureConnectedAnchor = true;
			joint.Joint.highTwistLimit = highTwistLimit;
			joint.Joint.lowTwistLimit = lowTwistLimit;
			joint.Joint.swing1Limit = swing1Limit;
			joint.Joint.swing2Limit = swing2Limit;
			yield break;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004577 File Offset: 0x00002777
		private void RagdollIn()
		{
			this.ActivateRagdollParts(true);
			this._anim.enabled = false;
			this._state = BzRagdoll.RagdollState.Ragdolled;
			this.ApplyVelocity(this._bzRagdollCharacter.CharacterVelocity);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000045A4 File Offset: 0x000027A4
		private void RagdollOut()
		{
			if (this._state == BzRagdoll.RagdollState.Ragdolled)
			{
				this._state = BzRagdoll.RagdollState.WaitStablePosition;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000045B8 File Offset: 0x000027B8
		private void GetUp()
		{
			this._ragdollingEndTime = Time.time;
			this._anim.enabled = true;
			this._state = BzRagdoll.RagdollState.BlendToAnim;
			this._storedHipsPositionPrivAnim = Vector3.zero;
			this._storedHipsPositionPrivBlend = Vector3.zero;
			this._storedHipsPosition = this._hipsTransform.position;
			Vector3 vector = this._hipsTransform.position - base.transform.position;
			vector.y = this.GetDistanceToFloor(vector.y);
			this.MoveNodeWithoutChildren(vector);
			foreach (BzRagdoll.TransformComponent transformComponent in this._transforms)
			{
				transformComponent.StoredRotation = transformComponent.Transform.localRotation;
				transformComponent.PrivRotation = transformComponent.Transform.localRotation;
				transformComponent.StoredPosition = transformComponent.Transform.localPosition;
				transformComponent.PrivPosition = transformComponent.Transform.localPosition;
			}
			string stateName = this.CheckIfLieOnBack() ? this._animationGetUpFromBack : this._animationGetUpFromBelly;
			this._anim.Play(stateName, 0, 0f);
			this.ActivateRagdollParts(false);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000046F4 File Offset: 0x000028F4
		private float GetDistanceToFloor(float currentY)
		{
			RaycastHit[] array = Physics.RaycastAll(new Ray(this._hipsTransform.position, Vector3.down));
			float num = float.MinValue;
			foreach (RaycastHit raycastHit in array)
			{
				if (!raycastHit.transform.IsChildOf(base.transform))
				{
					num = Mathf.Max(num, raycastHit.point.y);
				}
			}
			if (Mathf.Abs(num - -3.40282347E+38f) > Mathf.Epsilon)
			{
				currentY = num - base.transform.position.y;
			}
			return currentY;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004788 File Offset: 0x00002988
		private void MoveNodeWithoutChildren(Vector3 shiftPos)
		{
			Vector3 ragdollDirection = this.GetRagdollDirection();
			this._hipsTransform.position -= shiftPos;
			base.transform.position += shiftPos;
			Vector3 forward = base.transform.forward;
			base.transform.rotation = Quaternion.FromToRotation(forward, ragdollDirection) * base.transform.rotation;
			this._hipsTransform.rotation = Quaternion.FromToRotation(ragdollDirection, forward) * this._hipsTransform.rotation;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000481C File Offset: 0x00002A1C
		private bool CheckIfLieOnBack()
		{
			Vector3 vector = this._anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position;
			Vector3 vector2 = this._anim.GetBoneTransform(HumanBodyBones.RightUpperLeg).position;
			Vector3 position = this._hipsTransform.position;
			vector -= position;
			vector.y = 0f;
			vector2 -= position;
			vector2.y = 0f;
			return (Quaternion.FromToRotation(vector, Vector3.right) * vector2).z < 0f;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000048A0 File Offset: 0x00002AA0
		private Vector3 GetRagdollDirection()
		{
			Vector3 position = this._anim.GetBoneTransform(HumanBodyBones.Hips).position;
			Vector3 position2 = this._anim.GetBoneTransform(HumanBodyBones.Head).position;
			Vector3 vector = position - position2;
			vector.y = 0f;
			vector = vector.normalized;
			if (this.CheckIfLieOnBack())
			{
				return vector;
			}
			return -vector;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000048FC File Offset: 0x00002AFC
		private void ApplyVelocity(Vector3 predieVelocity)
		{
			foreach (BzRagdoll.RigidComponent rigidComponent in this._rigids)
			{
				rigidComponent.RigidBody.velocity = predieVelocity;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004954 File Offset: 0x00002B54
		private void ActivateRagdollParts(bool activate)
		{
			this._bzRagdollCharacter.CharacterEnable(!activate);
			foreach (BzRagdoll.RigidComponent rigidComponent in this._rigids)
			{
				Collider component = rigidComponent.RigidBody.GetComponent<Collider>();
				if (component == null)
				{
					string n = rigidComponent.RigidBody.name + "_ColliderRotator";
					component = rigidComponent.RigidBody.transform.Find(n).GetComponent<Collider>();
				}
				component.isTrigger = !activate;
				if (activate)
				{
					rigidComponent.RigidBody.isKinematic = false;
					base.StartCoroutine(BzRagdoll.FixTransformAndEnableJoint(rigidComponent));
				}
				else
				{
					rigidComponent.RigidBody.isKinematic = true;
				}
			}
		}

		// Token: 0x040000AB RID: 171
		[SerializeField]
		private string _animationGetUpFromBelly = "GetUp.GetUpFromBelly";

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private string _animationGetUpFromBack = "GetUp.GetUpFromBack";

		// Token: 0x040000AD RID: 173
		private Animator _anim;

		// Token: 0x040000AE RID: 174
		private IBzRagdollCharacter _bzRagdollCharacter;

		// Token: 0x040000AF RID: 175
		private const float AirSpeed = 5f;

		// Token: 0x040000B0 RID: 176
		private BzRagdoll.RagdollState _state;

		// Token: 0x040000B1 RID: 177
		private float _ragdollingEndTime;

		// Token: 0x040000B2 RID: 178
		private const float RagdollToMecanimBlendTime = 0.5f;

		// Token: 0x040000B3 RID: 179
		private readonly List<BzRagdoll.RigidComponent> _rigids = new List<BzRagdoll.RigidComponent>();

		// Token: 0x040000B4 RID: 180
		private readonly List<BzRagdoll.TransformComponent> _transforms = new List<BzRagdoll.TransformComponent>();

		// Token: 0x040000B5 RID: 181
		private Transform _hipsTransform;

		// Token: 0x040000B6 RID: 182
		private Rigidbody _hipsTransformRigid;

		// Token: 0x040000B7 RID: 183
		private Vector3 _storedHipsPosition;

		// Token: 0x040000B8 RID: 184
		private Vector3 _storedHipsPositionPrivAnim;

		// Token: 0x040000B9 RID: 185
		private Vector3 _storedHipsPositionPrivBlend;

		// Token: 0x02000032 RID: 50
		private sealed class TransformComponent
		{
			// Token: 0x060000E9 RID: 233 RVA: 0x0000597E File Offset: 0x00003B7E
			public TransformComponent(Transform t)
			{
				this.Transform = t;
			}

			// Token: 0x04000104 RID: 260
			public readonly Transform Transform;

			// Token: 0x04000105 RID: 261
			public Quaternion PrivRotation;

			// Token: 0x04000106 RID: 262
			public Quaternion StoredRotation;

			// Token: 0x04000107 RID: 263
			public Vector3 PrivPosition;

			// Token: 0x04000108 RID: 264
			public Vector3 StoredPosition;
		}

		// Token: 0x02000033 RID: 51
		private struct RigidComponent
		{
			// Token: 0x060000EA RID: 234 RVA: 0x0000598D File Offset: 0x00003B8D
			public RigidComponent(Rigidbody rigid)
			{
				this.RigidBody = rigid;
				this.Joint = rigid.GetComponent<CharacterJoint>();
				if (this.Joint != null)
				{
					this.ConnectedAnchorDefault = this.Joint.connectedAnchor;
					return;
				}
				this.ConnectedAnchorDefault = Vector3.zero;
			}

			// Token: 0x04000109 RID: 265
			public readonly Rigidbody RigidBody;

			// Token: 0x0400010A RID: 266
			public readonly CharacterJoint Joint;

			// Token: 0x0400010B RID: 267
			public readonly Vector3 ConnectedAnchorDefault;
		}

		// Token: 0x02000034 RID: 52
		private enum RagdollState
		{
			// Token: 0x0400010D RID: 269
			Animated,
			// Token: 0x0400010E RID: 270
			WaitStablePosition,
			// Token: 0x0400010F RID: 271
			Ragdolled,
			// Token: 0x04000110 RID: 272
			BlendToAnim
		}
	}
}
