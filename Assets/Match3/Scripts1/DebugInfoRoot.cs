using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

// Token: 0x02000845 RID: 2117
namespace Match3.Scripts1
{
	public class DebugInfoRoot : APtSceneRoot
	{
		// Token: 0x06003477 RID: 13431 RVA: 0x000FB18D File Offset: 0x000F958D
		protected override void Go()
		{
			if (this.state.Debug.ForceHideDebugMode)
			{
				base.Destroy();
				return;
			}
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.GetComponentInChildren<BuildDisplay>().RefreshStatic();
		}

		// Token: 0x04005C6B RID: 23659
		[WaitForService(true, true)]
		private GameStateService state;
	}
}
