using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7C RID: 2684
	[CreateAssetMenu(fileName = "ScriptTutorialHighlightLevelmap", menuName = "Puzzletown/Tutorials/Create/TutorialHighlightLevel30")]
	public class TutorialHighlightLevel30 : ATutorialScript
	{
		// Token: 0x0600401F RID: 16415 RVA: 0x0014970C File Offset: 0x00147B0C
		protected override IEnumerator ExecuteRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<M3_LevelSelectionUiRoot> root = SceneManager.Instance.Await<M3_LevelSelectionUiRoot>(true);
			yield return root;
			this.root = root.ReturnValue;
			int targetLevel = this.firstHighlightLevel;
			TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
			if (this.progressionService.GetTier(targetLevel) < 1)
			{
				tutorialRunner.currentTutorial.dontPersistCompletion = (this.progressionService.GetTier(this.secondHighlightLevel) < 1);
			}
			else
			{
				targetLevel = this.secondHighlightLevel;
				tutorialRunner.currentTutorial.dontPersistCompletion = false;
			}
			int levelOffset = (AUiAdjuster.SimilarOrientation != ScreenOrientation.LandscapeLeft) ? 1 : -1;
			this.root.ScrollToLevel(targetLevel + levelOffset, 0.5f);
			this.view = this.GetItem(targetLevel);
			while (this.view == null)
			{
				yield return null;
				this.view = this.GetItem(targetLevel);
				if (this.root == null)
				{
					yield break;
				}
			}
			this.overlay.maskHighlight.gameObject.SetActive(true);
			this.overlay.maskClick.gameObject.SetActive(true);
			this.overlay.backgroundImage.color = this.overlay.backgroundColor;
			this.overlay.ShowObject(this.view, new Padding(), false, false);
			yield return this.view.GetComponentInChildren<Button>().onClick.Await();
			yield break;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x00149727 File Offset: 0x00147B27
		public override void Tick()
		{
			if (this.overlay && this.view)
			{
				this.overlay.ShowObject(this.view, new Padding(), false, true);
			}
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00149764 File Offset: 0x00147B64
		private GameObject GetItem(int level)
		{
			if (this.root == null)
			{
				return null;
			}
			M3LevelSelectionMapItem[] componentsInChildren = this.root.GetComponentsInChildren<M3LevelSelectionMapItem>();
			if (componentsInChildren == null)
			{
				return null;
			}
			M3LevelSelectionMapItem m3LevelSelectionMapItem = componentsInChildren.FirstOrDefault((M3LevelSelectionMapItem v) => v.level == level);
			return (!(m3LevelSelectionMapItem != null)) ? null : m3LevelSelectionMapItem.GetComponentInChildren<TutorialHighlightElement>().gameObject;
		}

		// Token: 0x040069CD RID: 27085
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040069CE RID: 27086
		public int firstHighlightLevel;

		// Token: 0x040069CF RID: 27087
		public int secondHighlightLevel;

		// Token: 0x040069D0 RID: 27088
		private M3_LevelSelectionUiRoot root;

		// Token: 0x040069D1 RID: 27089
		private GameObject view;
	}
}
