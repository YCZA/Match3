using System;
using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Spine.Unity
{
	// Token: 0x02000229 RID: 553
	public class SkeletonDataAsset : ScriptableObject
	{
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0002EF1F File Offset: 0x0002D31F
		public float Scale
		{
			get
			{
				return 1f / (float)this.pixelsPerUnit;
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x0002EF2E File Offset: 0x0002D32E
		private void OnEnable()
		{
			if (this.atlasAssets == null)
			{
				this.atlasAssets = new AtlasAsset[0];
			}
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0002EF47 File Offset: 0x0002D347
		public void Reset()
		{
			this.skeletonData = null;
			this.stateData = null;
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x0002EF58 File Offset: 0x0002D358
		public SkeletonData GetSkeletonData(bool quiet)
		{
			if (this.atlasAssets == null)
			{
				this.atlasAssets = new AtlasAsset[0];
				if (!quiet)
				{
					global::UnityEngine.Debug.LogError("Atlas not set for SkeletonData asset: " + base.name, this);
				}
				this.Reset();
				return null;
			}
			if (this.skeletonJSON == null)
			{
				if (!quiet)
				{
					global::UnityEngine.Debug.LogError("Skeleton JSON file not set for SkeletonData asset: " + base.name, this);
				}
				this.Reset();
				return null;
			}
			if (this.atlasAssets.Length == 0)
			{
				this.Reset();
				return null;
			}
			Atlas[] array = new Atlas[this.atlasAssets.Length];
			for (int i = 0; i < this.atlasAssets.Length; i++)
			{
				if (this.atlasAssets[i] == null)
				{
					this.Reset();
					return null;
				}
				array[i] = this.atlasAssets[i].GetAtlas();
				if (array[i] == null)
				{
					this.Reset();
					return null;
				}
			}
			if (this.skeletonData != null)
			{
				return this.skeletonData;
			}
			AttachmentLoader attachmentLoader = new AtlasAttachmentLoader(array);
			float scale = this.Scale;
			try
			{
				if (this.skeletonJSON.name.ToLower().Contains(".skel"))
				{
					MemoryStream input = new MemoryStream(this.skeletonJSON.bytes);
					this.skeletonData = new SkeletonBinary(attachmentLoader)
					{
						Scale = scale
					}.ReadSkeletonData(input);
				}
				else
				{
					StringReader reader = new StringReader(this.skeletonJSON.text);
					this.skeletonData = new SkeletonJson(attachmentLoader)
					{
						Scale = scale
					}.ReadSkeletonData(reader);
				}
			}
			catch (Exception ex)
			{
				if (!quiet)
				{
					global::UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Error reading skeleton JSON file for SkeletonData asset: ",
						base.name,
						"\n",
						ex.Message,
						"\n",
						ex.StackTrace
					}), this);
				}
				return null;
			}
			this.stateData = new AnimationStateData(this.skeletonData);
			this.FillStateData();
			return this.skeletonData;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0002F180 File Offset: 0x0002D580
		public void FillStateData()
		{
			if (this.stateData == null)
			{
				return;
			}
			this.stateData.DefaultMix = this.defaultMix;
			int i = 0;
			int num = this.fromAnimation.Length;
			while (i < num)
			{
				if (this.fromAnimation[i].Length != 0 && this.toAnimation[i].Length != 0)
				{
					this.stateData.SetMix(this.fromAnimation[i], this.toAnimation[i], this.duration[i]);
				}
				i++;
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0002F211 File Offset: 0x0002D611
		public AnimationStateData GetAnimationStateData()
		{
			if (this.stateData != null)
			{
				return this.stateData;
			}
			this.GetSkeletonData(false);
			return this.stateData;
		}

		// Token: 0x040041B0 RID: 16816
		public AtlasAsset[] atlasAssets;

		// Token: 0x040041B1 RID: 16817
		public TextAsset skeletonJSON;

		// Token: 0x040041B2 RID: 16818
		public int pixelsPerUnit = 400;

		// Token: 0x040041B3 RID: 16819
		public string[] fromAnimation;

		// Token: 0x040041B4 RID: 16820
		public string[] toAnimation;

		// Token: 0x040041B5 RID: 16821
		public float[] duration;

		// Token: 0x040041B6 RID: 16822
		public float defaultMix;

		// Token: 0x040041B7 RID: 16823
		public RuntimeAnimatorController controller;

		// Token: 0x040041B8 RID: 16824
		public bool centerAtPivot;

		// Token: 0x040041B9 RID: 16825
		private SkeletonData skeletonData;

		// Token: 0x040041BA RID: 16826
		private AnimationStateData stateData;
	}
}
