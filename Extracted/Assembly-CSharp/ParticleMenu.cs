using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000017 RID: 23
public class ParticleMenu : MonoBehaviour
{
	// Token: 0x06000060 RID: 96 RVA: 0x0000388C File Offset: 0x00001A8C
	private void Start()
	{
		this.Navigate(0);
		this.currentIndex = 0;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000389C File Offset: 0x00001A9C
	public void Navigate(int i)
	{
		this.currentIndex = (this.particleSystems.Length + this.currentIndex + i) % this.particleSystems.Length;
		if (this.currentGO != null)
		{
			Object.Destroy(this.currentGO);
		}
		this.currentGO = Object.Instantiate<GameObject>(this.particleSystems[this.currentIndex].particleSystemGO, this.spawnLocation.position + this.particleSystems[this.currentIndex].particlePosition, Quaternion.Euler(this.particleSystems[this.currentIndex].particleRotation));
		this.gunGameObject.SetActive(this.particleSystems[this.currentIndex].isWeaponEffect);
		this.title.text = this.particleSystems[this.currentIndex].title;
		this.description.text = this.particleSystems[this.currentIndex].description;
		this.navigationDetails.text = (this.currentIndex + 1).ToString() + " out of " + this.particleSystems.Length.ToString();
	}

	// Token: 0x04000086 RID: 134
	public ParticleExamples[] particleSystems;

	// Token: 0x04000087 RID: 135
	public GameObject gunGameObject;

	// Token: 0x04000088 RID: 136
	private int currentIndex;

	// Token: 0x04000089 RID: 137
	private GameObject currentGO;

	// Token: 0x0400008A RID: 138
	public Transform spawnLocation;

	// Token: 0x0400008B RID: 139
	public Text title;

	// Token: 0x0400008C RID: 140
	public Text description;

	// Token: 0x0400008D RID: 141
	public Text navigationDetails;
}
