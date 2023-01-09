using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006E2 RID: 1762
	public class LevelMapActiveView : UiSimpleView<M3_LevelMapItemState>, IDataView<LevelSelectionData>
	{
		// Token: 0x06002BC2 RID: 11202 RVA: 0x000C8D84 File Offset: 0x000C7184
		public void Show(LevelSelectionData data)
		{
			this.Show(data.itemState);
			if (!this.IsVisible)
			{
				return;
			}
			this.SetupCollectable(data);
			this.SetupTiers(data);
			this.SetupText(data);
			this.ShowOnChildren(data.lockReason, true, true);
			if (data.unlockDate != null)
			{
				this.ShowOnChildren(data.unlockDate.Value, false, true);
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000C8DF3 File Offset: 0x000C71F3
		private void SetupText(LevelSelectionData data)
		{
			if (!this.labelLevel)
			{
				return;
			}
			this.labelLevel.text = data.setConfig.level.ToString();
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000C8E28 File Offset: 0x000C7228
		private void SetupCollectable(LevelSelectionData data)
		{
			if (!this.iconCollectable)
			{
				return;
			}
			this.iconCollectable.transform.parent.gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(data.collectable))
			{
				this.iconCollectable.transform.parent.gameObject.SetActive(true);
				this.iconCollectable.sprite = this.collectableSprites.GetSimilar(data.collectable);
			}
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000C8EAA File Offset: 0x000C72AA
		private void SetupTiers(LevelSelectionData data)
		{
			if (this.tiersDataSource)
			{
				this.ShowTiersView(!data.isSingleTier);
				if (!data.isSingleTier)
				{
					this.tiersDataSource.Show(this.GetTiers(data));
				}
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000C8EEA File Offset: 0x000C72EA
		private void ShowTiersView(bool show)
		{
			if (this.tiersDataSource.gameObject.activeSelf != show)
			{
				this.tiersDataSource.gameObject.SetActive(show);
			}
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000C8F14 File Offset: 0x000C7314
		private IEnumerable<M3_LevelSelectionTier> GetTiers(LevelSelectionData data)
		{
			for (int i = 0; i < 3; i++)
			{
				M3_LevelSelectionTier.State state = M3_LevelSelectionTier.State.Pending;
				if (i < data.tier)
				{
					state = M3_LevelSelectionTier.State.Completed;
				}
				else if (i == data.tier)
				{
					state = M3_LevelSelectionTier.State.Active;
				}
				yield return new M3_LevelSelectionTier
				{
					state = state,
					tier = (AreaConfig.Tier)i
				};
			}
			yield break;
		}

		// Token: 0x040054E2 RID: 21730
		public Image iconCollectable;

		// Token: 0x040054E3 RID: 21731
		public TextMeshProUGUI labelLevel;

		// Token: 0x040054E4 RID: 21732
		public M3_LevelSelectionTiersDataSource tiersDataSource;

		// Token: 0x040054E5 RID: 21733
		public SpriteManager collectableSprites;
	}
}
