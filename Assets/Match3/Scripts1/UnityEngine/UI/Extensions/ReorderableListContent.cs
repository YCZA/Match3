using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BAD RID: 2989
	public class ReorderableListContent : MonoBehaviour
	{
		// Token: 0x06004603 RID: 17923 RVA: 0x001628AB File Offset: 0x00160CAB
		private void OnEnable()
		{
			if (this._rect)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x001628CA File Offset: 0x00160CCA
		public void OnTransformChildrenChanged()
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x001628E4 File Offset: 0x00160CE4
		public void Init(ReorderableList extList)
		{
			this._extList = extList;
			this._rect = base.GetComponent<RectTransform>();
			this._cachedChildren = new List<Transform>();
			this._cachedListElement = new List<ReorderableListElement>();
			base.StartCoroutine(this.RefreshChildren());
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x0016291C File Offset: 0x00160D1C
		private IEnumerator RefreshChildren()
		{
			for (int i = 0; i < this._rect.childCount; i++)
			{
				if (!this._cachedChildren.Contains(this._rect.GetChild(i)))
				{
					this._ele = (this._rect.GetChild(i).gameObject.GetComponent<ReorderableListElement>() ?? this._rect.GetChild(i).gameObject.AddComponent<ReorderableListElement>());
					this._ele.Init(this._extList);
					this._cachedChildren.Add(this._rect.GetChild(i));
					this._cachedListElement.Add(this._ele);
				}
			}
			yield return 0;
			for (int j = this._cachedChildren.Count - 1; j >= 0; j--)
			{
				if (this._cachedChildren[j] == null)
				{
					this._cachedChildren.RemoveAt(j);
					this._cachedListElement.RemoveAt(j);
				}
			}
			yield break;
		}

		// Token: 0x04006D82 RID: 28034
		private List<Transform> _cachedChildren;

		// Token: 0x04006D83 RID: 28035
		private List<ReorderableListElement> _cachedListElement;

		// Token: 0x04006D84 RID: 28036
		private ReorderableListElement _ele;

		// Token: 0x04006D85 RID: 28037
		private ReorderableList _extList;

		// Token: 0x04006D86 RID: 28038
		private RectTransform _rect;
	}
}
