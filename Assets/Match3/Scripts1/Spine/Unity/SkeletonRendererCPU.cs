using System;
using System.Collections.Generic;
using Match3.Scripts1.QStringUtil;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000261 RID: 609
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	[HelpURL("http://esotericsoftware.com/spine-unity-documentation#Rendering")]
	public class SkeletonRendererCPU : MonoBehaviour
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060012B6 RID: 4790 RVA: 0x00037E10 File Offset: 0x00036210
		// (remove) Token: 0x060012B7 RID: 4791 RVA: 0x00037E48 File Offset: 0x00036248
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event SkeletonRendererCPU.InstructionDelegate generateMeshOverride;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060012B8 RID: 4792 RVA: 0x00037E7E File Offset: 0x0003627E
		// (remove) Token: 0x060012B9 RID: 4793 RVA: 0x00037EB0 File Offset: 0x000362B0
		public event SkeletonRendererCPU.InstructionDelegate GenerateMeshOverride;

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060012BA RID: 4794 RVA: 0x00037EE2 File Offset: 0x000362E2
		public Dictionary<Material, Material> CustomMaterialOverride
		{
			get
			{
				return this.customMaterialOverride;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060012BB RID: 4795 RVA: 0x00037EEA File Offset: 0x000362EA
		public Dictionary<Slot, Material> CustomSlotMaterials
		{
			get
			{
				return this.customSlotMaterials;
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00037EF2 File Offset: 0x000362F2
		public static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			return SkeletonRenderer.AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00037F04 File Offset: 0x00036304
		public static T AddSpineComponent<T>(GameObject gameObject, SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			T t = gameObject.AddComponent<T>();
			if (skeletonDataAsset != null)
			{
				t.skeletonDataAsset = skeletonDataAsset;
				t.Initialize(false);
			}
			return t;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00037F3F File Offset: 0x0003633F
		public virtual void Awake()
		{
			this.Initialize(false);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x00037F48 File Offset: 0x00036348
		public virtual void Initialize(bool overwrite)
		{
			if (this.valid && !overwrite)
			{
				return;
			}
			if (this.meshFilter != null)
			{
				this.meshFilter.sharedMesh = null;
			}
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			if (this.meshRenderer != null)
			{
				this.meshRenderer.sharedMaterial = null;
			}
			this.currentInstructions.Clear();
			this.vertices = null;
			this.colors = null;
			this.uvs = null;
			this.sharedMaterials = new Material[0];
			this.submeshMaterials.Clear(true);
			this.submeshes.Clear(true);
			this.skeleton = null;
			this.valid = false;
			if (!this.skeletonDataAsset)
			{
				if (this.logErrors)
				{
					global::UnityEngine.Debug.LogError("Missing SkeletonData asset.", this);
				}
				return;
			}
			SkeletonData skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
			if (skeletonData == null)
			{
				return;
			}
			this.valid = true;
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.doubleBufferedMesh = new DoubleBuffered<SkeletonRendererCPU.SmartMesh>();
			this.vertices = new Vector3[0];
			this.skeleton = new Skeleton(skeletonData);
			if (this.initialSkinName != null && this.initialSkinName.Length > 0 && this.initialSkinName != "default")
			{
				this.skeleton.SetSkin(this.initialSkinName);
			}
			this.separatorSlots.Clear();
			for (int i = 0; i < this.separatorSlotNames.Length; i++)
			{
				this.separatorSlots.Add(this.skeleton.FindSlot(this.separatorSlotNames[i]));
			}
			this.LateUpdate();
			if (this.OnRebuild != null)
			{
				this.OnRebuild(this);
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x00038120 File Offset: 0x00036520
		public virtual void LateUpdate()
		{
			if (!this.valid)
			{
				return;
			}
			if (!this.meshRenderer.enabled && this.generateMeshOverride == null)
			{
				return;
			}
			ExposedList<Slot> drawOrder = this.skeleton.drawOrder;
			Slot[] items = drawOrder.Items;
			int count = drawOrder.Count;
			int count2 = this.separatorSlots.Count;
			bool flag = this.renderMeshes;
			SkeletonRendererCPU.SmartMesh.Instruction instruction = this.currentInstructions;
			ExposedList<Attachment> attachments = instruction.attachments;
			attachments.Clear(false);
			attachments.GrowIfNeeded(count);
			attachments.Count = count;
			Attachment[] items2 = instruction.attachments.Items;
			ExposedList<bool> attachmentFlips = instruction.attachmentFlips;
			attachmentFlips.Clear(false);
			attachmentFlips.GrowIfNeeded(count);
			attachmentFlips.Count = count;
			bool[] items3 = attachmentFlips.Items;
			ExposedList<SubmeshInstruction> submeshInstructions = instruction.submeshInstructions;
			submeshInstructions.Clear(false);
			bool flag2 = this.customSlotMaterials.Count > 0;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int firstVertexIndex = 0;
			int startSlot = 0;
			Material material = null;
			int i = 0;
			while (i < count)
			{
				Slot slot = items[i];
				Attachment attachment = slot.attachment;
				items2[i] = attachment;
				bool flag3 = this.frontFacing && slot.bone.WorldSignX != slot.bone.WorldSignY;
				items3[i] = flag3;
				RegionAttachment regionAttachment = attachment as RegionAttachment;
				int num4;
				int num5;
				object rendererObject;
				if (regionAttachment != null)
				{
					rendererObject = regionAttachment.RendererObject;
					num4 = 8;
					num5 = 12;
					goto IL_1DE;
				}
				if (flag)
				{
					MeshAttachment meshAttachment = attachment as MeshAttachment;
					if (meshAttachment != null)
					{
						rendererObject = meshAttachment.RendererObject;
						num4 = meshAttachment.vertices.Length >> 1;
						num5 = meshAttachment.triangles.Length;
						goto IL_1DE;
					}
					WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
					if (weightedMeshAttachment != null)
					{
						rendererObject = weightedMeshAttachment.RendererObject;
						num4 = weightedMeshAttachment.uvs.Length >> 1;
						num5 = weightedMeshAttachment.triangles.Length;
						goto IL_1DE;
					}
				}
				IL_2ED:
				i++;
				continue;
				IL_1DE:
				Material material2;
				if (flag2)
				{
					if (!this.customSlotMaterials.TryGetValue(slot, out material2))
					{
						material2 = (Material)((AtlasRegion)rendererObject).page.rendererObject;
					}
				}
				else
				{
					material2 = (Material)((AtlasRegion)rendererObject).page.rendererObject;
				}
				bool flag4 = count2 > 0 && this.separatorSlots.Contains(slot);
				if ((num > 0 && material.GetInstanceID() != material2.GetInstanceID()) || flag4)
				{
					submeshInstructions.Add(new SubmeshInstruction
					{
						skeleton = this.skeleton,
						material = material,
						startSlot = startSlot,
						endSlot = i,
						triangleCount = num3,
						firstVertexIndex = firstVertexIndex,
						vertexCount = num2,
						forceSeparate = flag4
					});
					num3 = 0;
					num2 = 0;
					firstVertexIndex = num;
					startSlot = i;
				}
				material = material2;
				num3 += num5;
				num += num4;
				num2 += num4;
				goto IL_2ED;
			}
			if (num2 != 0)
			{
				submeshInstructions.Add(new SubmeshInstruction
				{
					skeleton = this.skeleton,
					material = material,
					startSlot = startSlot,
					endSlot = count,
					triangleCount = num3,
					firstVertexIndex = firstVertexIndex,
					vertexCount = num2,
					forceSeparate = false
				});
			}
			instruction.vertexCount = num;
			instruction.immutableTriangles = this.immutableTriangles;
			instruction.frontFacing = this.frontFacing;
			if (this.customMaterialOverride.Count > 0)
			{
				SubmeshInstruction[] items4 = submeshInstructions.Items;
				for (int j = 0; j < submeshInstructions.Count; j++)
				{
					Material material3 = items4[j].material;
					Material material4;
					if (this.customMaterialOverride.TryGetValue(material3, out material4))
					{
						items4[j].material = material4;
					}
				}
			}
			if (this.generateMeshOverride != null)
			{
				this.generateMeshOverride(instruction);
				if (this.disableRenderingOnOverride)
				{
					return;
				}
			}
			Vector3[] array = this.vertices;
			bool flag5 = num > array.Length;
			if (flag5)
			{
				array = (this.vertices = new Vector3[num]);
				this.colors = new Color32[num];
				this.uvs = new Vector2[num];
				if (this.calculateNormals)
				{
					Vector3[] array2 = this.normals = new Vector3[num];
					Vector3 vector = new Vector3(0f, 0f, -1f);
					for (int k = 0; k < num; k++)
					{
						array2[k] = vector;
					}
				}
				if (this.calculateTangents)
				{
					Vector4[] array3 = this.tangents = new Vector4[num];
					Vector4 vector2 = new Vector4(1f, 0f, 0f, -1f);
					for (int l = 0; l < num; l++)
					{
						array3[l] = vector2;
					}
				}
			}
			else
			{
				Vector3 zero = Vector3.zero;
				int m = num;
				int num6 = array.Length;
				while (m < num6)
				{
					array[m] = zero;
					m++;
				}
			}
			float[] array4 = this.tempVertices;
			Vector2[] array5 = this.uvs;
			Color32[] array6 = this.colors;
			int num7 = 0;
			int n = 0;
			bool flag6 = this.pmaVertexColors;
			Color32 color = default(Color32);
			float num8 = this.skeleton.a * 255f;
			float r = this.skeleton.r;
			float g = this.skeleton.g;
			float b = this.skeleton.b;
			if (num > 0)
			{
				int num9 = 0;
				for (;;)
				{
					Slot slot2 = items[num9];
					Attachment attachment2 = slot2.attachment;
					RegionAttachment regionAttachment2 = attachment2 as RegionAttachment;
					if (attachment2 != null && attachment2.type == Attachment.Type.unknown)
					{
						if (attachment2.Name.QStartsWith("ground") || slot2.data.Name.QStartsWith("ground"))
						{
							attachment2.type = Attachment.Type.ground;
						}
						else
						{
							attachment2.type = Attachment.Type.basic;
						}
					}
					if (regionAttachment2 != null)
					{
						regionAttachment2.ComputeWorldVertices(slot2.bone, array4);
						float x = array4[0];
						float num10 = array4[1];
						float x2 = array4[2];
						float num11 = array4[3];
						float x3 = array4[4];
						float num12 = array4[5];
						float x4 = array4[6];
						float num13 = array4[7];
						Bone bone = slot2.bone;
						if (bone != this.skeleton.RootBone)
						{
							while (bone.parent != this.skeleton.RootBone)
							{
								bone = bone.parent;
							}
						}
						float num14 = (!this.use2d) ? bone.worldY : 0f;
						float z = num14 * 2f;
						array[num7] = new Vector3(x, num10 - num14, z);
						array[num7 + 1] = new Vector3(x4, num13 - num14, z);
						array[num7 + 2] = new Vector3(x2, num11 - num14, z);
						array[num7 + 3] = new Vector3(x3, num12 - num14, z);
						if (flag6)
						{
							color.a = (byte)(num8 * slot2.a * regionAttachment2.a);
							color.r = (byte)(r * slot2.r * regionAttachment2.r * (float)color.a);
							color.g = (byte)(g * slot2.g * regionAttachment2.g * (float)color.a);
							color.b = (byte)(b * slot2.b * regionAttachment2.b * (float)color.a);
							if (slot2.data.blendMode == BlendMode.additive)
							{
								color.a = 0;
							}
						}
						else
						{
							color.a = (byte)(num8 * slot2.a * regionAttachment2.a);
							color.r = (byte)(r * slot2.r * regionAttachment2.r * 255f);
							color.g = (byte)(g * slot2.g * regionAttachment2.g * 255f);
							color.b = (byte)(b * slot2.b * regionAttachment2.b * 255f);
						}
						array6[num7] = color;
						array6[num7 + 1] = color;
						array6[num7 + 2] = color;
						array6[num7 + 3] = color;
						float[] array7 = regionAttachment2.uvs;
						array5[num7] = new Vector2(array7[0], array7[1]);
						array5[num7 + 1] = new Vector2(array7[6], array7[7]);
						array5[num7 + 2] = new Vector2(array7[2], array7[3]);
						array5[num7 + 3] = new Vector2(array7[4], array7[5]);
						bool flag7 = true;
						if (regionAttachment2.type == Attachment.Type.ground)
						{
							for (int num15 = 0; num15 < 4; num15++)
							{
								array[num7 + num15 + 4] = array[num7 + num15];
								array5[num7 + num15 + 4] = array5[num7 + num15];
								array6[num7 + num15 + 4] = array6[num7 + num15];
								array[num7 + num15] = Vector3.zero;
							}
						}
						else if (array[num7].y < 0f && array[num7 + 2].y > 0f)
						{
							for (int num16 = 0; num16 < 4; num16++)
							{
								array[num7 + num16 + 4] = array[num7 + num16];
								array5[num7 + num16 + 4] = array5[num7 + num16];
								array6[num7 + num16 + 4] = array6[num7 + num16];
							}
							int num17 = num7;
							float t = array[num17 + 2].y / (array[num17 + 2].y - array[num17].y);
							float t2 = array[num17 + 3].y / (array[num17 + 3].y - array[num17 + 1].y);
							array[num17] = Vector3.Lerp(array[num17 + 2], array[num17], t);
							array[num17 + 6] = Vector3.Lerp(array[num17 + 6], array[num17 + 4], t);
							if (array[num7 + 1].y < 0f)
							{
								array[num17 + 1] = Vector3.Lerp(array[num17 + 3], array[num17 + 1], t2);
								array[num17 + 7] = Vector3.Lerp(array[num17 + 7], array[num17 + 5], t2);
							}
							else
							{
								flag7 = false;
							}
							array5[num17] = Vector2.Lerp(array5[num17 + 2], array5[num17], t);
							array5[num17 + 1] = Vector2.Lerp(array5[num17 + 3], array5[num17 + 1], t2);
							array5[num17 + 6] = Vector2.Lerp(array5[num17 + 6], array5[num17 + 4], t);
							array5[num17 + 7] = Vector2.Lerp(array5[num17 + 7], array5[num17 + 5], t2);
						}
						else
						{
							for (int num18 = 4; num18 < 8; num18++)
							{
								array[num7 + num18] = Vector3.zero;
							}
						}
						if (!this.use2d)
						{
							for (int num19 = num7; num19 < num7 + 4; num19++)
							{
								Vector3 vector3 = array[num19];
								array[num19] = new Vector3(vector3.x, vector3.y * this.__cos6, vector3.y * this.__sin6 + vector3.z);
							}
							for (int num20 = num7 + 4; num20 < num7 + 8; num20++)
							{
								Vector3 vector4 = array[num20];
								array[num20] = new Vector3(vector4.x, vector4.y * this.__cos2 * 2f, vector4.y * this.__sin2 * 2f + vector4.z);
							}
						}
						if (!flag7)
						{
							array[num7 + 5] = array[num7 + 1];
							array[num7 + 7] = array[num7 + 1];
						}
						num7 += 8;
						goto IL_14D3;
					}
					if (flag)
					{
						MeshAttachment meshAttachment2 = attachment2 as MeshAttachment;
						if (meshAttachment2 != null)
						{
							int num21 = meshAttachment2.vertices.Length;
							if (array4.Length < num21)
							{
								array4 = (this.tempVertices = new float[num21]);
							}
							meshAttachment2.ComputeWorldVertices(slot2, array4);
							if (flag6)
							{
								color.a = (byte)(num8 * slot2.a * meshAttachment2.a);
								color.r = (byte)(r * slot2.r * meshAttachment2.r * (float)color.a);
								color.g = (byte)(g * slot2.g * meshAttachment2.g * (float)color.a);
								color.b = (byte)(b * slot2.b * meshAttachment2.b * (float)color.a);
								if (slot2.data.blendMode == BlendMode.additive)
								{
									color.a = 0;
								}
							}
							else
							{
								color.a = (byte)(num8 * slot2.a * meshAttachment2.a);
								color.r = (byte)(r * slot2.r * meshAttachment2.r * 255f);
								color.g = (byte)(g * slot2.g * meshAttachment2.g * 255f);
								color.b = (byte)(b * slot2.b * meshAttachment2.b * 255f);
							}
							Bone bone2 = slot2.Bone;
							if (bone2 != this.skeleton.RootBone)
							{
								while (bone2.parent != this.skeleton.RootBone)
								{
									bone2 = bone2.parent;
								}
							}
							float num22 = (!this.use2d) ? bone2.worldY : 0f;
							float z2 = num22 * 2f;
							float[] array8 = meshAttachment2.uvs;
							if (meshAttachment2.type == Attachment.Type.ground)
							{
								int num23 = 0;
								while (num23 < num21)
								{
									Vector3 vector5 = new Vector3(array4[num23], array4[num23 + 1] - num22, z2);
									array[num7] = ((!this.use2d) ? new Vector3(vector5.x, 0f, 2f * vector5.y + vector5.z) : vector5);
									array6[num7] = color;
									array5[num7] = new Vector2(array8[num23], array8[num23 + 1]);
									num23 += 2;
									num7++;
								}
							}
							else
							{
								int num24 = 0;
								while (num24 < num21)
								{
									Vector3 vector6 = new Vector3(array4[num24], array4[num24 + 1] - num22, z2);
									array[num7] = ((!this.use2d) ? new Vector3(vector6.x, vector6.y * this.__cos6, vector6.y * this.__sin6 + vector6.z) : vector6);
									array6[num7] = color;
									array5[num7] = new Vector2(array8[num24], array8[num24 + 1]);
									num24 += 2;
									num7++;
								}
							}
							goto IL_14D3;
						}
						WeightedMeshAttachment weightedMeshAttachment2 = attachment2 as WeightedMeshAttachment;
						if (weightedMeshAttachment2 == null)
						{
							goto IL_14D3;
						}
						int num25 = 3 * weightedMeshAttachment2.uvs.Length / 2;
						if (array4.Length < num25)
						{
							array4 = (this.tempVertices = new float[num25]);
						}
						weightedMeshAttachment2.ComputeWorldVertices(slot2, array4);
						if (flag6)
						{
							color.a = (byte)(num8 * slot2.a * weightedMeshAttachment2.a);
							color.r = (byte)(r * slot2.r * weightedMeshAttachment2.r * (float)color.a);
							color.g = (byte)(g * slot2.g * weightedMeshAttachment2.g * (float)color.a);
							color.b = (byte)(b * slot2.b * weightedMeshAttachment2.b * (float)color.a);
							if (slot2.data.blendMode == BlendMode.additive)
							{
								color.a = 0;
							}
						}
						else
						{
							color.a = (byte)(num8 * slot2.a * weightedMeshAttachment2.a);
							color.r = (byte)(r * slot2.r * weightedMeshAttachment2.r * 255f);
							color.g = (byte)(g * slot2.g * weightedMeshAttachment2.g * 255f);
							color.b = (byte)(b * slot2.b * weightedMeshAttachment2.b * 255f);
						}
						float[] array9 = weightedMeshAttachment2.uvs;
						if (weightedMeshAttachment2.type == Attachment.Type.ground)
						{
							int num26 = 0;
							int num27 = 0;
							while (num26 < num25)
							{
								Vector3 vector7 = new Vector3(array4[num26], array4[num26 + 1], array4[num26 + 2]);
								array[num7] = ((!this.use2d) ? new Vector3(vector7.x, 0f, 2f * vector7.y + vector7.z) : new Vector3(vector7.x, vector7.y + vector7.z / 2f, 0f));
								array6[num7] = color;
								array5[num7] = new Vector2(array9[num27], array9[num27 + 1]);
								num26 += 3;
								num27 += 2;
								num7++;
							}
							goto IL_14D3;
						}
						int num28 = 0;
						int num29 = 0;
						while (num28 < num25)
						{
							Vector3 vector8 = new Vector3(array4[num28], array4[num28 + 1], array4[num28 + 2]);
							array[num7] = ((!this.use2d) ? new Vector3(vector8.x, vector8.y * this.__cos6, vector8.y * this.__sin6 + vector8.z) : new Vector3(vector8.x, vector8.y + vector8.z / 2f, 0f));
							array6[num7] = color;
							array5[num7] = new Vector2(array9[num29], array9[num29 + 1]);
							num28 += 3;
							num29 += 2;
							num7++;
						}
						goto IL_14D3;
					}
					IL_14E7:
					if (++num9 >= count)
					{
						break;
					}
					continue;
					IL_14D3:
					while (n < num7)
					{
						n++;
					}
					goto IL_14E7;
				}
			}
			SkeletonRendererCPU.SmartMesh next = this.doubleBufferedMesh.GetNext();
			Mesh mesh = next.mesh;
			float num30 = Mathf.Sin(0.7853982f);
			float num31 = Mathf.Cos(0.7853982f);
			if (this.flipped)
			{
				for (int num32 = 0; num32 < array.Length; num32++)
				{
					Vector3 vector9 = array[num32];
					vector9.x = -vector9.x;
					array[num32] = vector9;
				}
			}
			if (!this.use2d)
			{
				for (int num33 = 0; num33 < array.Length; num33++)
				{
					Vector3 vector10 = array[num33] * SkeletonRendererCPU.ROOT_OF_TWO;
					array[num33] = new Vector3(vector10.x * num31 + vector10.z * num30 + ((!this.skeletonDataAsset.centerAtPivot) ? 0.5f : 0f), vector10.y + 0.002f, vector10.z * num31 - vector10.x * num30 + ((!this.skeletonDataAsset.centerAtPivot) ? 0.5f : 0f));
				}
			}
			mesh.vertices = array;
			mesh.colors32 = array6;
			mesh.uv = array5;
			if (this._cachedBounds == null)
			{
				if (this.skeletonDataAsset.centerAtPivot)
				{
					this._cachedBounds = new Bounds?(BuildingLocation.RealignBounds(mesh.bounds, Vector3.zero));
				}
				else
				{
					this._cachedBounds = new Bounds?(BuildingLocation.RealignBounds(mesh.bounds));
				}
			}
			mesh.bounds = this._cachedBounds.Value;
			SkeletonRendererCPU.SmartMesh.Instruction instructionUsed = next.instructionUsed;
			if (instructionUsed.vertexCount < num)
			{
				if (this.calculateNormals)
				{
					mesh.normals = this.normals;
				}
				if (this.calculateTangents)
				{
					mesh.tangents = this.tangents;
				}
			}
			bool flag8 = SkeletonRendererCPU.CheckIfMustUpdateMeshStructure(instruction, instructionUsed);
			if (flag8)
			{
				ExposedList<Material> exposedList = this.submeshMaterials;
				exposedList.Clear(false);
				int count3 = submeshInstructions.Count;
				int count4 = this.submeshes.Count;
				this.submeshes.Capacity = count3;
				for (int num34 = count4; num34 < count3; num34++)
				{
					this.submeshes.Items[num34] = new SkeletonRendererCPU.SubmeshTriangleBuffer();
				}
				bool flag9 = !instruction.immutableTriangles;
				int num35 = 0;
				int num36 = count3 - 1;
				while (num35 < count3)
				{
					SubmeshInstruction submeshInstructions2 = submeshInstructions.Items[num35];
					if (flag9 || num35 >= count4)
					{
						this.SetSubmesh(num35, submeshInstructions2, this.currentInstructions.attachmentFlips, num35 == num36);
					}
					exposedList.Add(submeshInstructions2.material);
					num35++;
				}
				mesh.subMeshCount = count3;
				for (int num37 = 0; num37 < count3; num37++)
				{
					mesh.SetTriangles(this.submeshes.Items[num37].triangles, num37);
				}
			}
			Material[] array10 = this.sharedMaterials;
			bool flag10 = flag8 || array10.Length != submeshInstructions.Count;
			if (!flag10)
			{
				SubmeshInstruction[] items5 = submeshInstructions.Items;
				int num38 = 0;
				int num39 = array10.Length;
				while (num38 < num39)
				{
					if (array10[num38].GetInstanceID() != items5[num38].material.GetInstanceID())
					{
						flag10 = true;
						break;
					}
					num38++;
				}
			}
			if (flag10)
			{
				if (this.submeshMaterials.Count == this.sharedMaterials.Length)
				{
					this.submeshMaterials.CopyTo(this.sharedMaterials);
				}
				else
				{
					this.sharedMaterials = this.submeshMaterials.ToArray();
				}
				this.meshRenderer.sharedMaterials = this.sharedMaterials;
			}
			this.meshFilter.sharedMesh = mesh;
			next.instructionUsed.Set(instruction);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00039A48 File Offset: 0x00037E48
		private static bool CheckIfMustUpdateMeshStructure(SkeletonRendererCPU.SmartMesh.Instruction a, SkeletonRendererCPU.SmartMesh.Instruction b)
		{
			if (a.vertexCount != b.vertexCount)
			{
				return true;
			}
			if (a.immutableTriangles != b.immutableTriangles)
			{
				return true;
			}
			int count = b.attachments.Count;
			if (a.attachments.Count != count)
			{
				return true;
			}
			Attachment[] items = a.attachments.Items;
			Attachment[] items2 = b.attachments.Items;
			for (int i = 0; i < count; i++)
			{
				if (items[i] != items2[i])
				{
					return true;
				}
			}
			if (a.frontFacing != b.frontFacing)
			{
				return true;
			}
			if (a.frontFacing)
			{
				bool[] items3 = a.attachmentFlips.Items;
				bool[] items4 = b.attachmentFlips.Items;
				for (int j = 0; j < count; j++)
				{
					if (items3[j] != items4[j])
					{
						return true;
					}
				}
			}
			int count2 = a.submeshInstructions.Count;
			int count3 = b.submeshInstructions.Count;
			if (count2 != count3)
			{
				return true;
			}
			SubmeshInstruction[] items5 = a.submeshInstructions.Items;
			SubmeshInstruction[] items6 = b.submeshInstructions.Items;
			for (int k = 0; k < count3; k++)
			{
				SubmeshInstruction submeshInstruction = items5[k];
				SubmeshInstruction submeshInstruction2 = items6[k];
				if (submeshInstruction.vertexCount != submeshInstruction2.vertexCount || submeshInstruction.startSlot != submeshInstruction2.startSlot || submeshInstruction.endSlot != submeshInstruction2.endSlot || submeshInstruction.triangleCount != submeshInstruction2.triangleCount || submeshInstruction.firstVertexIndex != submeshInstruction2.firstVertexIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x00039C08 File Offset: 0x00038008
		private void SetSubmesh(int submeshIndex, SubmeshInstruction submeshInstructions, ExposedList<bool> flipStates, bool isLastSubmesh)
		{
			SkeletonRendererCPU.SubmeshTriangleBuffer submeshTriangleBuffer = this.submeshes.Items[submeshIndex];
			int[] array = submeshTriangleBuffer.triangles;
			int triangleCount = submeshInstructions.triangleCount;
			int num = submeshInstructions.firstVertexIndex;
			int num2 = array.Length;
			if (isLastSubmesh && num2 > triangleCount)
			{
				for (int i = triangleCount; i < num2; i++)
				{
					array[i] = 0;
				}
				submeshTriangleBuffer.triangleCount = triangleCount;
			}
			else if (num2 != triangleCount)
			{
				array = (submeshTriangleBuffer.triangles = new int[triangleCount]);
				submeshTriangleBuffer.triangleCount = 0;
			}
			if (!this.renderMeshes && !this.frontFacing)
			{
				if (submeshTriangleBuffer.firstVertex != num || submeshTriangleBuffer.triangleCount < triangleCount)
				{
					submeshTriangleBuffer.triangleCount = triangleCount;
					submeshTriangleBuffer.firstVertex = num;
					int j = 0;
					while (j < triangleCount)
					{
						array[j] = num;
						array[j + 1] = num + 2;
						array[j + 2] = num + 1;
						array[j + 3] = num + 2;
						array[j + 4] = num + 3;
						array[j + 5] = num + 1;
						j += 6;
						num += 4;
					}
				}
				return;
			}
			bool[] items = flipStates.Items;
			Slot[] items2 = this.skeleton.DrawOrder.Items;
			int num3 = 0;
			int k = submeshInstructions.startSlot;
			int endSlot = submeshInstructions.endSlot;
			while (k < endSlot)
			{
				Attachment attachment = items2[k].attachment;
				bool flag = this.frontFacing && items[k];
				if (attachment is RegionAttachment)
				{
					for (int l = 0; l < 2; l++)
					{
						if (!flag)
						{
							array[num3] = num;
							array[num3 + 1] = num + 2;
							array[num3 + 2] = num + 1;
							array[num3 + 3] = num + 2;
							array[num3 + 4] = num + 3;
							array[num3 + 5] = num + 1;
						}
						else
						{
							array[num3] = num + 1;
							array[num3 + 1] = num + 2;
							array[num3 + 2] = num;
							array[num3 + 3] = num + 1;
							array[num3 + 4] = num + 3;
							array[num3 + 5] = num + 2;
						}
						num3 += 6;
						num += 4;
					}
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
							goto IL_2E3;
						}
						num4 = weightedMeshAttachment.uvs.Length >> 1;
						triangles = weightedMeshAttachment.triangles;
					}
					if (flag)
					{
						int m = 0;
						int num5 = triangles.Length;
						while (m < num5)
						{
							array[num3 + 2] = num + triangles[m];
							array[num3 + 1] = num + triangles[m + 1];
							array[num3] = num + triangles[m + 2];
							m += 3;
							num3 += 3;
						}
					}
					else
					{
						int n = 0;
						int num6 = triangles.Length;
						while (n < num6)
						{
							array[num3] = num + triangles[n];
							n++;
							num3++;
						}
					}
					num += num4;
				}
				IL_2E3:
				k++;
			}
		}

		// Token: 0x040042E6 RID: 17126
		private static readonly float ROOT_OF_TWO = Mathf.Sqrt(2f);

		// Token: 0x040042E7 RID: 17127
		public SkeletonRendererCPU.SkeletonRendererDelegate OnRebuild;

		// Token: 0x040042E8 RID: 17128
		public SkeletonDataAsset skeletonDataAsset;

		// Token: 0x040042E9 RID: 17129
		public string initialSkinName;

		// Token: 0x040042EA RID: 17130
		public bool flipped;

		// Token: 0x040042EB RID: 17131
		public bool use2d;

		// Token: 0x040042EC RID: 17132
		public int wobbleAnimationCycles = 2;

		// Token: 0x040042ED RID: 17133
		public SkeletonRendererCPU.StartAnimationCycle wobbleAnimationStart;

		// Token: 0x040042EE RID: 17134
		[FormerlySerializedAs("submeshSeparators")]
		[SpineSlot("", "", false)]
		public string[] separatorSlotNames = new string[0];

		// Token: 0x040042EF RID: 17135
		[NonSerialized]
		public readonly List<Slot> separatorSlots = new List<Slot>();

		// Token: 0x040042F0 RID: 17136
		public bool renderMeshes = true;

		// Token: 0x040042F1 RID: 17137
		public bool immutableTriangles;

		// Token: 0x040042F2 RID: 17138
		public bool pmaVertexColors = true;

		// Token: 0x040042F3 RID: 17139
		public bool calculateNormals;

		// Token: 0x040042F4 RID: 17140
		public bool calculateTangents;

		// Token: 0x040042F5 RID: 17141
		public bool frontFacing;

		// Token: 0x040042F6 RID: 17142
		public bool logErrors;

		// Token: 0x040042F7 RID: 17143
		public bool disableRenderingOnOverride = true;

		// Token: 0x040042F9 RID: 17145
		private Bounds? _cachedBounds;

		// Token: 0x040042FA RID: 17146
		[NonSerialized]
		private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();

		// Token: 0x040042FB RID: 17147
		[NonSerialized]
		private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();

		// Token: 0x040042FC RID: 17148
		private MeshRenderer meshRenderer;

		// Token: 0x040042FD RID: 17149
		private MeshFilter meshFilter;

		// Token: 0x040042FE RID: 17150
		[NonSerialized]
		public bool valid;

		// Token: 0x040042FF RID: 17151
		[NonSerialized]
		public Skeleton skeleton;

		// Token: 0x04004300 RID: 17152
		private DoubleBuffered<SkeletonRendererCPU.SmartMesh> doubleBufferedMesh;

		// Token: 0x04004301 RID: 17153
		private readonly SkeletonRendererCPU.SmartMesh.Instruction currentInstructions = new SkeletonRendererCPU.SmartMesh.Instruction();

		// Token: 0x04004302 RID: 17154
		private readonly ExposedList<SkeletonRendererCPU.SubmeshTriangleBuffer> submeshes = new ExposedList<SkeletonRendererCPU.SubmeshTriangleBuffer>();

		// Token: 0x04004303 RID: 17155
		private readonly ExposedList<Material> submeshMaterials = new ExposedList<Material>();

		// Token: 0x04004304 RID: 17156
		private Material[] sharedMaterials = new Material[0];

		// Token: 0x04004305 RID: 17157
		private float[] tempVertices = new float[8];

		// Token: 0x04004306 RID: 17158
		private Vector3[] vertices;

		// Token: 0x04004307 RID: 17159
		private Color32[] colors;

		// Token: 0x04004308 RID: 17160
		private Vector2[] uvs;

		// Token: 0x04004309 RID: 17161
		private Vector3[] normals;

		// Token: 0x0400430A RID: 17162
		private Vector4[] tangents;

		// Token: 0x0400430B RID: 17163
		private readonly float __cos6 = Mathf.Cos(0.5235988f);

		// Token: 0x0400430C RID: 17164
		private readonly float __sin6 = Mathf.Sin(0.5235988f);

		// Token: 0x0400430D RID: 17165
		private readonly float __cos2 = Mathf.Cos(1.5707964f);

		// Token: 0x0400430E RID: 17166
		private readonly float __sin2 = Mathf.Sin(1.5707964f);

		// Token: 0x02000262 RID: 610
		// (Invoke) Token: 0x060012C5 RID: 4805
		public delegate void SkeletonRendererDelegate(SkeletonRendererCPU skeletonRenderer);

		// Token: 0x02000263 RID: 611
		public enum StartAnimationCycle
		{
			// Token: 0x04004310 RID: 17168
			left,
			// Token: 0x04004311 RID: 17169
			middleToRight,
			// Token: 0x04004312 RID: 17170
			right,
			// Token: 0x04004313 RID: 17171
			middleToLeft
		}

		// Token: 0x02000264 RID: 612
		// (Invoke) Token: 0x060012C9 RID: 4809
		public delegate void InstructionDelegate(SkeletonRendererCPU.SmartMesh.Instruction instruction);

		// Token: 0x02000265 RID: 613
		public class SmartMesh
		{
			// Token: 0x04004314 RID: 17172
			public Mesh mesh = SpineMesh.NewMesh();

			// Token: 0x04004315 RID: 17173
			public SkeletonRendererCPU.SmartMesh.Instruction instructionUsed = new SkeletonRendererCPU.SmartMesh.Instruction();

			// Token: 0x02000266 RID: 614
			public class Instruction
			{
				// Token: 0x060012CE RID: 4814 RVA: 0x00039F66 File Offset: 0x00038366
				public void Clear()
				{
					this.attachments.Clear(false);
					this.submeshInstructions.Clear(false);
					this.attachmentFlips.Clear(false);
				}

				// Token: 0x060012CF RID: 4815 RVA: 0x00039F8C File Offset: 0x0003838C
				public void Set(SkeletonRendererCPU.SmartMesh.Instruction other)
				{
					this.immutableTriangles = other.immutableTriangles;
					this.vertexCount = other.vertexCount;
					this.attachments.Clear(false);
					this.attachments.GrowIfNeeded(other.attachments.Capacity);
					this.attachments.Count = other.attachments.Count;
					other.attachments.CopyTo(this.attachments.Items);
					this.frontFacing = other.frontFacing;
					this.attachmentFlips.Clear(false);
					this.attachmentFlips.GrowIfNeeded(other.attachmentFlips.Capacity);
					this.attachmentFlips.Count = other.attachmentFlips.Count;
					if (this.frontFacing)
					{
						other.attachmentFlips.CopyTo(this.attachmentFlips.Items);
					}
					this.submeshInstructions.Clear(false);
					this.submeshInstructions.GrowIfNeeded(other.submeshInstructions.Capacity);
					this.submeshInstructions.Count = other.submeshInstructions.Count;
					other.submeshInstructions.CopyTo(this.submeshInstructions.Items);
				}

				// Token: 0x04004316 RID: 17174
				public bool immutableTriangles;

				// Token: 0x04004317 RID: 17175
				public int vertexCount = -1;

				// Token: 0x04004318 RID: 17176
				public readonly ExposedList<Attachment> attachments = new ExposedList<Attachment>();

				// Token: 0x04004319 RID: 17177
				public readonly ExposedList<SubmeshInstruction> submeshInstructions = new ExposedList<SubmeshInstruction>();

				// Token: 0x0400431A RID: 17178
				public bool frontFacing;

				// Token: 0x0400431B RID: 17179
				public readonly ExposedList<bool> attachmentFlips = new ExposedList<bool>();
			}
		}

		// Token: 0x02000267 RID: 615
		private class SubmeshTriangleBuffer
		{
			// Token: 0x0400431C RID: 17180
			public int[] triangles = new int[0];

			// Token: 0x0400431D RID: 17181
			public int triangleCount;

			// Token: 0x0400431E RID: 17182
			public int firstVertex = -1;
		}
	}
}
