using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class PlayerMovement : MonoBehaviour
{
	// Token: 0x0600003A RID: 58 RVA: 0x00002ECE File Offset: 0x000010CE
	private void Start()
	{
		this.controller = base.GetComponent<CharacterController>();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002EDC File Offset: 0x000010DC
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			this.speed = 20f;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			this.speed = 12f;
		}
		this.IsGrounded = Physics.CheckSphere(this.groundCheck.position, this.groundDistance, this.groundMask);
		if (this.IsGrounded && this.velocity.y < 0f)
		{
			this.velocity.y = -2f;
		}
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		Vector3 a = base.transform.right * axis + base.transform.forward * axis2;
		this.controller.Move(a * this.speed * Time.deltaTime);
		if (Input.GetButtonDown("Jump") && this.IsGrounded)
		{
			this.velocity.y = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
		}
		this.velocity.y = this.velocity.y + this.gravity * Time.deltaTime;
		this.controller.Move(this.velocity * Time.deltaTime);
		if (this.lastPosition != base.gameObject.transform.position && this.IsGrounded)
		{
			this.isMoving = true;
		}
		else
		{
			this.isMoving = false;
		}
		this.lastPosition = base.gameObject.transform.position;
	}

	// Token: 0x04000040 RID: 64
	private CharacterController controller;

	// Token: 0x04000041 RID: 65
	public float speed = 12f;

	// Token: 0x04000042 RID: 66
	public float gravity = -19.62f;

	// Token: 0x04000043 RID: 67
	public float jumpHeight = 3f;

	// Token: 0x04000044 RID: 68
	public Transform groundCheck;

	// Token: 0x04000045 RID: 69
	public float groundDistance = 0.2f;

	// Token: 0x04000046 RID: 70
	public LayerMask groundMask;

	// Token: 0x04000047 RID: 71
	private Vector3 velocity;

	// Token: 0x04000048 RID: 72
	private bool IsGrounded;

	// Token: 0x04000049 RID: 73
	private bool isMoving;

	// Token: 0x0400004A RID: 74
	private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
}
