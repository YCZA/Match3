using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x02000230 RID: 560
	public class ArraysSubmeshedMeshGenerator : ArraysMeshGenerator, ISubmeshedMeshGenerator
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x0003067F File Offset: 0x0002EA7F
		public List<Slot> Separators
		{
			get
			{
				return this.separators;
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00030688 File Offset: 0x0002EA88
		public SubmeshedMeshInstruction GenerateInstruction(Skeleton skeleton)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton");
			}
			int num = 0;
			int num2 = 0;
			int firstVertexIndex = 0;
			int num3 = 0;
			int startSlot = 0;
			Material material = null;
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			Slot[] items = drawOrder.Items;
			int count = drawOrder.Count;
			int count2 = this.separators.Count;
			ExposedList<SubmeshInstruction> submeshInstructions = this.currentInstructions.submeshInstructions;
			submeshInstructions.Clear(false);
			this.currentInstructions.attachmentList.Clear(false);
			int i = 0;
			while (i < count)
			{
				Slot slot = items[i];
				Attachment attachment = slot.attachment;
				RegionAttachment regionAttachment = attachment as RegionAttachment;
				object rendererObject;
				int num4;
				int num5;
				if (regionAttachment != null)
				{
					rendererObject = regionAttachment.RendererObject;
					num4 = 4;
					num5 = 6;
					goto IL_119;
				}
				MeshAttachment meshAttachment = attachment as MeshAttachment;
				if (meshAttachment != null)
				{
					rendererObject = meshAttachment.RendererObject;
					num4 = meshAttachment.vertices.Length >> 1;
					num5 = meshAttachment.triangles.Length;
					goto IL_119;
				}
				WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
				if (weightedMeshAttachment != null)
				{
					rendererObject = weightedMeshAttachment.RendererObject;
					num4 = weightedMeshAttachment.uvs.Length >> 1;
					num5 = weightedMeshAttachment.triangles.Length;
					goto IL_119;
				}
				IL_1F0:
				i++;
				continue;
				IL_119:
				Material material2 = (Material)((AtlasRegion)rendererObject).page.rendererObject;
				bool flag = count2 > 0 && this.separators.Contains(slot);
				if ((num > 0 && material.GetInstanceID() != material2.GetInstanceID()) || flag)
				{
					submeshInstructions.Add(new SubmeshInstruction
					{
						skeleton = skeleton,
						material = material,
						triangleCount = num2,
						vertexCount = num3,
						startSlot = startSlot,
						endSlot = i,
						firstVertexIndex = firstVertexIndex,
						forceSeparate = flag
					});
					num2 = 0;
					num3 = 0;
					firstVertexIndex = num;
					startSlot = i;
				}
				material = material2;
				num2 += num5;
				num3 += num4;
				num += num4;
				this.currentInstructions.attachmentList.Add(attachment);
				goto IL_1F0;
			}
			submeshInstructions.Add(new SubmeshInstruction
			{
				skeleton = skeleton,
				material = material,
				triangleCount = num2,
				vertexCount = num3,
				startSlot = startSlot,
				endSlot = count,
				firstVertexIndex = firstVertexIndex,
				forceSeparate = false
			});
			this.currentInstructions.vertexCount = num;
			return this.currentInstructions;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000308FC File Offset: 0x0002ECFC
		public MeshAndMaterials GenerateMesh(SubmeshedMeshInstruction meshInstructions)
		{
			ArraysSubmeshedMeshGenerator.SmartMesh next = this.doubleBufferedSmartMesh.GetNext();
			Mesh mesh = next.mesh;
			int count = meshInstructions.submeshInstructions.Count;
			ExposedList<SubmeshInstruction> submeshInstructions = meshInstructions.submeshInstructions;
			bool flag = ArraysMeshGenerator.EnsureTriangleBuffersSize(this.submeshBuffers, count, submeshInstructions.Items);
			bool flag2 = ArraysMeshGenerator.EnsureSize(meshInstructions.vertexCount, ref this.meshVertices, ref this.meshUVs, ref this.meshColors32);
			Vector3[] meshVertices = this.meshVertices;
			float num = this.zSpacing;
			int count2 = meshInstructions.attachmentList.Count;
			Vector3 boundsMin;
			Vector3 boundsMax;
			if (count2 <= 0)
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
				if (num > 0f)
				{
					boundsMin.z = 0f;
					boundsMax.z = num * (float)(count2 - 1);
				}
				else
				{
					boundsMin.z = num * (float)(count2 - 1);
					boundsMax.z = 0f;
				}
			}
			bool flag3 = flag2 || flag || next.StructureDoesntMatch(meshInstructions);
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				SubmeshInstruction submeshInstruction = submeshInstructions.Items[i];
				int startSlot = submeshInstruction.startSlot;
				int endSlot = submeshInstruction.endSlot;
				Skeleton skeleton = submeshInstruction.skeleton;
				ArraysMeshGenerator.FillVerts(skeleton, startSlot, endSlot, num, this.premultiplyVertexColors, meshVertices, this.meshUVs, this.meshColors32, ref num2, ref this.attachmentVertexBuffer, ref boundsMin, ref boundsMax);
				if (flag3)
				{
					ArraysMeshGenerator.SubmeshTriangleBuffer submeshTriangleBuffer = this.submeshBuffers.Items[i];
					bool isLastSubmesh = i == count - 1;
					ArraysMeshGenerator.FillTriangles(skeleton, submeshInstruction.triangleCount, submeshInstruction.firstVertexIndex, startSlot, endSlot, ref submeshTriangleBuffer.triangles, isLastSubmesh);
				}
			}
			if (flag3)
			{
				mesh.Clear();
				this.sharedMaterials = meshInstructions.GetUpdatedMaterialArray(this.sharedMaterials);
			}
			next.Set(this.meshVertices, this.meshUVs, this.meshColors32, meshInstructions);
			mesh.bounds = ArraysMeshGenerator.ToBounds(boundsMin, boundsMax);
			if (flag3)
			{
				mesh.subMeshCount = count;
				for (int j = 0; j < count; j++)
				{
					mesh.SetTriangles(this.submeshBuffers.Items[j].triangles, j);
				}
			}
			return new MeshAndMaterials(next.mesh, this.sharedMaterials);
		}

		// Token: 0x040041D0 RID: 16848
		private readonly List<Slot> separators = new List<Slot>();

		// Token: 0x040041D1 RID: 16849
		public float zSpacing;

		// Token: 0x040041D2 RID: 16850
		private readonly DoubleBuffered<ArraysSubmeshedMeshGenerator.SmartMesh> doubleBufferedSmartMesh = new DoubleBuffered<ArraysSubmeshedMeshGenerator.SmartMesh>();

		// Token: 0x040041D3 RID: 16851
		private readonly SubmeshedMeshInstruction currentInstructions = new SubmeshedMeshInstruction();

		// Token: 0x040041D4 RID: 16852
		private readonly ExposedList<ArraysMeshGenerator.SubmeshTriangleBuffer> submeshBuffers = new ExposedList<ArraysMeshGenerator.SubmeshTriangleBuffer>();

		// Token: 0x040041D5 RID: 16853
		private Material[] sharedMaterials = new Material[0];

		// Token: 0x02000231 RID: 561
		private class SmartMesh
		{
			// Token: 0x060011A4 RID: 4516 RVA: 0x00030BC0 File Offset: 0x0002EFC0
			public void Set(Vector3[] verts, Vector2[] uvs, Color32[] colors, SubmeshedMeshInstruction instruction)
			{
				this.mesh.vertices = verts;
				this.mesh.uv = uvs;
				this.mesh.colors32 = colors;
				this.attachmentsUsed.Clear(false);
				this.attachmentsUsed.GrowIfNeeded(instruction.attachmentList.Capacity);
				this.attachmentsUsed.Count = instruction.attachmentList.Count;
				instruction.attachmentList.CopyTo(this.attachmentsUsed.Items);
				this.instructionsUsed.Clear(false);
				this.instructionsUsed.GrowIfNeeded(instruction.submeshInstructions.Capacity);
				this.instructionsUsed.Count = instruction.submeshInstructions.Count;
				instruction.submeshInstructions.CopyTo(this.instructionsUsed.Items);
			}

			// Token: 0x060011A5 RID: 4517 RVA: 0x00030C94 File Offset: 0x0002F094
			public bool StructureDoesntMatch(SubmeshedMeshInstruction instructions)
			{
				if (instructions.attachmentList.Count != this.attachmentsUsed.Count)
				{
					return true;
				}
				if (instructions.submeshInstructions.Count != this.instructionsUsed.Count)
				{
					return true;
				}
				Attachment[] items = instructions.attachmentList.Items;
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
				SubmeshInstruction[] items3 = instructions.submeshInstructions.Items;
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

			// Token: 0x040041D6 RID: 16854
			public readonly Mesh mesh = SpineMesh.NewMesh();

			// Token: 0x040041D7 RID: 16855
			private readonly ExposedList<Attachment> attachmentsUsed = new ExposedList<Attachment>();

			// Token: 0x040041D8 RID: 16856
			private readonly ExposedList<SubmeshInstruction> instructionsUsed = new ExposedList<SubmeshInstruction>();
		}
	}
}
