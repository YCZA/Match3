using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine.EventSystems.ResourceManager;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000906 RID: 2310
namespace Match3.Scripts1
{
	public class ChallengeProgressView : MonoBehaviour
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06003857 RID: 14423 RVA: 0x0011366A File Offset: 0x00111A6A
		// (set) Token: 0x06003858 RID: 14424 RVA: 0x00113672 File Offset: 0x00111A72
		public ChallengeGoal challenge { get; protected set; }

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003859 RID: 14425 RVA: 0x0011367B File Offset: 0x00111A7B
		protected virtual string CollectAnimationName
		{
			get
			{
				return "ChallengeViewTaskToCollect";
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x00113682 File Offset: 0x00111A82
		protected virtual string CompleteAnimationName
		{
			get
			{
				return "ChallengeViewCollectToCompleted";
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x00113689 File Offset: 0x00111A89
		protected virtual string ReturnToCollectAnimationName
		{
			get
			{
				return "ChallengeViewCompletedToCollectAgain";
			}
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x00113690 File Offset: 0x00111A90
		public virtual void SetChallengeData(int challengeIndex, GameStateService stateService, ILocalizationService localizationService, ChallengeService challengeService, SBSService sbsService, IVideoAdService adService)
		{
			this.gameStateService = stateService;
			this.localizationService = localizationService;
			this.challengeService = challengeService;
			this.sbsService = sbsService;
			this.videoAdService = adService;
			this.challenge = stateService.Challenges.CurrentChallenges[challengeIndex];
			this.pawReward = challengeService.GetPawReward(this.challenge);
			this.difficultyBackground.sprite = this.spriteManager.GetSimilar(this.challenge.difficulty.ToString());
			this.itemImage.sprite = this.spriteManager.GetSimilar(this.challenge.type);
			this.difficultyLabel.text = localizationService.GetText("ui.challenges.difficulty." + this.challenge.difficulty, new LocaParam[0]);
			this.pawLabel.text = this.pawReward.ToString();
			this.taskLabel.text = string.Format(localizationService.GetText("ui.challenges.task." + this.challenge.type, new LocaParam[0]), this.challenge.goal);
			this.collectButton.onClick.RemoveAllListeners();
			this.collectButton.onClick.AddListener(new UnityAction(this.OnCollectButton));
			this.collectButton.interactable = true;
			this.cover.SetActive(!challengeService.IsChallengeRunning && !this.challenge.DidSeeCompleted);
			this.SetupViewState(this.gameStateService.Resources.GetCollectedTotal(this.challenge.type), !challengeService.IsChallengeRunning);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x00113853 File Offset: 0x00111C53
		public void UpdatePawReward(int newAmount)
		{
			this.pawReward = newAmount;
			base.StartCoroutine(this.AnimateBonusTextRoutine());
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x0011386C File Offset: 0x00111C6C
		protected virtual void SetupViewState(int amountInGameState, bool locked)
		{
			int amountCollectedAndViewed = this.challenge.AmountCollectedAndViewed;
			int num = this.challenge.TargetAmount - this.challenge.start;
			float num2 = (float)amountCollectedAndViewed / (float)num;
			if (this.challenge.collected)
			{
				this.stateAnimation[this.CompleteAnimationName].time = 1f;
				this.stateAnimation.Play(this.CompleteAnimationName);
			}
			else if (amountCollectedAndViewed < num)
			{
				this.progressSlider.fillAmount = ((num2 >= 1f) ? 1f : num2);
				int num3 = amountInGameState - this.challenge.start;
				if (locked)
				{
					num3 = this.challenge.AmountCollectedAndViewed;
				}
				this.progressLabel.text = num3 + "/" + this.challenge.goal;
				if (!locked)
				{
					base.StartCoroutine(this.AnimateProgressRoutine(amountInGameState));
				}
			}
			else
			{
				this.stateAnimation[this.CollectAnimationName].time = 1f;
				this.stateAnimation.Play(this.CollectAnimationName);
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x0011399E File Offset: 0x00111D9E
		private void OnCollectButton()
		{
			this.collectButton.interactable = false;
			base.StartCoroutine(this.CollectPawsRoutine());
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x001139BC File Offset: 0x00111DBC
		protected virtual IEnumerator CollectPawsRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.stateAnimation != null)
			{
				this.stateAnimation.Play(this.CompleteAnimationName);
				yield return new WaitForSeconds(this.stateAnimation[this.CompleteAnimationName].length);
			}
			this.gameStateService.Resources.AddMaterial("paws", this.pawReward, true);
			this.trackingService.TrackChallengeEvent("collect_paws", true, this.challenge, this.pawReward, 0);
			this.challenge.collected = true;
			this.challenge.CollectedTime = DateTime.UtcNow;
			yield break;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x001139D8 File Offset: 0x00111DD8
		protected virtual IEnumerator AnimateProgressRoutine(int amountInGameState)
		{
			int collectedAmount = amountInGameState - this.challenge.start;
			if (collectedAmount == this.challenge.AmountCollectedAndViewed)
			{
				yield break;
			}
			yield return new WaitForSeconds(0.5f);
			float targetFill = (float)collectedAmount / (float)this.challenge.goal;
			targetFill = ((targetFill >= 1f) ? 1f : targetFill);
			float startFill = this.progressSlider.fillAmount;
			float elapsedTime = 0f;
			float ratio = 0f;
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / 0.5f;
				this.progressSlider.fillAmount = Mathf.Lerp(startFill, targetFill, ratio);
				yield return null;
			}
			this.progressSlider.fillAmount = targetFill;
			this.challenge.lastViewed = amountInGameState;
			if (amountInGameState >= this.challenge.TargetAmount && this.stateAnimation != null)
			{
				this.stateAnimation.Play(this.CollectAnimationName);
			}
			yield break;
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x001139FC File Offset: 0x00111DFC
		private IEnumerator AnimateBonusTextRoutine()
		{
			this.bonusAnimation.Play();
			yield return new WaitForSeconds(this.bonusAnimation["ChallengeAmountBounce"].length * 0.35f);
			this.pawLabel.text = this.pawReward.ToString();
			yield break;
		}

		// Token: 0x04006063 RID: 24675
		protected const string DIFFICULTY_PRFIX = "ui.challenges.difficulty.";

		// Token: 0x04006064 RID: 24676
		protected const string TASK_PREFIX = "ui.challenges.task.";

		// Token: 0x04006065 RID: 24677
		private const string BONUS_ANIMATION_NAME = "ChallengeAmountBounce";

		// Token: 0x04006066 RID: 24678
		private const float PROGRESS_ANIMATION_TIME = 0.5f;

		// Token: 0x04006067 RID: 24679
		private const float INITIAL_DELAY = 0.5f;

		// Token: 0x04006068 RID: 24680
		[WaitForService(true, true)]
		protected TrackingService trackingService;

		// Token: 0x04006069 RID: 24681
		[SerializeField]
		protected SpriteManagerWithOverride spriteManager;

		// Token: 0x0400606A RID: 24682
		[SerializeField]
		protected Button collectButton;

		// Token: 0x0400606B RID: 24683
		[SerializeField]
		protected Image progressSlider;

		// Token: 0x0400606C RID: 24684
		[SerializeField]
		protected Image itemImage;

		// Token: 0x0400606D RID: 24685
		[SerializeField]
		protected GameObject cover;

		// Token: 0x0400606E RID: 24686
		[SerializeField]
		protected TextMeshProUGUI taskLabel;

		// Token: 0x0400606F RID: 24687
		[SerializeField]
		protected Image difficultyBackground;

		// Token: 0x04006070 RID: 24688
		[SerializeField]
		protected TextMeshProUGUI difficultyLabel;

		// Token: 0x04006071 RID: 24689
		[SerializeField]
		protected TextMeshProUGUI pawLabel;

		// Token: 0x04006072 RID: 24690
		[SerializeField]
		protected TextMeshProUGUI progressLabel;

		// Token: 0x04006073 RID: 24691
		[SerializeField]
		protected Animation stateAnimation;

		// Token: 0x04006074 RID: 24692
		[SerializeField]
		protected Animation bonusAnimation;

		// Token: 0x04006075 RID: 24693
		protected GameStateService gameStateService;

		// Token: 0x04006076 RID: 24694
		protected ILocalizationService localizationService;

		// Token: 0x04006077 RID: 24695
		protected ChallengeService challengeService;

		// Token: 0x04006078 RID: 24696
		protected SBSService sbsService;

		// Token: 0x04006079 RID: 24697
		protected IVideoAdService videoAdService;

		// Token: 0x0400607B RID: 24699
		protected int pawReward;
	}
}
