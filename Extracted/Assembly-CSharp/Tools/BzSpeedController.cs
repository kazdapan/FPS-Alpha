using System;
using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Tools
{
	// Token: 0x0200001D RID: 29
	public class BzSpeedController : MonoBehaviour
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00003ECE File Offset: 0x000020CE
		private void Start()
		{
			this._fixedDeltaTime = Time.fixedDeltaTime;
			this._timeScale = Time.timeScale;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003EE8 File Offset: 0x000020E8
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Time.fixedDeltaTime = this._fixedDeltaTime;
				Time.timeScale = this._timeScale;
				return;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Time.fixedDeltaTime = this._fixedDeltaTime / 2f;
				Time.timeScale = this._timeScale / 2f;
				return;
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				Time.fixedDeltaTime = this._fixedDeltaTime / 5f;
				Time.timeScale = this._timeScale / 5f;
				return;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				Time.fixedDeltaTime = this._fixedDeltaTime / 10f;
				Time.timeScale = this._timeScale / 10f;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003F98 File Offset: 0x00002198
		private void OnGUI()
		{
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect(10f, 10f, 100f, 100f), this.text);
		}

		// Token: 0x040000A3 RID: 163
		private float _fixedDeltaTime;

		// Token: 0x040000A4 RID: 164
		private float _timeScale;

		// Token: 0x040000A5 RID: 165
		private string text = "Speed:\r\nPress 1 - normal\r\nPress 2 - 1/2\r\nPress 3 - 1/5\r\nPress 4 - 1/10";
	}
}
