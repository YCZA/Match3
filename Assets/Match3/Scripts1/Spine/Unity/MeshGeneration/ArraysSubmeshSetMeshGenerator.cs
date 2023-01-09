using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000232 RID: 562
	public class ArraysSubmeshSetMeshGenerator : ArraysMeshGenerator, ISubmeshSetMeshGenerator
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x00030E3C File Offset: 0x0002F23C
		public MeshAndMaterials GenerateMesh(ExposedList<SubmeshInstruction> instructions, int startSubmesh, int endSubmesh)
		{
			SubmeshInstruction[] items = instructions.Items;
			this.currentInstructions.Clear(false);
			for (int i = startSubmesh; i < endSubmesh; i++)
			{
				this.currentInstructions.Add(items[i]);
			}
			ArraysSubmeshSetMeshGenerator.SmartMesh next = this.doubleBufferedSmartMesh.GetNext();
			Mesh mesh = next.mesh;
			int count = this.currentInstructions.Count;
			SubmeshInstruction[] items2 = this.currentInstructions.Items;
			int num = 0;
			for (int j = 0; j < count; j++)
			{
				items2[j].firstVertexIndex = num;
				num += items2[j].vertexCount;
			}
			bool flag = ArraysMeshGenerator.EnsureSize(num, ref this.meshVertices, ref this.meshUVs, ref this.meshColors32);
			bool flag2 = ArraysMeshGenerator.EnsureTriangleBuffersSize(this.submeshBuffers, count, items2);
			float num2 = this.zSpacing;
			Vector3 boundsMin;
			Vector3 boundsMax;
			if (num <= 0)
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
				int endSlot = items2[count - 1].endSlot;
				if (num2 > 0f)
				{
					boundsMin.z = 0f;
					boundsMax.z = num2 * (float)endSlot;
				}
				else
				{
					boundsMin.z = num2 * (float)endSlot;
					boundsMax.z = 0f;
				}
			}
			ExposedList<Attachment> exposedList = this.attachmentBuffer;
			exposedList.Clear(false);
			int num3 = 0;
			for (int k = 0; k < count; k++)
			{
				SubmeshInstruction submeshInstruction = items2[k];
				int startSlot = submeshInstruction.startSlot;
				int endSlot2 = submeshInstruction.endSlot;
				Skeleton skeleton = submeshInstruction.skeleton;
				Slot[] items3 = skeleton.DrawOrder.Items;
				for (int l = startSlot; l < endSlot2; l++)
				{
					Attachment attachment = items3[l].attachment;
					if (attachment != null)
					{
						exposedList.Add(attachment);
					}
				}
				ArraysMeshGenerator.FillVerts(skeleton, startSlot, endSlot2, num2, this.premultiplyVertexColors, this.meshVertices, this.meshUVs, this.meshColors32, ref num3, ref this.attachmentVertexBuffer, ref boundsMin, ref boundsMax);
			}
			bool flag3 = flag || flag2 || next.StructureDoesntMatch(exposedList, this.currentInstructions);
			for (int m = 0; m < count; m++)
			{
				SubmeshInstruction submeshInstruction2 = items2[m];
				if (flag3)
				{
					ArraysMeshGenerator.SubmeshTriangleBuffer submeshTriangleBuffer = this.submeshBuffers.Items[m];
					bool isLastSubmesh = m == count - 1;
					ArraysMeshGenerator.FillTriangles(submeshInstruction2.skeleton, submeshInstruction2.triangleCount, submeshInstruction2.firstVertexIndex, submeshInstruction2.startSlot, submeshInstruction2.endSlot, ref submeshTriangleBuffer.triangles, isLastSubmesh);
				}
			}
			if (flag3)
			{
				mesh.Clear();
				this.sharedMaterials = this.currentInstructions.GetUpdatedMaterialArray(this.sharedMaterials);
			}
			next.Set(this.meshVertices, this.meshUVs, this.meshColors32, exposedList, this.currentInstructions);
			mesh.bounds = ArraysMeshGenerator.ToBounds(boundsMin, boundsMax);
			base.TryAddNormalsTo(mesh, num);
			if (flag3)
			{
				mesh.subMeshCount = count;
				for (int n = 0; n < count; n++)
				{
					mesh.SetTriangles(this.submeshBuffers.Items[n].triangles, n);
				}
			}
			return new MeshAndMaterials(next.mesh, this.sharedMaterials);
		}

		// Token: 0x040041D9 RID: 16857
		public float zSpacing;

		// Token: 0x040041DA RID: 16858
		private readonly DoubleBuffered<ArraysSubmeshSetMeshGenerator.SmartMesh> doubleBufferedSmartMesh = new DoubleBuffered<ArraysSubmeshSetMeshGenerator.SmartMesh>();

		// Token: 0x040041DB RID: 16859
		private readonly ExposedList<SubmeshInstruction> currentInstructions = new ExposedList<SubmeshInstruction>();

		// Token: 0x040041DC RID: 16860
		private readonly ExposedList<Attachment> attachmentBuffer = new ExposedList<Attachment>();

		// Token: 0x040041DD RID: 16861
		private readonly ExposedList<ArraysMeshGenerator.SubmeshTriangleBuffer> submeshBuffers = new ExposedList<ArraysMeshGenerator.SubmeshTriangleBuffer>();

		// Token: 0x040041DE RID: 16862
		private Material[] sharedMaterials = new Material[0];

		// Token: 0x02000233 RID: 563
		private class SmartMesh
		{
			// Token: 0x060011A9 RID: 4521 RVA: 0x00031224 File Offset: 0x0002F624
			public void Set(Vector3[] verts, Vector2[] uvs, Color32[] colors, ExposedList<Attachment> attachments, ExposedList<SubmeshInstruction> instructions)
			{
				this.mesh.vertices = verts;
				this.mesh.uv = uvs;
				this.mesh.colors32 = colors;
				this.attachmentsUsed.Clear(false);
				this.attachmentsUsed.GrowIfNeeded(attachments.Capacity);
				this.attachmentsUsed.Count = attachments.Count;
				attachments.CopyTo(this.attachmentsUsed.Items);
				this.instructionsUsed.Clear(false);
				this.instructionsUsed.GrowIfNeeded(instructions.Capacity);
				this.instructionsUsed.Count = instructions.Count;
				instructions.CopyTo(this.instructionsUsed.Items);
			}

			// Token: 0x060011AA RID: 4522 RVA: 0x000312DC File Offset: 0x0002F6DC
			public bool StructureDoesntMatch(ExposedList<Attachment> attachments, ExposedList<SubmeshInstruction> instructions)
			{
				if (attachments.Count != this.attachmentsUsed.Count)
				{
					return true;
				}
				if (instructions.Count != this.instructionsUsed.Count)
				{
					return true;
				}
				Attachment[] items = attachments.Items;
				Attachment[] items2 = this.attachmentsUsed.Items;
				int i = 0;
				int count = this.attachmentsUsed.Count;
				while (i < count)
				{
					if (items[i] != items2[i])
					{
						return true;
					}
					i++;
				}
				SubmeshInstruction[] items3 = instructions.Items;
				SubmeshInstruction[] items4 = this.instructionsUsed.Items;
				int j = 0;
				int count2 = this.instructionsUsed.Count;
				while (j < count2)
				{
					SubmeshInstruction submeshInstruction = items3[j];
					SubmeshInstruction submeshInstruction2 = items4[j];
					if (submeshInstruction.material.GetInstanceID() != submeshInstruction2.material.GetInstanceID() || submeshInstruction.startSlot != submeshInstruction2.startSlot || submeshInstruction.endSlot != submeshInstruction2.endSlot || submeshInstruction.triangleCount != submeshInstruction2.triangleCount || submeshInstruction.vertexCount != submeshInstruction2.vertexCount || submeshInstruction.firstVertexIndex != submeshInstruction2.firstVertexIndex)
					{
						return true;
					}
					j++;
				}
				return false;
			}

			// Token: 0x040041DF RID: 16863
			public readonly Mesh mesh = SpineMesh.NewMesh();

			// Token: 0x040041E0 RID: 16864
			private readonly ExposedList<Attachment> attachmentsUsed = new ExposedList<Attachment>();

			// Token: 0x040041E1 RID: 16865
			private readonly ExposedList<SubmeshInstruction> instructionsUsed = new ExposedList<SubmeshInstruction>();
		}
	}
}
