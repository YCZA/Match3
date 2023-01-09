using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A3D RID: 2621
namespace Match3.Scripts1
{
	public class GrandPrizeView : MonoBehaviour
	{
		// Token: 0x06003ECD RID: 16077 RVA: 0x0013F8C4 File Offset: 0x0013DCC4
		public void Setup(GrandPrizeView.Config config)
		{
			this.shopView.Show(config.shopData);
			this.progressBar.fillAmount = (float)config.collected / (float)config.required;
			this.progressLabel.text = string.Format("{0}/{1}", config.collected, config.required);
			if (config.seasonSpriteManager != null)
			{
				this.glow.sprite = config.seasonSpriteManager.GetSimilar(config.shopData.data.name);
				this.icon.sprite = config.seasonSpriteManager.GetSimilar("season_currency");
				this.icon.color = Color.white;
			}
			this.infoButton.SetActive(config.showInfoButton);
			bool active = config.fxTexture != null;
			this.fxRenderer.sharedMaterial.mainTexture = config.fxTexture;
			this.fxTarget.SetActive(active);
			this.fxRendererContainer.SetActive(active);
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0013F9D5 File Offset: 0x0013DDD5
		public void Hide()
		{
			this.fxTarget.SetActive(false);
			this.fxRendererContainer.SetActive(false);
		}

		// Token: 0x04006806 RID: 26630
		[SerializeField]
		private Image icon;

		// Token: 0x04006807 RID: 26631
		[SerializeField]
		private TextMeshProUGUI progressLabel;

		// Token: 0x04006808 RID: 26632
		[SerializeField]
		private Image progressBar;

		// Token: 0x04006809 RID: 26633
		[SerializeField]
		private BuildingShopView shopView;

		// Token: 0x0400680A RID: 26634
		[SerializeField]
		private Image glow;

		// Token: 0x0400680B RID: 26635
		[SerializeField]
		private GameObject fxTarget;

		// Token: 0x0400680C RID: 26636
		[SerializeField]
		private GameObject fxRendererContainer;

		// Token: 0x0400680D RID: 26637
		[SerializeField]
		private ParticleSystemRenderer fxRenderer;

		// Token: 0x0400680E RID: 26638
		[SerializeField]
		private GameObject infoButton;

		// Token: 0x02000A3E RID: 2622
		public class Config
		{
			// Token: 0x0400680F RID: 26639
			public BuildingShopData shopData;

			// Token: 0x04006810 RID: 26640
			public SpriteManager seasonSpriteManager;

			// Token: 0x04006811 RID: 26641
			public Texture fxTexture;

			// Token: 0x04006812 RID: 26642
			public int required;

			// Token: 0x04006813 RID: 26643
			public int collected;

			// Token: 0x04006814 RID: 26644
			public bool showInfoButton;
		}
	}
}
