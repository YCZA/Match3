using UnityEngine;

// Token: 0x0200091D RID: 2333
namespace Match3.Scripts1
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Renderer))]
	public class MaterialProperties : MonoBehaviour
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x060038E3 RID: 14563 RVA: 0x00118221 File Offset: 0x00116621
		private MaterialPropertyBlock block
		{
			get
			{
				if (this._block == null)
				{
					this._block = new MaterialPropertyBlock();
				}
				return this._block;
			}
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0011823F File Offset: 0x0011663F
		private void Awake()
		{
			this.Refresh();
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x00118247 File Offset: 0x00116647
		public void SetSprite(Sprite sprite)
		{
			if (sprite)
			{
				this.block.SetTexture("_MainTex", sprite.texture);
			}
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0011826A File Offset: 0x0011666A
		public void SetTexture(Texture texture)
		{
			if (texture)
			{
				this.block.SetTexture("_MainTex", texture);
			}
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x00118288 File Offset: 0x00116688
		public void SetColor(Color color)
		{
			this.block.SetColor("_Color", color);
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x0011829B File Offset: 0x0011669B
		public void Refresh()
		{
			this.SetSprite(this.sprite);
			this.SetTexture(this.texture);
			this.SetColor(Color.white);
			this.Apply();
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x001182C6 File Offset: 0x001166C6
		private void Apply()
		{
			base.GetComponent<Renderer>().SetPropertyBlock(this.block);
		}

		// Token: 0x04006136 RID: 24886
		public Texture texture;

		// Token: 0x04006137 RID: 24887
		[HideInInspector]
		public Sprite sprite;

		// Token: 0x04006138 RID: 24888
		private MaterialPropertyBlock _block;
	}
}
