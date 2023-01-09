using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A49 RID: 2633
namespace Match3.Scripts1
{
	public class RewardPreviewContainer : MonoBehaviour
	{
		// Token: 0x06003F16 RID: 16150 RVA: 0x0014247C File Offset: 0x0014087C
		private void OnValidate()
		{
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x0014247E File Offset: 0x0014087E
		public void Setup(TournamentType tType, int playerPosition, MaterialAmount[] rewardsToShow, ILocalizationService locaService)
		{
			this.SetupUserRankIndicator(tType, playerPosition, locaService);
			this.SetupRewards(rewardsToShow);
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x00142494 File Offset: 0x00140894
		private void SetupRewards(MaterialAmount[] rewardsToShow)
		{
			if (rewardsToShow == null || rewardsToShow.Length == 0)
			{
				this.SetupEmptyRewards();
				return;
			}
			if (rewardsToShow.Length > 8)
			{
				WoogaDebug.LogWarningFormatted("RewardPreviewContainer: Trying to show {0} rewards; max is 8.", new object[]
				{
					rewardsToShow.Length
				});
			}
			int num = Mathf.Min(rewardsToShow.Length, 8);
			this.topRow.gameObject.SetActive(true);
			this.bottomRow.gameObject.SetActive(num > 4);
			int num2 = (num % 4 != 0) ? Mathf.Min(3, num) : 4;
			int num3 = (num % 4 != 0) ? Mathf.Max(0, num - 3) : Mathf.Max(0, num - 4);
			for (int i = 0; i < this.topRowRewards.Length; i++)
			{
				this.topRowRewards[i].gameObject.SetActive(i < num2);
			}
			for (int j = 0; j < this.bottomRowRewards.Length; j++)
			{
				this.bottomRowRewards[j].gameObject.SetActive(j < num3);
			}
			List<MaterialAmountView> list = new List<MaterialAmountView>();
			for (int k = 0; k < num2; k++)
			{
				list.Add(this.topRowRewards[k]);
			}
			for (int l = 0; l < num3; l++)
			{
				list.Add(this.bottomRowRewards[l]);
			}
			int num4 = 0;
			foreach (MaterialAmount mat in rewardsToShow)
			{
				if (num4 < list.Count)
				{
					list[num4].Show(mat);
					num4++;
				}
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x00142649 File Offset: 0x00140A49
		private void SetupEmptyRewards()
		{
			this.topRow.gameObject.SetActive(false);
			this.bottomRow.gameObject.SetActive(false);
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00142670 File Offset: 0x00140A70
		private void SetupUserRankIndicator(TournamentType tType, int playerPosition, ILocalizationService locaService)
		{
			if (playerPosition < 4)
			{
				this.trophyImage.gameObject.SetActive(true);
				this.trophyImage.sprite = this.trophySpriteManager.GetSprite(new TournamentTrophy(tType, playerPosition));
				this.rankOnTrophy.text = string.Format("{0}", playerPosition);
			}
			else
			{
				string key = "ui.tournaments.reward_preview.last_range";
				this.rankOnTrophy.text = locaService.GetText(key, new LocaParam[0]);
				this.trophyImage.gameObject.SetActive(false);
			}
		}

		// Token: 0x04006892 RID: 26770
		public GameObject topRow;

		// Token: 0x04006893 RID: 26771
		public GameObject bottomRow;

		// Token: 0x04006894 RID: 26772
		public MaterialAmountView[] topRowRewards;

		// Token: 0x04006895 RID: 26773
		public MaterialAmountView[] bottomRowRewards;

		// Token: 0x04006896 RID: 26774
		public IsoDecoTrophySpriteManager trophySpriteManager;

		// Token: 0x04006897 RID: 26775
		public Image trophyImage;

		// Token: 0x04006898 RID: 26776
		public TextMeshProUGUI rankOnTrophy;
	}
}
