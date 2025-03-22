using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[Serializable]
public class ParticleExamples
{
	// Token: 0x04000080 RID: 128
	public string title;

	// Token: 0x04000081 RID: 129
	[TextArea]
	public string description;

	// Token: 0x04000082 RID: 130
	public bool isWeaponEffect;

	// Token: 0x04000083 RID: 131
	public GameObject particleSystemGO;

	// Token: 0x04000084 RID: 132
	public Vector3 particlePosition;

	// Token: 0x04000085 RID: 133
	public Vector3 particleRotation;
}
