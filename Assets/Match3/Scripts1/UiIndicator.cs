using Match3.Scripts1.UnityEngine;
using Match3.Scripts1.Wooga.UI;
using TMPro;

// Token: 0x02000B46 RID: 2886
namespace Match3.Scripts1
{
	public class UiIndicator : AVisibleGameObject, IDataView<string>
	{
		// Token: 0x060043AF RID: 17327 RVA: 0x00159B84 File Offset: 0x00157F84
		public void Show(string text)
		{
			base.Show();
			this.ExecuteOnChild(delegate(TMP_Text t)
			{
				t.text = text;
			});
		}
	}
}
