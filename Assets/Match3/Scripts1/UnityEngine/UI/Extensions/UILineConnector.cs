using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C38 RID: 3128
	[AddComponentMenu("UI/Extensions/UI Line Connector")]
	[RequireComponent(typeof(UILineRenderer))]
	[ExecuteInEditMode]
	public class UILineConnector : MonoBehaviour
	{
		// Token: 0x060049BE RID: 18878 RVA: 0x001794B5 File Offset: 0x001778B5
		private void Awake()
		{
			this.canvas = base.GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
			this.rt = base.GetComponent<RectTransform>();
			this.lr = base.GetComponent<UILineRenderer>();
		}

		// Token: 0x060049BF RID: 18879 RVA: 0x001794E8 File Offset: 0x001778E8
		private void Update()
		{
			if (this.transforms == null || this.transforms.Length < 1)
			{
				return;
			}
			if (this.previousPositions != null && this.previousPositions.Length == this.transforms.Length)
			{
				bool flag = false;
				for (int i = 0; i < this.transforms.Length; i++)
				{
					if (!flag && this.previousPositions[i] != this.transforms[i].anchoredPosition)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			Vector2 pivot = this.rt.pivot;
			Vector2 pivot2 = this.canvas.pivot;
			Vector3[] array = new Vector3[this.transforms.Length];
			Vector3[] array2 = new Vector3[this.transforms.Length];
			Vector2[] array3 = new Vector2[this.transforms.Length];
			for (int j = 0; j < this.transforms.Length; j++)
			{
				array[j] = this.transforms[j].TransformPoint(pivot);
			}
			for (int k = 0; k < this.transforms.Length; k++)
			{
				array2[k] = this.canvas.InverseTransformPoint(array[k]);
			}
			for (int l = 0; l < this.transforms.Length; l++)
			{
				array3[l] = new Vector2(array2[l].x, array2[l].y);
			}
			this.lr.Points = array3;
			this.lr.RelativeSize = false;
			this.lr.drivenExternally = true;
			this.previousPositions = new Vector2[this.transforms.Length];
			for (int m = 0; m < this.transforms.Length; m++)
			{
				this.previousPositions[m] = this.transforms[m].anchoredPosition;
			}
		}

		// Token: 0x04007013 RID: 28691
		public RectTransform[] transforms;

		// Token: 0x04007014 RID: 28692
		private Vector2[] previousPositions;

		// Token: 0x04007015 RID: 28693
		private RectTransform canvas;

		// Token: 0x04007016 RID: 28694
		private RectTransform rt;

		// Token: 0x04007017 RID: 28695
		private UILineRenderer lr;
	}
}
