using System;
using TMPro;
using UnityEngine.UI;

// Token: 0x02000910 RID: 2320
namespace Match3.Scripts1
{
	public class ChapterIndicator : AVisibleGameObject
	{
		// Token: 0x06003898 RID: 14488 RVA: 0x001165CC File Offset: 0x001149CC
		public void SetChapter(int chapterId, ILocalizationService loc)
		{
			this.chapterId = chapterId;
			if (this.localizationService == null)
			{
				this.localizationService = loc;
				this.localizationService.LanguageChanged.AddListener(new Action(this.UpdateText));
			}
			this.UpdateText();
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0011660C File Offset: 0x00114A0C
		public void UpdateText()
		{
			this.chapterImage.sprite = this.chapterImages.GetSimilar(string.Format("chp_{0:00}", this.chapterId));
			this.title.text = string.Format(this.localizationService.GetText("quest.chapter.number", new LocaParam[0]), this.chapterId);
			this.description.text = this.localizationService.GetText(string.Format("quest.chapter.name_{0}", this.chapterId), new LocaParam[0]);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x001166A6 File Offset: 0x00114AA6
		private void OnDestroy()
		{
			if (this.localizationService != null)
			{
				this.localizationService.LanguageChanged.RemoveListener(new Action(this.UpdateText));
			}
		}

		// Token: 0x040060DA RID: 24794
		public SpriteManager chapterImages;

		// Token: 0x040060DB RID: 24795
		public Image chapterImage;

		// Token: 0x040060DC RID: 24796
		public TMP_Text title;

		// Token: 0x040060DD RID: 24797
		public TMP_Text description;

		// Token: 0x040060DE RID: 24798
		private ILocalizationService localizationService;

		// Token: 0x040060DF RID: 24799
		private int chapterId;
	}
}
