using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006AA RID: 1706
	public class FlashBomb : ATween
	{
		// Token: 0x06002AA7 RID: 10919 RVA: 0x000C33A7 File Offset: 0x000C17A7
		protected override void Show()
		{
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000C33AC File Offset: 0x000C17AC
		protected override void DoUpdate(float value)
		{
			Color color = new Color(1f, 1f, 1f, value);
			this.target.color = color;
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000C33DC File Offset: 0x000C17DC
		protected override void Finish()
		{
			this.target.color = Color.white;
		}

		// Token: 0x040053F8 RID: 21496
		[SerializeField]
		private SpriteRenderer target;
	}
}
