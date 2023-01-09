using System;
using System.Collections.Generic;
using System.IO;

namespace Match3.Scripts1.Spine
{
	// Token: 0x020001F9 RID: 505
	public class Atlas
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00023E80 File Offset: 0x00022280
		public Atlas(TextReader reader, string dir, TextureLoader textureLoader)
		{
			this.Load(reader, dir, textureLoader);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00023EA7 File Offset: 0x000222A7
		public Atlas(List<AtlasPage> pages, List<AtlasRegion> regions)
		{
			this.pages = pages;
			this.regions = regions;
			this.textureLoader = null;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00023EDC File Offset: 0x000222DC
		private void Load(TextReader reader, string imagesDir, TextureLoader textureLoader)
		{
			if (textureLoader == null)
			{
				throw new ArgumentNullException("textureLoader cannot be null.");
			}
			this.textureLoader = textureLoader;
			string[] array = new string[4];
			AtlasPage atlasPage = null;
			for (;;)
			{
				string text = reader.ReadLine();
				if (text == null)
				{
					break;
				}
				if (text.Trim().Length == 0)
				{
					atlasPage = null;
				}
				else if (atlasPage == null)
				{
					atlasPage = new AtlasPage();
					atlasPage.name = text;
					if (Atlas.ReadTuple(reader, array) == 2)
					{
						atlasPage.width = int.Parse(array[0]);
						atlasPage.height = int.Parse(array[1]);
						Atlas.ReadTuple(reader, array);
					}
					atlasPage.format = (Format)Enum.Parse(typeof(Format), array[0], false);
					Atlas.ReadTuple(reader, array);
					atlasPage.minFilter = (TextureFilter)Enum.Parse(typeof(TextureFilter), array[0], false);
					atlasPage.magFilter = (TextureFilter)Enum.Parse(typeof(TextureFilter), array[1], false);
					string a = Atlas.ReadValue(reader);
					atlasPage.uWrap = TextureWrap.ClampToEdge;
					atlasPage.vWrap = TextureWrap.ClampToEdge;
					if (a == "x")
					{
						atlasPage.uWrap = TextureWrap.Repeat;
					}
					else if (a == "y")
					{
						atlasPage.vWrap = TextureWrap.Repeat;
					}
					else if (a == "xy")
					{
						atlasPage.uWrap = (atlasPage.vWrap = TextureWrap.Repeat);
					}
					textureLoader.Load(atlasPage, Path.Combine(imagesDir, text));
					this.pages.Add(atlasPage);
				}
				else
				{
					AtlasRegion atlasRegion = new AtlasRegion();
					atlasRegion.name = text;
					atlasRegion.page = atlasPage;
					atlasRegion.rotate = bool.Parse(Atlas.ReadValue(reader));
					Atlas.ReadTuple(reader, array);
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					Atlas.ReadTuple(reader, array);
					int num3 = int.Parse(array[0]);
					int num4 = int.Parse(array[1]);
					atlasRegion.u = (float)num / (float)atlasPage.width;
					atlasRegion.v = (float)num2 / (float)atlasPage.height;
					if (atlasRegion.rotate)
					{
						atlasRegion.u2 = (float)(num + num4) / (float)atlasPage.width;
						atlasRegion.v2 = (float)(num2 + num3) / (float)atlasPage.height;
					}
					else
					{
						atlasRegion.u2 = (float)(num + num3) / (float)atlasPage.width;
						atlasRegion.v2 = (float)(num2 + num4) / (float)atlasPage.height;
					}
					atlasRegion.x = num;
					atlasRegion.y = num2;
					atlasRegion.width = Math.Abs(num3);
					atlasRegion.height = Math.Abs(num4);
					if (Atlas.ReadTuple(reader, array) == 4)
					{
						atlasRegion.splits = new int[]
						{
							int.Parse(array[0]),
							int.Parse(array[1]),
							int.Parse(array[2]),
							int.Parse(array[3])
						};
						if (Atlas.ReadTuple(reader, array) == 4)
						{
							atlasRegion.pads = new int[]
							{
								int.Parse(array[0]),
								int.Parse(array[1]),
								int.Parse(array[2]),
								int.Parse(array[3])
							};
							Atlas.ReadTuple(reader, array);
						}
					}
					atlasRegion.originalWidth = int.Parse(array[0]);
					atlasRegion.originalHeight = int.Parse(array[1]);
					Atlas.ReadTuple(reader, array);
					atlasRegion.offsetX = (float)int.Parse(array[0]);
					atlasRegion.offsetY = (float)int.Parse(array[1]);
					atlasRegion.index = int.Parse(Atlas.ReadValue(reader));
					this.regions.Add(atlasRegion);
				}
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00024280 File Offset: 0x00022680
		private static string ReadValue(TextReader reader)
		{
			string text = reader.ReadLine();
			int num = text.IndexOf(':');
			if (num == -1)
			{
				throw new Exception("Invalid line: " + text);
			}
			return text.Substring(num + 1).Trim();
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000242C4 File Offset: 0x000226C4
		private static int ReadTuple(TextReader reader, string[] tuple)
		{
			string text = reader.ReadLine();
			int num = text.IndexOf(':');
			if (num == -1)
			{
				throw new Exception("Invalid line: " + text);
			}
			int i = 0;
			int num2 = num + 1;
			while (i < 3)
			{
				int num3 = text.IndexOf(',', num2);
				if (num3 == -1)
				{
					break;
				}
				tuple[i] = text.Substring(num2, num3 - num2).Trim();
				num2 = num3 + 1;
				i++;
			}
			tuple[i] = text.Substring(num2).Trim();
			return i + 1;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00024354 File Offset: 0x00022754
		public void FlipV()
		{
			int i = 0;
			int count = this.regions.Count;
			while (i < count)
			{
				AtlasRegion atlasRegion = this.regions[i];
				atlasRegion.v = 1f - atlasRegion.v;
				atlasRegion.v2 = 1f - atlasRegion.v2;
				i++;
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000243B0 File Offset: 0x000227B0
		public AtlasRegion FindRegion(string name)
		{
			int i = 0;
			int count = this.regions.Count;
			while (i < count)
			{
				if (this.regions[i].name == name)
				{
					return this.regions[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00024408 File Offset: 0x00022808
		public void Dispose()
		{
			if (this.textureLoader == null)
			{
				return;
			}
			int i = 0;
			int count = this.pages.Count;
			while (i < count)
			{
				this.textureLoader.Unload(this.pages[i].rendererObject);
				i++;
			}
		}

		// Token: 0x04004042 RID: 16450
		private List<AtlasPage> pages = new List<AtlasPage>();

		// Token: 0x04004043 RID: 16451
		private List<AtlasRegion> regions = new List<AtlasRegion>();

		// Token: 0x04004044 RID: 16452
		private TextureLoader textureLoader;
	}
}
