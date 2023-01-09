using Match3.Scripts1.com.adjust.sdk;
using Match3.Scripts1.Wooga.Core.DeviceInfo;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200073A RID: 1850
	public class AdjustService : AService, IAdjustService, IService, IInitializable
	{
		// Token: 0x06002DDC RID: 11740 RVA: 0x000D5096 File Offset: 0x000D3496
		public AdjustService(TrackingService trackingService)
		{
			this.tracking = trackingService;
			this.Init();
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000D50AC File Offset: 0x000D34AC
		public void Init()
		{
			string appToken = "nv5pac6hq5mo";
			AdjustEnvironment environment = (GameEnvironment.CurrentEnvironment != GameEnvironment.Environment.PRODUCTION) ? AdjustEnvironment.Sandbox : AdjustEnvironment.Production;
			Adjust.addSessionCallbackParameter("mid", DeviceId.uniqueIdentifier);
			Adjust.addSessionPartnerParameter("mid", DeviceId.uniqueIdentifier);
			AdjustConfig adjustConfig = new AdjustConfig(appToken, environment, true);
			adjustConfig.setLogLevel(AdjustLogLevel.Suppress);
			if (Application.platform == RuntimePlatform.Android)
			{
				adjustConfig.setAppSecret(1L, 1953445234L, 306705448L, 1401965326L, 1523863550L);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				adjustConfig.setAppSecret(2L, 1732187218L, 997260513L, 1865063024L, 2005116956L);
			}
			Adjust.start(adjustConfig);
			base.OnInitialized.Dispatch();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000D5170 File Offset: 0x000D3570
		// public void TrackPurchase(Product product)
		// {
		// 	AdjustEvent adjustEvent = new AdjustEvent("lephxt");
		// 	adjustEvent.setTransactionId(product.transactionID);
		// 	adjustEvent.setRevenue((double)product.metadata.localizedPrice, product.metadata.isoCurrencyCode);
		// 	this.Track(adjustEvent);
		// }

		// Token: 0x06002DDF RID: 11743 RVA: 0x000D51BC File Offset: 0x000D35BC
		public void TrackFbLogin()
		{
			this.Track("811kva");
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000D51C9 File Offset: 0x000D35C9
		public void TrackTutorialFinished()
		{
			this.Track("wxv9cc");
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000D51D6 File Offset: 0x000D35D6
		public void TrackQuestComplete(string questId)
		{
			if (questId == "chp3_q1")
			{
				this.Track("78b14i");
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000D51F4 File Offset: 0x000D35F4
		public void TrackLevelComplete(Level level)
		{
			if (level.level == 14 && level.tier == 0)
			{
				this.Track("sfmye5");
			}
			else if (level.level == 61 && level.tier == 0)
			{
				this.Track("9bkecq");
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000D5250 File Offset: 0x000D3650
		public void TrackThirdPurchase()
		{
			this.Track("38c1yn");
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000D525D File Offset: 0x000D365D
		public void TrackPurchaseScreenOpen()
		{
			this.Track("41hgme");
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000D526A File Offset: 0x000D366A
		private void Track(string name)
		{
			WoogaDebug.LogFormatted("send event: {0}", new object[]
			{
				name
			});
			this.Track(new AdjustEvent(name));
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000D528C File Offset: 0x000D368C
		private void Track(AdjustEvent ev)
		{
			this.tracking.TrackEvent(new object[]
			{
				"adjust",
				ev.eventToken,
				ev.revenue
			});
			Adjust.trackEvent(ev);
		}

		// Token: 0x0400575B RID: 22363
		private const string chapter1_complete = "n9gg7y";

		// Token: 0x0400575C RID: 22364
		private const string chapter2_complete = "bnox8k";

		// Token: 0x0400575D RID: 22365
		private const string facebook_login = "811kva";

		// Token: 0x0400575E RID: 22366
		private const string level14_won = "sfmye5";

		// Token: 0x0400575F RID: 22367
		private const string level_61a_won = "9bkecq";

		// Token: 0x04005760 RID: 22368
		private const string purchase = "lephxt";

		// Token: 0x04005761 RID: 22369
		private const string tutorial_complete = "wxv9cc";

		// Token: 0x04005762 RID: 22370
		private const string chapter3_q1_complete = "78b14i";

		// Token: 0x04005763 RID: 22371
		private const string thirdPurchase = "38c1yn";

		// Token: 0x04005764 RID: 22372
		private const string purchaseScreen = "41hgme";

		// Token: 0x04005765 RID: 22373
		private TrackingService tracking;
	}
}
