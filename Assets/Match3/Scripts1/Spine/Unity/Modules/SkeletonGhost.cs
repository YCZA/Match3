using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000246 RID: 582
	[RequireComponent(typeof(SkeletonRenderer))]
	public class SkeletonGhost : MonoBehaviour
	{
		// Token: 0x060011DB RID: 4571 RVA: 0x00032044 File Offset: 0x00030444
		private void Start()
		{
			if (this.ghostShader == null)
			{
				this.ghostShader = Shader.Find("Spine/SkeletonGhost");
			}
			this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.nextSpawnTime = Time.time + this.spawnRate;
			this.pool = new SkeletonGhostRenderer[this.maximumGhosts];
			for (int i = 0; i < this.maximumGhosts; i++)
			{
				GameObject gameObject = new GameObject(base.gameObject.name + " Ghost", new Type[]
				{
					typeof(SkeletonGhostRenderer)
				});
				this.pool[i] = gameObject.GetComponent<SkeletonGhostRenderer>();
				gameObject.SetActive(false);
				gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
			if (this.skeletonRenderer is SkeletonAnimation)
			{
				((SkeletonAnimation)this.skeletonRenderer).state.Event += this.OnEvent;
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0003214C File Offset: 0x0003054C
		private void OnEvent(AnimationState state, int trackIndex, Event e)
		{
			if (e.Data.Name == "Ghosting")
			{
				this.ghostingEnabled = (e.Int > 0);
				if (e.Float > 0f)
				{
					this.spawnRate = e.Float;
				}
				if (e.String != null)
				{
					this.color = SkeletonGhost.HexToColor(e.String);
				}
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x000321BA File Offset: 0x000305BA
		private void Ghosting(float val)
		{
			this.ghostingEnabled = (val > 0f);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000321CC File Offset: 0x000305CC
		private void Update()
		{
			if (!this.ghostingEnabled)
			{
				return;
			}
			if (Time.time >= this.nextSpawnTime)
			{
				GameObject gameObject = this.pool[this.poolIndex].gameObject;
				Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					Material material = sharedMaterials[i];
					Material material2;
					if (!this.materialTable.ContainsKey(material))
					{
						material2 = new Material(material);
						material2.shader = this.ghostShader;
						material2.color = Color.white;
						if (material2.HasProperty("_TextureFade"))
						{
							material2.SetFloat("_TextureFade", this.textureFade);
						}
						this.materialTable.Add(material, material2);
					}
					else
					{
						material2 = this.materialTable[material];
					}
					sharedMaterials[i] = material2;
				}
				Transform transform = gameObject.transform;
				transform.parent = base.transform;
				this.pool[this.poolIndex].Initialize(this.meshFilter.sharedMesh, sharedMaterials, this.color, this.additive, this.fadeSpeed, this.meshRenderer.sortingLayerID, (!this.sortWithDistanceOnly) ? (this.meshRenderer.sortingOrder - 1) : this.meshRenderer.sortingOrder);
				transform.localPosition = new Vector3(0f, 0f, this.zOffset);
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				transform.parent = null;
				this.poolIndex++;
				if (this.poolIndex == this.pool.Length)
				{
					this.poolIndex = 0;
				}
				this.nextSpawnTime = Time.time + this.spawnRate;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00032398 File Offset: 0x00030798
		private void OnDestroy()
		{
			for (int i = 0; i < this.maximumGhosts; i++)
			{
				if (this.pool[i] != null)
				{
					this.pool[i].Cleanup();
				}
			}
			foreach (Material obj in this.materialTable.Values)
			{
				global::UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00032430 File Offset: 0x00030830
		private static Color32 HexToColor(string hex)
		{
			if (hex.Length < 6)
			{
				return Color.magenta;
			}
			hex = hex.Replace("#", string.Empty);
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			byte a = byte.MaxValue;
			if (hex.Length == 8)
			{
				a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
			}
			return new Color32(r, g, b, a);
		}

		// Token: 0x04004216 RID: 16918
		public bool ghostingEnabled = true;

		// Token: 0x04004217 RID: 16919
		public float spawnRate = 0.05f;

		// Token: 0x04004218 RID: 16920
		public Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

		// Token: 0x04004219 RID: 16921
		[Tooltip("Remember to set color alpha to 0 if Additive is true")]
		public bool additive = true;

		// Token: 0x0400421A RID: 16922
		public int maximumGhosts = 10;

		// Token: 0x0400421B RID: 16923
		public float fadeSpeed = 10f;

		// Token: 0x0400421C RID: 16924
		public Shader ghostShader;

		// Token: 0x0400421D RID: 16925
		[Tooltip("0 is Color and Alpha, 1 is Alpha only.")]
		[Range(0f, 1f)]
		public float textureFade = 1f;

		// Token: 0x0400421E RID: 16926
		[Header("Sorting")]
		public bool sortWithDistanceOnly;

		// Token: 0x0400421F RID: 16927
		public float zOffset;

		// Token: 0x04004220 RID: 16928
		private float nextSpawnTime;

		// Token: 0x04004221 RID: 16929
		private SkeletonGhostRenderer[] pool;

		// Token: 0x04004222 RID: 16930
		private int poolIndex;

		// Token: 0x04004223 RID: 16931
		private SkeletonRenderer skeletonRenderer;

		// Token: 0x04004224 RID: 16932
		private MeshRenderer meshRenderer;

		// Token: 0x04004225 RID: 16933
		private MeshFilter meshFilter;

		// Token: 0x04004226 RID: 16934
		private Dictionary<Material, Material> materialTable = new Dictionary<Material, Material>();
	}
}
