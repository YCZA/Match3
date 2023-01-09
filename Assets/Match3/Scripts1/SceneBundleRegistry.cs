using UnityEngine;

// Token: 0x02000B61 RID: 2913
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "SceneBundleRegistry", menuName = "Puzzletown/SceneBundleRegistry")]
	public class SceneBundleRegistry : ScriptableObject
	{
		// Token: 0x0600441D RID: 17437 RVA: 0x0015AC2D File Offset: 0x0015902D
		public string GetAssetBundleName(string sceneName)
		{
			if (!this.map.ContainsKey(sceneName))
			{
				WoogaDebug.Log(new object[]
				{
					string.Format("Scene {0} was not in bundle to scene map", sceneName)
				});
				return "none";
			}
			return this.map[sceneName];
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0015AC6B File Offset: 0x0015906B
		public void RemoveSceneEntry(string sceneName)
		{
			if (this.map.ContainsKey(sceneName))
			{
				this.map.Remove(sceneName);
			}
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0015AC8C File Offset: 0x0015908C
		public void AddEnvironmentScene(int environment)
		{
			string sceneName = string.Format(SceneBundleRegistry.TOWN_ENVIRONMENT_SCENE_NAME, environment);
			string bundleName = string.Format(SceneBundleRegistry.TOWN_ENVIRONMENT_BUNDLE_NAME, environment);
			this.AddSceneBundleEntry(sceneName, bundleName);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x0015ACC3 File Offset: 0x001590C3
		public void AddSceneBundleEntry(string sceneName, string bundleName)
		{
			this.map[sceneName] = bundleName;
		}

		// Token: 0x04006C64 RID: 27748
		public SceneToBundleMap map;

		// Token: 0x04006C65 RID: 27749
		private static string TOWN_ENVIRONMENT_SCENE_NAME = "TownEnvironment_{0}";

		// Token: 0x04006C66 RID: 27750
		private static string TOWN_ENVIRONMENT_BUNDLE_NAME = "scene_townenvironment_{0}";
	}
}
