using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class ParticleCollision : MonoBehaviour
{
	// Token: 0x0600005C RID: 92 RVA: 0x0000380C File Offset: 0x00001A0C
	private void Start()
	{
		this.m_ParticleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000381C File Offset: 0x00001A1C
	private void OnParticleCollision(GameObject other)
	{
		int collisionEvents = this.m_ParticleSystem.GetCollisionEvents(other, this.m_CollisionEvents);
		for (int i = 0; i < collisionEvents; i++)
		{
			ExtinguishableFire component = this.m_CollisionEvents[i].colliderComponent.GetComponent<ExtinguishableFire>();
			if (component != null)
			{
				component.Extinguish();
			}
		}
	}

	// Token: 0x0400007E RID: 126
	private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();

	// Token: 0x0400007F RID: 127
	private ParticleSystem m_ParticleSystem;
}
