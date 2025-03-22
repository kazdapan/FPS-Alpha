using System;
using BzKovSoft.RagdollTemplate.Scripts.Charachter;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Tools
{
	// Token: 0x0200001A RID: 26
	public sealed class BzDisplayHealth : MonoBehaviour
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00003A74 File Offset: 0x00001C74
		private void OnGUI()
		{
			if (this._bzHealth == null)
			{
				return;
			}
			if (this._labelStile == null)
			{
				this._labelStile = GUI.skin.GetStyle("Label");
				this._labelStile.alignment = TextAnchor.UpperCenter;
			}
			GUI.Label(new Rect((float)((Screen.width - 100) / 2), 10f, 100f, 100f), "Health: " + (this._bzHealth.Health * 100f).ToString("N0"), this._labelStile);
		}

		// Token: 0x0400008F RID: 143
		[SerializeField]
		private BzHealth _bzHealth;

		// Token: 0x04000090 RID: 144
		private GUIStyle _labelStile;
	}
}
