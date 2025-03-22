using System;
using BzKovSoft.RagdollTemplate.Scripts.Charachter;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Tools
{
	// Token: 0x0200001C RID: 28
	public sealed class BzHealthItem : MonoBehaviour
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00003E7C File Offset: 0x0000207C
		private void OnTriggerEnter(Collider collider)
		{
			IBzDamageable component = collider.GetComponent<IBzDamageable>();
			if (component == null)
			{
				return;
			}
			if (component.IsFullHealth())
			{
				return;
			}
			component.Health += this._addHealth;
			Object.Destroy(base.gameObject);
		}

		// Token: 0x040000A2 RID: 162
		[SerializeField]
		private float _addHealth = 0.25f;
	}
}
