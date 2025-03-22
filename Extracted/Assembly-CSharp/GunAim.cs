using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class GunAim : MonoBehaviour
{
	// Token: 0x06000053 RID: 83 RVA: 0x0000349F File Offset: 0x0000169F
	private void Start()
	{
		this.parentCamera = base.GetComponentInParent<Camera>();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000034B0 File Offset: 0x000016B0
	private void Update()
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		if (x <= (float)this.borderLeft || x >= (float)(Screen.width - this.borderRight) || y <= (float)this.borderBottom || y >= (float)(Screen.height - this.borderTop))
		{
			this.isOutOfBounds = true;
		}
		else
		{
			this.isOutOfBounds = false;
		}
		if (!this.isOutOfBounds)
		{
			base.transform.LookAt(this.parentCamera.ScreenToWorldPoint(new Vector3(x, y, 5f)));
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003541 File Offset: 0x00001741
	public bool GetIsOutOfBounds()
	{
		return this.isOutOfBounds;
	}

	// Token: 0x04000069 RID: 105
	public int borderLeft;

	// Token: 0x0400006A RID: 106
	public int borderRight;

	// Token: 0x0400006B RID: 107
	public int borderTop;

	// Token: 0x0400006C RID: 108
	public int borderBottom;

	// Token: 0x0400006D RID: 109
	private Camera parentCamera;

	// Token: 0x0400006E RID: 110
	private bool isOutOfBounds;
}
