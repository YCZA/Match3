using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F5 RID: 2549
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	public class AtlasUVRemapper : MonoBehaviour
	{
		// Token: 0x06003D7A RID: 15738 RVA: 0x00136671 File Offset: 0x00134A71
		private void Start()
		{
			this.image = base.GetComponent<Image>();
			this.Remap();
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00136688 File Offset: 0x00134A88
		private void Remap()
		{
			Texture2D texture = this.image.sprite.texture;
			Rect textureRect = this.image.sprite.textureRect;
			Vector4 padding = DataUtility.GetPadding(this.image.sprite);
			float x = (textureRect.xMin - padding.x) / (float)texture.width;
			float z = (textureRect.xMax + padding.z) / (float)texture.width;
			float y = (textureRect.yMin - padding.y) / (float)texture.height;
			float w = (textureRect.yMax + padding.w) / (float)texture.height;
			this.atlasRemap = new Vector4(x, y, z, w);
			this.image.material.SetVector("_AtlasUVRemap", this.atlasRemap);
		}

		// Token: 0x04006648 RID: 26184
		private Image image;

		// Token: 0x04006649 RID: 26185
		private Vector4 atlasRemap;
	}
}
