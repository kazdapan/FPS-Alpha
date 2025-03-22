using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class Readme : ScriptableObject
{
	// Token: 0x04000060 RID: 96
	public Texture2D icon;

	// Token: 0x04000061 RID: 97
	public string title;

	// Token: 0x04000062 RID: 98
	public Readme.Section[] sections;

	// Token: 0x04000063 RID: 99
	public bool loadedLayout;

	// Token: 0x0200002D RID: 45
	[Serializable]
	public class Section
	{
		// Token: 0x040000F0 RID: 240
		public string heading;

		// Token: 0x040000F1 RID: 241
		public string text;

		// Token: 0x040000F2 RID: 242
		public string linkText;

		// Token: 0x040000F3 RID: 243
		public string url;
	}
}
