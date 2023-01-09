using System;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Match3.Scripts1.Wta.UISliceSetup
{
	// Token: 0x02000434 RID: 1076
	[AddComponentMenu("UI/Seventeen Slice Image", 12)]
	public class SeventeenSliceImage : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
	{
		// Token: 0x06001F65 RID: 8037 RVA: 0x00083808 File Offset: 0x00081C08
		protected SeventeenSliceImage()
		{
			this.AlphaHitTestMinimumThreshold = 0f;
			base.useLegacyMeshGeneration = false;
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x0008385E File Offset: 0x00081C5E
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x00083866 File Offset: 0x00081C66
		public Sprite Sprite
		{
			get
			{
				return this.sprite;
			}
			set
			{
				if (this.sprite != value)
				{
					this.sprite = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x00083886 File Offset: 0x00081C86
		// (set) Token: 0x06001F69 RID: 8041 RVA: 0x0008388E File Offset: 0x00081C8E
		public Sprite OverrideSprite
		{
			get
			{
				return this.ActiveSprite;
			}
			set
			{
				if (this.overrideSprite != value)
				{
					this.overrideSprite = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x000838AE File Offset: 0x00081CAE
		private Sprite ActiveSprite
		{
			get
			{
				return (!(this.overrideSprite != null)) ? this.Sprite : this.overrideSprite;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x000838D2 File Offset: 0x00081CD2
		// (set) Token: 0x06001F6C RID: 8044 RVA: 0x000838DA File Offset: 0x00081CDA
		public bool FillCenter
		{
			get
			{
				return this.fillCenter;
			}
			set
			{
				if (this.fillCenter != value)
				{
					this.fillCenter = value;
					this.SetVerticesDirty();
				}
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000838F5 File Offset: 0x00081CF5
		// (set) Token: 0x06001F6E RID: 8046 RVA: 0x000838FD File Offset: 0x00081CFD
		private float AlphaHitTestMinimumThreshold { get; set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x00083906 File Offset: 0x00081D06
		// (set) Token: 0x06001F70 RID: 8048 RVA: 0x0008390E File Offset: 0x00081D0E
		public Vector2 BottomLeftVert
		{
			get
			{
				return this.bottomLeftVert;
			}
			set
			{
				if (this.bottomLeftVert != value)
				{
					this.bottomLeftVert = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x0008392E File Offset: 0x00081D2E
		// (set) Token: 0x06001F72 RID: 8050 RVA: 0x00083936 File Offset: 0x00081D36
		public Vector2 TopRightVert
		{
			get
			{
				return this.topRightVert;
			}
			set
			{
				if (this.topRightVert != value)
				{
					this.topRightVert = value;
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x00083956 File Offset: 0x00081D56
		private static Material DefaultEtc1GraphicMaterial
		{
			get
			{
				Material result;
				if ((result = SeventeenSliceImage._etc1DefaultUi) == null)
				{
					result = (SeventeenSliceImage._etc1DefaultUi = Canvas.GetETC1SupportedCanvasMaterial());
				}
				return result;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x00083970 File Offset: 0x00081D70
		public override Texture mainTexture
		{
			get
			{
				if (!(this.ActiveSprite == null))
				{
					return this.ActiveSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x000839D4 File Offset: 0x00081DD4
		public bool HasBorder
		{
			get
			{
				return this.ActiveSprite != null && this.ActiveSprite.border.sqrMagnitude > 0f;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x00083A10 File Offset: 0x00081E10
		private float PixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.ActiveSprite)
				{
					num = this.ActiveSprite.pixelsPerUnit;
				}
				float num2 = 100f;
				if (base.canvas)
				{
					num2 = base.canvas.referencePixelsPerUnit;
				}
				return num / num2;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x00083A64 File Offset: 0x00081E64
		// (set) Token: 0x06001F78 RID: 8056 RVA: 0x00083ABB File Offset: 0x00081EBB
		public override Material material
		{
			get
			{
				if (this.m_Material != null)
				{
					return this.m_Material;
				}
				if (this.ActiveSprite && this.ActiveSprite.associatedAlphaSplitTexture != null)
				{
					return SeventeenSliceImage.DefaultEtc1GraphicMaterial;
				}
				return this.defaultMaterial;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00083AC4 File Offset: 0x00081EC4
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00083AC6 File Offset: 0x00081EC6
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00083AC8 File Offset: 0x00081EC8
		private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			Vector4 vector = (!(this.ActiveSprite == null)) ? DataUtility.GetPadding(this.ActiveSprite) : Vector4.zero;
			Vector2 vector2 = (!(this.ActiveSprite == null)) ? new Vector2(this.ActiveSprite.rect.width, this.ActiveSprite.rect.height) : Vector2.zero;
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			int num = Mathf.RoundToInt(vector2.x);
			int num2 = Mathf.RoundToInt(vector2.y);
			Vector4 result = new Vector4(vector.x / (float)num, vector.y / (float)num2, ((float)num - vector.z) / (float)num, ((float)num2 - vector.w) / (float)num2);
			if (shouldPreserveAspect && vector2.sqrMagnitude > 0f)
			{
				float num3 = vector2.x / vector2.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > num4)
				{
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
				}
				else
				{
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
				}
			}
			result = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * result.x, pixelAdjustedRect.y + pixelAdjustedRect.height * result.y, pixelAdjustedRect.x + pixelAdjustedRect.width * result.z, pixelAdjustedRect.y + pixelAdjustedRect.height * result.w);
			return result;
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00083CE4 File Offset: 0x000820E4
		public override void SetNativeSize()
		{
			if (this.ActiveSprite != null)
			{
				float x = this.ActiveSprite.rect.width / this.PixelsPerUnit;
				float y = this.ActiveSprite.rect.height / this.PixelsPerUnit;
				base.rectTransform.anchorMax = base.rectTransform.anchorMin;
				base.rectTransform.sizeDelta = new Vector2(x, y);
				this.SetAllDirty();
			}
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00083D66 File Offset: 0x00082166
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (this.ActiveSprite == null)
			{
				base.OnPopulateMesh(toFill);
				return;
			}
			this.GenerateSlicedSprite(toFill);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x00083D88 File Offset: 0x00082188
		protected override void UpdateMaterial()
		{
			base.UpdateMaterial();
			if (this.ActiveSprite == null)
			{
				base.canvasRenderer.SetAlphaTexture(null);
				return;
			}
			Texture2D associatedAlphaSplitTexture = this.ActiveSprite.associatedAlphaSplitTexture;
			if (associatedAlphaSplitTexture != null)
			{
				base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00083DE0 File Offset: 0x000821E0
		private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
		{
			Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
			Vector4 vector = (!(this.ActiveSprite != null)) ? Vector4.zero : DataUtility.GetOuterUV(this.ActiveSprite);
			Color color = this.color;
			vh.Clear();
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, new Vector2(vector.x, vector.y));
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, new Vector2(vector.x, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, new Vector2(vector.z, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, new Vector2(vector.z, vector.y));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00083F04 File Offset: 0x00082304
		public Vector4[] GetSliceVertices()
		{
			return new Vector4[]
			{
				new Vector4(SeventeenSliceImage.VertScratch[0].x, SeventeenSliceImage.VertScratch[0].y, SeventeenSliceImage.VertScratch[5].x, SeventeenSliceImage.VertScratch[5].y),
				new Vector4(SeventeenSliceImage.VertScratch[1].x, SeventeenSliceImage.VertScratch[1].y, SeventeenSliceImage.VertScratch[4].x, SeventeenSliceImage.VertScratch[4].y),
				new Vector4(SeventeenSliceImage.VertScratch[2].x, SeventeenSliceImage.VertScratch[2].y, SeventeenSliceImage.VertScratch[3].x, SeventeenSliceImage.VertScratch[3].y)
			};
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0008400A File Offset: 0x0008240A
		public Vector2[] GetImageVertices()
		{
			return SeventeenSliceImage.VertScratch;
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x00084011 File Offset: 0x00082411
		private static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
		{
			return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00084024 File Offset: 0x00082424
		private void GenerateSlicedSprite(VertexHelper toFill)
		{
			if (!this.HasBorder)
			{
				this.GenerateSimpleSprite(toFill, false);
				return;
			}
			Vector4 vector;
			Vector4 vector2;
			Vector4 a;
			Vector4 a2;
			Vector4 zero;
			Vector4 zero2;
			if (this.ActiveSprite != null)
			{
				vector = DataUtility.GetOuterUV(this.ActiveSprite);
				vector2 = DataUtility.GetInnerUV(this.ActiveSprite);
				a = DataUtility.GetPadding(this.ActiveSprite);
				a2 = this.ActiveSprite.border;
				zero = new Vector4(SeventeenSliceImage.Remap(vector2.x, vector.x, vector.z, 0f, 1f), SeventeenSliceImage.Remap(vector2.y, vector.y, vector.w, 0f, 1f), SeventeenSliceImage.Remap(vector2.z, vector.x, vector.z, 0f, 1f), SeventeenSliceImage.Remap(vector2.w, vector.y, vector.w, 0f, 1f));
				Vector2 vector3 = new Vector2(SeventeenSliceImage.Remap(this.bottomLeftVert.x, 0f, 1f, vector.x, vector.z), SeventeenSliceImage.Remap(this.bottomLeftVert.y, 0f, 1f, vector.y, vector.w));
				Vector2 vector4 = new Vector2(SeventeenSliceImage.Remap(this.topRightVert.x, 0f, 1f, vector.x, vector.z), SeventeenSliceImage.Remap(this.topRightVert.y, 0f, 1f, vector.y, vector.w));
				zero2 = new Vector4(vector3.x, vector3.y, vector4.x, vector4.y);
			}
			else
			{
				vector = Vector4.zero;
				vector2 = Vector4.zero;
				a = Vector4.zero;
				a2 = Vector4.zero;
				zero2 = Vector4.zero;
				zero = Vector4.zero;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			Vector4 adjustedBorders = this.GetAdjustedBorders(a2 / this.PixelsPerUnit, pixelAdjustedRect);
			a /= this.PixelsPerUnit;
			Vector2 vector5 = this.topRightVert - this.bottomLeftVert;
			Vector2 adjustedCoordinates = this.GetAdjustedCoordinates(Vector2.Scale(vector5, this.ActiveSprite.rect.size) / this.PixelsPerUnit, pixelAdjustedRect);
			Vector2 b = adjustedCoordinates + new Vector2(a2.x, a2.y) + new Vector2(a2.z, a2.w);
			Vector2 vector6 = new Vector2(zero.z, zero.w) - new Vector2(zero.x, zero.y) - vector5;
			Vector2 a3 = pixelAdjustedRect.size - b;
			SeventeenSliceImage.VertScratch[0] = new Vector2(a.x, a.y);
			SeventeenSliceImage.VertScratch[5] = new Vector2(pixelAdjustedRect.width - a.z, pixelAdjustedRect.height - a.w);
			SeventeenSliceImage.VertScratch[1].x = adjustedBorders.x;
			SeventeenSliceImage.VertScratch[1].y = adjustedBorders.y;
			SeventeenSliceImage.VertScratch[4].x = pixelAdjustedRect.width - adjustedBorders.z;
			SeventeenSliceImage.VertScratch[4].y = pixelAdjustedRect.height - adjustedBorders.w;
			Vector2 b2 = new Vector2((this.bottomLeftVert.x - zero.x) / vector6.x, (this.bottomLeftVert.y - zero.y) / vector6.y);
			Vector2 b3 = new Vector2((zero.z - this.topRightVert.x) / vector6.x, (zero.w - this.topRightVert.y) / vector6.y);
			SeventeenSliceImage.VertScratch[2] = SeventeenSliceImage.VertScratch[1] + Vector2.Scale(a3, b2);
			SeventeenSliceImage.VertScratch[3] = SeventeenSliceImage.VertScratch[4] - Vector2.Scale(a3, b3);
			for (int i = 0; i < 6; i++)
			{
				Vector2[] vertScratch = SeventeenSliceImage.VertScratch;
				int num = i;
				vertScratch[num].x = vertScratch[num].x + pixelAdjustedRect.x;
				Vector2[] vertScratch2 = SeventeenSliceImage.VertScratch;
				int num2 = i;
				vertScratch2[num2].y = vertScratch2[num2].y + pixelAdjustedRect.y;
			}
			SeventeenSliceImage.UVScratch[0] = new Vector2(vector.x, vector.y);
			SeventeenSliceImage.UVScratch[1] = new Vector2(vector2.x, vector2.y);
			SeventeenSliceImage.UVScratch[2] = new Vector2(zero2.x, zero2.y);
			SeventeenSliceImage.UVScratch[3] = new Vector2(zero2.z, zero2.w);
			SeventeenSliceImage.UVScratch[4] = new Vector2(vector2.z, vector2.w);
			SeventeenSliceImage.UVScratch[5] = new Vector2(vector.z, vector.w);
			toFill.Clear();
			for (int j = 0; j < 5; j++)
			{
				int num3 = j + 1;
				for (int k = 0; k < 5; k++)
				{
					if (j >= 1 && j <= 3 && k >= 1 && k <= 3)
					{
						if (this.fillCenter && j == 1 && k == 1)
						{
							SeventeenSliceImage.AddQuad(toFill, new Vector2(SeventeenSliceImage.VertScratch[1].x, SeventeenSliceImage.VertScratch[1].y), new Vector2(SeventeenSliceImage.VertScratch[4].x, SeventeenSliceImage.VertScratch[4].y), this.color, new Vector2(SeventeenSliceImage.UVScratch[1].x, SeventeenSliceImage.UVScratch[1].y), new Vector2(SeventeenSliceImage.UVScratch[4].x, SeventeenSliceImage.UVScratch[4].y));
						}
					}
					else
					{
						int num4 = k + 1;
						SeventeenSliceImage.AddQuad(toFill, new Vector2(SeventeenSliceImage.VertScratch[j].x, SeventeenSliceImage.VertScratch[k].y), new Vector2(SeventeenSliceImage.VertScratch[num3].x, SeventeenSliceImage.VertScratch[num4].y), this.color, new Vector2(SeventeenSliceImage.UVScratch[j].x, SeventeenSliceImage.UVScratch[k].y), new Vector2(SeventeenSliceImage.UVScratch[num3].x, SeventeenSliceImage.UVScratch[num4].y));
					}
				}
			}
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x00084790 File Offset: 0x00082B90
		private static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0f), color, new Vector2(uvMin.x, uvMin.y));
			vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0f), color, new Vector2(uvMin.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0f), color, new Vector2(uvMax.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0f), color, new Vector2(uvMax.x, uvMin.y));
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x00084888 File Offset: 0x00082C88
		private Vector2 GetAdjustedCoordinates(Vector2 coordinates, Rect adjustedRect)
		{
			Rect rect = base.rectTransform.rect;
			for (int i = 0; i <= 1; i++)
			{
				if (rect.size[i] != 0f)
				{
					float num = adjustedRect.size[i] / rect.size[i];
					Vector2 ptr = coordinates;
					int index;
					coordinates[index = i] = ptr[index] * num;
				}
				float num2 = coordinates[i];
				if (adjustedRect.size[i] < num2 && num2 != 0f)
				{
					float num = adjustedRect.size[i] / num2;
					Vector2 ptr = coordinates;
					int index2;
					coordinates[index2 = i] = ptr[index2] * num;
				}
			}
			return coordinates;
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x00084968 File Offset: 0x00082D68
		private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
		{
			Rect rect = base.rectTransform.rect;
			for (int i = 0; i <= 1; i++)
			{
				if (rect.size[i] != 0f)
				{
					float num = adjustedRect.size[i] / rect.size[i];
					Vector4 ptr = border;
					int index;
					border[index = i] = ptr[index] * num;
					ptr = border;
					int index2;
					border[index2 = i + 2] = ptr[index2] * num;
				}
				float num2 = border[i] + border[i + 2];
				if (adjustedRect.size[i] < num2 && num2 != 0f)
				{
					float num = adjustedRect.size[i] / num2;
					Vector4 ptr = border;
					int index3;
					border[index3 = i] = ptr[index3] * num;
					ptr = border;
					int index4;
					border[index4 = i + 2] = ptr[index4] * num;
				}
			}
			return border;
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x00084A88 File Offset: 0x00082E88
		public void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x00084A8A File Offset: 0x00082E8A
		public void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x00084A8C File Offset: 0x00082E8C
		public float minWidth
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x00084A94 File Offset: 0x00082E94
		public float preferredWidth
		{
			get
			{
				if (this.ActiveSprite == null)
				{
					return 0f;
				}
				return DataUtility.GetMinSize(this.ActiveSprite).x / this.PixelsPerUnit;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x00084AD2 File Offset: 0x00082ED2
		public float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x00084AD9 File Offset: 0x00082ED9
		public float minHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x00084AE0 File Offset: 0x00082EE0
		public float preferredHeight
		{
			get
			{
				if (this.ActiveSprite == null)
				{
					return 0f;
				}
				return DataUtility.GetMinSize(this.ActiveSprite).y / this.PixelsPerUnit;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x00084B1E File Offset: 0x00082F1E
		public float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x00084B25 File Offset: 0x00082F25
		public int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x00084B28 File Offset: 0x00082F28
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.AlphaHitTestMinimumThreshold <= 0f)
			{
				return true;
			}
			if (this.AlphaHitTestMinimumThreshold > 1f)
			{
				return false;
			}
			if (this.ActiveSprite == null)
			{
				return true;
			}
			Vector2 local;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out local))
			{
				return false;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			local.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			local.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			local = this.MapCoordinate(local, pixelAdjustedRect);
			Rect textureRect = this.ActiveSprite.textureRect;
			Vector2 vector = new Vector2(local.x / textureRect.width, local.y / textureRect.height);
			float x = Mathf.Lerp(textureRect.x, textureRect.xMax, vector.x) / (float)this.ActiveSprite.texture.width;
			float y = Mathf.Lerp(textureRect.y, textureRect.yMax, vector.y) / (float)this.ActiveSprite.texture.height;
			bool result;
			try
			{
				result = (this.ActiveSprite.texture.GetPixelBilinear(x, y).a >= this.AlphaHitTestMinimumThreshold);
			}
			catch (UnityException ex)
			{
				global::UnityEngine.Debug.LogError("Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				result = true;
			}
			return result;
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x00084CDC File Offset: 0x000830DC
		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.ActiveSprite.rect;
			Vector4 border = this.ActiveSprite.border;
			Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.PixelsPerUnit, rect);
			for (int i = 0; i < 2; i++)
			{
				if (local[i] > adjustedBorders[i])
				{
					if (rect.size[i] - local[i] <= adjustedBorders[i + 2])
					{
						Vector2 ptr = local;
						int index;
						local[index = i] = ptr[index] - (rect.size[i] - rect2.size[i]);
					}
					else
					{
						float t = Mathf.InverseLerp(adjustedBorders[i], rect.size[i] - adjustedBorders[i + 2], local[i]);
						local[i] = Mathf.Lerp(border[i], rect2.size[i] - border[i + 2], t);
					}
				}
			}
			return local;
		}

		// Token: 0x04004AE2 RID: 19170
		private static Material _etc1DefaultUi = null;

		// Token: 0x04004AE3 RID: 19171
		private static readonly Vector2[] VertScratch = new Vector2[6];

		// Token: 0x04004AE4 RID: 19172
		private static readonly Vector2[] UVScratch = new Vector2[6];

		// Token: 0x04004AE5 RID: 19173
		[SerializeField]
		private Sprite sprite;

		// Token: 0x04004AE6 RID: 19174
		[NonSerialized]
		private Sprite overrideSprite;

		// Token: 0x04004AE7 RID: 19175
		[SerializeField]
		private bool fillCenter = true;

		// Token: 0x04004AE9 RID: 19177
		[SerializeField]
		private Vector2 bottomLeftVert = new Vector2(0.4f, 0.2f);

		// Token: 0x04004AEA RID: 19178
		[SerializeField]
		private Vector2 topRightVert = new Vector2(0.4f, 0.2f);
	}
}
