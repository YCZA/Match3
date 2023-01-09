using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A11 RID: 2577
namespace Match3.Scripts1
{
	public class UiGauge : MonoBehaviour
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0013A168 File Offset: 0x00138568
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0013A175 File Offset: 0x00138575
		public float Value
		{
			get
			{
				return this.progressBar.fillAmount;
			}
			set
			{
				this.progressBar.fillAmount = value;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0013A183 File Offset: 0x00138583
		// (set) Token: 0x06003DFF RID: 15871 RVA: 0x0013A190 File Offset: 0x00138590
		public string Label
		{
			get
			{
				return this.label.text;
			}
			set
			{
				this.label.text = value;
			}
		}

		// Token: 0x040066D8 RID: 26328
		[SerializeField]
		private TMP_Text label;

		// Token: 0x040066D9 RID: 26329
		[SerializeField]
		private Image progressBar;
	}
}
