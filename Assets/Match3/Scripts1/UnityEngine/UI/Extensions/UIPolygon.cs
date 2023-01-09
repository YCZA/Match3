using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C0D RID: 3085
	[AddComponentMenu("UI/Extensions/Primitives/UI Polygon")]
	public class UIPolygon : UIPrimitiveBase
	{
		// Token: 0x060048A6 RID: 18598 RVA: 0x00174020 File Offset: 0x00172420
		public void DrawPolygon(int _sides)
		{
			this.sides = _sides;
			this.VerticesDistances = new float[_sides + 1];
			for (int i = 0; i < _sides; i++)
			{
				this.VerticesDistances[i] = 1f;
			}
			this.rotation = 0f;
			this.SetAllDirty();
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x00174072 File Offset: 0x00172472
		public void DrawPolygon(int _sides, float[] _VerticesDistances)
		{
			this.sides = _sides;
			this.VerticesDistances = _VerticesDistances;
			this.rotation = 0f;
			this.SetAllDirty();
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00174093 File Offset: 0x00172493
		public void DrawPolygon(int _sides, float[] _VerticesDistances, float _rotation)
		{
			this.sides = _sides;
			this.VerticesDistances = _VerticesDistances;
			this.rotation = _rotation;
			this.SetAllDirty();
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x001740B0 File Offset: 0x001724B0
		private void Update()
		{
			this.size = base.rectTransform.rect.width;
			if (base.rectTransform.rect.width > base.rectTransform.rect.height)
			{
				this.size = base.rectTransform.rect.height;
			}
			else
			{
				this.size = base.rectTransform.rect.width;
			}
			this.thickness = Mathf.Clamp(this.thickness, 0f, this.size / 2f);
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0017415C File Offset: 0x0017255C
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			Vector2 vector3 = new Vector2(0f, 0f);
			Vector2 vector4 = new Vector2(0f, 1f);
			Vector2 vector5 = new Vector2(1f, 1f);
			Vector2 vector6 = new Vector2(1f, 0f);
			float num = 360f / (float)this.sides;
			int num2 = this.sides + 1;
			if (this.VerticesDistances.Length != num2)
			{
				this.VerticesDistances = new float[num2];
				for (int i = 0; i < num2 - 1; i++)
				{
					this.VerticesDistances[i] = 1f;
				}
			}
			this.VerticesDistances[num2 - 1] = this.VerticesDistances[0];
			for (int j = 0; j < num2; j++)
			{
				float num3 = -base.rectTransform.pivot.x * this.size * this.VerticesDistances[j];
				float num4 = -base.rectTransform.pivot.x * this.size * this.VerticesDistances[j] + this.thickness;
				float f = 0.017453292f * ((float)j * num + this.rotation);
				float num5 = Mathf.Cos(f);
				float num6 = Mathf.Sin(f);
				vector3 = new Vector2(0f, 1f);
				vector4 = new Vector2(1f, 1f);
				vector5 = new Vector2(1f, 0f);
				vector6 = new Vector2(0f, 0f);
				Vector2 vector7 = vector;
				Vector2 vector8 = new Vector2(num3 * num5, num3 * num6);
				Vector2 zero;
				Vector2 vector9;
				if (this.fill)
				{
					zero = Vector2.zero;
					vector9 = Vector2.zero;
				}
				else
				{
					zero = new Vector2(num4 * num5, num4 * num6);
					vector9 = vector2;
				}
				vector = vector8;
				vector2 = zero;
				vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
				{
					vector7,
					vector8,
					zero,
					vector9
				}, new Vector2[]
				{
					vector3,
					vector4,
					vector5,
					vector6
				}));
			}
		}

		// Token: 0x04006F5A RID: 28506
		public bool fill = true;

		// Token: 0x04006F5B RID: 28507
		public float thickness = 5f;

		// Token: 0x04006F5C RID: 28508
		[Range(3f, 360f)]
		public int sides = 3;

		// Token: 0x04006F5D RID: 28509
		[Range(0f, 360f)]
		public float rotation;

		// Token: 0x04006F5E RID: 28510
		[Range(0f, 1f)]
		public float[] VerticesDistances = new float[3];

		// Token: 0x04006F5F RID: 28511
		private float size;
	}
}
