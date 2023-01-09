using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Legal
{
	// Token: 0x02000B57 RID: 2903
	[RequireComponent(typeof(TMP_Text))]
	public class TermsAndConditionsLocalizer : MonoBehaviour
	{
		// Token: 0x060043EF RID: 17391 RVA: 0x0015A880 File Offset: 0x00158C80
		private void Start()
		{
			TMP_Text component = base.GetComponent<TMP_Text>();
			component.text = TermsLocalisation.GetLocalization(this.text.ToString());
		}

		// Token: 0x04006C40 RID: 27712
		[SerializeField]
		private TermsAndConditionsLocalizer.Text text;

		// Token: 0x02000B58 RID: 2904
		public enum Text
		{
			// Token: 0x04006C42 RID: 27714
			title,
			// Token: 0x04006C43 RID: 27715
			header,
			// Token: 0x04006C44 RID: 27716
			terms,
			// Token: 0x04006C45 RID: 27717
			footer,
			// Token: 0x04006C46 RID: 27718
			button_agree
		}
	}
}
