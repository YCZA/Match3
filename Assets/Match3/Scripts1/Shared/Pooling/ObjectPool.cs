using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Pooling
{
	// Token: 0x02000B0D RID: 2829
	public class ObjectPool : MonoBehaviour
	{
		// Token: 0x060042AF RID: 17071 RVA: 0x00155CD4 File Offset: 0x001540D4
		public void Release(GameObject go)
		{
			PoolMarker component = go.GetComponent<PoolMarker>();
			int prefabId = component.prefabId;
			if (!this.pools.ContainsKey(prefabId))
			{
				this.pools[prefabId] = new List<GameObject>();
			}
			this.pools[prefabId].Add(go);
			component.onReleased.Dispatch();
			go.SetActive(false);
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x00155D35 File Offset: 0x00154135
		public void Release(GameObject go, float delay)
		{
			base.StartCoroutine(this.ReleaseRoutine(go, delay));
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00155D48 File Offset: 0x00154148
		public GameObject Get(GameObject prefab)
		{
			int instanceID = prefab.GetInstanceID();
			GameObject gameObject;
			if (this.HasInstance(instanceID))
			{
				gameObject = this.pools[instanceID][0];
				this.pools[instanceID].RemoveAt(0);
			}
			else
			{
				gameObject = this.CreateInstance(prefab);
			}
			gameObject.SetActive(true);
			return gameObject;
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00155DA4 File Offset: 0x001541A4
		public bool HasInstance(GameObject prefab)
		{
			return this.HasInstance(prefab.GetInstanceID());
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00155DB4 File Offset: 0x001541B4
		private void Start()
		{
			foreach (PoolEntry poolEntry in this.preloadEntries)
			{
				for (int i = 0; i < poolEntry.numOfPreloadedInstances; i++)
				{
					this.CreateInstance(poolEntry.prefab).Release();
				}
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x00155E34 File Offset: 0x00154234
		private IEnumerator ReleaseRoutine(GameObject go, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.Release(go);
			yield break;
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x00155E5D File Offset: 0x0015425D
		private bool HasInstance(int prefabId)
		{
			return this.pools.ContainsKey(prefabId) && this.pools[prefabId].Count > 0;
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00155E88 File Offset: 0x00154288
		private GameObject CreateInstance(GameObject prefab)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(prefab);
			PoolMarker poolMarker = gameObject.AddComponent<PoolMarker>();
			poolMarker.prefabId = prefab.GetInstanceID();
			poolMarker.pool = this;
			gameObject.transform.SetParent(base.transform, false);
			return gameObject;
		}

		// Token: 0x04006B8A RID: 27530
		[SerializeField]
		private List<PoolEntry> preloadEntries;

		// Token: 0x04006B8B RID: 27531
		private Dictionary<int, List<GameObject>> pools = new Dictionary<int, List<GameObject>>();
	}
}
