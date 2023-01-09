using System;
using System.Runtime.CompilerServices;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.KeyValueStore;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Legal
{
	// Token: 0x02000B56 RID: 2902
	public static class Terms
	{
		// Token: 0x060043E4 RID: 17380 RVA: 0x0015A688 File Offset: 0x00158A88
		public static bool HasAccepted()
		{
			TermsState state = Terms.GetState();
			return state.accepted_version == TermsLocalisation.CurrentVersion;
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0015A6A8 File Offset: 0x00158AA8
		public static void FetchState()
		{
			if (Terms.HasAccepted())
			{
				return;
			}
			SBS.KeyValueStore.ReadFromBucket("z_consent", null, 5).ContinueWith(delegate(SbsReadResult result)
			{
				if (result.Data != null && result.Data.FormatVersion == 1)
				{
					TermsState state = TermsState.FromJsonOrDefault(result.Data.Data);
					if (!Terms.HasAccepted())
					{
						Terms.SetState(state);
					}
				}
			}).Catch(delegate(Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex,
					"LegalTerms: failed reading Terms bucket"
				});
			}).Start();
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0015A71B File Offset: 0x00158B1B
		public static void OnAccepted(bool syncToSbs)
		{
			Terms.SetState(new TermsState(TermsLocalisation.CurrentVersion));
			TermsLocalisation.Unload();
			if (syncToSbs)
			{
				Terms.SyncStateToSbs();
			}
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x0015A73C File Offset: 0x00158B3C
		public static void DebugReset()
		{
			Terms.SetState(new TermsState(0L));
			Terms.SyncStateToSbs();
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0015A74F File Offset: 0x00158B4F
		private static void SetState(TermsState state)
		{
			PlayerPrefs.SetString("Wooga.Legal.z_consent", state.ToJson());
			PlayerPrefs.Save();
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0015A768 File Offset: 0x00158B68
		private static TermsState GetState()
		{
			string @string = PlayerPrefs.GetString("Wooga.Legal.z_consent");
			return TermsState.FromJsonOrDefault(@string);
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0015A788 File Offset: 0x00158B88
		private static void SyncStateToSbs()
		{
			ISbsKeyValueStore keyValueStore = SBS.KeyValueStore;
			string bucket = "z_consent";
			string json = Terms.GetState().ToJson();
			int formatVersion = 1;
			if (Terms._003C_003Ef__mg_0024cache0 == null)
			{
				Terms._003C_003Ef__mg_0024cache0 = new SbsMergeHandler(TermsState.MergeHandler);
			}
			keyValueStore.WriteJsonToBucket(bucket, json, formatVersion, Terms._003C_003Ef__mg_0024cache0, null).Catch(delegate(Exception ex)
			{
				WoogaDebug.Log(new object[]
				{
					ex,
					"LegalTerms: failed writing Terms bucket"
				});
			}).Start();
		}

		// Token: 0x04006C3B RID: 27707
		private const string PREFIX = "Wooga.Legal.";

		// Token: 0x04006C3E RID: 27710
		[CompilerGenerated]
		private static SbsMergeHandler _003C_003Ef__mg_0024cache0;
	}
}
