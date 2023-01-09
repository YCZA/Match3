using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Datasources
{
	// Token: 0x020008AB RID: 2219
	public class MultiFriendsSelectorDataSource : ArrayDataSource<MultiFriendsSelectorView, MultiFriendsSelectorFriendData>
	{
		// Token: 0x06003630 RID: 13872 RVA: 0x0010661E File Offset: 0x00104A1E
		public override void Show(IEnumerable<MultiFriendsSelectorFriendData> friends)
		{
			base.Show(friends);
			this.loadingSpinner.gameObject.SetActive(friends == null);
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0010663B File Offset: 0x00104A3B
		public override int GetReusableIdForIndex(int index)
		{
			return (int)this.dataArray[index].CurrentState;
		}

		// Token: 0x04005E32 RID: 24114
		public GameObject loadingSpinner;
	}
}
