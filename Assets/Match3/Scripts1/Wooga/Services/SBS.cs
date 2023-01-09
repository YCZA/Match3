using System;
using JetBrains.Annotations;
using Match3.Scripts1.Wooga.Core.Network;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Core.Utilities.RequestCache;
using Match3.Scripts1.Wooga.Leagues;
using Match3.Scripts1.Wooga.Services.Authentication;
using Match3.Scripts1.Wooga.Services.KeyValueStore;
using Match3.Scripts1.Wooga.Services.Payment;

namespace Match3.Scripts1.Wooga.Services
{
	// Token: 0x020003C0 RID: 960
	public class SBS
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x0007DA6F File Offset: 0x0007BE6F
		// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x0007DA76 File Offset: 0x0007BE76
		public static bool IsInitialized { get; private set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x0007DA7E File Offset: 0x0007BE7E
		// (set) Token: 0x06001CFA RID: 7418 RVA: 0x0007DA85 File Offset: 0x0007BE85
		public static ISbsNetworking Networking { get; private set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x0007DA8D File Offset: 0x0007BE8D
		// (set) Token: 0x06001CFC RID: 7420 RVA: 0x0007DA94 File Offset: 0x0007BE94
		public static SbsAuthentication Authentication { get; private set; }

		// Token: 0x06001CFD RID: 7421 RVA: 0x0007DA9C File Offset: 0x0007BE9C
		public static void Init(ISbsNetworking networking, SbsAuthentication authentication)
		{
			if (SBS.IsInitialized)
			{
				return;
			}
			Unity3D.Init();
			SBS.Networking = networking;
			SBS.Authentication = authentication;
			SBS.IsInitialized = true;
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x0007DAC0 File Offset: 0x0007BEC0
		public static void Init(string sbsGameId)
		{
			SbsNetworkingUnityWebRequest networking = new SbsNetworkingUnityWebRequest(sbsGameId);
			SBS.Init(networking, new SbsAuthentication(networking, sbsGameId));
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x0007DAE2 File Offset: 0x0007BEE2
		[Obsolete("configVersion is not used anymore. Use SBS.Init(sbsGameId) or SBS.Init(ISbsNetworking, SbsAuthentication) instead.")]
		public static void Init(string sbsGameId, string configVersion)
		{
			SBS.Init(sbsGameId);
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x0007DAEA File Offset: 0x0007BEEA
		[UsedImplicitly]
		public static void Dispose()
		{
			SBS.Authentication = null;
			SBS.Networking = null;
			SBS.IsInitialized = false;
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x0007DB00 File Offset: 0x0007BF00
		[UsedImplicitly]
		public static IRequestCache CreateRequestCache(string databaseFileName = "wdkcache.3.db")
		{
			// eli key point 经过服务器认证了才会创建wdkcache.3.db
			return new RequestCache(new SQLiteStorageStrategy(databaseFileName));
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x0007DB1A File Offset: 0x0007BF1A
		public static ISbsKeyValueStore KeyValueStore
		{
			get
			{
				if (SBS._keyValueStore == null)
				{
					SBS._keyValueStore = new SbsKeyValueStore(SBS.Networking, SBS.Authentication, SBS.CreateRequestCache("wdkcache.3.db"));
				}
				return SBS._keyValueStore;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x0007DB49 File Offset: 0x0007BF49
		public static LeagueService LeagueService
		{
			get
			{
				if (SBS._leagueService == null)
				{
					SBS._leagueService = new LeagueService(SBS.Networking, SBS.Authentication);
				}
				return SBS._leagueService;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0007DB6E File Offset: 0x0007BF6E
		public static SbsPaymentValidation PaymentValidation
		{
			get
			{
				if (SBS._paymentValidation == null)
				{
					SBS._paymentValidation = new SbsPaymentValidation(SBS.Networking, SBS.Authentication);
				}
				return SBS._paymentValidation;
			}
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x0007DB93 File Offset: 0x0007BF93
		public static bool IsAuthenticated()
		{
			return SBS.Authentication.IsAuthenticated();
		}

		// Token: 0x040049B9 RID: 18873
		private static ISbsKeyValueStore _keyValueStore;

		// Token: 0x040049BA RID: 18874
		private static LeagueService _leagueService;

		// Token: 0x040049BB RID: 18875
		private static SbsPaymentValidation _paymentValidation;
	}
}
