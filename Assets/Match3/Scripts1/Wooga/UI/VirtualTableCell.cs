using UnityEngine;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B3B RID: 2875
	[RequireComponent(typeof(RectTransform))]
	public class VirtualTableCell : ATableViewCell
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x00158554 File Offset: 0x00156954
		// (set) Token: 0x0600436A RID: 17258 RVA: 0x0015855C File Offset: 0x0015695C
		public bool IsActive { get; set; }

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x00158565 File Offset: 0x00156965
		// (set) Token: 0x0600436C RID: 17260 RVA: 0x0015856D File Offset: 0x0015696D
		public ATableViewReusableCell DataCell { get; set; }

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600436D RID: 17261 RVA: 0x00158576 File Offset: 0x00156976
		// (set) Token: 0x0600436E RID: 17262 RVA: 0x0015857E File Offset: 0x0015697E
		public RectTransform RectTransform { get; private set; }

		// Token: 0x0600436F RID: 17263 RVA: 0x00158587 File Offset: 0x00156987
		private void Awake()
		{
			this.RectTransform = base.GetComponent<RectTransform>();
		}
	}
}
