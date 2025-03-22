using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class MouseMovement : MonoBehaviour
{
	// Token: 0x06000031 RID: 49 RVA: 0x00002D8A File Offset: 0x00000F8A
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002D94 File Offset: 0x00000F94
	private void Update()
	{
		float num = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;
		float num2 = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;
		this.xRotation -= num2;
		this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
		this.yRotation += num;
		base.transform.localRotation = Quaternion.Euler(this.xRotation, this.yRotation, 0f);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002E29 File Offset: 0x00001029
	public void LowSens()
	{
		this.mouseSensitivity = 150f;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002E36 File Offset: 0x00001036
	public void MediumSens()
	{
		this.mouseSensitivity = 300f;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002E43 File Offset: 0x00001043
	public void HighSens()
	{
		this.mouseSensitivity = 450f;
	}

	// Token: 0x0400003A RID: 58
	public float mouseSensitivity = 300f;

	// Token: 0x0400003B RID: 59
	private float xRotation;

	// Token: 0x0400003C RID: 60
	private float yRotation;
}
