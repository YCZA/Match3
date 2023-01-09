using Match3.Scripts1.Wooga.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200068D RID: 1677
	[RequireComponent(typeof(SpriteRenderer))]
	public class BackgroundImageSwitcher : MonoBehaviour, IDataView<LevelTheme>
	{
		// Token: 0x060029AC RID: 10668 RVA: 0x000BD792 File Offset: 0x000BBB92
		public void Show(LevelTheme theme)
		{
			base.GetComponent<SpriteRenderer>().sprite = theme.background;
		}
	}
}
