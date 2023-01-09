using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009B0 RID: 2480
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Button))]
	public class CheatMenuTrigger : MonoBehaviour
	{
		// Token: 0x06003C14 RID: 15380 RVA: 0x0012A9B8 File Offset: 0x00128DB8
		private void Start()
		{
			base.GetComponent<Button>().image.raycastTarget = false;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0012A9CB File Offset: 0x00128DCB
		private void HandleButton()
		{
			this.count++;
			if (this.count >= this.numClicksRequired)
			{
				this.count = 0;
				base.StartCoroutine(this.ActivateCheatMenus());
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0012AA00 File Offset: 0x00128E00
		private IEnumerator ActivateCheatMenus()
		{
			yield return ServiceLocator.Instance.Inject(this);
			if (this.state.Debug.ForceHideDebugMode)
			{
				this.state.Debug.ForceHideDebugMode = false;
				SceneManager.Instance.LoadSceneWithParams<TownCheatsRoot, TownSceneLoader>(null, null);
				SceneManager.Instance.LoadScene<DebugInfoRoot>(null);
				this.state.Debug.ForceHideDebugMode = false;
				this.state.Save(true);
			}
			yield break;
		}

		// Token: 0x04006425 RID: 25637
		[WaitForService(true, true)]
		private GameStateService state;

		// Token: 0x04006426 RID: 25638
		public int numClicksRequired = 5;

		// Token: 0x04006427 RID: 25639
		private int count;
	}
}
