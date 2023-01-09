using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity.Modules
{
	// Token: 0x02000241 RID: 577
	[ExecuteInEditMode]
	public class SkeletonRendererCustomMaterials : MonoBehaviour
	{
		// Token: 0x060011CF RID: 4559 RVA: 0x00031B94 File Offset: 0x0002FF94
		private void SetCustomSlotMaterials()
		{
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customSlotMaterials.Count; i++)
			{
				SkeletonRendererCustomMaterials.SlotMaterialOverride slotMaterialOverride = this.customSlotMaterials[i];
				if (!slotMaterialOverride.overrideDisabled && !string.IsNullOrEmpty(slotMaterialOverride.slotName))
				{
					Slot key = this.skeletonRenderer.skeleton.FindSlot(slotMaterialOverride.slotName);
					this.skeletonRenderer.CustomSlotMaterials[key] = slotMaterialOverride.material;
				}
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00031C38 File Offset: 0x00030038
		private void RemoveCustomSlotMaterials()
		{
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customSlotMaterials.Count; i++)
			{
				SkeletonRendererCustomMaterials.SlotMaterialOverride slotMaterialOverride = this.customSlotMaterials[i];
				if (!string.IsNullOrEmpty(slotMaterialOverride.slotName))
				{
					Slot key = this.skeletonRenderer.skeleton.FindSlot(slotMaterialOverride.slotName);
					Material x;
					if (this.skeletonRenderer.CustomSlotMaterials.TryGetValue(key, out x))
					{
						if (!(x != slotMaterialOverride.material))
						{
							this.skeletonRenderer.CustomSlotMaterials.Remove(key);
						}
					}
				}
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00031D00 File Offset: 0x00030100
		private void SetCustomMaterialOverrides()
		{
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonRendererCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				if (!atlasMaterialOverride.overrideDisabled)
				{
					this.skeletonRenderer.CustomMaterialOverride[atlasMaterialOverride.originalMaterial] = atlasMaterialOverride.replacementMaterial;
				}
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00031D84 File Offset: 0x00030184
		private void RemoveCustomMaterialOverrides()
		{
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			for (int i = 0; i < this.customMaterialOverrides.Count; i++)
			{
				SkeletonRendererCustomMaterials.AtlasMaterialOverride atlasMaterialOverride = this.customMaterialOverrides[i];
				Material x;
				if (this.skeletonRenderer.CustomMaterialOverride.TryGetValue(atlasMaterialOverride.originalMaterial, out x))
				{
					if (!(x != atlasMaterialOverride.replacementMaterial))
					{
						this.skeletonRenderer.CustomMaterialOverride.Remove(atlasMaterialOverride.originalMaterial);
					}
				}
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00031E28 File Offset: 0x00030228
		private void OnEnable()
		{
			if (this.skeletonRenderer == null)
			{
				this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
			}
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			this.skeletonRenderer.Initialize(false);
			this.SetCustomMaterialOverrides();
			this.SetCustomSlotMaterials();
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00031E86 File Offset: 0x00030286
		private void OnDisable()
		{
			if (this.skeletonRenderer == null)
			{
				global::UnityEngine.Debug.LogError("skeletonRenderer == null");
				return;
			}
			this.RemoveCustomMaterialOverrides();
			this.RemoveCustomSlotMaterials();
		}

		// Token: 0x04004206 RID: 16902
		public SkeletonRenderer skeletonRenderer;

		// Token: 0x04004207 RID: 16903
		[SerializeField]
		private List<SkeletonRendererCustomMaterials.SlotMaterialOverride> customSlotMaterials = new List<SkeletonRendererCustomMaterials.SlotMaterialOverride>();

		// Token: 0x04004208 RID: 16904
		[SerializeField]
		private List<SkeletonRendererCustomMaterials.AtlasMaterialOverride> customMaterialOverrides = new List<SkeletonRendererCustomMaterials.AtlasMaterialOverride>();

		// Token: 0x02000242 RID: 578
		[Serializable]
		public struct SlotMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.SlotMaterialOverride>
		{
			// Token: 0x060011D5 RID: 4565 RVA: 0x00031EB0 File Offset: 0x000302B0
			public bool Equals(SkeletonRendererCustomMaterials.SlotMaterialOverride other)
			{
				return this.overrideDisabled == other.overrideDisabled && this.slotName == other.slotName && this.material == other.material;
			}

			// Token: 0x04004209 RID: 16905
			public bool overrideDisabled;

			// Token: 0x0400420A RID: 16906
			[SpineSlot("", "", false)]
			public string slotName;

			// Token: 0x0400420B RID: 16907
			public Material material;
		}

		// Token: 0x02000243 RID: 579
		[Serializable]
		public struct AtlasMaterialOverride : IEquatable<SkeletonRendererCustomMaterials.AtlasMaterialOverride>
		{
			// Token: 0x060011D6 RID: 4566 RVA: 0x00031EF0 File Offset: 0x000302F0
			public bool Equals(SkeletonRendererCustomMaterials.AtlasMaterialOverride other)
			{
				return this.overrideDisabled == other.overrideDisabled && this.originalMaterial == other.originalMaterial && this.replacementMaterial == other.replacementMaterial;
			}

			// Token: 0x0400420C RID: 16908
			public bool overrideDisabled;

			// Token: 0x0400420D RID: 16909
			public Material originalMaterial;

			// Token: 0x0400420E RID: 16910
			public Material replacementMaterial;
		}
	}
}
