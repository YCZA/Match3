using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200025A RID: 602
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	[HelpURL("http://esotericsoftware.com/spine-unity-documentation#Rendering")]
	public class SkeletonRenderer : MonoBehaviour
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00035FE2 File Offset: 0x000343E2
		public Dictionary<Material, Material> CustomMaterialOverride
		{
			get
			{
				return this.customMaterialOverride;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00035FEA File Offset: 0x000343EA
		public Dictionary<Slot, Material> CustomSlotMaterials
		{
			get
			{
				return this.customSlotMaterials;
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00035FF2 File Offset: 0x000343F2
		public static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			return SkeletonRenderer.AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00036004 File Offset: 0x00034404
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

		// Token: 0x060012A2 RID: 4770 RVA: 0x0003603F File Offset: 0x0003443F
		public virtual void Awake()
		{
			this.Initialize(false);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x00036048 File Offset: 0x00034448
		private static int FindRootBone(Bone bone)
		{
			if (bone == null || bone.parent == null)
			{
				return Array.IndexOf<Bone>(bone.skeleton.bones.Items, bone);
			}
			Bone rootBone = bone.parent.Skeleton.RootBone;
			if (bone == rootBone)
			{
				return Array.IndexOf<Bone>(bone.skeleton.bones.Items, bone);
			}
			while (bone.parent != rootBone)
			{
				bone = bone.parent;
			}
			return Array.IndexOf<Bone>(bone.skeleton.bones.Items, bone);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x000360DC File Offset: 0x000344DC
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
			this.m_propertyBlock = new MaterialPropertyBlock();
			this.skeleton = new Skeleton(skeletonData);
			if (!string.IsNullOrEmpty(this.initialSkinName) && this.initialSkinName != "default")
			{
				this.skeleton.SetSkin(this.initialSkinName);
			}
			this.separatorSlots.Clear();
			for (int i = 0; i < this.separatorSlotNames.Length; i++)
			{
				this.separatorSlots.Add(this.skeleton.FindSlot(this.separatorSlotNames[i]));
			}
			Bone[] items = this.skeleton.bones.Items;
			if (SkeletonRenderer._003C_003Ef__mg_0024cache0 == null)
			{
				SkeletonRenderer._003C_003Ef__mg_0024cache0 = new Converter<Bone, int>(SkeletonRenderer.FindRootBone);
			}
			this.m_rootBones = Array.ConvertAll<Bone, int>(items, SkeletonRenderer._003C_003Ef__mg_0024cache0);
			if (this.OnRebuild != null)
			{
				this.OnRebuild(this);
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0003626C File Offset: 0x0003466C
		private void OnDestroy()
		{
			if (this.meshCache != null)
			{
				foreach (KeyValuePair<int, Mesh> keyValuePair in this.meshCache)
				{
					if (keyValuePair.Value != null)
					{
						global::UnityEngine.Object.Destroy(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000362EC File Offset: 0x000346EC
		public void LateUpdate()
		{
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000362F0 File Offset: 0x000346F0
		private Mesh CreateSkinnedMesh()
		{
			for (int i = 0; i < Mathf.Min(35, this.skeleton.bones.Count); i++)
			{
				Vector4 vector = this.m_boneRotations[i];
				Vector2 vector2 = this.m_currentPositions[i];
				Vector2 vector3 = this.m_currentPositions[this.m_rootBones[i]];
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetRow(0, new Vector4(vector.x, vector.y, 0f, vector2.x));
				identity.SetRow(1, new Vector4(vector.z, vector.w, 0f, vector2.y - vector3.y));
				identity.SetRow(2, new Vector4(0f, 0f, 1f, vector3.y / 0.6979f));
				this.m_boneMatrices[i] = identity;
			}
			this.m_drawOrderHash = this.GetDrawOrderHash();
			if (this.skeleton.bones.Count > 35)
			{
				Log.Warning("SpineError", string.Format("Could not build mesh for {0}, too many bones {1}", base.name, this.skeleton.data.Bones.Count), null);
				return null;
			}
			return SkeletonRenderer.s_meshBuilder.Build(this.skeleton, this.m_boneMatrices, this.m_rootBones, this.skeletonDataAsset.centerAtPivot);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0003647C File Offset: 0x0003487C
		public virtual void FixedUpdate()
		{
			if (!this.valid)
			{
				return;
			}
			if (!this.meshRenderer.enabled)
			{
				return;
			}
			this.UpdateAnimation(false);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x000364A4 File Offset: 0x000348A4
		private int GetDrawOrderHash()
		{
			int num = 0;
			Slot[] items = this.skeleton.drawOrder.Items;
			for (int i = 0; i < this.skeleton.drawOrder.Count; i++)
			{
				Slot slot = items[i];
				if (slot.attachment != null)
				{
					num ^= slot.data.name.GetHashCode() * slot.attachment.Name.GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0003651C File Offset: 0x0003491C
		protected void UpdateAnimation(bool force)
		{
			if (this.skeleton == null)
			{
				return;
			}
			int num = (!this.flipped) ? 1 : -1;
			bool flag = this.m_flippedState == null || this.m_flippedState.Value != this.flipped || !base.enabled || force;
			int num2 = Mathf.Min(35, this.skeleton.bones.Count);
			for (int i = 0; i < num2; i++)
			{
				Bone bone = this.skeleton.bones.Items[i];
				float x = (bone.skeleton.x + bone.worldX) * (float)num;
				float y = bone.skeleton.y + bone.worldY;
				this.m_boneRotations[i] = new Vector4(bone.a * (float)num, bone.b * (float)num, bone.c, bone.d);
				this.m_currentPositions[i] = new Vector2(x, y);
			}
			if (flag)
			{
				for (int j = 0; j < num2; j++)
				{
					Vector2 vector = this.m_currentPositions[j];
					this.m_bonePositions[j] = new Vector4(vector.x, vector.y, vector.x, vector.y);
				}
			}
			else if (FixedFrameLerp.FillFrame == 0)
			{
				for (int k = 0; k < num2; k++)
				{
					Vector2 vector2 = this.m_currentPositions[k];
					Vector4 vector3 = this.m_bonePositions[k];
					vector3.x = vector2.x;
					vector3.y = vector2.y;
					this.m_bonePositions[k] = vector3;
				}
			}
			else
			{
				for (int l = 0; l < num2; l++)
				{
					Vector2 vector4 = this.m_currentPositions[l];
					Vector4 vector5 = this.m_bonePositions[l];
					vector5.z = vector4.x;
					vector5.w = vector4.y;
					this.m_bonePositions[l] = vector5;
				}
			}
			for (int m = 0; m < Mathf.Min(60, this.skeleton.slots.Count); m++)
			{
				this.m_slotColors[m] = this.skeleton.slots.Items[m].a;
			}
			if (flag)
			{
				this.m_propertyBlock.SetVectorArray("_Rotations0", this.m_boneRotations);
				this.m_propertyBlock.SetVectorArray("_Rotations1", this.m_boneRotations);
			}
			else
			{
				this.m_propertyBlock.SetVectorArray((FixedFrameLerp.FillFrame != 0) ? "_Rotations1" : "_Rotations0", this.m_boneRotations);
			}
			this.m_propertyBlock.SetVectorArray("_Positions", this.m_bonePositions);
			this.m_propertyBlock.SetFloatArray("_Colors", this.m_slotColors);
			this.m_propertyBlock.SetFloat("_centerAtTile", (float)((!this.skeletonDataAsset.centerAtPivot) ? 1 : 0));
			int drawOrderHash = this.GetDrawOrderHash();
			if (this.m_drawOrderHash != drawOrderHash || this.m_flippedState != this.flipped)
			{
				Mesh mesh = null;
				if (!this.meshCache.TryGetValue(drawOrderHash, out mesh))
				{
					mesh = this.CreateSkinnedMesh();
					mesh.name = string.Format("sr_{0}_{1}", base.gameObject.name, drawOrderHash);
					this.meshCache.Add(drawOrderHash, mesh);
				}
				this.meshFilter.sharedMesh = mesh;
			}
			this.m_flippedState = new bool?(this.flipped);
			this.meshRenderer.SetPropertyBlock(this.m_propertyBlock);
		}

		// Token: 0x040042A3 RID: 17059
		public SkeletonRenderer.SkeletonRendererDelegate OnRebuild;

		// Token: 0x040042A4 RID: 17060
		public SkeletonDataAsset skeletonDataAsset;

		// Token: 0x040042A5 RID: 17061
		public string initialSkinName;

		// Token: 0x040042A6 RID: 17062
		public bool flipped;

		// Token: 0x040042A7 RID: 17063
		public int wobbleAnimationCycles = 2;

		// Token: 0x040042A8 RID: 17064
		public SkeletonRenderer.StartAnimationCycle wobbleAnimationStart;

		// Token: 0x040042A9 RID: 17065
		private Dictionary<int, Mesh> meshCache = new Dictionary<int, Mesh>();

		// Token: 0x040042AA RID: 17066
		[FormerlySerializedAs("submeshSeparators")]
		[SpineSlot("", "", false)]
		public string[] separatorSlotNames = new string[0];

		// Token: 0x040042AB RID: 17067
		[NonSerialized]
		public readonly List<Slot> separatorSlots = new List<Slot>();

		// Token: 0x040042AC RID: 17068
		public bool renderMeshes = true;

		// Token: 0x040042AD RID: 17069
		public bool immutableTriangles;

		// Token: 0x040042AE RID: 17070
		public bool pmaVertexColors = true;

		// Token: 0x040042AF RID: 17071
		public bool calculateNormals;

		// Token: 0x040042B0 RID: 17072
		public bool calculateTangents;

		// Token: 0x040042B1 RID: 17073
		public bool frontFacing;

		// Token: 0x040042B2 RID: 17074
		public bool logErrors;

		// Token: 0x040042B3 RID: 17075
		[NonSerialized]
		private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();

		// Token: 0x040042B4 RID: 17076
		[NonSerialized]
		private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();

		// Token: 0x040042B5 RID: 17077
		private MeshRenderer meshRenderer;

		// Token: 0x040042B6 RID: 17078
		private MeshFilter meshFilter;

		// Token: 0x040042B7 RID: 17079
		[NonSerialized]
		public bool valid;

		// Token: 0x040042B8 RID: 17080
		[NonSerialized]
		public Skeleton skeleton;

		// Token: 0x040042B9 RID: 17081
		private static readonly List<SkeletonRenderer.SpineBone> s_skinBuffer = new List<SkeletonRenderer.SpineBone>(64);

		// Token: 0x040042BA RID: 17082
		private static readonly Vector2[] s_skinPos = new Vector2[]
		{
			Vector2.zero,
			Vector2.zero,
			Vector2.zero
		};

		// Token: 0x040042BB RID: 17083
		private static readonly SkeletonRenderer.MeshBuilder s_meshBuilder = new SkeletonRenderer.MeshBuilder();

		// Token: 0x040042BC RID: 17084
		private const int BONES_COUNT = 35;

		// Token: 0x040042BD RID: 17085
		private const int SLOTS_COUNT = 60;

		// Token: 0x040042BE RID: 17086
		private readonly Vector4[] m_boneRotations = new Vector4[35];

		// Token: 0x040042BF RID: 17087
		private readonly Vector4[] m_bonePositions = new Vector4[35];

		// Token: 0x040042C0 RID: 17088
		private readonly Matrix4x4[] m_boneMatrices = new Matrix4x4[35];

		// Token: 0x040042C1 RID: 17089
		private readonly Vector2[] m_currentPositions = new Vector2[35];

		// Token: 0x040042C2 RID: 17090
		private readonly float[] m_slotColors = new float[60];

		// Token: 0x040042C3 RID: 17091
		private MaterialPropertyBlock m_propertyBlock;

		// Token: 0x040042C4 RID: 17092
		private int[] m_rootBones = new int[35];

		// Token: 0x040042C5 RID: 17093
		private int m_drawOrderHash;

		// Token: 0x040042C6 RID: 17094
		private bool? m_flippedState;

		// Token: 0x040042C7 RID: 17095
		private const string rotations0 = "_Rotations0";

		// Token: 0x040042C8 RID: 17096
		private const string rotations1 = "_Rotations1";

		// Token: 0x040042C9 RID: 17097
		private const string positions = "_Positions";

		// Token: 0x040042CA RID: 17098
		private const string colors = "_Colors";

		// Token: 0x040042CB RID: 17099
		private const string centerAtTile = "_centerAtTile";

		// Token: 0x040042CC RID: 17100
		[CompilerGenerated]
		private static Converter<Bone, int> _003C_003Ef__mg_0024cache0;

		// Token: 0x0200025B RID: 603
		// (Invoke) Token: 0x060012AD RID: 4781
		public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);

		// Token: 0x0200025C RID: 604
		public enum StartAnimationCycle
		{
			// Token: 0x040042CE RID: 17102
			left,
			// Token: 0x040042CF RID: 17103
			middleToRight,
			// Token: 0x040042D0 RID: 17104
			right,
			// Token: 0x040042D1 RID: 17105
			middleToLeft
		}

		// Token: 0x0200025D RID: 605
		private struct SpineBone
		{
			// Token: 0x040042D2 RID: 17106
			public Vector2 position;

			// Token: 0x040042D3 RID: 17107
			public float weight;

			// Token: 0x040042D4 RID: 17108
			public int index;

			// Token: 0x040042D5 RID: 17109
			public int root;
		}

		// Token: 0x0200025E RID: 606
		private class MeshBuilder
		{
			// Token: 0x060012B1 RID: 4785 RVA: 0x00036A78 File Offset: 0x00034E78
			public Mesh Build(Skeleton skeleton, Matrix4x4[] bones, int[] rootBones, bool centerAtPivot)
			{
				int verticesCount = 0;
				int trianglesCount = 0;
				foreach (Slot slot in skeleton.drawOrder)
				{
					Attachment attachment = slot.attachment;
					if (attachment != null)
					{
						if (attachment is RegionAttachment)
						{
							verticesCount += 10;
							trianglesCount += this.REGION_TRIANGLES.Length;
						}
						else if (attachment is WeightedMeshAttachment)
						{
							WeightedMeshAttachment weightedMeshAttachment = attachment as WeightedMeshAttachment;
							verticesCount += weightedMeshAttachment.uvs.Length / 2;
							trianglesCount += weightedMeshAttachment.triangles.Length;
						}
						else if (attachment is MeshAttachment)
						{
							MeshAttachment meshAttachment = attachment as MeshAttachment;
							verticesCount += meshAttachment.uvs.Length / 2;
							trianglesCount += meshAttachment.triangles.Length;
						}
					}
				}
				SkeletonRenderer.MeshBuilder.VertexBuffers vertexBuffers = Array.Find<SkeletonRenderer.MeshBuilder.VertexBuffers>(this.vertexBuffers, (SkeletonRenderer.MeshBuilder.VertexBuffers b) => b.size >= verticesCount);
				if (vertexBuffers == null)
				{
					Log.Warning("SpineError", string.Format("Could not find vertex buffer for mesh {0} of {1} vertices", skeleton.data.name, verticesCount), null);
					return new Mesh();
				}
				SkeletonRenderer.MeshBuilder.TriangleBuffer triangleBuffer = Array.Find<SkeletonRenderer.MeshBuilder.TriangleBuffer>(this.triangleBuffers, (SkeletonRenderer.MeshBuilder.TriangleBuffer b) => b.size >= trianglesCount);
				if (triangleBuffer == null)
				{
					Log.Warning("SpineError", string.Format("Could not find triangle buffer for mesh {0} of {1} triangles", skeleton.data.name, trianglesCount), null);
					return new Mesh();
				}
				int num = 0;
				int num2 = 0;
				Array.Clear(triangleBuffer.triangles, 0, triangleBuffer.triangles.Length);
				foreach (Slot slot2 in skeleton.drawOrder)
				{
					Attachment attachment2 = slot2.attachment;
					if (attachment2 != null)
					{
						int num3 = num;
						int num4 = num2;
						byte b4 = (byte)((!slot2.data.name.StartsWith("ground") && (slot2.bone == null || !slot2.bone.data.name.StartsWith("ground"))) ? 0 : 1);
						int num5 = Array.IndexOf<Slot>(skeleton.slots.Items, slot2);
						if (attachment2 is RegionAttachment)
						{
							RegionAttachment regionAttachment = attachment2 as RegionAttachment;
							float[] offset = regionAttachment.offset;
							float[] uvs = regionAttachment.uvs;
							byte b2 = (byte)Array.IndexOf<Bone>(skeleton.bones.Items, slot2.bone);
							int i = 0;
							while (i < 8)
							{
								int num6 = i++;
								int num7 = i++;
								vertexBuffers.positions[0][num] = new Vector2(offset[num6], offset[num7]);
								vertexBuffers.positions[1][num] = Vector2.zero;
								vertexBuffers.positions[2][num] = Vector2.zero;
								vertexBuffers.uv[num] = new Vector2(uvs[num6], uvs[num7]);
								vertexBuffers.boneWeights[num] = new Vector3(1f, 0f, 0f);
								vertexBuffers.boneRoots[num] = new Vector4((float)rootBones[(int)b2], 0f, 0f, (float)num5);
								vertexBuffers.boneIndices[num] = new Color32(b2, 0, 0, b4);
								num++;
							}
							Vector2[] array = vertexBuffers.positions[0];
							Vector2[] uv = vertexBuffers.uv;
							vertexBuffers.positions[0][num] = array[num3];
							vertexBuffers.positions[1][num] = array[num3 + 1];
							vertexBuffers.uv[num] = uv[num3];
							vertexBuffers.positions[2][num] = uv[num3 + 1];
							vertexBuffers.boneWeights[num] = new Vector3(1f, 0f, 0f);
							vertexBuffers.boneRoots[num] = new Vector4((float)rootBones[(int)b2], (float)rootBones[(int)b2], 0f, (float)num5);
							vertexBuffers.boneIndices[num] = new Color32(b2, b2, 0, (byte)(b4 | 2));
							num++;
							vertexBuffers.positions[0][num] = array[num3 + 3];
							vertexBuffers.positions[1][num] = array[num3 + 2];
							vertexBuffers.uv[num] = uv[num3 + 3];
							vertexBuffers.positions[2][num] = uv[num3 + 2];
							vertexBuffers.boneWeights[num] = new Vector3(1f, 0f, 0f);
							vertexBuffers.boneRoots[num] = new Vector4((float)rootBones[(int)b2], (float)rootBones[(int)b2], 0f, (float)num5);
							vertexBuffers.boneIndices[num] = new Color32(b2, b2, 0, (byte)(b4 | 2));
							num++;
							Array.Copy(this.REGION_TRIANGLES, 0, triangleBuffer.triangles, num2, this.REGION_TRIANGLES.Length);
							num2 += this.REGION_TRIANGLES.Length;
						}
						else if (attachment2 is WeightedMeshAttachment)
						{
							WeightedMeshAttachment weightedMeshAttachment2 = attachment2 as WeightedMeshAttachment;
							int j = 0;
							int num8 = 0;
							int num9 = weightedMeshAttachment2.bones.Length;
							int num10 = 0;
							while (j < num9)
							{
								Vector3 zero = Vector3.zero;
								Vector3 zero2 = Vector3.zero;
								Vector4 vector = new Vector4(0f, 0f, 0f, (float)num5);
								SkeletonRenderer.s_skinBuffer.Clear();
								Array.Clear(SkeletonRenderer.s_skinPos, 0, SkeletonRenderer.s_skinPos.Length);
								int num11 = weightedMeshAttachment2.bones[j++] + j;
								int num12 = 0;
								while (j < num11)
								{
									SkeletonRenderer.s_skinBuffer.Add(new SkeletonRenderer.SpineBone
									{
										position = new Vector2(weightedMeshAttachment2.weights[num8], weightedMeshAttachment2.weights[num8 + 1]),
										weight = weightedMeshAttachment2.weights[num8 + 2],
										index = weightedMeshAttachment2.bones[j],
										root = rootBones[weightedMeshAttachment2.bones[j]]
									});
									j++;
									num8 += 3;
									num12++;
								}
								SkeletonRenderer.s_skinBuffer.Sort((SkeletonRenderer.SpineBone a0, SkeletonRenderer.SpineBone a1) => a1.weight.CompareTo(a0.weight));
								for (int k = 0; k < Mathf.Min(3, SkeletonRenderer.s_skinBuffer.Count); k++)
								{
									SkeletonRenderer.SpineBone spineBone = SkeletonRenderer.s_skinBuffer[k];
									SkeletonRenderer.s_skinPos[k] = spineBone.position;
									zero[k] = spineBone.weight;
									zero2[k] = (float)spineBone.index;
									vector[k] = (float)spineBone.root;
								}
								vertexBuffers.boneWeights[num] = zero;
								vertexBuffers.boneRoots[num] = vector;
								vertexBuffers.boneIndices[num] = new Color32((byte)zero2.x, (byte)zero2.y, (byte)zero2.z, b4);
								vertexBuffers.uv[num] = new Vector2(weightedMeshAttachment2.uvs[num10++], weightedMeshAttachment2.uvs[num10++]);
								for (int l = 0; l < 3; l++)
								{
									vertexBuffers.positions[l][num] = SkeletonRenderer.s_skinPos[l];
								}
								num++;
							}
							Array.Copy(weightedMeshAttachment2.triangles, 0, triangleBuffer.triangles, num2, weightedMeshAttachment2.triangles.Length);
							num2 += weightedMeshAttachment2.triangles.Length;
						}
						else if (attachment2 is MeshAttachment)
						{
							MeshAttachment meshAttachment2 = attachment2 as MeshAttachment;
							byte b3 = (byte)Array.IndexOf<Bone>(skeleton.bones.Items, slot2.bone);
							int m = 0;
							while (m < meshAttachment2.uvs.Length)
							{
								vertexBuffers.uv[num] = new Vector2(meshAttachment2.uvs[m], meshAttachment2.uvs[m + 1]);
								vertexBuffers.positions[0][num] = new Vector2(meshAttachment2.vertices[m], meshAttachment2.vertices[m + 1]);
								vertexBuffers.positions[1][num] = Vector2.zero;
								vertexBuffers.positions[2][num] = Vector2.zero;
								vertexBuffers.boneWeights[num] = new Vector4(1f, 0f, 0f, 0f);
								vertexBuffers.boneRoots[num] = new Vector4((float)rootBones[(int)b3], 0f, 0f, (float)num5);
								vertexBuffers.boneIndices[num] = new Color32(b3, 0, 0, b4);
								m += 2;
								num++;
							}
							Array.Copy(meshAttachment2.triangles, 0, triangleBuffer.triangles, num2, meshAttachment2.triangles.Length);
							num2 += meshAttachment2.triangles.Length;
						}
						int[] triangles = triangleBuffer.triangles;
						for (int n = num4; n < num2; n++)
						{
							triangles[n] += num3;
						}
					}
				}
				float num13 = 0f;
				Matrix4x4 matrix4x = (!centerAtPivot) ? FixedFrameLerp._IsoTransform : FixedFrameLerp._IsoTransform2;
				Matrix4x4 matrix4x2 = (!centerAtPivot) ? FixedFrameLerp._GroundTransform : FixedFrameLerp._GroundTransform2;
				for (int num14 = 0; num14 < num; num14++)
				{
					Color32 color = vertexBuffers.boneIndices[num14];
					Vector3 vector2 = vertexBuffers.boneWeights[num14];
					Vector3 vector3 = (((vertexBuffers.boneIndices[num14].a & 1) != 0) ? matrix4x2 : matrix4x).MultiplyPoint3x4(bones[(int)color.r].MultiplyPoint3x4(vertexBuffers.positions[0][num14]) * vector2.x + bones[(int)color.g].MultiplyPoint3x4(vertexBuffers.positions[1][num14]) * vector2.y + bones[(int)color.b].MultiplyPoint3x4(vertexBuffers.positions[2][num14]) * vector2.z);
					num13 = Mathf.Max(vector3.y, num13);
					vertexBuffers.vertices[num14] = vector3;
				}
				return new Mesh
				{
					vertices = vertexBuffers.vertices,
					uv = vertexBuffers.uv,
					uv2 = vertexBuffers.positions[0],
					uv3 = vertexBuffers.positions[1],
					uv4 = vertexBuffers.positions[2],
					triangles = triangleBuffer.triangles,
					normals = vertexBuffers.boneWeights,
					tangents = vertexBuffers.boneRoots,
					colors32 = vertexBuffers.boneIndices,
					bounds = new Bounds((!centerAtPivot) ? new Vector3(0.625f, 0f, 0.375f) : Vector3.zero, new Vector3(1.25f, num13 * 2f, 1.25f))
				};
			}

			// Token: 0x040042D6 RID: 17110
			private const int VERTICES_BUFFER = 64;

			// Token: 0x040042D7 RID: 17111
			private const int TRIANGLES_BUFFER = 180;

			// Token: 0x040042D8 RID: 17112
			private const int MAX_BONES = 3;

			// Token: 0x040042D9 RID: 17113
			private readonly int[] REGION_TRIANGLES = new int[]
			{
				0,
				4,
				5,
				5,
				3,
				0,
				5,
				4,
				1,
				1,
				2,
				5
			};

			// Token: 0x040042DA RID: 17114
			private readonly SkeletonRenderer.MeshBuilder.VertexBuffers[] vertexBuffers = new SkeletonRenderer.MeshBuilder.VertexBuffers[]
			{
				new SkeletonRenderer.MeshBuilder.VertexBuffers(64),
				new SkeletonRenderer.MeshBuilder.VertexBuffers(128),
				new SkeletonRenderer.MeshBuilder.VertexBuffers(256),
				new SkeletonRenderer.MeshBuilder.VertexBuffers(512),
				new SkeletonRenderer.MeshBuilder.VertexBuffers(1024)
			};

			// Token: 0x040042DB RID: 17115
			private readonly SkeletonRenderer.MeshBuilder.TriangleBuffer[] triangleBuffers = new SkeletonRenderer.MeshBuilder.TriangleBuffer[]
			{
				new SkeletonRenderer.MeshBuilder.TriangleBuffer(180),
				new SkeletonRenderer.MeshBuilder.TriangleBuffer(540),
				new SkeletonRenderer.MeshBuilder.TriangleBuffer(1080),
				new SkeletonRenderer.MeshBuilder.TriangleBuffer(1620),
				new SkeletonRenderer.MeshBuilder.TriangleBuffer(2160)
			};

			// Token: 0x0200025F RID: 607
			private class VertexBuffers
			{
				// Token: 0x060012B3 RID: 4787 RVA: 0x000377E4 File Offset: 0x00035BE4
				public VertexBuffers(int size)
				{
					this.size = size;
					this.positions = new Vector2[][]
					{
						new Vector2[size],
						new Vector2[size],
						new Vector2[size]
					};
					this.uv = new Vector2[size];
					this.vertices = new Vector3[size];
					this.boneWeights = new Vector3[size];
					this.boneRoots = new Vector4[size];
					this.boneIndices = new Color32[size];
				}

				// Token: 0x040042DD RID: 17117
				public readonly Vector2[][] positions;

				// Token: 0x040042DE RID: 17118
				public readonly Vector2[] uv;

				// Token: 0x040042DF RID: 17119
				public readonly Vector3[] vertices;

				// Token: 0x040042E0 RID: 17120
				public readonly Vector3[] boneWeights;

				// Token: 0x040042E1 RID: 17121
				public readonly Vector4[] boneRoots;

				// Token: 0x040042E2 RID: 17122
				public readonly Color32[] boneIndices;

				// Token: 0x040042E3 RID: 17123
				public readonly int size;
			}

			// Token: 0x02000260 RID: 608
			private class TriangleBuffer
			{
				// Token: 0x060012B4 RID: 4788 RVA: 0x00037861 File Offset: 0x00035C61
				public TriangleBuffer(int size)
				{
					this.size = size;
					this.triangles = new int[size];
				}

				// Token: 0x040042E4 RID: 17124
				public readonly int size;

				// Token: 0x040042E5 RID: 17125
				public readonly int[] triangles;
			}
		}
	}
}
