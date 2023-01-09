using UnityEngine;

// Token: 0x02000A52 RID: 2642
namespace Match3.Scripts1
{
	public class TournamentProfileSpriteManager : SpriteManager
	{
		// Token: 0x06003F44 RID: 16196 RVA: 0x001434D8 File Offset: 0x001418D8
		public Sprite GetSprite(string displayedPlayerName)
		{
			if (this.resources == null || this.resources.Length < 1)
			{
				return null;
			}
			int num = Mathf.Abs(displayedPlayerName.GetHashCode()) % this.resources.Length;
			return this.resources[num];
		}
	}
}
