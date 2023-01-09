using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C22 RID: 3106
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Input Field Submit")]
	public class InputFieldEnterSubmit : MonoBehaviour
	{
		// Token: 0x06004958 RID: 18776 RVA: 0x00177178 File Offset: 0x00175578
		private void Awake()
		{
			this._input = base.GetComponent<InputField>();
			this._input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x001771A2 File Offset: 0x001755A2
		public void OnEndEdit(string txt)
		{
			if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				return;
			}
			this.EnterSubmit.Invoke(txt);
		}

		// Token: 0x04006FC5 RID: 28613
		public InputFieldEnterSubmit.EnterSubmitEvent EnterSubmit;

		// Token: 0x04006FC6 RID: 28614
		private InputField _input;

		// Token: 0x02000C23 RID: 3107
		[Serializable]
		public class EnterSubmitEvent : UnityEvent<string>
		{
		}
	}
}
