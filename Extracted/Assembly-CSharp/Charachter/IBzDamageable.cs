using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000024 RID: 36
	public interface IBzDamageable
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000B7 RID: 183
		// (set) Token: 0x060000B8 RID: 184
		float Health { get; set; }

		// Token: 0x060000B9 RID: 185
		void Shot(Ray ray, float impact, float maxDistance);

		// Token: 0x060000BA RID: 186
		bool IsDead();

		// Token: 0x060000BB RID: 187
		bool IsFullHealth();
	}
}
