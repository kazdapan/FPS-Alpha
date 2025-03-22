using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000025 RID: 37
	public interface IBzRagdoll
	{
		// Token: 0x060000BC RID: 188
		bool Raycast(Ray ray, out RaycastHit hit, float distance);

		// Token: 0x060000BD RID: 189
		void AddExtraMove(Vector3 move);

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000BE RID: 190
		// (set) Token: 0x060000BF RID: 191
		bool IsRagdolled { get; set; }
	}
}
