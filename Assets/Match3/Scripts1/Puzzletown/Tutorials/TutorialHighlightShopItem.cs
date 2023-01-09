using System.Collections;
using System.Linq;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7D RID: 2685
	[CreateAssetMenu(fileName = "ScriptTutorialHighlightShopItem", menuName = "Puzzletown/Tutorials/Create/TutorialHighlightShopItem")]
	public class TutorialHighlightShopItem : ATutorialScript
	{
		// Token: 0x06004023 RID: 16419 RVA: 0x00149AE0 File Offset: 0x00147EE0
		protected override IEnumerator ExecuteRoutine()
		{
			yield return null;
			Wooroutine<TownShopRoot> shopScene = SceneManager.Instance.Await<TownShopRoot>(true);
			yield return shopScene;
			this.root = shopScene.ReturnValue;
			this.root.onClose.AddListener(delegate
			{
				this.shopOpen = false;
			});
			this.shopOpen = true;
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
			this.overlay.maskHighlight = this.overlay.squareHighlight.rectTransform;
			this.overlay.maskHighlight.gameObject.SetActive(true);
			this.overlay.maskClick.gameObject.SetActive(true);
			this.overlay.backgroundImage.color = this.overlay.backgroundColor;
			this.PlaceMaskOverItem();
			while (this.shopOpen)
			{
				yield return true;
			}
			yield break;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00149AFB File Offset: 0x00147EFB
		public override void Tick()
		{
			if (this.overlay && this.view)
			{
				this.PlaceMaskOverItem();
			}
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00149B24 File Offset: 0x00147F24
		private void PlaceMaskOverItem()
		{
			this.overlay.ShowObject(this.view, this.step.StepPadding, false, false);
			this.overlay.maskHighlight.SetPivotWithoutPositionChange(new Vector2(0f, 0f));
			float x = this.overlay.maskHighlight.sizeDelta.x * 2f;
			this.overlay.maskHighlight.sizeDelta = new Vector2(x, this.overlay.maskHighlight.sizeDelta.y);
			this.overlay.maskHighlight.SetPivotWithoutPositionChange(new Vector2(0.5f, 0.5f));
			this.overlay.maskClick.sizeDelta = this.overlay.maskHighlight.sizeDelta * 0.85f;
			this.overlay.maskClick.pivot = this.overlay.maskHighlight.pivot;
			this.overlay.maskClick.position = this.overlay.maskHighlight.position;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00149C44 File Offset: 0x00148044
		private GameObject GetItem()
		{
			if (this.root == null)
			{
				return null;
			}
			BuildingShopView[] componentsInChildren = this.root.GetComponentsInChildren<BuildingShopView>();
			if (componentsInChildren == null)
			{
				return null;
			}
			BuildingShopView buildingShopView = componentsInChildren.FirstOrDefault((BuildingShopView v) => v.BuildingId == this.itemName);
			if (buildingShopView != null)
			{
				return buildingShopView.GetComponentInChildren<TutorialHighlightElement>().gameObject;
			}
			return null;
		}

		// Token: 0x040069D2 RID: 27090
		private const float MASK_CLICK_SCALE_FACTOR = 0.85f;

		// Token: 0x040069D3 RID: 27091
		public string itemName;

		// Token: 0x040069D4 RID: 27092
		private TownShopRoot root;

		// Token: 0x040069D5 RID: 27093
		private GameObject view;

		// Token: 0x040069D6 RID: 27094
		private bool shopOpen;
	}
}
