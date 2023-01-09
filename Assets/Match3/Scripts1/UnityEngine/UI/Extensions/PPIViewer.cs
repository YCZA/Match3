using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C26 RID: 3110
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Extensions/PPIViewer")]
	public class PPIViewer : MonoBehaviour
	{
		// Token: 0x06004969 RID: 18793 RVA: 0x001773FD File Offset: 0x001757FD
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x0017740C File Offset: 0x0017580C
		private void Start()
		{
			if (this.label != null)
			{
				this.label.text = "PPI: " + Screen.dpi.ToString();
			}
		}

		// Token: 0x04006FCA RID: 28618
		private Text label;
	}
}
