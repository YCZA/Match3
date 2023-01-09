using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.MeshGeneration
{
	// Token: 0x0200022D RID: 557
	public class ArraysMeshGenerator
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0002F59C File Offset: 0x0002D99C
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x0002F5A4 File Offset: 0x0002D9A4
		public bool PremultiplyVertexColors
		{
			get
			{
				return this.premultiplyVertexColors;
			}
			set
			{
				this.premultiplyVertexColors = value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0002F5AD File Offset: 0x0002D9AD
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x0002F5B5 File Offset: 0x0002D9B5
		public bool GenerateNormals
		{
			get
			{
				return this.generateNormals;
			}
			set
			{
				this.generateNormals = value;
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0002F5C0 File Offset: 0x0002D9C0
		public void TryAddNormalsTo(Mesh mesh, int targetVertexCount)
		{
			if (this.generateNormals)
			{
				bool flag = this.meshNormals == null || targetVertexCount > this.meshNormals.Length;
				if (flag)
				{
					this.meshNormals = new Vector3[targetVertexCount];
					Vector3 vector = new Vector3(0f, 0f, -1f);
					Vector3[] array = this.meshNormals;
					for (int i = 0; i < targetVertexCount; i++)
					{
						array[i] = vector;
					}
				}
				mesh.normals = this.meshNormals;
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0002F64C File Offset: 0x0002DA4C
		public static bool EnsureSize(int targetVertexCount, ref Vector3[] vertices, ref Vector2[] uvs, ref Color32[] colors)
		{
			Vector3[] array = vertices;
			bool flag = array == null || targetVertexCount > array.Length;
			if (flag)
			{
				vertices = new Vector3[targetVertexCount];
				colors = new Color32[targetVertexCount];
				uvs = new Vector2[targetVertexCount];
			}
			else
			{
				Vector3 zero = Vector3.zero;
				int i = targetVertexCount;
				int num = array.Length;
				while (i < num)
				{
					array[i] = zero;
					i++;
				}
			}
			return flag;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0002F6BC File Offset: 0x0002DABC
		public static bool EnsureTriangleBuffersSize(ExposedList<ArraysMeshGenerator.SubmeshTriangleBuffer> submeshBuffers, int targetSubmeshCount, SubmeshInstruction[] instructionItems)
		{
			bool flag = submeshBuffers.Count < targetSubmeshCount;
			if (flag)
			{
				submeshBuffers.GrowIfNeeded(targetSubmeshCount - submeshBuffers.Count);
				int num = submeshBuffers.Count;
				while (submeshBuffers.Count < targetSubmeshCount)
				{
					submeshBuffers.Add(new ArraysMeshGenerator.SubmeshTriangleBuffer(instructionItems[num].triangleCount));
					num++;
				}
			}
			return flag;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0002F71C File Offset: 0x0002DB1C
		public static void FillVerts(Skeleton skeleton, int startSlot, int endSlot, float zSpacing, bool pmaColors, Vector3[] verts, Vector2[] uvs, Color32[] colors, ref int vertexIndex, ref float[] tempVertBuffer, ref Vector3 boundsMin, ref Vector3 boundsMax)
		{
			Color32 color = default(Color32);
			Slot[] items = skeleton.DrawOrder.Items;
			float num = skeleton.a * 255f;
			float r = skeleton.r;
			float g = skeleton.g;
			float b = skeleton.b;
			int num2 = vertexIndex;
			float[] array = tempVertBuffer;
			Vector3 vector = boundsMin;
			Vector3 vector2 = boundsMax;
			for (int i = startSlot; i < endSlot; i++)
			{
				Slot slot = items[i];
				Attachment attachment = slot.attachment;
				float z = (float)i * zSpacing;
				RegionAttachment regionAttachment = attachment as RegionAttachment;
				if (regionAttachment != null)
				{
					regionAttachment.ComputeWorldVertices(slot.bone, array);
					float num3 = array[0];
					float num4 = array[1];
					float num5 = array[2];
					float num6 = array[3];
					float num7 = array[4];
					float num8 = array[5];
					float num9 = array[6];
					float num10 = array[7];
					verts[num2].x = num3;
					verts[num2].y = num4;
					verts[num2].z = z;
					verts[num2 + 1].x = num9;
					verts[num2 + 1].y = num10;
					verts[num2 + 1].z = z;
					verts[num2 + 2].x = num5;
					verts[num2 + 2].y = num6;
					verts[num2 + 2].z = z;
					verts[num2 + 3].x = num7;
					verts[num2 + 3].y = num8;
					verts[num2 + 3].z = z;
					if (pmaColors)
					{
						color.a = (byte)(num * slot.a * regionAttachment.a);
						color.r = (byte)(r * slot.r * regionAttachment.r * (float)color.a);
						color.g = (byte)(g * slot.g * regionAttachment.g * (float)color.a);
						color.b = (byte)(b * slot.b * regionAttachment.b * (float)color.a);
						if (slot.data.blendMode == BlendMode.additive)
						{
							color.a = 0;
						}
					}
					else
					{
						color.a = (byte)(num * slot.a * regionAttachment.a);
						color.r = (byte)(r * slot.r * regionAttachment.r * 255f);
						color.g = (byte)(g * slot.g * regionAttachment.g * 255f);
						color.b = (byte)(b * slot.b * regionAttachment.b * 255f);
					}
					colors[num2] = color;
					colors[num2 + 1] = color;
					colors[num2 + 2] = color;
					colors[num2 + 3] = color;
					float[] uvs2 = regionAttachment.uvs;
					uvs[num2].x = uvs2[0];
					uvs[num2].y = uvs2[1];
					uvs[num2 + 1].x = uvs2[6];
					uvs[num2 + 1].y = uvs2[7];
					uvs[num2 + 2].x = uvs2[2];
					uvs[num2 + 2].y = uvs2[3];
					uvs[num2 + 3].x = uvs2[4];
					uvs[num2 + 3].y = uvs2[5];
					if (num3 < vector.x)
					{
						vector.x = num3;
					}
					else if (num3 > vector2.x)
					{
						vector2.x = num3;
					}
					if (num5 < vector.x)
					{
						vector.x = num5;
					}
					else if (num5 > vector2.x)
					{
						vector2.x = num5;
					}
					if (num7 < vector.x)
					{
						vector.x = num7;
					}
					else if (num7 > vector2.x)
					{
						vector2.x = num7;
					}
					if (num9 < vector.x)
					{
						vector.x = num9;
					}
					else if (num9 > vector2.x)
					{
						vector2.x = num9;
					}
					if (num4 < vector.y)
					{
						vector.y = num4;
					}
					else if (num4 > vector2.y)
					{
						vector2.y = num4;
					}
					if (num6 < vector.y)
					{
						vector.y = num6;
					}
					else if (num6 > vector2.y)
					{
						vector2.y = num6;
					}
					if (num8 < vector.y)
					{
						vector.y = num8;
					}
					else if (num8 > vector2.y)
					{
						vector2.y = num8;
					}
					if (num10 < vector.y)
					{
						vector.y = num10;
					}
					else if (num10 > vector2.y)
					{
						vector2.y = num10;
					}
					num2 += 4;
				}
				else
				{
					MeshAttachment meshAttachment = attachment as MeshAttachment;
					if (meshAttachment != null)
					{
						int num11 = meshAttachment.vertices.Length;
						if (array.Length < num11)
						{
							array = new float[num11];
						}
						meshAttachment.ComputeWorldVertices(slot, array);
						if (pmaColors)
						{
							color.a = (byte)(num * slot.a * meshAttachment.a);
							color.r = (byte)(r * slot.r * meshAttachment.r * (float)color.a);
							color.g = (byte)(g * slot.g * meshAttachment.g * (float)color.a);
							color.b = (byte)(b * slot.b * meshAttachment.b * (float)color.a);
							if (slot.data.blendMode == BlendMode.additive)
							{
								color.a = 0;
							}
						}
						else
						{
							color.a = (byte)(num * slot.a * meshAttachment.a);
							color.r = (byte)(r * slot.r * meshAttachment.r * 255f);
							color.g = (byte)(g * slot.g * meshAttachment.g * 255f);
							color.b = (byte)(b * slot.b * meshAttachment.b * 255f);
						}
						float[] uvs3 = meshAttachment.uvs;
						for (int j = 0; j < num11; j += 2)
						{
							float num12 = array[j];
							float num13 = array[j + 1];
							verts[num2].x = num12;
							verts[num2].y = num13;
							verts[num2].z = z;
							colors[num2] = color;
							uvs[num2].x = uvs3[j];
							uvs[num2].y = uvs3[j + 1];
							if (num12 < vector.x)
							{
								vector.x = num12;
							}
							else if (num12 > vector2.x)
							{
								vector2.x = num12;
							}
							if (num13 < vector.y)
							{
								vector.y = num13;
							}
							else if (num13 > vector2.y)
							{
								vector2.y = num13;
							}
							num2++;
						}
					}
					else
					{
						WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
						if (weightedMeshAttachment != null)
						{
							int num14 = 3 * weightedMeshAttachment.uvs.Length / 2;
							if (array.Length < num14)
							{
								array = new float[num14];
							}
							weightedMeshAttachment.ComputeWorldVertices(slot, array);
							if (pmaColors)
							{
								color.a = (byte)(num * slot.a * weightedMeshAttachment.a);
								color.r = (byte)(r * slot.r * weightedMeshAttachment.r * (float)color.a);
								color.g = (byte)(g * slot.g * weightedMeshAttachment.g * (float)color.a);
								color.b = (byte)(b * slot.b * weightedMeshAttachment.b * (float)color.a);
								if (slot.data.blendMode == BlendMode.additive)
								{
									color.a = 0;
								}
							}
							else
							{
								color.a = (byte)(num * slot.a * weightedMeshAttachment.a);
								color.r = (byte)(r * slot.r * weightedMeshAttachment.r * 255f);
								color.g = (byte)(g * slot.g * weightedMeshAttachment.g * 255f);
								color.b = (byte)(b * slot.b * weightedMeshAttachment.b * 255f);
							}
							float[] uvs4 = weightedMeshAttachment.uvs;
							int k = 0;
							int num15 = 0;
							while (k < num14)
							{
								float num16 = array[k];
								float num17 = array[k + 1];
								verts[num2].x = num16;
								verts[num2].y = num17;
								verts[num2].z = z;
								colors[num2] = color;
								uvs[num2].x = uvs4[num15];
								uvs[num2].y = uvs4[num15 + 1];
								if (num16 < vector.x)
								{
									vector.x = num16;
								}
								else if (num16 > vector2.x)
								{
									vector2.x = num16;
								}
								if (num17 < vector.y)
								{
									vector.y = num17;
								}
								else if (num17 > vector2.y)
								{
									vector2.y = num17;
								}
								num2++;
								k += 3;
								num15 += 2;
							}
						}
					}
				}
			}
			vertexIndex = num2;
			tempVertBuffer = array;
			boundsMin = vector;
			boundsMax = vector2;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0003015C File Offset: 0x0002E55C
		public static void FillTriangles(Skeleton skeleton, int triangleCount, int firstVertex, int startSlot, int endSlot, ref int[] triangleBuffer, bool isLastSubmesh)
		{
			int num = triangleBuffer.Length;
			int[] array = triangleBuffer;
			if (isLastSubmesh)
			{
				if (num > triangleCount)
				{
					for (int i = triangleCount; i < num; i++)
					{
						array[i] = 0;
					}
				}
				else if (num < triangleCount)
				{
					triangleBuffer = (array = new int[triangleCount]);
				}
			}
			else if (num != triangleCount)
			{
				triangleBuffer = (array = new int[triangleCount]);
			}
			int num2 = 0;
			int num3 = firstVertex;
			Slot[] items = skeleton.drawOrder.Items;
			for (int j = startSlot; j < endSlot; j++)
			{
				Attachment attachment = items[j].attachment;
				if (attachment is RegionAttachment)
				{
					array[num2] = num3;
					array[num2 + 1] = num3 + 2;
					array[num2 + 2] = num3 + 1;
					array[num2 + 3] = num3 + 2;
					array[num2 + 4] = num3 + 3;
					array[num2 + 5] = num3 + 1;
					num2 += 6;
					num3 += 4;
				}
				else
				{
					MeshAttachment meshAttachment = attachment as MeshAttachment;
					int num4;
					int[] triangles;
					if (meshAttachment != null)
					{
						num4 = meshAttachment.vertices.Length >> 1;
						triangles = meshAttachment.triangles;
					}
					else
					{
						WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
						if (weightedMeshAttachment == null)
						{
							goto IL_161;
						}
						num4 = weightedMeshAttachment.uvs.Length >> 1;
						triangles = weightedMeshAttachment.triangles;
					}
					int k = 0;
					int num5 = triangles.Length;
					while (k < num5)
					{
						array[num2] = num3 + triangles[k];
						k++;
						num2++;
					}
					num3 += num4;
				}
				IL_161:;
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x000302DC File Offset: 0x0002E6DC
		public static Bounds ToBounds(Vector3 boundsMin, Vector3 boundsMax)
		{
			Vector3 vector = boundsMax - boundsMin;
			Vector3 center = boundsMin + vector * 0.5f;
			return new Bounds(center, vector);
		}

		// Token: 0x040041C3 RID: 16835
		protected bool premultiplyVertexColors = true;

		// Token: 0x040041C4 RID: 16836
		protected float[] attachmentVertexBuffer = new float[8];

		// Token: 0x040041C5 RID: 16837
		protected Vector3[] meshVertices;

		// Token: 0x040041C6 RID: 16838
		protected Color32[] meshColors32;

		// Token: 0x040041C7 RID: 16839
		protected Vector2[] meshUVs;

		// Token: 0x040041C8 RID: 16840
		protected bool generateNormals;

		// Token: 0x040041C9 RID: 16841
		private Vector3[] meshNormals;

		// Token: 0x0200022E RID: 558
		public class SubmeshTriangleBuffer
		{
			// Token: 0x06001199 RID: 4505 RVA: 0x0003030A File Offset: 0x0002E70A
			public SubmeshTriangleBuffer(int triangleCount)
			{
				this.triangles = new int[triangleCount];
			}

			// Token: 0x040041CA RID: 16842
			public int[] triangles;
		}
	}
}
