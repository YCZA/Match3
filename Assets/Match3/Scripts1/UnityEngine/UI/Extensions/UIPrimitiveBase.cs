using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C0F RID: 3087
	public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
	{
		// Token: 0x060048AB RID: 18603 RVA: 0x001713B5 File Offset: 0x0016F7B5
		protected UIPrimitiveBase()
		{
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x060048AC RID: 18604 RVA: 0x001713CF File Offset: 0x0016F7CF
		// (set) Token: 0x060048AD RID: 18605 RVA: 0x001713D7 File Offset: 0x0016F7D7
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x060048AE RID: 18606 RVA: 0x001713F6 File Offset: 0x0016F7F6
		// (set) Token: 0x060048AF RID: 18607 RVA: 0x001713FE File Offset: 0x0016F7FE
		public Sprite overrideSprite
		{
			get
			{
				return this.activeSprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
				{
					this.GeneratedUVs();
				}
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060048B0 RID: 18608 RVA: 0x0017141D File Offset: 0x0016F81D
		protected Sprite activeSprite
		{
			get
			{
				return (!(this.m_OverrideSprite != null)) ? this.sprite : this.m_OverrideSprite;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060048B1 RID: 18609 RVA: 0x00171441 File Offset: 0x0016F841
		// (set) Token: 0x060048B2 RID: 18610 RVA: 0x00171449 File Offset: 0x0016F849
		public float eventAlphaThreshold
		{
			get
			{
				return this.m_EventAlphaThreshold;
			}
			set
			{
				this.m_EventAlphaThreshold = value;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060048B3 RID: 18611 RVA: 0x00171452 File Offset: 0x0016F852
		// (set) Token: 0x060048B4 RID: 18612 RVA: 0x0017145A File Offset: 0x0016F85A
		public ResolutionMode ImproveResolution
		{
			get
			{
				return this.m_improveResolution;
			}
			set
			{
				this.m_improveResolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060048B5 RID: 18613 RVA: 0x00171469 File Offset: 0x0016F869
		// (set) Token: 0x060048B6 RID: 18614 RVA: 0x00171471 File Offset: 0x0016F871
		public float Resoloution
		{
			get
			{
				return this.m_Resolution;
			}
			set
			{
				this.m_Resolution = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x060048B7 RID: 18615 RVA: 0x00171480 File Offset: 0x0016F880
		// (set) Token: 0x060048B8 RID: 18616 RVA: 0x00171488 File Offset: 0x0016F888
		public bool UseNativeSize
		{
			get
			{
				return this.m_useNativeSize;
			}
			set
			{
				this.m_useNativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x060048B9 RID: 18617 RVA: 0x00171497 File Offset: 0x0016F897
		public static Material defaultETC1GraphicMaterial
		{
			get
			{
				if (UIPrimitiveBase.s_ETC1DefaultUI == null)
				{
					UIPrimitiveBase.s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
				}
				return UIPrimitiveBase.s_ETC1DefaultUI;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x060048BA RID: 18618 RVA: 0x001714B8 File Offset: 0x0016F8B8
		public override Texture mainTexture
		{
			get
			{
				if (!(this.activeSprite == null))
				{
					return this.activeSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x060048BB RID: 18619 RVA: 0x0017151C File Offset: 0x0016F91C
		public bool hasBorder
		{
			get
			{
				return this.activeSprite != null && this.activeSprite.border.sqrMagnitude > 0f;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x060048BC RID: 18620 RVA: 0x00171558 File Offset: 0x0016F958
		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.activeSprite)
				{
					num = this.activeSprite.pixelsPerUnit;
				}
				float num2 = 100f;
				if (base.canvas)
				{
					num2 = base.canvas.referencePixelsPerUnit;
				}
				return num / num2;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060048BD RID: 18621 RVA: 0x001715AC File Offset: 0x0016F9AC
		// (set) Token: 0x060048BE RID: 18622 RVA: 0x00171603 File Offset: 0x0016FA03
		public override Material material
		{
			get
			{
				if (this.m_Material != null)
				{
					return this.m_Material;
				}
				if (this.activeSprite && this.activeSprite.associatedAlphaSplitTexture != null)
				{
					return UIPrimitiveBase.defaultETC1GraphicMaterial;
				}
				return this.defaultMaterial;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x0017160C File Offset: 0x0016FA0C
		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = this.color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x00171688 File Offset: 0x0016FA88
		protected Vector2[] IncreaseResolution(Vector2[] input)
		{
			List<Vector2> list = new List<Vector2>();
			ResolutionMode improveResolution = this.ImproveResolution;
			if (improveResolution != ResolutionMode.PerLine)
			{
				if (improveResolution == ResolutionMode.PerSegment)
				{
					for (int i = 0; i < input.Length - 1; i++)
					{
						Vector2 vector = input[i];
						list.Add(vector);
						Vector2 vector2 = input[i + 1];
						this.ResolutionToNativeSize(Vector2.Distance(vector, vector2));
						float num = 1f / this.m_Resolution;
						for (float num2 = 1f; num2 < this.m_Resolution; num2 += 1f)
						{
							list.Add(Vector2.Lerp(vector, vector2, num * num2));
						}
						list.Add(vector2);
					}
				}
			}
			else
			{
				float num3 = 0f;
				for (int j = 0; j < input.Length - 1; j++)
				{
					num3 += Vector2.Distance(input[j], input[j + 1]);
				}
				this.ResolutionToNativeSize(num3);
				float num = num3 / this.m_Resolution;
				int num4 = 0;
				for (int k = 0; k < input.Length - 1; k++)
				{
					Vector2 vector3 = input[k];
					list.Add(vector3);
					Vector2 vector4 = input[k + 1];
					float num5 = Vector2.Distance(vector3, vector4) / num;
					float num6 = 1f / num5;
					int num7 = 0;
					while ((float)num7 < num5)
					{
						list.Add(Vector2.Lerp(vector3, vector4, (float)num7 * num6));
						num4++;
						num7++;
					}
					list.Add(vector4);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x00171852 File Offset: 0x0016FC52
		protected virtual void GeneratedUVs()
		{
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00171854 File Offset: 0x0016FC54
		protected virtual void ResolutionToNativeSize(float distance)
		{
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00171856 File Offset: 0x0016FC56
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00171858 File Offset: 0x0016FC58
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060048C5 RID: 18629 RVA: 0x0017185A File Offset: 0x0016FC5A
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060048C6 RID: 18630 RVA: 0x00171864 File Offset: 0x0016FC64
		public virtual float preferredWidth
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060048C7 RID: 18631 RVA: 0x001718AA File Offset: 0x0016FCAA
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x001718B1 File Offset: 0x0016FCB1
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060048C9 RID: 18633 RVA: 0x001718B8 File Offset: 0x0016FCB8
		public virtual float preferredHeight
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x001718FE File Offset: 0x0016FCFE
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x00171905 File Offset: 0x0016FD05
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x00171908 File Offset: 0x0016FD08
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.m_EventAlphaThreshold >= 1f)
			{
				return true;
			}
			Sprite overrideSprite = this.overrideSprite;
			if (overrideSprite == null)
			{
				return true;
			}
			Vector2 local;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out local);
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			local.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			local.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			local = this.MapCoordinate(local, pixelAdjustedRect);
			Rect textureRect = overrideSprite.textureRect;
			Vector2 vector = new Vector2(local.x / textureRect.width, local.y / textureRect.height);
			float x = Mathf.Lerp(textureRect.x, textureRect.xMax, vector.x) / (float)overrideSprite.texture.width;
			float y = Mathf.Lerp(textureRect.y, textureRect.yMax, vector.y) / (float)overrideSprite.texture.height;
			bool result;
			try
			{
				result = (overrideSprite.texture.GetPixelBilinear(x, y).a >= this.m_EventAlphaThreshold);
			}
			catch (UnityException ex)
			{
				global::UnityEngine.Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				result = true;
			}
			return result;
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x00171A90 File Offset: 0x0016FE90
		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.sprite.rect;
			return new Vector2(local.x * rect.width, local.y * rect.height);
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x00171ACC File Offset: 0x0016FECC
		private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
		{
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (rect.size[i] < num && num != 0f)
				{
					float num2 = rect.size[i] / num;
					Vector4 ptr = border;
					int index;
					border[index = i] = ptr[index] * num2;
					ptr = border;
					int index2;
					border[index2 = i + 2] = ptr[index2] * num2;
				}
			}
			return border;
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x00171B69 File Offset: 0x0016FF69
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetAllDirty();
		}

		// Token: 0x04006F64 RID: 28516
		protected static Material s_ETC1DefaultUI;

		// Token: 0x04006F65 RID: 28517
		[SerializeField]
		private Sprite m_Sprite;

		// Token: 0x04006F66 RID: 28518
		[NonSerialized]
		private Sprite m_OverrideSprite;

		// Token: 0x04006F67 RID: 28519
		internal float m_EventAlphaThreshold = 1f;

		// Token: 0x04006F68 RID: 28520
		[SerializeField]
		private ResolutionMode m_improveResolution;

		// Token: 0x04006F69 RID: 28521
		[SerializeField]
		protected float m_Resolution;

		// Token: 0x04006F6A RID: 28522
		[SerializeField]
		private bool m_useNativeSize;
	}
}
