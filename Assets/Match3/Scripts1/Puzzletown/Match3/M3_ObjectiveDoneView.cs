using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200070A RID: 1802
	public class M3_ObjectiveDoneView : ATableViewReusableCell<M3_ObjectiveDoneView.State>, IDataView<MaterialAmount>
	{
		// Token: 0x06002CBF RID: 11455 RVA: 0x000CF835 File Offset: 0x000CDC35
		public void Show(MaterialAmount data)
		{
			this.materialImage.sprite = this.objectivesSprites.GetSimilar(data.type);
		}

		// Token: 0x04005632 RID: 22066
		public SpriteManager objectivesSprites;

		// Token: 0x04005633 RID: 22067
		[SerializeField]
		private Image materialImage;

		// Token: 0x0200070B RID: 1803
		public enum State
		{
			// Token: 0x04005635 RID: 22069
			Pending,
			// Token: 0x04005636 RID: 22070
			Succeeded,
			// Token: 0x04005637 RID: 22071
			Failed
		}
	}
}
