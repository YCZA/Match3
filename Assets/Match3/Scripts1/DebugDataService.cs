using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts2.Env;
using Match3.Scripts2.PlayerData;

// Token: 0x020007A3 RID: 1955
namespace Match3.Scripts1
{
	public class DebugDataService : ADataService
	{
		// Token: 0x06002FE5 RID: 12261 RVA: 0x000E16EA File Offset: 0x000DFAEA
		public DebugDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// eli key point: 是否在关卡中显示作弊菜单
		public bool ShowCheatMenus
		{
			get
			{
				GameEnvironment.Environment currentEnvironment = GameEnvironment.CurrentEnvironment;
				// return (currentEnvironment == SbsEnvironment.Environment.CI || currentEnvironment == SbsEnvironment.Environment.STAGING) && !base.state.debugData.forceHideCheatMenus;
				// return true;
				return false;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000E172E File Offset: 0x000DFB2E
		// (set) Token: 0x06002FE8 RID: 12264 RVA: 0x000E1740 File Offset: 0x000DFB40
		public string ForcedAbTests
		{
			get
			{
				return base.state.debugData.forcedAbTests;
			}
			set
			{
				base.state.debugData.forcedAbTests = value;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000E1753 File Offset: 0x000DFB53
		// (set) Token: 0x06002FEA RID: 12266 RVA: 0x000E1765 File Offset: 0x000DFB65
		public bool ForceHideDebugMode
		{
			get
			{
				return base.state.debugData.forceHideCheatMenus;
			}
			set
			{
				base.state.debugData.forceHideCheatMenus = value;
			}
		}
	}
}
