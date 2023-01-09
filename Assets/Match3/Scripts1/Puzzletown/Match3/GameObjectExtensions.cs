using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000652 RID: 1618
	public static class GameObjectExtensions
	{
		// Token: 0x060028E8 RID: 10472 RVA: 0x000B68A9 File Offset: 0x000B4CA9
		public static void AttachFxToGemView(this MonoBehaviour self, GameObject go)
		{
			go.transform.SetParent(self.transform.parent);
			go.transform.position = self.transform.position;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000B68D7 File Offset: 0x000B4CD7
		public static void AttachFxToGemViewAsChild(this MonoBehaviour self, GameObject go)
		{
			go.transform.SetParent(self.transform);
			go.transform.localPosition = Vector3.zero;
		}
	}
}
