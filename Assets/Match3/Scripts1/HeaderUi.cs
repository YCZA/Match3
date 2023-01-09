using TMPro;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
namespace Match3.Scripts1
{
	public class HeaderUi : MonoBehaviour
	{
		// Token: 0x06002BC0 RID: 11200 RVA: 0x000C8CC6 File Offset: 0x000C70C6
		public void Set(string description, int number)
		{
			this.description.text = description;
			this.number.text = number.ToString();
		}

		// Token: 0x040054D8 RID: 21720
		[SerializeField]
		private TMP_Text number;

		// Token: 0x040054D9 RID: 21721
		[SerializeField]
		private TMP_Text description;
	}
}
