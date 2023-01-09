using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA1 RID: 2977
	[RequireComponent(typeof(RectTransform), typeof(Button))]
	public class DropDownListButton
	{
		// Token: 0x060045B6 RID: 17846 RVA: 0x00161D5C File Offset: 0x0016015C
		public DropDownListButton(GameObject btnObj)
		{
			this.gameobject = btnObj;
			this.rectTransform = btnObj.GetComponent<RectTransform>();
			this.btnImg = btnObj.GetComponent<Image>();
			this.btn = btnObj.GetComponent<Button>();
			this.txt = this.rectTransform.Find("Text").GetComponent<Text>();
			this.img = this.rectTransform.Find("Image").GetComponent<Image>();
		}

		// Token: 0x04006D44 RID: 27972
		public RectTransform rectTransform;

		// Token: 0x04006D45 RID: 27973
		public Button btn;

		// Token: 0x04006D46 RID: 27974
		public Text txt;

		// Token: 0x04006D47 RID: 27975
		public Image btnImg;

		// Token: 0x04006D48 RID: 27976
		public Image img;

		// Token: 0x04006D49 RID: 27977
		public GameObject gameobject;
	}
}
