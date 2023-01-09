namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020008F4 RID: 2292
	public class BuildingAssetWrapper<T> where T : global::UnityEngine.Object
	{
		// Token: 0x060037DA RID: 14298 RVA: 0x00111598 File Offset: 0x0010F998
		public BuildingAssetWrapper(T asset, bool isPlaceHolder)
		{
			this.asset = asset;
			this.isPlaceHolder = isPlaceHolder;
		}

		// Token: 0x04006003 RID: 24579
		public readonly T asset;

		// Token: 0x04006004 RID: 24580
		public readonly bool isPlaceHolder;
	}
}
