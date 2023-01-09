using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Wooga.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A5E RID: 2654
namespace Match3.Scripts1
{
	public class TournamentStandingView : ATableViewReusableCell, IDataView<TournamentStandingData>, IEditorDescription
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x00145F25 File Offset: 0x00144325
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00145F28 File Offset: 0x00144328
		public void Show(TournamentStandingData data)
		{
			this.playerName.text = data.name;
			this.SetupRankAndTrophy(data.tournamentType, data.trophySprite, data.rank);
			this.points.text = string.Format("{0}", data.points);
			this.otherBackground.SetActive(!data.isSelf);
			this.selfBackground.SetActive(data.isSelf);
			this.taskIcon.sprite = data.taskSprite;
			this.avatarImg.sprite = data.avatar;
			this.onlineStatus.gameObject.SetActive(data.isOnline);
			this.ShowReward(data.rewardSprite, data.rewardShadowSprite);
			this.data = data;
			data.OnAvatarAvailable.RemoveAllListeners();
			data.OnAvatarAvailable.AddListenerOnce(new Action<string, Sprite>(this.HandleAvatarAvailable));
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00146018 File Offset: 0x00144418
		protected void ShowReward(Sprite rewardSprite, Sprite rewardShadowSprite)
		{
			if (this.rewardIcon == null || this.rewardContainer == null)
			{
				return;
			}
			this.rewardIcon.sprite = rewardSprite;
			this.rewardContainer.gameObject.SetActive(rewardSprite != null);
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0014606B File Offset: 0x0014446B
		private void SetupRankAndTrophy(TournamentType tType, Sprite trophySprite, int playerRank)
		{
			this.SetupTrophy(tType, trophySprite, playerRank);
			if (playerRank > 3)
			{
				this.rank.text = string.Format("{0}", playerRank);
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x00146098 File Offset: 0x00144498
		private void SetupTrophy(TournamentType tType, Sprite trophySprite, int playerRank)
		{
			int num = tType - TournamentType.Bomb;
			for (int i = 0; i < this.trophies.Length; i++)
			{
				this.trophies[i].gameObject.SetActive(i == num && playerRank < 4);
			}
			if (playerRank < 4)
			{
				this.trophies[num].sprite = trophySprite;
				this.rankingsOnTrophy[num].text = string.Format("{0}", playerRank);
			}
			this.plainRankContainer.gameObject.SetActive(playerRank > 3);
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x00146128 File Offset: 0x00144528
		public void HandleAvatarAvailable(string id, Sprite sprite)
		{
			if (id == this.data.fbData.ID)
			{
				this.SetAvatarSprite(sprite);
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0014614C File Offset: 0x0014454C
		public void SetAvatarSprite(Sprite sprite)
		{
			if (this.avatarImg != null && sprite != null)
			{
				this.avatarImg.sprite = sprite;
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x00146177 File Offset: 0x00144577
		public string GetEditorDescription()
		{
			return "League entry view";
		}

		// Token: 0x0400693D RID: 26941
		[SerializeField]
		private TMP_Text playerName;

		// Token: 0x0400693E RID: 26942
		[SerializeField]
		private TMP_Text rank;

		// Token: 0x0400693F RID: 26943
		[SerializeField]
		private TMP_Text points;

		// Token: 0x04006940 RID: 26944
		[SerializeField]
		private GameObject otherBackground;

		// Token: 0x04006941 RID: 26945
		[SerializeField]
		private GameObject selfBackground;

		// Token: 0x04006942 RID: 26946
		[SerializeField]
		private GameObject plainRankContainer;

		// Token: 0x04006943 RID: 26947
		[SerializeField]
		private Image taskIcon;

		// Token: 0x04006944 RID: 26948
		[SerializeField]
		private Image avatarImg;

		// Token: 0x04006945 RID: 26949
		[SerializeField]
		private Image rewardIcon;

		// Token: 0x04006946 RID: 26950
		[SerializeField]
		private Image onlineStatus;

		// Token: 0x04006947 RID: 26951
		[SerializeField]
		private GameObject rewardContainer;

		// Token: 0x04006948 RID: 26952
		[SerializeField]
		private Image[] trophies;

		// Token: 0x04006949 RID: 26953
		[SerializeField]
		private TMP_Text[] rankingsOnTrophy;

		// Token: 0x0400694A RID: 26954
		private TournamentStandingData data;
	}
}
