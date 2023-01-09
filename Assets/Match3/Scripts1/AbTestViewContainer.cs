using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.ConfigServiceV2;
using UnityEngine;

// Token: 0x020009AA RID: 2474
namespace Match3.Scripts1
{
	public class AbTestViewContainer : MonoBehaviour
	{
		// Token: 0x06003BFF RID: 15359 RVA: 0x0012A3A0 File Offset: 0x001287A0
		public void Cleanup()
		{
			foreach (AbTestItemView abTestItemView in this.abTestViewsByIndex.Values)
			{
				global::UnityEngine.Object.Destroy(abTestItemView.gameObject);
			}
			this.abTestViewsByIndex.Clear();
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x0012A410 File Offset: 0x00128810
		public void Setup(List<AbTestConfig.AbTest> abTests, int[] chosenGroups)
		{
			this.chosenGroups = chosenGroups;
			this.abTests = abTests;
			if (abTests != null)
			{
				for (int i = 0; i < abTests.Count; i++)
				{
					AbTestItemView abTestItemView = global::UnityEngine.Object.Instantiate<AbTestItemView>(this.abTestPrefabTemplate, this.scrollRectContentParent);
					AbTestConfig.AbTest abTest = abTests[i];
					abTestItemView.Set(abTest.name, abTest.groups[chosenGroups[i]].name, i);
					abTestItemView.OnGroupChanged += this.HandleTestGroupChanged;
					this.abTestViewsByIndex.Add(i, abTestItemView);
				}
			}
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0012A4A4 File Offset: 0x001288A4
		private void HandleTestGroupChanged(int index)
		{
			AbTestConfig.AbTest abTest = this.abTests[index];
			this.chosenGroups[index] = (this.chosenGroups[index] + 1) % abTest.groups.Count;
			AbTestItemView abTestItemView = this.abTestViewsByIndex[index];
			abTestItemView.Set(abTest.name, abTest.groups[this.chosenGroups[index]].name, index);
		}

		// Token: 0x04006411 RID: 25617
		public AbTestItemView abTestPrefabTemplate;

		// Token: 0x04006412 RID: 25618
		private Dictionary<int, AbTestItemView> abTestViewsByIndex = new Dictionary<int, AbTestItemView>();

		// Token: 0x04006413 RID: 25619
		private int[] chosenGroups;

		// Token: 0x04006414 RID: 25620
		private List<AbTestConfig.AbTest> abTests;

		// Token: 0x04006415 RID: 25621
		public Transform scrollRectContentParent;
	}
}
