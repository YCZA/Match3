using System.Collections;
using DG.Tweening;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A0A RID: 2570
	public class QuestViewChapterIntro : QuestView
	{
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0013941B File Offset: 0x0013781B
		public override QuestView.CellType questStatus
		{
			get
			{
				return QuestView.CellType.chapterIntro;
			}
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00139420 File Offset: 0x00137820
		public override void Show(QuestUIData data)
		{
			base.Show(data);
			if (base.root == null)
			{
				return;
			}
			int num = base.root.config.chapter.ChapterForLevel(data.data.level);
			this.chapterDescription = base.root.loc.GetText(string.Format("quest.chapter.name_{0}", num), new LocaParam[0]);
			this.chapterIllustrationContainer.SetActive(false);
			this.chapterImageBackgroundFar.gameObject.SetActive(false);
			this.chapterImageBackground.gameObject.SetActive(false);
			this.chapterImageMiddleground.gameObject.SetActive(false);
			this.chapterImageDecoration.gameObject.SetActive(false);
			this.chapterImageForeground.gameObject.SetActive(false);
			this.noConnectionIllustration.gameObject.SetActive(false);
			base.StartCoroutine(this.LoadAssetsRoutine(num));
			base.transform.HandleOnParent(data.data);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00139524 File Offset: 0x00137924
		private void AssignAssets(ChapterIntroAssets assets)
		{
			if (assets == null || (assets.bg == null && assets.bg_far == null && assets.decoration == null && assets.foreground == null && assets.middleground == null))
			{
				this.cellTitleOffline.text = this.chapterDescription;
				this.noConnectionIllustration.gameObject.SetActive(true);
				this.noConnectionIllustration.DOFade(0f, 0.2f).From<Tweener>();
			}
			else
			{
				this.cellTitle.text = this.chapterDescription;
				this.chapterIllustrationContainer.SetActive(true);
				this.chapterImageBackground.sprite = assets.bg;
				this.SetLayerActive(this.chapterImageBackground, assets.bg);
				this.chapterImageBackgroundFar.sprite = assets.bg_far;
				this.SetLayerActive(this.chapterImageBackgroundFar, assets.bg_far);
				this.chapterImageDecoration.sprite = assets.decoration;
				this.SetLayerActive(this.chapterImageDecoration, assets.decoration);
				this.chapterImageForeground.sprite = assets.foreground;
				this.SetLayerActive(this.chapterImageForeground, assets.foreground);
				this.chapterImageMiddleground.sprite = assets.middleground;
				this.SetLayerActive(this.chapterImageMiddleground, assets.middleground);
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x001396A0 File Offset: 0x00137AA0
		private void SetLayerActive(Image image, Sprite sprite)
		{
			image.gameObject.SetActive(sprite != null);
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x001396B4 File Offset: 0x00137AB4
		private IEnumerator LoadAssetsRoutine(int chapter)
		{
			Wooroutine<ChapterIntroAssets> assetsFlow = new ChapterIntroIllustrationAssetFlow().Start(chapter);
			yield return assetsFlow;
			this.AssignAssets(assetsFlow.ReturnValue);
			yield break;
		}

		// Token: 0x040066C6 RID: 26310
		[SerializeField]
		private Image chapterImageBackgroundFar;

		// Token: 0x040066C7 RID: 26311
		[SerializeField]
		private Image chapterImageBackground;

		// Token: 0x040066C8 RID: 26312
		[SerializeField]
		private Image chapterImageMiddleground;

		// Token: 0x040066C9 RID: 26313
		[SerializeField]
		private Image chapterImageDecoration;

		// Token: 0x040066CA RID: 26314
		[SerializeField]
		private Image chapterImageForeground;

		// Token: 0x040066CB RID: 26315
		[SerializeField]
		private GameObject chapterIllustrationContainer;

		// Token: 0x040066CC RID: 26316
		[SerializeField]
		private Image noConnectionIllustration;

		// Token: 0x040066CD RID: 26317
		[SerializeField]
		private TMP_Text cellTitle;

		// Token: 0x040066CE RID: 26318
		[SerializeField]
		private TMP_Text cellTitleOffline;

		// Token: 0x040066CF RID: 26319
		private string chapterDescription;
	}
}
