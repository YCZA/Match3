using System.Collections;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Building.Shop;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7E RID: 2686
	[CreateAssetMenu(fileName = "ScriptTutorialOpenDecoStorage", menuName = "Puzzletown/Tutorials/Create/TutorialOpenDecoStorage")]
	public class TutorialOpenDecoStorage : ATutorialScript
	{
		// Token: 0x06004029 RID: 16425 RVA: 0x00149F10 File Offset: 0x00148310
		protected override IEnumerator ExecuteRoutine()
		{
			Wooroutine<TownShopRoot> shopScene = SceneManager.Instance.LoadScene<TownShopRoot>(null);
			yield return shopScene;
			shopScene.ReturnValue.Open(ShopTag.Storage);
			yield break;
		}
	}
}
