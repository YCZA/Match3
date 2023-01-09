using System;

// Token: 0x02000844 RID: 2116
namespace Match3.Scripts1
{
	[Serializable]
	public class ChapterConfig
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x000FB140 File Offset: 0x000F9540
		public int ChapterForLevel(int levelNumber)
		{
			int i = 0;
			int num = this.chapters.Length;
			while (i < num)
			{
				if (this.chapters[i].first_level > levelNumber)
				{
					return i;
				}
				i++;
			}
			return this.chapters.Length;
		}

		// Token: 0x04005C6A RID: 23658
		public ChapterData[] chapters;
	}
}
