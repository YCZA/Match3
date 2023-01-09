using Match3.Scripts1.Localization;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x020007D9 RID: 2009
namespace Match3.Scripts1
{
	public class LocalizationEditorService : LocalizationService
	{
		// Token: 0x0600318D RID: 12685 RVA: 0x000E9579 File Offset: 0x000E7979
		public LocalizationEditorService(GameSettingsService gameSettingsService) : base(gameSettingsService)
		{
			base.ChangeLanguage(WoogaSystemLanguage.English);
			base.OnInitialized.Dispatch();
		}
	}
}
