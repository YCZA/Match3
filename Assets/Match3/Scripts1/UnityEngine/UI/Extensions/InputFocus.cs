using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA5 RID: 2981
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/InputFocus")]
	public class InputFocus : MonoBehaviour
	{
		// Token: 0x060045D5 RID: 17877 RVA: 0x001620D6 File Offset: 0x001604D6
		private void Start()
		{
			this._inputField = base.GetComponent<InputField>();
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x001620E4 File Offset: 0x001604E4
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyUp(KeyCode.Return) && !this._inputField.isFocused)
			{
				if (this._ignoreNextActivation)
				{
					this._ignoreNextActivation = false;
				}
				else
				{
					this._inputField.Select();
					this._inputField.ActivateInputField();
				}
			}
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0016213C File Offset: 0x0016053C
		public void buttonPressed()
		{
			bool flag = this._inputField.text == string.Empty;
			this._inputField.text = string.Empty;
			if (!flag)
			{
				this._inputField.Select();
				this._inputField.ActivateInputField();
			}
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x0016218C File Offset: 0x0016058C
		public void OnEndEdit(string textString)
		{
			if (!Input.GetKeyDown(KeyCode.Return))
			{
				return;
			}
			bool flag = this._inputField.text == string.Empty;
			this._inputField.text = string.Empty;
			if (flag)
			{
				this._ignoreNextActivation = true;
			}
		}

		// Token: 0x04006D5C RID: 27996
		protected InputField _inputField;

		// Token: 0x04006D5D RID: 27997
		public bool _ignoreNextActivation;
	}
}
