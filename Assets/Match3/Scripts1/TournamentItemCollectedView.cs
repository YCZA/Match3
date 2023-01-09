using UnityEngine;

// Token: 0x020006CF RID: 1743
namespace Match3.Scripts1
{
	public class TournamentItemCollectedView : MonoBehaviour
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x000C7564 File Offset: 0x000C5964
		public Sprite sprite
		{
			get
			{
				return this.spriteRenderer.sprite;
			}
		}

		// Token: 0x0400548A RID: 21642
		public SpriteRenderer spriteRenderer;
	}
}
