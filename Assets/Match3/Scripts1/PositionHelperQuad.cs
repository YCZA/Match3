using Match3.Scripts1.Wooga.UI;
using UnityEngine;

// Token: 0x02000921 RID: 2337
namespace Match3.Scripts1
{
	public class PositionHelperQuad : MonoBehaviour, IDataView<int>
	{
		// Token: 0x060038F9 RID: 14585 RVA: 0x00118C00 File Offset: 0x00117000
		public void Show(int size)
		{
			base.transform.localPosition = new Vector3((float)size * 0.5f, 0f, (float)size * 0.5f);
			base.transform.localScale = Vector3.one * (float)size;
		}
	}
}
