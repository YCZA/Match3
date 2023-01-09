using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200079D RID: 1949
	public class GameSettingsService : AService
	{
		// Token: 0x06002FC5 RID: 12229 RVA: 0x000E0E92 File Offset: 0x000DF292
		public GameSettingsService()
		{
			this.Load();
			base.OnInitialized.Dispatch();
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000E0EB6 File Offset: 0x000DF2B6
		// (set) Token: 0x06002FC7 RID: 12231 RVA: 0x000E0EC3 File Offset: 0x000DF2C3
		public WoogaSystemLanguage Language
		{
			get
			{
				return this._currentSettings.CurrentLanguage;
			}
			set
			{
				this._currentSettings.CurrentLanguage = value;
				this.Save();
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000E0ED7 File Offset: 0x000DF2D7
		private string GetKey<T>(T setting)
		{
			return string.Format("{0}_{1}", "pt_settings_key", setting);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000E0EEE File Offset: 0x000DF2EE
		public bool HasToggle(ToggleSetting setting)
		{
			return PlayerPrefs.HasKey(this.GetKey<ToggleSetting>(setting));
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000E0EFC File Offset: 0x000DF2FC
		public bool GetToggle(ToggleSetting setting)
		{
			return PlayerPrefs.GetInt(this.GetKey<ToggleSetting>(setting), 1) > 0;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000E0F0E File Offset: 0x000DF30E
		public void SetToggle(ToggleSetting setting, bool value)
		{
			PlayerPrefs.SetInt(this.GetKey<ToggleSetting>(setting), (!value) ? 0 : 1);
			this.Save();
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x000E0F30 File Offset: 0x000DF330
		protected void Load()
		{
			string @string = PlayerPrefs.GetString("pt_settings_key", "{}");
			this._currentSettings = GamesSettingsData.SettingsFromString(@string);
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000E0F5C File Offset: 0x000DF35C
		public void Save()
		{
			string value = GamesSettingsData.SettingsToString(this._currentSettings);
			PlayerPrefs.SetString("pt_settings_key", value);
			PlayerPrefs.Save();
		}

		// Token: 0x040058ED RID: 22765
		public const string PLAYER_PREFS_KEY = "pt_settings_key";

		// Token: 0x040058EE RID: 22766
		private GamesSettingsData _currentSettings = new GamesSettingsData();
	}
}
