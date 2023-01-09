using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004E6 RID: 1254
	[Serializable]
	public class AreasConfig: IInitializable
	{
		public void Init()
		{
			areas.Add(area1);
			areas.Add(area2);
			areas.Add(area3);
			areas.Add(area4);
			areas.Add(area5);
			areas.Add(area6);
			areas.Add(area7);
			areas.Add(area8);
			areas.Add(area9);
			areas.Add(area10);
			areas.Add(area11);
			areas.Add(area12);
			areas.Add(area13);
			areas.Add(area14);
			areas.Add(area15);
			areas.Add(area16);
			areas.Add(area17);
			areas.Add(area18);
			areas.Add(area19);
			areas.Add(area20);
			areas.Add(area21);
			areas.Add(area22);
			areas.Add(area23);
			areas.Add(area24);
			areas.Add(area25);
			areas.Add(area26);
			areas.Add(area27);
			areas.Add(area28);
			areas.Add(area29);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0009A294 File Offset: 0x00098694
		public int AreaIndexForLevel(int level)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				Area area = this.areas[i];
				if (level >= area.levels.First<AreaConfig>().level && level <= area.levels.Last<AreaConfig>().level)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0009A2F9 File Offset: 0x000986F9
		public int AreaForLevel(int level)
		{
			return this.AreaIndexForLevel(level) + 1;
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x0009A304 File Offset: 0x00098704
		public IEnumerable<AreaConfig> AllLevels
		{
			get
			{
				return this.areas.SelectMany((Area a) => a.levels);
			}
		}

		// Token: 0x04004E6F RID: 20079
		public List<Area> areas = new List<Area>();
		
		[SerializeField]
		private Area area1 = new Area();
		[SerializeField]
		private Area area2 = new Area();
		[SerializeField]
		private Area area3 = new Area();
		[SerializeField]
		private Area area4 = new Area();
		[SerializeField]
		private Area area5 = new Area();
		[SerializeField]
		private Area area6 = new Area();
		[SerializeField]
		private Area area7 = new Area();
		[SerializeField]
		private Area area8 = new Area();
		[SerializeField]
		private Area area9 = new Area();
		[SerializeField]
		private Area area10 = new Area();
		[SerializeField]
		private Area area11 = new Area();
		[SerializeField]
		private Area area12 = new Area();
		[SerializeField]
		private Area area13 = new Area();
		[SerializeField]
		private Area area14 = new Area();
		[SerializeField]
		private Area area15 = new Area();
		[SerializeField]
		private Area area16 = new Area();
		[SerializeField]
		private Area area17 = new Area();
		[SerializeField]
		private Area area18 = new Area();
		[SerializeField]
		private Area area19 = new Area();
		[SerializeField]
		private Area area20 = new Area();
		[SerializeField]
		private Area area21 = new Area();
		[SerializeField]
		private Area area22 = new Area();
		[SerializeField]
		private Area area23 = new Area();
		[SerializeField]
		private Area area24 = new Area();
		[SerializeField]
		private Area area25 = new Area();
		[SerializeField]
		private Area area26 = new Area();
		[SerializeField]
		private Area area27 = new Area();
		[SerializeField]
		private Area area28 = new Area();
		[SerializeField]
		private Area area29 = new Area();
	}
}
