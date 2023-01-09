namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x0200026F RID: 623
	public class SpineSlot : SpineAttributeBase
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x0003C27D File Offset: 0x0003A67D
		public SpineSlot(string startsWith = "", string dataField = "", bool containsBoundingBoxes = false)
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
			this.containsBoundingBoxes = containsBoundingBoxes;
		}

		// Token: 0x04004346 RID: 17222
		public bool containsBoundingBoxes;
	}
}
