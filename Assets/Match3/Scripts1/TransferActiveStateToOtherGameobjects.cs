using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
namespace Match3.Scripts1
{
	public class TransferActiveStateToOtherGameobjects : MonoBehaviour
	{
		// Token: 0x060044D0 RID: 17616 RVA: 0x0015D7F4 File Offset: 0x0015BBF4
		private void OnEnable()
		{
			foreach (GameObject gameObject in this.gameObjects)
			{
				gameObject.SetActive(true);
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0015D850 File Offset: 0x0015BC50
		private void OnDisable()
		{
			foreach (GameObject gameObject in this.gameObjects)
			{
				gameObject.SetActive(false);
			}
		}

		// Token: 0x04006C95 RID: 27797
		[SerializeField]
		private List<GameObject> gameObjects = new List<GameObject>();
	}
}
