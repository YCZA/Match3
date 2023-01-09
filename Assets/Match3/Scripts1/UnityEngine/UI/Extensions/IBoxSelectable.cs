using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BB7 RID: 2999
	public interface IBoxSelectable
	{
		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06004650 RID: 18000
		// (set) Token: 0x06004651 RID: 18001
		bool selected { get; set; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06004652 RID: 18002
		// (set) Token: 0x06004653 RID: 18003
		bool preSelected { get; set; }

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004654 RID: 18004
		Transform transform { get; }
	}
}
