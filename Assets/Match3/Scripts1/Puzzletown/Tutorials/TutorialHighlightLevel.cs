using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7B RID: 2683
	[CreateAssetMenu(fileName = "ScriptTutorialHighlightLevelmap", menuName = "Puzzletown/Tutorials/Create/TutorialHighlightLevel")]
	public class TutorialHighlightLevel : ATutorialScript
	{
		// Token: 0x0600401A RID: 16410 RVA: 0x001493F0 File Offset: 0x001477F0
		protected override IEnumerator ExecuteRoutine()
		{
			yield return null;
			Wooroutine<M3_LevelSelectionUiRoot> root = SceneManager.Instance.Await<M3_LevelSelectionUiRoot>(true);
			yield return root;
			this.root = root.ReturnValue;
			this.view = this.GetItem();
			while (this.view == null)
			{
				yield return null;
				this.view = this.GetItem();
				if (this.root == null)
				{
					yield break;
				}
			}
			this.overlay.maskHighlight.gameObject.SetActive(true);
			this.overlay.maskClick.gameObject.SetActive(true);
			this.overlay.backgroundImage.color = this.overlay.backgroundColor;
			this.overlay.ShowObject(this.view, new Padding(), false, false);
			if (this.centerOnLevel)
			{
				int num = (AUiAdjuster.SimilarOrientation != ScreenOrientation.LandscapeLeft) ? 1 : -1;
				this.root.ScrollToLevel(this.level + num, 0.5f);
			}
			yield return this.view.GetComponentInChildren<Button>().onClick.Await();
			yield break;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0014940B File Offset: 0x0014780B
		public override void Tick()
		{
			if (this.overlay && this.view)
			{
				this.overlay.ShowObject(this.view, new Padding(), false, true);
			}
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00149448 File Offset: 0x00147848
		private GameObject GetItem()
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
			M3LevelSelectionMapItem m3LevelSelectionMapItem = componentsInChildren.FirstOrDefault((M3LevelSelectionMapItem v) => v.level == this.level);
			return (!(m3LevelSelectionMapItem != null)) ? null : m3LevelSelectionMapItem.GetComponentInChildren<TutorialHighlightElement>().gameObject;
		}

		// Token: 0x040069C9 RID: 27081
		public int level;

		// Token: 0x040069CA RID: 27082
		private M3_LevelSelectionUiRoot root;

		// Token: 0x040069CB RID: 27083
		private GameObject view;

		// Token: 0x040069CC RID: 27084
		public bool centerOnLevel;
	}
}
