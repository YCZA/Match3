using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.Shared.UI;
using Match3.Scripts1.Wooga.Core.Network;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.Tracking.Calls;
using Match3.Scripts1.Wooga.Services.Tracking.LifeCycle;
using Match3.Scripts1.Wooga.Services.Tracking.Tools;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007BD RID: 1981
	public class GiftLinksService : AService
	{
		// Token: 0x060030CE RID: 12494 RVA: 0x000E4BCF File Offset: 0x000E2FCF
		static GiftLinksService()
		{
			if (GiftLinksService._003C_003Ef__mg_0024cache0 == null)
			{
				GiftLinksService._003C_003Ef__mg_0024cache0 = new AppLinks.LinkReceivedHandler(GiftLinksService.OnReceiveAppLink);
			}
			AppLinks.OnAppLink += GiftLinksService._003C_003Ef__mg_0024cache0;
			GiftLinksService.subscribedToAppLink = true;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000E4C03 File Offset: 0x000E3003
		public GiftLinksService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000E4C18 File Offset: 0x000E3018
		// (set) Token: 0x060030D1 RID: 12497 RVA: 0x000E4C20 File Offset: 0x000E3020
		public List<Gift> ClaimableGifts { get; set; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000E4C29 File Offset: 0x000E3029
		// (set) Token: 0x060030D3 RID: 12499 RVA: 0x000E4C31 File Offset: 0x000E3031
		public bool AllowedToProcessGiftLinks { get; set; }

		// Token: 0x060030D4 RID: 12500 RVA: 0x000E4C3C File Offset: 0x000E303C
		public IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (!GiftLinksService.subscribedToAppLink)
			{
				if (GiftLinksService._003C_003Ef__mg_0024cache1 == null)
				{
					GiftLinksService._003C_003Ef__mg_0024cache1 = new AppLinks.LinkReceivedHandler(GiftLinksService.OnReceiveAppLink);
				}
				AppLinks.OnAppLink += GiftLinksService._003C_003Ef__mg_0024cache1;
				GiftLinksService.subscribedToAppLink = true;
			}
			this.ClaimableGifts = new List<Gift>();
			base.OnInitialized.Dispatch();
			WooroutineRunner.StartCoroutine(this.RefreshRoutine(), null);
			yield break;
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000E4C58 File Offset: 0x000E3058
		private IEnumerator RefreshRoutine()
		{
			WaitForSeconds waitTime = new WaitForSeconds(1f);
			for (;;)
			{
				TrackingLifeCycleDispatcher.CheckAppLinks();
				yield return this.ProcessGiftLinks();
				yield return waitTime;
			}
			yield break;
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000E4C74 File Offset: 0x000E3074
		private static void OnReceiveAppLink(string applink)
		{
			Uri uri = new Uri(applink);
			string eventType = GiftLinksService.GetEventType(uri);
			if (eventType == "gift")
			{
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uri.Query);
				string code = nameValueCollection.Get("code");
				if (!string.IsNullOrEmpty(code) && !GiftLinksService.giftLinks.Exists((GiftLinkProcess gift) => gift.code == code))
				{
					GiftLinksService.giftLinks.Add(new GiftLinkProcess(GiftLinkStatus.readyForProccessing, code));
				}
			}
			else if (eventType == "open")
			{
				NameValueCollection nameValueCollection2 = HttpUtility.ParseQueryString(uri.Query);
				string text = nameValueCollection2.Get("url");
				if (!string.IsNullOrEmpty(text))
				{
					string user_id = SBS.Authentication.GetUserContext().user_id;
					string url = text.Replace("[sbs_id_value]", WWW.EscapeURL(user_id));
					Application.OpenURL(url);
				}
			}
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000E4D6C File Offset: 0x000E316C
		public Coroutine ProcessGiftLinks()
		{
			return WooroutineRunner.StartCoroutine(this.ProcessGiftLinksRoutine(), null);
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000E4D7C File Offset: 0x000E317C
		private IEnumerator ProcessGiftLinksRoutine()
		{
			if (this.currentGiftLinksProcess != null)
			{
				yield break;
			}
			this.currentGiftLinksProcess = WooroutineRunner.StartCoroutine(this.ProccesCurrentGiftLinksRoutine(), null);
			yield return this.currentGiftLinksProcess;
			GiftLinksService.giftLinks.RemoveAll((GiftLinkProcess giftLink) => giftLink.status == GiftLinkStatus.done);
			this.currentGiftLinksProcess = null;
			yield break;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000E4D98 File Offset: 0x000E3198
		private IEnumerator ProccesCurrentGiftLinksRoutine()
		{
			yield return null;
			for (int index = 0; index < GiftLinksService.giftLinks.Count; index++)
			{
				if (GiftLinksService.giftLinks[index].status == GiftLinkStatus.readyForProccessing)
				{
					yield return this.ProcessGiftLink(GiftLinksService.giftLinks[index]);
				}
			}
			yield break;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000E4DB4 File Offset: 0x000E31B4
		private IEnumerator ProcessGiftLink(GiftLinkProcess giftLink)
		{
			giftLink.status = GiftLinkStatus.inProcess;
			Gift gift = null;
			Wooroutine<SbsResponse> verificationResult = this.sbsService.serviceRunner.WaitForSBSRequest(this.CreateVerificationRequest(giftLink.code));
			yield return verificationResult;
			SbsResponse response = verificationResult.ReturnValue;
			string errorCode = "invalid";
			if (verificationResult.ReturnValue != null && verificationResult.ReturnValue.StatusCode == HttpStatusCode.OK)
			{
				IDictionary<string, JSONNode> dictionary = response.ParseBody();
				JSONNode jsonnode;
				if (dictionary.TryGetValue("status", out jsonnode))
				{
					string text = jsonnode.AsString();
					if (text == "ok")
					{
						JSONNode jsonnode2;
						if (dictionary.TryGetValue("rewards", out jsonnode2))
						{
							GiftLinkPayload giftLinkPayload = JSON.Deserialize<GiftLinkPayload>(jsonnode2.ToString());
							if (giftLinkPayload == null)
							{
								WoogaDebug.LogWarning(new object[]
								{
									"GIFTLINKS: Failed to parse giftlink payload",
									"Code: " + giftLink.code + " Json: " + jsonnode2.ToString()
								});
							}
							else
							{
								gift = new Gift();
								gift.body_textkey = "ui.gift_link.body.valid";
								gift.id = giftLink.code;
								gift.gift_link_id = giftLink.code;
								gift.valid = true;
								if (giftLinkPayload.rewardsList[0].type == "u_lives")
								{
									gift.gift_type = "lives_unlimited";
								}
								else
								{
									gift.gift_type = giftLinkPayload.rewardsList[0].type;
								}
								gift.gift_amount = giftLinkPayload.rewardsList[0].amount;
							}
						}
						else
						{
							WoogaDebug.LogWarning(new object[]
							{
								"GIFTLINKS: no reward json found",
								"Code: " + giftLink.code
							});
						}
					}
					else
					{
						errorCode = text;
					}
				}
			}
			if (gift == null)
			{
				gift = new Gift();
				gift.error_code = errorCode;
				gift.id = giftLink.code;
				gift.valid = false;
			}
			if (this.AllowedToProcessGiftLinks)
			{
				if (gift.valid)
				{
					yield return SceneManager.Instance.LoadSceneWithParams<PopupFreeGiftRoot, Gift>(gift, null);
				}
				else
				{
					yield return this.ShowErrorOrClaimedDialog(gift.id, errorCode);
				}
			}
			else
			{
				this.ClaimableGifts.Add(gift);
			}
			giftLink.status = GiftLinkStatus.done;
			yield break;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000E4DD8 File Offset: 0x000E31D8
		public Coroutine ShowErrorOrClaimedDialog(string id, string errorCode)
		{
			this.trackingService.TrackUi("gift_link", id, "open", errorCode, new object[0]);
			PopupSortingOrder order = new PopupSortingOrder(UILayer.Top);
			return PopupDialogRoot.ShowOkDialog(this.localizationService.GetText("ui.gift_link.title", new LocaParam[0]), this.localizationService.GetText("ui.gift_link.body.invalid", new LocaParam[0]), null, order);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000E4E3C File Offset: 0x000E323C
		private static string GetEventType(Uri uri)
		{
			if (uri.Scheme.Equals("http") || (uri.Scheme.Equals("https") && uri.Segments.Length > 0))
			{
				return uri.Segments[uri.Segments.Length - 1];
			}
			return uri.Host;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000E4E9C File Offset: 0x000E329C
		private SbsRequest CreateVerificationRequest(string code)
		{
			return new SbsRequest
			{
				TimeoutInSeconds = this.sbsService.SbsConfig.sbs_timeouts.giftlink_verification_request,
				Method = HttpMethod.GET,
				// Host = "gls.gbs.wooga.com",
				Host = "host2333",
				Path = string.Format("/api/v1/gift/claim/{0}", code),
				UseSignature = true,
				UserContext = SBS.Authentication.GetUserContext()
			};
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000E4F05 File Offset: 0x000E3305
		public override void DeInit()
		{
			if (GiftLinksService._003C_003Ef__mg_0024cache2 == null)
			{
				GiftLinksService._003C_003Ef__mg_0024cache2 = new AppLinks.LinkReceivedHandler(GiftLinksService.OnReceiveAppLink);
			}
			AppLinks.OnAppLink -= GiftLinksService._003C_003Ef__mg_0024cache2;
			GiftLinksService.subscribedToAppLink = false;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000E4F30 File Offset: 0x000E3330
		public override void OnResume()
		{
			if (base.OnInitialized.WasDispatched && !SceneManager.IsPlayingMatch3)
			{
				TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
				if (tutorialRunner == null || !tutorialRunner.IsRunning)
				{
					this.ProcessGiftLinks();
				}
			}
			base.OnResume();
		}

		// Token: 0x04005982 RID: 22914
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005983 RID: 22915
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005984 RID: 22916
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005985 RID: 22917
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005986 RID: 22918
		private static readonly List<GiftLinkProcess> giftLinks = new List<GiftLinkProcess>();

		// Token: 0x04005987 RID: 22919
		private static bool subscribedToAppLink;

		// Token: 0x04005988 RID: 22920
		private Coroutine currentGiftLinksProcess;

		// Token: 0x0400598B RID: 22923
		[CompilerGenerated]
		private static AppLinks.LinkReceivedHandler _003C_003Ef__mg_0024cache0;

		// Token: 0x0400598C RID: 22924
		[CompilerGenerated]
		private static AppLinks.LinkReceivedHandler _003C_003Ef__mg_0024cache1;

		// Token: 0x0400598D RID: 22925
		[CompilerGenerated]
		private static AppLinks.LinkReceivedHandler _003C_003Ef__mg_0024cache2;
	}
}
