using System;
using System.Collections.Generic;

// Token: 0x0200047C RID: 1148
namespace Match3.Scripts1
{
	[Serializable]
	public class BuildingToBundleMap
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x0008B860 File Offset: 0x00089C60
		public Dictionary<string, BuildingToBundleMap.Entry> Map
		{
			get
			{
				if (this.map == null)
				{
					this.map = new Dictionary<string, BuildingToBundleMap.Entry>();
					foreach (BuildingToBundleMap.Entry entry in this.entries)
					{
						this.map[entry.id] = entry;
					}
				}
				return this.map;
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0008B8E4 File Offset: 0x00089CE4
		public BuildingToBundleMap.Entry FindEntry(string buildingID, bool asDestroyed, bool asIcon)
		{
			BuildingToBundleMap.Entry entry;
			if (!this.Map.TryGetValue(buildingID, out entry))
			{
				WoogaDebug.Log(new object[]
				{
					"Asset not found in any bundle: ",
					buildingID,
					"asDestroyed: ",
					asDestroyed,
					"asIcon: ",
					asIcon
				});
			}
			else
			{
				if (asIcon && !entry.HasCompleteIconInfo)
				{
					return null;
				}
				if (asDestroyed && !entry.HasCompleteDestroyedInfo)
				{
					return null;
				}
				if (!asIcon && !asDestroyed && !entry.HasCompleteBuildingInfo)
				{
					return null;
				}
			}
			return entry;
		}

		// Token: 0x04004BC8 RID: 19400
		public List<BuildingToBundleMap.Entry> entries = new List<BuildingToBundleMap.Entry>();

		// Token: 0x04004BC9 RID: 19401
		private Dictionary<string, BuildingToBundleMap.Entry> map;

		// Token: 0x0200047D RID: 1149
		[Serializable]
		public class Entry
		{
			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x06002122 RID: 8482 RVA: 0x0008B98A File Offset: 0x00089D8A
			public bool HasCompleteBuildingInfo
			{
				get
				{
					return !this.id.IsNullOrEmpty() && !this.prefabBundle.IsNullOrEmpty() && !this.path.IsNullOrEmpty();
				}
			}

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x06002123 RID: 8483 RVA: 0x0008B9BD File Offset: 0x00089DBD
			public bool HasCompleteIconInfo
			{
				get
				{
					return !this.id.IsNullOrEmpty() && !this.iconBundle.IsNullOrEmpty() && !this.icon.IsNullOrEmpty();
				}
			}

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x06002124 RID: 8484 RVA: 0x0008B9F0 File Offset: 0x00089DF0
			public bool HasCompleteDestroyedInfo
			{
				get
				{
					return !this.id.IsNullOrEmpty() && !this.destroyedBundle.IsNullOrEmpty() && !this.destroyedPath.IsNullOrEmpty();
				}
			}

			// Token: 0x06002125 RID: 8485 RVA: 0x0008BA23 File Offset: 0x00089E23
			public string GetAssetPath(bool asDestroyed, bool asIcon)
			{
				if (asIcon)
				{
					return this.icon;
				}
				return (!asDestroyed) ? this.path : this.destroyedPath;
			}

			// Token: 0x04004BCA RID: 19402
			public string id;

			// Token: 0x04004BCB RID: 19403
			public string prefabBundle;

			// Token: 0x04004BCC RID: 19404
			public string iconBundle;

			// Token: 0x04004BCD RID: 19405
			public string destroyedBundle;

			// Token: 0x04004BCE RID: 19406
			public string path;

			// Token: 0x04004BCF RID: 19407
			public string icon;

			// Token: 0x04004BD0 RID: 19408
			public string destroyedPath;
		}
	}
}
