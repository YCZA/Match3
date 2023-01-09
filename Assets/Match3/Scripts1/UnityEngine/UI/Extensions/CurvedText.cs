using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BCA RID: 3018
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Curved Text")]
	public class CurvedText : BaseMeshEffect
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060046D6 RID: 18134 RVA: 0x00168434 File Offset: 0x00166834
		// (set) Token: 0x060046D7 RID: 18135 RVA: 0x0016843C File Offset: 0x0016683C
		public AnimationCurve CurveForText
		{
			get
			{
				return this._curveForText;
			}
			set
			{
				this._curveForText = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060046D8 RID: 18136 RVA: 0x00168450 File Offset: 0x00166850
		// (set) Token: 0x060046D9 RID: 18137 RVA: 0x00168458 File Offset: 0x00166858
		public float CurveMultiplier
		{
			get
			{
				return this._curveMultiplier;
			}
			set
			{
				this._curveMultiplier = value;
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x0016846C File Offset: 0x0016686C
		protected override void Awake()
		{
			base.Awake();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00168486 File Offset: 0x00166886
		protected override void OnEnable()
		{
			base.OnEnable();
			this.rectTrans = base.GetComponent<RectTransform>();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x001684A0 File Offset: 0x001668A0
		public override void ModifyMesh(VertexHelper vh)
		{
			int currentVertCount = vh.currentVertCount;
			if (!this.IsActive() || currentVertCount == 0)
			{
				return;
			}
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				UIVertex vertex = default(UIVertex);
				vh.PopulateUIVertex(ref vertex, i);
				vertex.position.y = vertex.position.y + this._curveForText.Evaluate(this.rectTrans.rect.width * this.rectTrans.pivot.x + vertex.position.x) * this._curveMultiplier;
				vh.SetUIVertex(vertex, i);
			}
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x00168550 File Offset: 0x00166950
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.rectTrans)
			{
				Keyframe key = this._curveForText[this._curveForText.length - 1];
				key.time = this.rectTrans.rect.width;
				this._curveForText.MoveKey(this._curveForText.length - 1, key);
			}
		}

		// Token: 0x04006E0F RID: 28175
		[SerializeField]
		private AnimationCurve _curveForText = AnimationCurve.Linear(0f, 0f, 1f, 10f);

		// Token: 0x04006E10 RID: 28176
		[SerializeField]
		private float _curveMultiplier = 1f;

		// Token: 0x04006E11 RID: 28177
		private RectTransform rectTrans;
	}
}
