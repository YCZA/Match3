using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	public class TerrainGenerator : MonoBehaviour
	{
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060042FC RID: 17148 RVA: 0x00156525 File Offset: 0x00154925
		public int Width
		{
			get
			{
				return 1 << this.widthTesselation;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00156532 File Offset: 0x00154932
		public int Height
		{
			get
			{
				return 1 << this.heightTesselation;
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060042FE RID: 17150 RVA: 0x0015653F File Offset: 0x0015493F
		private int stride
		{
			get
			{
				return this.Height + 1;
			}
		}

		// Token: 0x170009AA RID: 2474
		private Vector3 this[int x, int y]
		{
			get
			{
				return this.m_vertices[this.GetVertexIndex(x, y)];
			}
			set
			{
				this.m_vertices[this.GetVertexIndex(x, y)] = value;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x0015657E File Offset: 0x0015497E
		private int VerticesCount
		{
			get
			{
				return (this.Width + 3) * (this.Height + 3);
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x00156591 File Offset: 0x00154991
		private int QuadsCount
		{
			get
			{
				return (this.Width + 2) * (this.Height + 2);
			}
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x001565A4 File Offset: 0x001549A4
		private int GetVertexIndex(int x, int y)
		{
			return x * this.stride + y;
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x001565B0 File Offset: 0x001549B0
		private int GetSideIndex(int index, TerrainGenerator.Side side)
		{
			int num = (this.Width + 1) * (this.Height + 1);
			switch (side)
			{
				case TerrainGenerator.Side.Left:
					return num + index;
				case TerrainGenerator.Side.Top:
					return num + index + (this.Height + 1);
				case TerrainGenerator.Side.Right:
					return num + index + (this.Height + 1) + (this.Width + 1);
				case TerrainGenerator.Side.Bottom:
					return num + index + (this.Height + 1) * 2 + (this.Width + 1);
				default:
					return 0;
			}
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x0015662C File Offset: 0x00154A2C
		private void AddQuad(int posx, int posy, int width, int height)
		{
			int vertexIndex = this.GetVertexIndex(posx, posy);
			int vertexIndex2 = this.GetVertexIndex(posx, posy + height);
			int vertexIndex3 = this.GetVertexIndex(posx + width, posy + height);
			int vertexIndex4 = this.GetVertexIndex(posx + width, posy);
			this.AddQuadIndices(vertexIndex, vertexIndex2, vertexIndex3, vertexIndex4);
			this.InterpolateVertices(posx, posy, width, height);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0015667C File Offset: 0x00154A7C
		private void AddQuadIndices(int a, int b, int c, int d)
		{
			if (Mathf.Abs(this.m_vertices[a].y - this.m_vertices[c].y) > Mathf.Abs(this.m_vertices[b].y - this.m_vertices[d].y))
			{
				this.m_triangles.Add(a);
				this.m_triangles.Add(b);
				this.m_triangles.Add(d);
				this.m_triangles.Add(d);
				this.m_triangles.Add(b);
				this.m_triangles.Add(c);
			}
			else
			{
				this.m_triangles.Add(a);
				this.m_triangles.Add(b);
				this.m_triangles.Add(c);
				this.m_triangles.Add(c);
				this.m_triangles.Add(d);
				this.m_triangles.Add(a);
			}
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x00156778 File Offset: 0x00154B78
		private void InterpolateVertices(int posx, int posy, int width, int height)
		{
			Vector3 a = this[posx, posy];
			Vector3 vector = this[posx + width, posy];
			Vector3 vector2 = this[posx, posy + height];
			Vector3 b = this[posx + width, posy + height];
			for (int i = posx + 1; i < posx + width; i++)
			{
				float t = (float)(i - posx) / (float)width;
				this[i, posy] = Vector3.Lerp(a, vector, t);
				this[i, posy + height] = Vector3.Lerp(vector2, b, t);
			}
			for (int j = posy + 1; j < posy + height; j++)
			{
				float t2 = (float)(j - posy) / (float)height;
				this[posx, j] = Vector3.Lerp(a, vector2, t2);
				this[posx + width, j] = Vector3.Lerp(vector, b, t2);
			}
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x0015684C File Offset: 0x00154C4C
		private void AddQuad(TerrainGenerator.Quad quad)
		{
			if (quad.x == 0)
			{
				int sideIndex = this.GetSideIndex(quad.y, TerrainGenerator.Side.Left);
				int sideIndex2 = this.GetSideIndex(quad.y + quad.height, TerrainGenerator.Side.Left);
				int vertexIndex = this.GetVertexIndex(quad.x, quad.y + quad.height);
				int vertexIndex2 = this.GetVertexIndex(quad.x, quad.y);
				this.AddQuadIndices(sideIndex, sideIndex2, vertexIndex, vertexIndex2);
			}
			if (quad.x + quad.width == this.Width)
			{
				int vertexIndex3 = this.GetVertexIndex(quad.x + quad.width, quad.y);
				int vertexIndex4 = this.GetVertexIndex(quad.x + quad.width, quad.y + quad.height);
				int sideIndex3 = this.GetSideIndex(quad.y + quad.height, TerrainGenerator.Side.Right);
				int sideIndex4 = this.GetSideIndex(quad.y, TerrainGenerator.Side.Right);
				this.AddQuadIndices(vertexIndex3, vertexIndex4, sideIndex3, sideIndex4);
			}
			if (quad.y == 0)
			{
				int sideIndex5 = this.GetSideIndex(quad.x, TerrainGenerator.Side.Top);
				int vertexIndex5 = this.GetVertexIndex(quad.x, quad.y);
				int vertexIndex6 = this.GetVertexIndex(quad.x + quad.width, quad.y);
				int sideIndex6 = this.GetSideIndex(quad.x + quad.width, TerrainGenerator.Side.Top);
				this.AddQuadIndices(sideIndex5, vertexIndex5, vertexIndex6, sideIndex6);
			}
			if (quad.y + quad.height == this.Height)
			{
				int vertexIndex7 = this.GetVertexIndex(quad.x, quad.y + quad.height);
				int sideIndex7 = this.GetSideIndex(quad.x, TerrainGenerator.Side.Bottom);
				int sideIndex8 = this.GetSideIndex(quad.x + quad.width, TerrainGenerator.Side.Bottom);
				int vertexIndex8 = this.GetVertexIndex(quad.x + quad.width, quad.y + quad.height);
				this.AddQuadIndices(vertexIndex7, sideIndex7, sideIndex8, vertexIndex8);
			}
			this.AddQuad(quad.x, quad.y, quad.width, quad.height);
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x00156A88 File Offset: 0x00154E88
		private void ParseQuad(int posx, int posy, int width, int height)
		{
			float num = float.MaxValue;
			float num2 = float.MinValue;
			for (int i = posx; i <= posx + width; i++)
			{
				for (int j = posy; j <= posy + height; j++)
				{
					Vector3 vector = this[i, j];
					num = Mathf.Min(num, vector.y);
					num2 = Mathf.Max(num2, vector.y);
				}
			}
			float num3 = (num2 >= 0f) ? this.threshold : (this.threshold * 4f);
			if (num2 - num < num3 || width == 1 || height == 1)
			{
				this.m_quads.Add(new TerrainGenerator.Quad
				{
					x = posx,
					y = posy,
					width = width,
					height = height
				});
			}
			else
			{
				width /= 2;
				height /= 2;
				this.ParseQuad(posx, posy, width, height);
				this.ParseQuad(posx + width, posy, width, height);
				this.ParseQuad(posx + width, posy + height, width, height);
				this.ParseQuad(posx, posy + height, width, height);
			}
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x00156BAC File Offset: 0x00154FAC
		public void Recalculate()
		{
			if (Application.isPlaying)
			{
				base.GetComponentsInChildren<MeshFilter>().ForEach(delegate(MeshFilter c)
				{
					global::UnityEngine.Object.Destroy(c.gameObject);
				});
			}
			else
			{
				base.GetComponentsInChildren<MeshFilter>().ForEach(delegate(MeshFilter c)
				{
					global::UnityEngine.Object.DestroyImmediate(c.gameObject);
				});
			}
			this.AddPiece(new TerrainGenerator.Quad(0, 0, this.Width, this.Height));
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00156C31 File Offset: 0x00155031
		private void AddPiece(TerrainGenerator.Quad quad)
		{
			this.AddPiece("Land", this.CreateMesh(quad, true)).sharedMaterial = this.landMaterial;
			this.AddPiece("Ocean", this.CreateMesh(quad, false)).sharedMaterial = this.oceanMaterial;
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00156C70 File Offset: 0x00155070
		private Mesh CreateMesh(TerrainGenerator.Quad quad, bool optimize)
		{
			this.AllocateBuffers();
			this.ReadHeightmap();
			if (optimize)
			{
				this.ParseQuad(quad.x, quad.y, quad.width, quad.height);
			}
			else
			{
				int num = 1 << this.oceanTesselation;
				for (int i = 0; i < this.Width; i += num)
				{
					for (int j = 0; j < this.Height; j += num)
					{
						if (this[i, j].y <= 0.1f || this[i + num, j].y <= 0.1f || this[i + num, j + num].y <= 0.1f || this[i, j + num].y <= 0.1f)
						{
							this.AddQuad(new TerrainGenerator.Quad(i, j, num, num));
						}
					}
				}
			}
			this.ProcessQuads();
			this.OptimizeMesh();
			Mesh mesh = new Mesh
			{
				name = "Terrain",
				vertices = this.m_vertices,
				uv = this.m_uv,
				uv2 = this.m_uv2,
				triangles = this.m_triangles.ToArray()
			};
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00156DE4 File Offset: 0x001551E4
		private void AllocateBuffers()
		{
			this.m_uv = new Vector2[this.VerticesCount];
			this.m_uv2 = new Vector2[this.VerticesCount];
			this.m_vertices = new Vector3[this.VerticesCount];
			this.m_triangles = new List<int>(this.QuadsCount * 6);
			this.m_quads = new List<TerrainGenerator.Quad>(this.QuadsCount);
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00156E48 File Offset: 0x00155248
		private Vector2 CalculateAnimation(Vector3 v)
		{
			return new Vector2((v.x + v.z) * 0.5f + Mathf.Sin(v.x * 0.5f + v.z) * 5f, v.x + v.y + Mathf.Sin(v.x * 2f + v.y) * 5f);
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00156EC0 File Offset: 0x001552C0
		private void ReadHeightmap()
		{
			int i = 0;
			int num = 0;
			while (i <= this.Width)
			{
				int j = 0;
				while (j <= this.Height)
				{
					Vector2 vector = new Vector2((float)i / (float)this.Width, (float)j / (float)this.Height);
					Color pixelBilinear = this.heightmap.GetPixelBilinear(vector.x, vector.y);
					float num2 = (pixelBilinear.r <= this.waterLevel) ? this.underwaterScale : this.landScale;
					float y = (Mathf.Min(this.heightCap, pixelBilinear.r) - this.waterLevel) * num2;
					this.m_vertices[num] = new Vector3(vector.x * this.size.x, y, vector.y * this.size.y);
					this.m_uv[num] = vector;
					this.m_uv2[num] = this.CalculateAnimation(this.m_vertices[num]);
					j++;
					num++;
				}
				i++;
			}
			for (int k = 0; k <= this.Height; k++)
			{
				float num3 = (float)k / (float)this.Height;
				float z = Mathf.Lerp(-1f, 1f, num3);
				this.m_vertices[this.GetSideIndex(k, TerrainGenerator.Side.Left)] = this.m_vertices[this.GetVertexIndex(0, k)] + new Vector3(-1f, 0f, z) * this.border;
				this.m_vertices[this.GetSideIndex(k, TerrainGenerator.Side.Right)] = this.m_vertices[this.GetVertexIndex(this.Width, k)] + new Vector3(1f, 0f, z) * this.border;
				this.m_uv[this.GetSideIndex(k, TerrainGenerator.Side.Left)] = new Vector2(-this.border / this.size.x, num3);
				this.m_uv[this.GetSideIndex(k, TerrainGenerator.Side.Right)] = new Vector2(1f + this.border / this.size.x, num3);
				this.m_uv2[this.GetSideIndex(k, TerrainGenerator.Side.Left)] = this.CalculateAnimation(this.m_vertices[this.GetSideIndex(k, TerrainGenerator.Side.Left)]);
				this.m_uv2[this.GetSideIndex(k, TerrainGenerator.Side.Right)] = this.CalculateAnimation(this.m_vertices[this.GetSideIndex(k, TerrainGenerator.Side.Right)]);
			}
			for (int l = 0; l <= this.Width; l++)
			{
				float num4 = (float)l / (float)this.Width;
				float x = Mathf.Lerp(-1f, 1f, num4);
				this.m_vertices[this.GetSideIndex(l, TerrainGenerator.Side.Top)] = this.m_vertices[this.GetVertexIndex(l, 0)] + new Vector3(x, 0f, -1f) * this.border;
				this.m_vertices[this.GetSideIndex(l, TerrainGenerator.Side.Bottom)] = this.m_vertices[this.GetVertexIndex(l, this.Height)] + new Vector3(x, 0f, 1f) * this.border;
				this.m_uv[this.GetSideIndex(l, TerrainGenerator.Side.Top)] = new Vector2(num4, -this.border / this.size.y);
				this.m_uv[this.GetSideIndex(l, TerrainGenerator.Side.Bottom)] = new Vector2(num4, 1f + this.border / this.size.y);
				this.m_uv2[this.GetSideIndex(l, TerrainGenerator.Side.Top)] = this.CalculateAnimation(this.m_vertices[this.GetSideIndex(l, TerrainGenerator.Side.Top)]);
				this.m_uv2[this.GetSideIndex(l, TerrainGenerator.Side.Bottom)] = this.CalculateAnimation(this.m_vertices[this.GetSideIndex(l, TerrainGenerator.Side.Bottom)]);
			}
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x00157374 File Offset: 0x00155774
		private void ProcessQuads()
		{
			List<TerrainGenerator.Quad> quads = this.m_quads;
			if (TerrainGenerator._003C_003Ef__mg_0024cache0 == null)
			{
				TerrainGenerator._003C_003Ef__mg_0024cache0 = new Comparison<TerrainGenerator.Quad>(TerrainGenerator.Quad.Compare);
			}
			quads.Sort(TerrainGenerator._003C_003Ef__mg_0024cache0);
			this.m_quads.ForEach(new Action<TerrainGenerator.Quad>(this.AddQuad));
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x001573C0 File Offset: 0x001557C0
		private void OptimizeMesh()
		{
			int[] array = new HashSet<int>(this.m_triangles).ToArray<int>();
			int[] array2 = new int[this.m_vertices.Length];
			Vector3[] array3 = new Vector3[array.Length];
			Vector2[] array4 = new Vector2[array.Length];
			Vector2[] array5 = new Vector2[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array3[i] = this.m_vertices[array[i]];
				array4[i] = this.m_uv[array[i]];
				array5[i] = this.m_uv2[array[i]];
				array2[array[i]] = i;
			}
			for (int j = 0; j < this.m_triangles.Count; j++)
			{
				this.m_triangles[j] = array2[this.m_triangles[j]];
			}
			this.m_vertices = array3;
			this.m_uv = array4;
			this.m_uv2 = array5;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x001574E0 File Offset: 0x001558E0
		private MeshRenderer AddPiece(string name, Mesh mesh)
		{
			GameObject gameObject = new GameObject(name);
			MeshRenderer result = gameObject.AddComponent<MeshRenderer>();
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			mesh.RecalculateBounds();
			meshFilter.sharedMesh = mesh;
			meshFilter.transform.SetParent(base.transform, false);
			return result;
		}

		// Token: 0x04006BA8 RID: 27560
		public Texture2D heightmap;

		// Token: 0x04006BA9 RID: 27561
		public Material landMaterial;

		// Token: 0x04006BAA RID: 27562
		public Material oceanMaterial;

		// Token: 0x04006BAB RID: 27563
		public Vector2 size = new Vector2(10f, 10f);

		// Token: 0x04006BAC RID: 27564
		[SerializeField]
		private int widthTesselation = 9;

		// Token: 0x04006BAD RID: 27565
		[SerializeField]
		private int heightTesselation = 9;

		// Token: 0x04006BAE RID: 27566
		[SerializeField]
		private int oceanTesselation = 4;

		// Token: 0x04006BAF RID: 27567
		public float border = 20f;

		// Token: 0x04006BB0 RID: 27568
		public float landScale = 1f;

		// Token: 0x04006BB1 RID: 27569
		public float underwaterScale = 2f;

		// Token: 0x04006BB2 RID: 27570
		[Range(0f, 1f)]
		public float threshold = 0.3f;

		// Token: 0x04006BB3 RID: 27571
		[Range(0f, 1f)]
		public float heightCap = 0.85f;

		// Token: 0x04006BB4 RID: 27572
		[Range(0f, 1f)]
		public float waterLevel = 0.5f;

		// Token: 0x04006BB5 RID: 27573
		private List<int> m_triangles;

		// Token: 0x04006BB6 RID: 27574
		private Vector2[] m_uv;

		// Token: 0x04006BB7 RID: 27575
		private Vector2[] m_uv2;

		// Token: 0x04006BB8 RID: 27576
		private Vector3[] m_vertices;

		// Token: 0x04006BB9 RID: 27577
		private List<TerrainGenerator.Quad> m_quads;

		// Token: 0x04006BBC RID: 27580
		[CompilerGenerated]
		private static Comparison<TerrainGenerator.Quad> _003C_003Ef__mg_0024cache0;

		// Token: 0x02000B28 RID: 2856
		private enum Side
		{
			// Token: 0x04006BBE RID: 27582
			Left,
			// Token: 0x04006BBF RID: 27583
			Top,
			// Token: 0x04006BC0 RID: 27584
			Right,
			// Token: 0x04006BC1 RID: 27585
			Bottom
		}

		// Token: 0x02000B29 RID: 2857
		public struct Quad
		{
			// Token: 0x06004315 RID: 17173 RVA: 0x0015753C File Offset: 0x0015593C
			public Quad(int x, int y, int width, int height)
			{
				this.x = x;
				this.y = y;
				this.width = width;
				this.height = height;
			}

			// Token: 0x06004316 RID: 17174 RVA: 0x0015755C File Offset: 0x0015595C
			public static int Compare(TerrainGenerator.Quad b, TerrainGenerator.Quad a)
			{
				return (a.width + a.height).CompareTo(b.width + b.height);
			}

			// Token: 0x04006BC2 RID: 27586
			public int x;

			// Token: 0x04006BC3 RID: 27587
			public int y;

			// Token: 0x04006BC4 RID: 27588
			public int width;

			// Token: 0x04006BC5 RID: 27589
			public int height;
		}
	}
}
