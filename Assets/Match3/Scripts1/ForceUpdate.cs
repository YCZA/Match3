using System.Collections;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.Flows;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using UnityEngine;

// eli key point: 强制更新
// 另一相关脚本:PopupUpdateRoot, 场景名叫PopupUpdate
namespace Match3.Scripts1
{
	public class ForceUpdate : AFlow
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x00108DB0 File Offset: 0x001071B0
		protected override IEnumerator FlowRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			ForceUpdateConfig config = this.sbs.SbsConfig.force_update;
			Puzzletown.Build.Version newest = new Puzzletown.Build.Version(config.newestVersion);
			Puzzletown.Build.Version required = new Puzzletown.Build.Version(config.requiredVersion);
			// 强制更新
			// if (BuildVersion.IsLowerThan(required))
			// {
			// 	WoogaDebug.Log(new object[]
			// 	{
			// 		"force update"
			// 	});
			// 	Wooroutine<PopupUpdateRoot> popup = SceneManager.Instance.LoadSceneWithParams<PopupUpdateRoot, bool>(true, null);
			// 	yield return popup;
			// 	yield return true;
			// 	yield break;
			// }
			// if (BuildVersion.IsLowerThan(newest))
			// {
			// 	Wooroutine<PopupUpdateRoot> popup2 = SceneManager.Instance.LoadSceneWithParams<PopupUpdateRoot, bool>(false, null);
			// 	yield return popup2;
			// 	yield return popup2.ReturnValue.onClose;
			// }
			yield return false;
		}

		// 有2处调用
		public static void GoToShop()
		{
			Application.OpenURL(GameEnvironment.StoreUrl);
		}

		// Token: 0x04005E7E RID: 24190
		[WaitForService(true, true)]
		private SBSService sbs;
	}
}
