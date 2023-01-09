using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

// Token: 0x0200091A RID: 2330
namespace Match3.Scripts1
{
	public class DooberTest : MonoBehaviour
	{
		// Token: 0x060038C7 RID: 14535 RVA: 0x00117B04 File Offset: 0x00115F04
		public IEnumerator Start()
		{
			yield return ServiceLocator.Instance.Inject(this);
			yield return SceneManager.Instance.Inject(this);
			yield break;
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x00117B20 File Offset: 0x00115F20
		public void Spawn()
		{
			DoobersRoot componentInParent = base.GetComponentInParent<DoobersRoot>();
			if (this.to)
			{
				componentInParent.SpawnDoobers(new MaterialAmount(this.type, this.howMany, MaterialAmountUsage.Undefined, 0), this.from, this.to, null);
			}
			else
			{
				this.gameStateService.Resources.AddMaterial(this.type, this.howMany, true);
				this.townResourcePanelRoot.CollectMaterials(new MaterialAmount(this.type, this.howMany, MaterialAmountUsage.Undefined, 0), this.from, true);
			}
		}

		// Token: 0x04006127 RID: 24871
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006128 RID: 24872
		[WaitForRoot(false, false)]
		private TownResourcePanelRoot townResourcePanelRoot;

		// Token: 0x04006129 RID: 24873
		public Transform from;

		// Token: 0x0400612A RID: 24874
		public Transform to;

		// Token: 0x0400612B RID: 24875
		public string type;

		// Token: 0x0400612C RID: 24876
		public int howMany;
	}
}
