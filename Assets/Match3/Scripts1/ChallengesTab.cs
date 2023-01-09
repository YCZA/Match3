using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200090C RID: 2316
namespace Match3.Scripts1
{
	public class ChallengesTab : MonoBehaviour
	{
		// Token: 0x0600387E RID: 14462 RVA: 0x00114D04 File Offset: 0x00113104
		public void Setup(GameStateService gamestateService, ChallengeService challengeService, ILocalizationService localizationService, SBSService sbsService, IVideoAdService videoAdService, AudioService audioService)
		{
			this.gamestateService = gamestateService;
			this.localizationService = localizationService;
			this.challengeService = challengeService;
			this.videoAdService = videoAdService;
			this.audioService = audioService;
			for (int i = this.taskList.childCount - 1; i >= 0; i--)
			{
				global::UnityEngine.Object.Destroy(this.taskList.GetChild(i).gameObject);
			}
			this.challengeViews = new List<ChallengeProgressView>();
			for (int j = 0; j < gamestateService.Challenges.CurrentChallenges.Count; j++)
			{
				ChallengeProgressView challengeProgressView = global::UnityEngine.Object.Instantiate<ChallengeProgressView>(this.challengeProgressViewPrefab);
				challengeProgressView.transform.SetParent(this.taskList.transform, false);
				challengeProgressView.SetChallengeData(j, gamestateService, localizationService, challengeService, sbsService, videoAdService);
				this.challengeViews.Add(challengeProgressView);
			}
			if (this.adBonusButton != null)
			{
				this.adBonusButton.interactable = this.AdBonusButtonShouldBeActive();
			}
			if (!this.isAnimatingBar)
			{
				this.adBonusSlider.fillAmount = (float)gamestateService.Challenges.CurrentAdBonus * 0.2f;
				if (this.adPercentLabel != null)
				{
					this.adPercentLabel.text = string.Format(localizationService.GetText("ui.challenges.pawbonus.percent", new LocaParam[0]), (float)gamestateService.Challenges.CurrentAdBonus * 0.2f * 50f);
				}
			}
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x00114E6C File Offset: 0x0011326C
		public void Refresh(bool isChallengeActive)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			base.GetComponentInChildren<ChallengeControls>().Refresh(isChallengeActive);
			if (this.adBonusButton != null)
			{
				this.adBonusButton.interactable = this.AdBonusButtonShouldBeActive();
			}
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x00114EB8 File Offset: 0x001132B8
		public Coroutine ShowAd()
		{
			if (this.adBonusButton != null)
			{
				this.adBonusButton.interactable = false;
			}
			return base.StartCoroutine(this.WatchAd());
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x00114EE4 File Offset: 0x001132E4
		private IEnumerator WatchAd()
		{
			bool adSuccessful = false;
			if (Application.isEditor)
			{
				this.videoAdService.EditorPretendYouJustWatchedAnAd(AdPlacement.Challenges);
				adSuccessful = true;
				yield return null;
			}
			else
			{
				Wooroutine<VideoShowResult> showAdRoutine = WooroutineRunner.StartWooroutine<VideoShowResult>(this.videoAdService.ShowAd(AdPlacement.Challenges));
				yield return showAdRoutine;
				if (showAdRoutine.ReturnValue == VideoShowResult.Success)
				{
					adSuccessful = true;
				}
			}
			if (adSuccessful)
			{
				this.gamestateService.Challenges.CurrentAdBonus++;
				this.videoAdService.TrackClaim();
				yield return this.AnimateAdBonusGauge();
			}
			if (this.adBonusButton != null)
			{
				this.adBonusButton.interactable = this.AdBonusButtonShouldBeActive();
			}
			yield break;
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00114F00 File Offset: 0x00113300
		private IEnumerator AnimateAdBonusGauge()
		{
			this.isAnimatingBar = true;
			float targetFill = (float)this.gamestateService.Challenges.CurrentAdBonus * 0.2f;
			float startFill = this.adBonusSlider.fillAmount;
			float elapsedTime = 0f;
			float ratio = 0f;
			this.audioService.PlaySFX(AudioId.AdWheelJackpotCharge, false, false, false);
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / 1f;
				this.adBonusSlider.fillAmount = Mathf.Lerp(startFill, targetFill, ratio);
				yield return null;
			}
			this.adBonusSlider.fillAmount = targetFill;
			foreach (ChallengeProgressView challengeProgressView in this.challengeViews)
			{
				int pawReward = this.challengeService.GetPawReward(challengeProgressView.challenge);
				challengeProgressView.UpdatePawReward(pawReward);
			}
			yield return this.AnimateAdBonusText();
			this.isAnimatingBar = false;
			yield break;
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x00114F1C File Offset: 0x0011331C
		private IEnumerator AnimateAdBonusText()
		{
			this.bonusAnimation.Play();
			yield return new WaitForSeconds(this.bonusAnimation["ChallengeAmountBounce"].length * 0.35f);
			this.audioService.PlaySFX(AudioId.AdWheelYouWon, false, false, false);
			this.adPercentLabel.text = string.Format(this.localizationService.GetText("ui.challenges.pawbonus.percent", new LocaParam[0]), (float)this.gamestateService.Challenges.CurrentAdBonus * 0.2f * 50f);
			yield break;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x00114F38 File Offset: 0x00113338
		private bool AdBonusButtonShouldBeActive()
		{
			bool flag = this.videoAdService.IsVideoAvailable(true) || Application.isEditor;
			bool flag2 = this.gamestateService.Challenges.CurrentAdBonus < 5;
			bool flag3 = this.videoAdService.IsAllowedToWatchAd(AdPlacement.Challenges);
			return flag && flag2 && flag3;
		}

		// Token: 0x040060B0 RID: 24752
		private const int MAX_AD_BONUS = 5;

		// Token: 0x040060B1 RID: 24753
		private const float FILL_GAUGE_PERCENT = 0.2f;

		// Token: 0x040060B2 RID: 24754
		private const float BAR_ANIMATION_TIME = 1f;

		// Token: 0x040060B3 RID: 24755
		private const string BONUS_ANIMATION_NAME = "ChallengeAmountBounce";

		// Token: 0x040060B4 RID: 24756
		[SerializeField]
		private Transform taskList;

		// Token: 0x040060B5 RID: 24757
		[SerializeField]
		private ChallengeProgressView challengeProgressViewPrefab;

		// Token: 0x040060B6 RID: 24758
		[SerializeField]
		private Button adBonusButton;

		// Token: 0x040060B7 RID: 24759
		[SerializeField]
		private TextMeshProUGUI adPercentLabel;

		// Token: 0x040060B8 RID: 24760
		[SerializeField]
		private Image adBonusSlider;

		// Token: 0x040060B9 RID: 24761
		[SerializeField]
		private Animation bonusAnimation;

		// Token: 0x040060BA RID: 24762
		private List<ChallengeProgressView> challengeViews;

		// Token: 0x040060BB RID: 24763
		private GameStateService gamestateService;

		// Token: 0x040060BC RID: 24764
		private ILocalizationService localizationService;

		// Token: 0x040060BD RID: 24765
		private ChallengeService challengeService;

		// Token: 0x040060BE RID: 24766
		private IVideoAdService videoAdService;

		// Token: 0x040060BF RID: 24767
		private AudioService audioService;

		// Token: 0x040060C0 RID: 24768
		private bool isAnimatingBar;
	}
}
