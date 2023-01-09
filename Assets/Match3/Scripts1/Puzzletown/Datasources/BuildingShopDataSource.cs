using System.Collections;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Datasources
{
	// Token: 0x02000A2E RID: 2606
	public class BuildingShopDataSource : ArrayDataSource<BuildingShopView, BuildingShopData>
	{
		// Token: 0x06003EAA RID: 16042 RVA: 0x0013EC24 File Offset: 0x0013D024
		protected override void Awake()
		{
			base.Awake();
			base.StartCoroutine(this.Load());
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x0013EC3C File Offset: 0x0013D03C
		private IEnumerator Load()
		{
			yield return SceneManager.Instance.Inject(this);
			yield return ServiceLocator.Instance.Inject(this);
			yield break;
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x0013EC57 File Offset: 0x0013D057
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.GetDataForIndex(index).state;
		}
	}
}
