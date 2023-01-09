using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B0 RID: 1712
	public class GemViewFactory : MonoBehaviour
	{
		// eli key point 生成GemView
		public GemView Create(Gem gem, Transform fieldView)
		{
			// Debug.LogError("create gem view:" + gem.modifier);
			if (gem.color == GemColor.Undefined)
			{
				return null;
			}
			GemView component = this.pool.Get(this.prefabGem.gameObject).GetComponent<GemView>();
			component.transform.position = fieldView.position;
			component.transform.SetParent(base.transform);
			component.transform.localScale = fieldView.localScale;
			component.transform.position = fieldView.position;
			component.objectPool = this.pool;
			component.Show(gem);
			return component;
		}

		// Token: 0x0400541A RID: 21530
		[SerializeField]
		private GemView prefabGem;

		// Token: 0x0400541B RID: 21531
		[SerializeField]
		private ObjectPool pool;
	}
}
