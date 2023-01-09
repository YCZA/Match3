using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x02000768 RID: 1896
namespace Match3.Scripts1
{
	public class DebugSettingsService : AService
	{
		// Token: 0x06002F0F RID: 12047 RVA: 0x000DBF1E File Offset: 0x000DA31E
		public DebugSettingsService()
		{
			this.Settings = APlayerPrefsObject<DebugSettings>.Load();
			base.OnInitialized.Dispatch();
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002F10 RID: 12048 RVA: 0x000DBF3C File Offset: 0x000DA33C
		// (set) Token: 0x06002F11 RID: 12049 RVA: 0x000DBF44 File Offset: 0x000DA344
		public DebugSettings Settings { get; private set; }
	}
}
