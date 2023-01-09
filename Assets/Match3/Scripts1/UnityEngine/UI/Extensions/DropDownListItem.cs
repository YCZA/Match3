using System;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA2 RID: 2978
	[Serializable]
	public class DropDownListItem
	{
		// Token: 0x060045B7 RID: 17847 RVA: 0x00161DD0 File Offset: 0x001601D0
		public DropDownListItem(string caption = "", string inId = "", Sprite image = null, bool disabled = false, Action onSelect = null)
		{
			this._caption = caption;
			this._image = image;
			this._id = inId;
			this._isDisabled = disabled;
			this.OnSelect = onSelect;
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00161DFD File Offset: 0x001601FD
		// (set) Token: 0x060045B9 RID: 17849 RVA: 0x00161E05 File Offset: 0x00160205
		public string Caption
		{
			get
			{
				return this._caption;
			}
			set
			{
				this._caption = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060045BA RID: 17850 RVA: 0x00161E24 File Offset: 0x00160224
		// (set) Token: 0x060045BB RID: 17851 RVA: 0x00161E2C File Offset: 0x0016022C
		public Sprite Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060045BC RID: 17852 RVA: 0x00161E4B File Offset: 0x0016024B
		// (set) Token: 0x060045BD RID: 17853 RVA: 0x00161E53 File Offset: 0x00160253
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				this._isDisabled = value;
				if (this.OnUpdate != null)
				{
					this.OnUpdate();
				}
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x00161E72 File Offset: 0x00160272
		// (set) Token: 0x060045BF RID: 17855 RVA: 0x00161E7A File Offset: 0x0016027A
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x04006D4A RID: 27978
		[SerializeField]
		private string _caption;

		// Token: 0x04006D4B RID: 27979
		[SerializeField]
		private Sprite _image;

		// Token: 0x04006D4C RID: 27980
		[SerializeField]
		private bool _isDisabled;

		// Token: 0x04006D4D RID: 27981
		[SerializeField]
		private string _id;

		// Token: 0x04006D4E RID: 27982
		public Action OnSelect;

		// Token: 0x04006D4F RID: 27983
		internal Action OnUpdate;
	}
}
