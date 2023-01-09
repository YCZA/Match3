using UnityEngine;

// Token: 0x020004DC RID: 1244
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	public class ParticlesSortingLayer : MonoBehaviour
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x0009944A File Offset: 0x0009784A
		// (set) Token: 0x060022A0 RID: 8864 RVA: 0x00099452 File Offset: 0x00097852
		public string SortingLayerName
		{
			get
			{
				return this._sortingLayerName;
			}
			set
			{
				this._sortingLayerName = value;
				this.myRenderer.sortingLayerName = this._sortingLayerName;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x0009946C File Offset: 0x0009786C
		// (set) Token: 0x060022A2 RID: 8866 RVA: 0x00099474 File Offset: 0x00097874
		public int SortingOrder
		{
			get
			{
				return this._sortingOrder;
			}
			set
			{
				this._sortingOrder = value;
				this.myRenderer.sortingOrder = this._sortingOrder;
			}
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0009948E File Offset: 0x0009788E
		private void Start()
		{
			this.myRenderer = base.GetComponent<Renderer>();
			this.SortingLayerName = this._sortingLayerName;
			this.SortingOrder = this._sortingOrder;
		}

		// Token: 0x04004E35 RID: 20021
		[SerializeField]
		private string _sortingLayerName = "Default";

		// Token: 0x04004E36 RID: 20022
		[SerializeField]
		private int _sortingOrder;

		// Token: 0x04004E37 RID: 20023
		private Renderer myRenderer;
	}
}
