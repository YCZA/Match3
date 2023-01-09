using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.iOS;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x02000731 RID: 1841
	public class PopupRatingRoot : APtSceneRoot
	{
		// Token: 0x06002D9E RID: 11678 RVA: 0x000D4464 File Offset: 0x000D2864
		private void Start()
		{
			this.canvases = base.GetComponentsInChildren<Canvas>(true);
			this.buttonEnjoyYes.onClick.AddListener(delegate()
			{
				this.HandleEnjoy(true);
			});
			this.buttonEnjoyNo.onClick.AddListener(delegate()
			{
				this.HandleEnjoy(false);
			});
			this.buttonRate.onClick.AddListener(new UnityAction(this.HandleRate));
			this.buttonRemindLater.onClick.AddListener(new UnityAction(this.HandleRemindLater));
			this.buttonSendFeedback.onClick.AddListener(new UnityAction(this.HandleSendFeedback));
			foreach (Button button in this.closeButtons)
			{
				button.onClick.AddListener(new UnityAction(this.HandleClose));
			}
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000D4540 File Offset: 0x000D2940
		protected override void Go()
		{
			this.SwitchToCanvas(this.canvasEnjoy);
			this.Track("enjoy_tropicats", "open");
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000D455E File Offset: 0x000D295E
		private void Track(string ui_det, string action)
		{
			this.tracking.TrackUi("rate_us", ui_det, action, null, null);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000D4574 File Offset: 0x000D2974
		private void SwitchToCanvas(Canvas canvas)
		{
			foreach (Canvas canvas2 in this.canvases)
			{
				canvas2.gameObject.SetActive(canvas2 == canvas);
			}
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000D45B4 File Offset: 0x000D29B4
		private void HandleEnjoy(bool isEnjoying)
		{
			BackButtonManager.Instance.AddAction(new Action(this.HandleClose));
			this.Track("enjoy_tropicats", (!isEnjoying) ? "no" : "yes");
			if (isEnjoying)
			{
				this.SwitchToCanvas(this.canvasRate);
			}
			else
			{
				this.StopAsking();
				this.SwitchToCanvas(this.canvasFeedback);
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000D4620 File Offset: 0x000D2A20
		private void HandleClose()
		{
			string text = null;
			if (this.canvasRate.isActiveAndEnabled)
			{
				this.StopAsking();
				text = "rate_us";
			}
			else if (this.canvasFeedback.isActiveAndEnabled)
			{
				text = "tell_us_why";
			}
			else if (this.canvasThanks.isActiveAndEnabled)
			{
				text = "thank_you";
			}
			if (text != null)
			{
				this.Track(text, "close");
			}
			base.Destroy();
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000D4699 File Offset: 0x000D2A99
		private void HandleRate()
		{
			this.Track("rate_us", "rate_us");
			ForceUpdate.GoToShop();
			this.StopAsking();
			this.SwitchToCanvas(this.canvasThanks);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000D46C4 File Offset: 0x000D2AC4
		private void HandleRemindLater()
		{
			this.Track("rate_us", "remind_me_later");
			this.session.ratingInfo.nextReminderAtLevel = this.progression.UnlockedLevel + this.configService.general.rating_filter.wait_num_levels;
			this.session.ratingInfo.Save();
			base.Destroy();
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000D4728 File Offset: 0x000D2B28
		private void HandleSendFeedback()
		{
			this.Track("tell_us_why", "send_feedback");
			this.helpshift.ShowRatingFeedback();
			base.Destroy();
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000D474B File Offset: 0x000D2B4B
		private void StopAsking()
		{
			this.session.ratingInfo.stopShowing = true;
			this.session.ratingInfo.Save();
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000D476E File Offset: 0x000D2B6E
		protected override void OnDestroy()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleClose));
			base.OnDestroy();
		}

		// Token: 0x0400572E RID: 22318
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x0400572F RID: 22319
		[WaitForService(true, true)]
		private SessionService session;

		// Token: 0x04005730 RID: 22320
		[WaitForService(true, true)]
		private HelpshiftService helpshift;

		// Token: 0x04005731 RID: 22321
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04005732 RID: 22322
		[WaitForService(true, true)]
		private ProgressionDataService.Service progression;

		// Token: 0x04005733 RID: 22323
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005734 RID: 22324
		[SerializeField]
		private Canvas canvasEnjoy;

		// Token: 0x04005735 RID: 22325
		[SerializeField]
		private Canvas canvasRate;

		// Token: 0x04005736 RID: 22326
		[SerializeField]
		private Canvas canvasThanks;

		// Token: 0x04005737 RID: 22327
		[SerializeField]
		private Canvas canvasFeedback;

		// Token: 0x04005738 RID: 22328
		[SerializeField]
		private Button buttonEnjoyYes;

		// Token: 0x04005739 RID: 22329
		[SerializeField]
		private Button buttonEnjoyNo;

		// Token: 0x0400573A RID: 22330
		[SerializeField]
		private Button buttonRate;

		// Token: 0x0400573B RID: 22331
		[SerializeField]
		private Button buttonRemindLater;

		// Token: 0x0400573C RID: 22332
		[SerializeField]
		private Button buttonSendFeedback;

		// Token: 0x0400573D RID: 22333
		[SerializeField]
		private Button[] closeButtons;

		// Token: 0x0400573E RID: 22334
		private Canvas[] canvases;

		// Token: 0x02000732 RID: 1842
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x06002DAB RID: 11691 RVA: 0x000D479E File Offset: 0x000D2B9E
			public Trigger(SessionService session, ConfigService configService, ProgressionDataService.Service progression, SBSService sbs, TrackingService tracking)
			{
				this.session = session;
				this.configService = configService;
				this.progression = progression;
				this.sbs = sbs;
				this.trackingService = tracking;
			}

			// Token: 0x06002DAC RID: 11692 RVA: 0x000D47CC File Offset: 0x000D2BCC
			public override bool ShouldTrigger()
			{
				if (this.ShouldUseIosNativeRating() && !IosReviewRequest.IsNewRatingSystem())
				{
					return false;
				}
				SessionService.RatingInfo ratingInfo = this.session.ratingInfo;
				WoogaDebug.Log(new object[]
				{
					ratingInfo,
					this.session.NumTrackedSessions,
					this.session.SessionsWithoutCrashes
				});
				RatingFilter rating_filter = this.configService.general.rating_filter;
				return !ratingInfo.stopShowing && this.session.SessionsWithoutCrashes >= rating_filter.sessions_without_crashes && this.progression.UnlockedLevel >= ratingInfo.nextReminderAtLevel && this.session.roundsPlayed >= rating_filter.rounds_played_this_session && this.session.wasLastRoundSuccesfull && Application.internetReachability != NetworkReachability.NotReachable;
			}

			// Token: 0x06002DAD RID: 11693 RVA: 0x000D48B0 File Offset: 0x000D2CB0
			public override IEnumerator Run()
			{
				// eli key point popup rating
				// if (this.ShouldUseIosNativeRating())
				// {
				// 	this.trackingService.TrackUi("rate_us", "apple_native_popup", "triggered", null, null);
				// 	IosReviewRequest.RequestReview();
				// }
				// else
				// {
				// 	Wooroutine<PopupRatingRoot> popup = SceneManager.Instance.LoadScene<PopupRatingRoot>(null);
				// 	yield return popup;
				// 	yield return popup.ReturnValue.onDestroyed;
				// }
				yield break;
			}

			// Token: 0x06002DAE RID: 11694 RVA: 0x000D48CB File Offset: 0x000D2CCB
			private bool ShouldUseIosNativeRating()
			{
				return false;
			}

			// Token: 0x0400573F RID: 22335
			private readonly SessionService session;

			// Token: 0x04005740 RID: 22336
			private readonly ConfigService configService;

			// Token: 0x04005741 RID: 22337
			private ProgressionDataService.Service progression;

			// Token: 0x04005742 RID: 22338
			private readonly SBSService sbs;

			// Token: 0x04005743 RID: 22339
			private readonly TrackingService trackingService;
		}
	}
}
