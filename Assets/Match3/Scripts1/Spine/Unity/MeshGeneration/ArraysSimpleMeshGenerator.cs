using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x0200022F RID: 559
	public class ArraysSimpleMeshGenerator : ArraysMeshGenerator, ISimpleMeshGenerator
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x0003033C File Offset: 0x0002E73C
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x00030344 File Offset: 0x0002E744
		public float Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x0003034D File Offset: 0x0002E74D
		public Mesh LastGeneratedMesh
		{
			get
			{
				return this.lastGeneratedMesh;
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00030358 File Offset: 0x0002E758
		public Mesh GenerateMesh(Skeleton skeleton)
		{
			int num = 0;
			int num2 = 0;
			Slot[] items = skeleton.drawOrder.Items;
			int count = skeleton.drawOrder.Count;
			int i = 0;
			while (i < count)
			{
				Slot slot = items[i];
				Attachment attachment = slot.attachment;
				RegionAttachment regionAttachment = attachment as RegionAttachment;
				int num3;
				int num4;
				if (regionAttachment != null)
				{
					num3 = 4;
					num4 = 6;
					goto IL_AD;
				}
				MeshAttachment meshAttachment = attachment as MeshAttachment;
				if (meshAttachment != null)
				{
					num3 = meshAttachment.vertices.Length >> 1;
					num4 = meshAttachment.triangles.Length;
					goto IL_AD;
				}
				WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
				if (weightedMeshAttachment != null)
				{
					num3 = weightedMeshAttachment.uvs.Length >> 1;
					num4 = weightedMeshAttachment.triangles.Length;
					goto IL_AD;
				}
				IL_B7:
				i++;
				continue;
				IL_AD:
				num2 += num4;
				num += num3;
				goto IL_B7;
			}
			ArraysMeshGenerator.EnsureSize(num, ref this.meshVertices, ref this.meshUVs, ref this.meshColors32);
			this.triangles = (this.triangles ?? new int[num2]);
			Vector3 boundsMin;
			Vector3 boundsMax;
			if (num == 0)
			{
				boundsMin = new Vector3(0f, 0f, 0f);
				boundsMax = new Vector3(0f, 0f, 0f);
			}
			else
			{
				boundsMin.x = 2.1474836E+09f;
				boundsMin.y = 2.1474836E+09f;
				boundsMax.x = -2.1474836E+09f;
				boundsMax.y = -2.1474836E+09f;
				boundsMin.z = -0.01f * this.scale;
				boundsMax.z = 0.01f * this.scale;
				int num5 = 0;
				ArraysMeshGenerator.FillVerts(skeleton, 0, count, -1f, this.premultiplyVertexColors, this.meshVertices, this.meshUVs, this.meshColors32, ref num5, ref this.attachmentVertexBuffer, ref boundsMin, ref boundsMax);
				boundsMax.x *= this.scale;
				boundsMax.y *= this.scale;
				boundsMin.x *= this.scale;
				boundsMax.y *= this.scale;
				Vector3[] meshVertices = this.meshVertices;
				for (int j = 0; j < num; j++)
				{
					Vector3 vector = meshVertices[j];
					vector.x *= this.scale;
					vector.y *= this.scale;
					meshVertices[j] = vector;
				}
			}
			ArraysMeshGenerator.FillTriangles(skeleton, num2, 0, 0, count, ref this.triangles, true);
			Mesh nextMesh = this.doubleBufferedMesh.GetNextMesh();
			nextMesh.vertices = this.meshVertices;
			nextMesh.colors32 = this.meshColors32;
			nextMesh.uv = this.meshUVs;
			nextMesh.bounds = ArraysMeshGenerator.ToBounds(boundsMin, boundsMax);
			nextMesh.triangles = this.triangles;
			this.lastGeneratedMesh = nextMesh;
			return nextMesh;
		}

		// Token: 0x040041CB RID: 16843
		protected float scale = 1f;

		// Token: 0x040041CC RID: 16844
		private Mesh lastGeneratedMesh;

		// Token: 0x040041CD RID: 16845
		private readonly DoubleBufferedMesh doubleBufferedMesh = new DoubleBufferedMesh();

		// Token: 0x040041CE RID: 16846
		private int[] triangles;

		// Token: 0x040041CF RID: 16847
		private int triangleBufferCount;
	}
}
