using System;
using TMPro;

// Token: 0x02000A26 RID: 2598
namespace Match3.Scripts1
{
	public class TownAreaLabel : AVisibleGameObject
	{
		// Token: 0x06003E5D RID: 15965 RVA: 0x0013C756 File Offset: 0x0013AB56
		public void SetArea(int area, ILocalizationService loc)
		{
			this.area = area;
			if (this.localizationService == null)
			{
				this.localizationService = loc;
				this.localizationService.LanguageChanged.AddListener(new Action(this.UpdateText));
			}
			this.UpdateText();
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0013C793 File Offset: 0x0013AB93
		public void RemoveLocaListener()
		{
			if (this.localizationService != null)
			{
				this.localizationService.LanguageChanged.RemoveListener(new Action(this.UpdateText));
			}
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x0013C7BC File Offset: 0x0013ABBC
		private void UpdateText()
		{
			string text = string.Format(this.localizationService.GetText("ui.area.number", new LocaParam[0]), this.area);
			TMP_Text[] componentsInChildren = base.GetComponentsInChildren<TMP_Text>(true);
			if (componentsInChildren != null)
			{
				foreach (TMP_Text tmp_Text in componentsInChildren)
				{
					tmp_Text.text = text;
				}
			}
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x0013C824 File Offset: 0x0013AC24
		private void OnDestroy()
		{
			this.RemoveLocaListener();
		}

		// Token: 0x04006771 RID: 26481
		private ILocalizationService localizationService;

		// Token: 0x04006772 RID: 26482
		private int area;
	}
}
