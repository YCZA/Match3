namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000275 RID: 629
	public class SpineBone : SpineAttributeBase
	{
		// Token: 0x06001308 RID: 4872 RVA: 0x0003C42E File Offset: 0x0003A82E
		public SpineBone(string startsWith = "", string dataField = "")
		{
			this.startsWith = startsWith;
			this.dataField = dataField;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0003C444 File Offset: 0x0003A844
		public static Bone GetBone(string boneName, SkeletonRenderer renderer)
		{
			return (renderer.skeleton != null) ? renderer.skeleton.FindBone(boneName) : null;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0003C464 File Offset: 0x0003A864
		public static BoneData GetBoneData(string boneName, SkeletonDataAsset skeletonDataAsset)
		{
			SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
			return skeletonData.FindBone(boneName);
		}
	}
}
