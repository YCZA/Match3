using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004D0 RID: 1232
namespace Match3.Scripts1
{
	public class GameStateView : MonoBehaviour
	{
		// Token: 0x06002270 RID: 8816 RVA: 0x00098044 File Offset: 0x00096444
		private void Awake()
		{
			this.selectButton = base.GetComponentInChildren<Button>();
			this.selectedLabel.SetActive(false);
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0009805E File Offset: 0x0009645E
		public void Show(string title, GameState state, bool sameDay, bool showTime = true)
		{
			base.StartCoroutine(this.ShowRoutine(title, state, sameDay, showTime));
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00098074 File Offset: 0x00096474
		public void Highlight(bool selected)
		{
			this.selectButton.interactable = !selected;
			this.selectButton.gameObject.SetActive(!selected);
			this.selectedLabel.gameObject.SetActive(selected);
			this.background.sprite = ((!selected) ? this.backgroundInactive : this.backgroundActive);
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000980D8 File Offset: 0x000964D8
		private IEnumerator ShowRoutine(string title, GameState state, bool sameDay, bool showTime = true)
		{
			yield return ServiceLocator.Instance.Inject(this);
			this.resourceDataService = new ResourceDataService(() => state);
			this.villageRankObserver = new VillageRankHarmonyObserver(this.configService.SbsConfig.villagerankconfig, this.resourceDataService);
			int remaining = this.villageRankObserver.HarmonyRequried - this.villageRankObserver.HarmonyCollected;
			DateTime formattedTimestamp = Scripts1.DateTimeExtensions.FromUnixTimeStamp(state.timestamp, DateTimeKind.Utc).ToLocalTime();
			this.gameStateTitle.text = title;
			this.unlockedLevelValue.text = this.localizationService.GetText("ui.level", new LocaParam[0]) + (state.progression.tiers.Count + 1).ToString();
			this.villageRankValue.text = this.villageRankObserver.CurrentRank.ToString();
			this.remainingPointsValue.text = string.Format(this.localizationService.GetText("ui.social.login.merge_vr_description", new LocaParam[0]), remaining);
			this.diamondsValue.text = this.resourceDataService.GetAmount("diamonds").ToString();
			this.coinsValue.text = this.resourceDataService.GetAmount("coins").ToString();
			string timeString = string.Empty;
			if (showTime)
			{
				if (sameDay)
				{
					timeString = "time: " + formattedTimestamp.ToString("HH:mm");
				}
				else
				{
					timeString = formattedTimestamp.ToString("d.M.yyyy, HH:mm");
				}
			}
			this.timestampValue.text = timeString;
			yield break;
		}

		// Token: 0x04004DDA RID: 19930
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04004DDB RID: 19931
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04004DDC RID: 19932
		[SerializeField]
		private Image background;

		// Token: 0x04004DDD RID: 19933
		[SerializeField]
		private TextMeshProUGUI gameStateTitle;

		// Token: 0x04004DDE RID: 19934
		[SerializeField]
		private GameObject selectedLabel;

		// Token: 0x04004DDF RID: 19935
		[SerializeField]
		private TextMeshProUGUI unlockedLevelValue;

		// Token: 0x04004DE0 RID: 19936
		[SerializeField]
		private TextMeshProUGUI villageRankValue;

		// Token: 0x04004DE1 RID: 19937
		[SerializeField]
		private TextMeshProUGUI remainingPointsValue;

		// Token: 0x04004DE2 RID: 19938
		[SerializeField]
		private TextMeshProUGUI diamondsValue;

		// Token: 0x04004DE3 RID: 19939
		[SerializeField]
		private TextMeshProUGUI coinsValue;

		// Token: 0x04004DE4 RID: 19940
		[SerializeField]
		private TextMeshProUGUI timestampValue;

		// Token: 0x04004DE5 RID: 19941
		[SerializeField]
		private Sprite backgroundActive;

		// Token: 0x04004DE6 RID: 19942
		[SerializeField]
		private Sprite backgroundInactive;

		// Token: 0x04004DE7 RID: 19943
		private VillageRankHarmonyObserver villageRankObserver;

		// Token: 0x04004DE8 RID: 19944
		private ResourceDataService resourceDataService;

		// Token: 0x04004DE9 RID: 19945
		private Button selectButton;
	}
}
