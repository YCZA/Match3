using System.Collections;
using Match3.Scripts1.Localization;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x020007DC RID: 2012
namespace Match3.Scripts1
{
	public class PTLocalizationService : LocalizationService
	{
		// Token: 0x060031A1 RID: 12705 RVA: 0x000E95C4 File Offset: 0x000E79C4
		public PTLocalizationService(GameSettingsService gameSettingsService) : base(gameSettingsService)
		{
			this.Load();
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000E95D4 File Offset: 0x000E79D4
		public Coroutine Load()
		{
			if (this._loadRoutine == null)
			{
				this._loadRoutine = WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
			}
			return this._loadRoutine;
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x000E95FC File Offset: 0x000E79FC
		private IEnumerator LoadRoutine()
		{
			base.ChangeLanguage(this.DetermineLanguage());
			yield return ServiceLocator.Instance.Inject(this);
			base.SetGlobalReplaceKeys(this.gameState.GlobalReplaceKeys);
			base.OnInitialized.Dispatch();
			this._loadRoutine = null;
			yield break;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x000E9618 File Offset: 0x000E7A18
		public WoogaSystemLanguage DetermineLanguage()
		{
			WoogaSystemLanguage woogaSystemLanguage;
			if (this._gameSettingsService.Language == WoogaSystemLanguage.LocaKeys)
			{
				woogaSystemLanguage = WoogaSystemLanguageExtensions.ApplicationLanguage();
			}
			else
			{
				woogaSystemLanguage = this._gameSettingsService.Language;
			}
			if (this.IsLanguageAvailable(woogaSystemLanguage))
			{
				return woogaSystemLanguage;
			}
			WoogaSystemLanguage woogaSystemLanguage2 = WoogaSystemLanguageExtensions.ApplicationLanguage();
			if (this.IsLanguageAvailable(woogaSystemLanguage2))
			{
				return woogaSystemLanguage2;
			}
			return WoogaSystemLanguage.English;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x000E9674 File Offset: 0x000E7A74
		public static string ToCrowdinCode(WoogaSystemLanguage lang)
		{
			switch (lang)
			{
				case WoogaSystemLanguage.Italian:
					return "it";
				case WoogaSystemLanguage.Japanese:
					return "ja";
				case WoogaSystemLanguage.Korean:
					return "ko";
				default:
					switch (lang)
					{
						case WoogaSystemLanguage.Chinese:
							return "zh-TW";
						default:
							if (lang != WoogaSystemLanguage.SimplifiedChinese)
							{
								return "en";
							}
							return "zh-CN";
						case WoogaSystemLanguage.Danish:
							return "da";
						case WoogaSystemLanguage.Dutch:
							return "nl";
						case WoogaSystemLanguage.English:
							return "en";
						case WoogaSystemLanguage.French:
							return "fr";
						case WoogaSystemLanguage.German:
							return "de";
					}
					break;
				case WoogaSystemLanguage.Norwegian:
					return "nb";
				case WoogaSystemLanguage.Portuguese:
					return "pt-BR";
				case WoogaSystemLanguage.Russian:
					return "ru";
				case WoogaSystemLanguage.Spanish:
					return "es-ES";
				case WoogaSystemLanguage.Swedish:
					return "sv-SE";
			}
		}

		// Token: 0x04005A21 RID: 23073
		[WaitForService(true, true)]
		protected GameStateService gameState;

		// Token: 0x04005A22 RID: 23074
		private Coroutine _loadRoutine;
	}
}
