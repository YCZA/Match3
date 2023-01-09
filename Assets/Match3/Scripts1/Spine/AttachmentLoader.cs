namespace Match3.Scripts1.Spine
{
	// Token: 0x02000203 RID: 515
	public interface AttachmentLoader
	{
		// Token: 0x06000EE3 RID: 3811
		RegionAttachment NewRegionAttachment(Skin skin, string name, string path);

		// Token: 0x06000EE4 RID: 3812
		MeshAttachment NewMeshAttachment(Skin skin, string name, string path);

		// Token: 0x06000EE5 RID: 3813
		WeightedMeshAttachment NewWeightedMeshAttachment(Skin skin, string name, string path);

		// Token: 0x06000EE6 RID: 3814
		BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name);
	}
}
