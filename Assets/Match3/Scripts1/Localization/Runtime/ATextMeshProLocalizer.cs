using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Localization.Runtime
{
	// Token: 0x02000AF1 RID: 2801
	public abstract class ATextMeshProLocalizer : ATextLocalizer
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x0015521C File Offset: 0x0015361C
		private TextMeshProUGUI textComponent
		{
			get
			{
				if (!this._textComponent && this != null)
				{
					this._textComponent = base.GetComponent<TextMeshProUGUI>();
				}
				return this._textComponent;
			}
		}
		
		private Text textComponentUGUI
		{
			get
			{
				if (!this._textComponentUGUI && this != null)
				{
					this._textComponentUGUI = base.GetComponent<Text>();
				}
				return this._textComponentUGUI;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x0015524C File Offset: 0x0015364C
		private Button buttonComponent
		{
			get
			{
				if (!this._buttonComponent && this != null)
				{
					this._buttonComponent = base.GetComponent<Button>();
				}
				return this._buttonComponent;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0015527C File Offset: 0x0015367C
		private Toggle toggleComponent
		{
			get
			{
				if (!this._toggleComponent && this != null)
				{
					this._toggleComponent = base.GetComponent<Toggle>();
				}
				return this._toggleComponent;
			}
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x001552AC File Offset: 0x001536AC
		protected override void SetLocalizedText(string localizedText)
		{
			if (this == null)
			{
				return;
			}
			if (this.target != null)
			{
				this.target.Text = localizedText;
			}
			else if (this.textComponent)
			{
				this.textComponent.text = localizedText;
				this.textComponent.ForceMeshUpdate();
			}
			else if (this.textComponentUGUI)
			{
				textComponentUGUI.text = localizedText;
			}
			else if (this.buttonComponent)
			{
				Debug.Log("onchangeLanguage, tmp button: " + gameObject.name);
				ATextMeshProLocalizer.GetLabel(this.buttonComponent).text = localizedText;
			}
			else if (this.toggleComponent)
			{
				Debug.Log("onchangeLanguage, toggle");
				ATextMeshProLocalizer.GetLabel(this.toggleComponent).text = localizedText;
			}
			else
			{
				Debug.Log("onchangeLanguage, no tmp");
				global::UnityEngine.Debug.LogWarning(string.Format("There is no TextMeshProUGUI component to update with Text '{0}' on GameObject '{1}'!", localizedText, base.name));
			}
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00155376 File Offset: 0x00153776
		public static TextMeshProUGUI GetLabel(Button component)
		{
			return component.GetComponentsInChildren<TextMeshProUGUI>(true)[0];
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00155381 File Offset: 0x00153781
		public static TextMeshProUGUI GetLabel(Toggle component)
		{
			return component.GetComponentsInChildren<TextMeshProUGUI>(true)[0];
		}

		// Token: 0x04006B4E RID: 27470
		private Button _buttonComponent;

		// Token: 0x04006B4F RID: 27471
		private TextMeshProUGUI _textComponent;
		private Text _textComponentUGUI;

		// Token: 0x04006B50 RID: 27472
		private Toggle _toggleComponent;

		// Token: 0x04006B51 RID: 27473
		public ALocalizableComponent target;
	}
}
