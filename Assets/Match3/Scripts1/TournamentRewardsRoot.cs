using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A59 RID: 2649
namespace Match3.Scripts1
{
	public class TournamentRewardsRoot : APtSceneRoot<Materials>, IHandler<PopupOperation>
	{
		// Token: 0x06003F72 RID: 16242 RVA: 0x001449EE File Offset: 0x00142DEE
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				this.parameters = this.testRewards;
				this.Show();
			}
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00144A10 File Offset: 0x00142E10
		public void Show()
		{
			base.Enable();
			this.rewards = this.parameters;
			this.buttonContinue.onClick.RemoveListener(new UnityAction(this.HandleButtonClose));
			this.buttonContinue.onClick.AddListener(new UnityAction(this.HandleButtonClose));
			BackButtonManager.Instance.AddAction(new Action(this.HandleButtonClose));
			this.HideViews();
			this.SwitchOffRowsNotNeeded();
			this.audioService.PlaySFX(AudioId.LevelCompleted, false, false, false);
			this.canvasGroup.interactable = true;
			if (this.windowAnimation != null)
			{
				this.windowAnimation.Rewind();
			}
			if (this.rewards != null)
			{
				this.ApplyRewards();
				WooroutineRunner.StartCoroutine(this.Animate(), null);
			}
			else
			{
				this.CollectAndClose();
			}
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x00144AEE File Offset: 0x00142EEE
		protected override void OnDisable()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleButtonClose));
			base.OnDisable();
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00144B0C File Offset: 0x00142F0C
		protected void SwitchOffRowsNotNeeded()
		{
			float num = Mathf.Ceil((float)this.rewards.Count / 2f);
			for (int i = 0; i < this.rewardRows.Length; i++)
			{
				this.rewardRows[i].SetActive((float)i < num);
			}
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x00144B5C File Offset: 0x00142F5C
		protected void HideViews()
		{
			this.coinView.Hide();
			this.diamondView.Hide();
			foreach (MaterialAmountView materialAmountView in this.boosterViews)
			{
				materialAmountView.Hide();
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00144BA4 File Offset: 0x00142FA4
		private void ApplyRewards()
		{
			if (this.rewards == null)
			{
				return;
			}
			int num = this.ApplyAndCountNonBoosterRewards();
			foreach (MaterialAmount materialAmount in this.rewards)
			{
				if (num >= this.boosterViews.Length)
				{
					break;
				}
				if (materialAmount.amount > 0 && !(materialAmount.type == "coins") && !(materialAmount.type == "diamonds"))
				{
					this.boosterViews[num++].Show(new MaterialAmount(materialAmount.type, materialAmount.amount, MaterialAmountUsage.Undefined, 0));
				}
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00144C80 File Offset: 0x00143080
		private int ApplyAndCountNonBoosterRewards()
		{
			bool flag = false;
			bool flag2 = false;
			foreach (MaterialAmount mat in this.rewards)
			{
				if (mat.type == "coins")
				{
					flag = true;
					this.coinView.Show(mat);
				}
				if (mat.type == "diamonds")
				{
					flag2 = true;
					this.diamondView.Show(mat);
				}
			}
			int num = (!flag) ? 0 : 1;
			return (!flag2) ? num : (num + 1);
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00144D44 File Offset: 0x00143144
		private IEnumerator Animate()
		{
			this.coinView.canvasGroup.alpha = 0f;
			this.coinView.gameObject.SetActive(false);
			this.diamondView.canvasGroup.alpha = 0f;
			this.diamondView.gameObject.SetActive(false);
			foreach (MaterialAmountView materialAmountView in this.boosterViews)
			{
				materialAmountView.canvasGroup.alpha = 0f;
				materialAmountView.gameObject.SetActive(false);
			}
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			yield return this.ShowAndWaitFor(this.diamondView);
			yield return this.ShowAndWaitFor(this.coinView);
			foreach (MaterialAmountView view in this.boosterViews)
			{
				yield return this.ShowAndWaitFor(view);
			}
			this.audioService.PlaySFX(AudioId.BannerShowDefault, false, false, false);
			yield return this.PlayAnimation("ButtonCollectOpen");
			yield break;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00144D60 File Offset: 0x00143160
		private IEnumerator ShowAndWaitFor(MaterialAmountView view)
		{
			if (view.Data.amount > 0)
			{
				view.Show();
				this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
				if (!this.showAllRewardsAtOnce)
				{
					yield return this.WaitForAnimation(view);
				}
				else
				{
					base.StartCoroutine(this.WaitForAnimation(view));
				}
			}
			yield break;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00144D84 File Offset: 0x00143184
		private IEnumerator WaitForAnimation(MaterialAmountView view)
		{
			Animation animComponent = view.GetComponent<Animation>();
			if (animComponent != null)
			{
				while (animComponent.isPlaying && !this.isSkipping)
				{
					yield return null;
				}
				if (this.isSkipping)
				{
					string name = animComponent.clip.name;
					animComponent[name].normalizedTime = 1f;
					animComponent.Sample();
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x00144DA8 File Offset: 0x001431A8
		private IEnumerator PlayAnimation(string name)
		{
			this.currentAnimation = name;
			this.windowAnimation.Play(name);
			this.windowAnimation.wrapMode = WrapMode.Once;
			if (this.isSkipping)
			{
				this.windowAnimation[this.currentAnimation].normalizedTime = 1f;
				this.windowAnimation.Sample();
				yield break;
			}
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			this.windowAnimation.wrapMode = WrapMode.Default;
			yield break;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x00144DCA File Offset: 0x001431CA
		private void CollectAndClose()
		{
			BackButtonManager.Instance.SetEnabled(true);
			base.Disable();
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x00144DE0 File Offset: 0x001431E0
		private void HandleButtonClose()
		{
			BackButtonManager.Instance.SetEnabled(false);
			TownResourcePanelRoot townResourcePanelRoot = global::UnityEngine.Object.FindObjectOfType<TownResourcePanelRoot>();
			if (townResourcePanelRoot)
			{
				foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in townResourcePanelRoot.GetComponentsInChildren<MaterialAmountDisplayLabel>())
				{
					materialAmountDisplayLabel.allowReserving = false;
				}
			}
			MaterialAmountDisplayLabel[] array = global::UnityEngine.Object.FindObjectsOfType<MaterialAmountDisplayLabel>();
			using (IEnumerator<MaterialAmount> enumerator = this.rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MaterialAmount reward = enumerator.Current;
					if (reward.amount > 0)
					{
						MaterialAmountDisplayLabel materialAmountDisplayLabel2 = Array.Find<MaterialAmountDisplayLabel>(array, (MaterialAmountDisplayLabel d) => d.material == reward.type);
						if (materialAmountDisplayLabel2)
						{
							string type = reward.type;
							if (type != null)
							{
								if (!(type == "diamonds"))
								{
									if (type == "coins")
									{
										this.doobers.SpawnDoobers(reward, this.coinView.transform, materialAmountDisplayLabel2.icon.transform, null);
									}
								}
								else
								{
									this.doobers.SpawnDoobers(reward, this.diamondView.transform, materialAmountDisplayLabel2.icon.transform, null);
								}
							}
						}
					}
				}
			}
			WooroutineRunner.StartCoroutine(this.TryToCollectAndClose(), null);
			this.canvasGroup.interactable = false;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00144F8C File Offset: 0x0014338C
		private IEnumerator TryToCollectAndClose()
		{
			yield return null;
			while (Doober.ActiveDoobers > 0)
			{
				yield return null;
			}
			this.CollectAndClose();
			yield break;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00144FA8 File Offset: 0x001433A8
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.OK)
			{
				if (op == PopupOperation.Skip)
				{
					this.isSkipping = true;
					if (!string.IsNullOrEmpty(this.currentAnimation))
					{
						this.windowAnimation[this.currentAnimation].normalizedTime = 1f;
					}
				}
			}
		}

		// Token: 0x0400690A RID: 26890
		private const string ButtonCollectOpen = "ButtonCollectOpen";

		// Token: 0x0400690B RID: 26891
		private bool isSkipping;

		// Token: 0x0400690C RID: 26892
		private string currentAnimation;

		// Token: 0x0400690D RID: 26893
		private Materials rewards;

		// Token: 0x0400690E RID: 26894
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x0400690F RID: 26895
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04006910 RID: 26896
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04006911 RID: 26897
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04006912 RID: 26898
		[SerializeField]
		private bool showAllRewardsAtOnce;

		// Token: 0x04006913 RID: 26899
		[SerializeField]
		private Button buttonContinue;

		// Token: 0x04006914 RID: 26900
		[SerializeField]
		private MaterialAmountView coinView;

		// Token: 0x04006915 RID: 26901
		[SerializeField]
		private MaterialAmountView diamondView;

		// Token: 0x04006916 RID: 26902
		[SerializeField]
		private MaterialAmountView[] boosterViews;

		// Token: 0x04006917 RID: 26903
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04006918 RID: 26904
		[SerializeField]
		private Animation windowAnimation;

		// Token: 0x04006919 RID: 26905
		[SerializeField]
		private Materials testRewards;

		// Token: 0x0400691A RID: 26906
		[SerializeField]
		private GameObject[] rewardRows;
	}
}
