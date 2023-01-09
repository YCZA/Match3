using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x020009A2 RID: 2466
namespace Match3.Scripts1
{
	public class VillagerSpawnConfiguration : MonoBehaviour, IEditorDescription
	{
		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x00129424 File Offset: 0x00127824
		public DedicatedPosition[] SpawnPoints
		{
			get
			{
				return base.GetComponentsInChildren<DedicatedPosition>();
			}
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0012942C File Offset: 0x0012782C
		protected void OnDrawGizmos()
		{
			Vector3 center = Vector3.up * 0.33f * 0.5f;
			Gizmos.color = new Color(1f, 0f, 1f, 0.5f);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawSphere(center, 0.33f);
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0012948C File Offset: 0x0012788C
		public string GetEditorDescription()
		{
			int num = base.GetComponentsInChildren<DedicatedPosition>().Length;
			if (num != 1)
			{
				return string.Format("{0} Spawn Points", base.GetComponentsInChildren<DedicatedPosition>().Length);
			}
			return "1 Spawn Point";
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x001294C6 File Offset: 0x001278C6
		public void SetLocation(Vector3 location, bool storyMode)
		{
			base.transform.position = location;
			base.transform.localScale = ((!storyMode) ? (Vector3.one * 0.5f) : Vector3.one);
		}

		// Token: 0x040063F1 RID: 25585
		public const float NON_STORY_SCALE = 0.5f;
	}
}
