using UnityEngine.UI;

namespace Match3.Scripts1.Localization.Runtime
{
	// Token: 0x02000AEF RID: 2799
	public abstract class AStandardTextLocalizer : ATextLocalizer
	{
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x001550DD File Offset: 0x001534DD
		private Text textComponent
		{
			get
			{
				if (!this._textComponent)
				{
					this._textComponent = base.GetComponent<Text>();
				}
				return this._textComponent;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06004223 RID: 16931 RVA: 0x00155101 File Offset: 0x00153501
		private Button buttonComponent
		{
			get
			{
				if (!this._buttonComponent)
				{
					this._buttonComponent = base.GetComponent<Button>();
				}
				return this._buttonComponent;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x00155125 File Offset: 0x00153525
		private Toggle toggleComponent
		{
			get
			{
				if (!this._toggleComponent)
				{
					this._toggleComponent = base.GetComponent<Toggle>();
				}
				return this._toggleComponent;
			}
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0015514C File Offset: 0x0015354C
		protected override void SetLocalizedText(string text)
		{
			if (this.target != null)
			{
				this.target.Text = text;
			}
			else if (this.textComponent)
			{
				this.textComponent.text = text;
			}
			else if (this.buttonComponent)
			{
				AStandardTextLocalizer.GetLabel(this.buttonComponent).text = text;
			}
			else if (this.toggleComponent)
			{
				AStandardTextLocalizer.GetLabel(this.toggleComponent).text = text;
			}
			else
			{
				global::UnityEngine.Debug.LogWarning(string.Format("There is no TMPro.TextMeshProUGUI component to update with TMPro.TextMeshProUGUI '{0}' on GameObject '{1}'!", text, base.name));
			}
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x001551FE File Offset: 0x001535FE
		public static Text GetLabel(Button component)
		{
			return component.GetComponentsInChildren<Text>(true)[0];
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00155209 File Offset: 0x00153609
		public static Text GetLabel(Toggle component)
		{
			return component.GetComponentsInChildren<Text>(true)[0];
		}

		// Token: 0x04006B48 RID: 27464
		private Button _buttonComponent;

		// Token: 0x04006B49 RID: 27465
		private Text _textComponent;

		// Token: 0x04006B4A RID: 27466
		private Toggle _toggleComponent;

		// Token: 0x04006B4B RID: 27467
		public ALocalizableComponent target;
	}
}
