using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020004B1 RID: 1201
namespace Match3.Scripts1
{
	public class PirateBreakOutLevelStory : MonoBehaviour
	{
		// Token: 0x060021CD RID: 8653 RVA: 0x0008F0F5 File Offset: 0x0008D4F5
		public void Setup(ILocalizationService locaService)
		{
			this.locaService = locaService;
			this.button.onClick.AddListener(new UnityAction(this.OnButtonTapped));
			this.container.gameObject.SetActive(false);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0008F12C File Offset: 0x0008D52C
		public IEnumerator ShowDialog(List<string> dialog)
		{
			foreach (string line in dialog)
			{
				this.label.text = this.locaService.GetText(line, new LocaParam[0]);
				yield return new WaitWhile(() => !this.tapped);
				this.tapped = false;
			}
			yield break;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0008F14E File Offset: 0x0008D54E
		private void OnButtonTapped()
		{
			this.tapped = true;
		}

		// Token: 0x04004D0D RID: 19725
		[SerializeField]
		private GameObject container;

		// Token: 0x04004D0E RID: 19726
		[SerializeField]
		private Button button;

		// Token: 0x04004D0F RID: 19727
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04004D10 RID: 19728
		private ILocalizationService locaService;

		// Token: 0x04004D11 RID: 19729
		private bool tapped;
	}
}
