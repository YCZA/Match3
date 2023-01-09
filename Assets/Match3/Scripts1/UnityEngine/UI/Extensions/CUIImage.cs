using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BC7 RID: 3015
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Image")]
	public class CUIImage : CUIGraphic
	{
		// Token: 0x060046CB RID: 18123 RVA: 0x00167E44 File Offset: 0x00166244
		public static int ImageTypeCornerRefVertexIdx(Image.Type _type)
		{
			if (_type == Image.Type.Sliced)
			{
				return CUIImage.SlicedImageCornerRefVertexIdx;
			}
			return CUIImage.FilledImageCornerRefVertexIdx;
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x00167E58 File Offset: 0x00166258
		public Vector2 OriCornerPosRatio
		{
			get
			{
				return this.oriCornerPosRatio;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x00167E60 File Offset: 0x00166260
		public Image UIImage
		{
			get
			{
				return (Image)this.uiGraphic;
			}
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x00167E6D File Offset: 0x0016626D
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Image>();
			}
			base.ReportSet();
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x00167E94 File Offset: 0x00166294
		protected override void modifyVertices(List<UIVertex> _verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.UIImage.type == Image.Type.Filled)
			{
				global::UnityEngine.Debug.LogWarning("Might not work well Radial Filled at the moment!");
			}
			else if (this.UIImage.type == Image.Type.Sliced || this.UIImage.type == Image.Type.Tiled)
			{
				if (this.cornerPosRatio == Vector2.one * -1f)
				{
					this.cornerPosRatio = _verts[CUIImage.ImageTypeCornerRefVertexIdx(this.UIImage.type)].position;
					this.cornerPosRatio.x = (this.cornerPosRatio.x + this.rectTrans.pivot.x * this.rectTrans.rect.width) / this.rectTrans.rect.width;
					this.cornerPosRatio.y = (this.cornerPosRatio.y + this.rectTrans.pivot.y * this.rectTrans.rect.height) / this.rectTrans.rect.height;
					this.oriCornerPosRatio = this.cornerPosRatio;
				}
				if (this.cornerPosRatio.x < 0f)
				{
					this.cornerPosRatio.x = 0f;
				}
				if (this.cornerPosRatio.x >= 0.5f)
				{
					this.cornerPosRatio.x = 0.5f;
				}
				if (this.cornerPosRatio.y < 0f)
				{
					this.cornerPosRatio.y = 0f;
				}
				if (this.cornerPosRatio.y >= 0.5f)
				{
					this.cornerPosRatio.y = 0.5f;
				}
				for (int i = 0; i < _verts.Count; i++)
				{
					UIVertex value = _verts[i];
					float num = (value.position.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
					float num2 = (value.position.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
					if (num < this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(0f, this.cornerPosRatio.x, num / this.oriCornerPosRatio.x);
					}
					else if (num > 1f - this.oriCornerPosRatio.x)
					{
						num = Mathf.Lerp(1f - this.cornerPosRatio.x, 1f, (num - (1f - this.oriCornerPosRatio.x)) / this.oriCornerPosRatio.x);
					}
					else
					{
						num = Mathf.Lerp(this.cornerPosRatio.x, 1f - this.cornerPosRatio.x, (num - this.oriCornerPosRatio.x) / (1f - this.oriCornerPosRatio.x * 2f));
					}
					if (num2 < this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(0f, this.cornerPosRatio.y, num2 / this.oriCornerPosRatio.y);
					}
					else if (num2 > 1f - this.oriCornerPosRatio.y)
					{
						num2 = Mathf.Lerp(1f - this.cornerPosRatio.y, 1f, (num2 - (1f - this.oriCornerPosRatio.y)) / this.oriCornerPosRatio.y);
					}
					else
					{
						num2 = Mathf.Lerp(this.cornerPosRatio.y, 1f - this.cornerPosRatio.y, (num2 - this.oriCornerPosRatio.y) / (1f - this.oriCornerPosRatio.y * 2f));
					}
					value.position.x = num * this.rectTrans.rect.width - this.rectTrans.rect.width * this.rectTrans.pivot.x;
					value.position.y = num2 * this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
					_verts[i] = value;
				}
			}
			base.modifyVertices(_verts);
		}

		// Token: 0x04006E0A RID: 28170
		public static int SlicedImageCornerRefVertexIdx = 2;

		// Token: 0x04006E0B RID: 28171
		public static int FilledImageCornerRefVertexIdx;

		// Token: 0x04006E0C RID: 28172
		[Tooltip("For changing the size of the corner for tiled or sliced Image")]
		[HideInInspector]
		[SerializeField]
		public Vector2 cornerPosRatio = Vector2.one * -1f;

		// Token: 0x04006E0D RID: 28173
		[HideInInspector]
		[SerializeField]
		protected Vector2 oriCornerPosRatio = Vector2.one * -1f;
	}
}
