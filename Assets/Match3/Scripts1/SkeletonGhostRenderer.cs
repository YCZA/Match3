using System.Collections;
using UnityEngine;

// Token: 0x02000247 RID: 583
namespace Match3.Scripts1
{
	public class SkeletonGhostRenderer : MonoBehaviour
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x000324EF File Offset: 0x000308EF
		private void Awake()
		{
			this.meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00032514 File Offset: 0x00030914
		public void Initialize(Mesh mesh, Material[] materials, Color32 color, bool additive, float speed, int sortingLayerID, int sortingOrder)
		{
			base.StopAllCoroutines();
			base.gameObject.SetActive(true);
			this.meshRenderer.sharedMaterials = materials;
			this.meshRenderer.sortingLayerID = sortingLayerID;
			this.meshRenderer.sortingOrder = sortingOrder;
			this.meshFilter.sharedMesh = global::UnityEngine.Object.Instantiate<Mesh>(mesh);
			this.colors = this.meshFilter.sharedMesh.colors32;
			if (color.a + color.r + color.g + color.b > 0)
			{
				for (int i = 0; i < this.colors.Length; i++)
				{
					this.colors[i] = color;
				}
			}
			this.fadeSpeed = speed;
			if (additive)
			{
				base.StartCoroutine(this.FadeAdditive());
			}
			else
			{
				base.StartCoroutine(this.Fade());
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00032600 File Offset: 0x00030A00
		private IEnumerator Fade()
		{
			for (int t = 0; t < 500; t++)
			{
				bool breakout = true;
				for (int i = 0; i < this.colors.Length; i++)
				{
					Color32 c = this.colors[i];
					if (c.a > 0)
					{
						breakout = false;
					}
					this.colors[i] = Color32.Lerp(c, this.black, Time.deltaTime * this.fadeSpeed);
				}
				this.meshFilter.sharedMesh.colors32 = this.colors;
				if (breakout)
				{
					break;
				}
				yield return null;
			}
			global::UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0003261C File Offset: 0x00030A1C
		private IEnumerator FadeAdditive()
		{
			Color32 black = this.black;
			for (int t = 0; t < 500; t++)
			{
				bool breakout = true;
				for (int i = 0; i < this.colors.Length; i++)
				{
					Color32 c = this.colors[i];
					black.a = c.a;
					if (c.r > 0 || c.g > 0 || c.b > 0)
					{
						breakout = false;
					}
					this.colors[i] = Color32.Lerp(c, black, Time.deltaTime * this.fadeSpeed);
				}
				this.meshFilter.sharedMesh.colors32 = this.colors;
				if (breakout)
				{
					break;
				}
				yield return null;
			}
			global::UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00032638 File Offset: 0x00030A38
		public void Cleanup()
		{
			if (this.meshFilter != null && this.meshFilter.sharedMesh != null)
			{
				global::UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04004227 RID: 16935
		public float fadeSpeed = 10f;

		// Token: 0x04004228 RID: 16936
		private Color32[] colors;

		// Token: 0x04004229 RID: 16937
		private Color32 black = new Color32(0, 0, 0, 0);

		// Token: 0x0400422A RID: 16938
		private MeshFilter meshFilter;

		// Token: 0x0400422B RID: 16939
		private MeshRenderer meshRenderer;
	}
}
