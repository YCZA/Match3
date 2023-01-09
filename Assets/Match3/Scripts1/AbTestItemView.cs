using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009A9 RID: 2473
namespace Match3.Scripts1
{
	public class AbTestItemView : MonoBehaviour
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06003BF9 RID: 15353 RVA: 0x0012A2E8 File Offset: 0x001286E8
		// (remove) Token: 0x06003BFA RID: 15354 RVA: 0x0012A320 File Offset: 0x00128720
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> OnGroupChanged;

		// Token: 0x06003BFB RID: 15355 RVA: 0x0012A356 File Offset: 0x00128756
		private void OnValidate()
		{
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0012A358 File Offset: 0x00128758
		public void Set(string testName, string grp, int index)
		{
			this.index = index;
			this.testNameLabel.text = testName;
			this.testGroupLabel.text = grp;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0012A379 File Offset: 0x00128779
		public void OnButtonTap()
		{
			this.OnGroupChanged(this.index);
		}

		// Token: 0x0400640C RID: 25612
		public Text testNameLabel;

		// Token: 0x0400640D RID: 25613
		public Button testGroupCycleThroughButton;

		// Token: 0x0400640E RID: 25614
		public Text testGroupLabel;

		// Token: 0x0400640F RID: 25615
		private int index;
	}
}
