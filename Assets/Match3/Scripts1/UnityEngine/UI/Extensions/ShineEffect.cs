using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDC RID: 3036
	public class ShineEffect : MaskableGraphic
	{
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600472C RID: 18220 RVA: 0x0016A78A File Offset: 0x00168B8A
		// (set) Token: 0x0600472D RID: 18221 RVA: 0x0016A792 File Offset: 0x00168B92
		public float Yoffset
		{
			get
			{
				return this.yoffset;
			}
			set
			{
				this.SetVerticesDirty();
				this.yoffset = value;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x0600472E RID: 18222 RVA: 0x0016A7A1 File Offset: 0x00168BA1
		// (set) Token: 0x0600472F RID: 18223 RVA: 0x0016A7A9 File Offset: 0x00168BA9
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.SetAllDirty();
				this.width = value;
			}
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0016A7B8 File Offset: 0x00168BB8
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
			float num = (vector.w - vector.y) * 2f;
			Color32 color = this.color;
			vh.Clear();
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(0f, 0f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.y + this.yoffset * num), color, new Vector2(1f, 0f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.y / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			vh.AddVert(new Vector3(vector.x - 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * (vector.w / 4f) + this.yoffset * num), color, new Vector2(1f, 1f));
			color.a = (byte)(this.color.a * 255f);
			color.a = 0;
			vh.AddVert(new Vector3(vector.x - 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(0f, 1f));
			vh.AddVert(new Vector3(vector.z + 50f, this.width * vector.w + this.yoffset * num), color, new Vector2(1f, 1f));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 1);
			vh.AddTriangle(2, 3, 4);
			vh.AddTriangle(4, 5, 3);
			vh.AddTriangle(4, 5, 6);
			vh.AddTriangle(6, 7, 5);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0016AAD4 File Offset: 0x00168ED4
		public void Triangulate(VertexHelper vh)
		{
			int num = vh.currentVertCount - 2;
			global::UnityEngine.Debug.Log(num);
			for (int i = 0; i <= num / 2 + 1; i += 2)
			{
				vh.AddTriangle(i, i + 1, i + 2);
				vh.AddTriangle(i + 2, i + 3, i + 1);
			}
		}

		// Token: 0x04006E45 RID: 28229
		[SerializeField]
		private float yoffset = -1f;

		// Token: 0x04006E46 RID: 28230
		[SerializeField]
		private float width = 1f;
	}
}
