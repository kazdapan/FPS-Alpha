using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	// Token: 0x02000026 RID: 38
	public interface IBzRagdollCharacter
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000C0 RID: 192
		Vector3 CharacterVelocity { get; }

		// Token: 0x060000C1 RID: 193
		void CharacterEnable(bool enable);
	}
}
