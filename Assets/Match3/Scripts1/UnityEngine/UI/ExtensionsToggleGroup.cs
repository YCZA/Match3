using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI
{
	// Token: 0x02000C20 RID: 3104
	[AddComponentMenu("UI/Extensions/Extensions Toggle Group")]
	[DisallowMultipleComponent]
	public class ExtensionsToggleGroup : UIBehaviour
	{
		// Token: 0x06004945 RID: 18757 RVA: 0x00176EFB File Offset: 0x001752FB
		protected ExtensionsToggleGroup()
		{
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004946 RID: 18758 RVA: 0x00176F24 File Offset: 0x00175324
		// (set) Token: 0x06004947 RID: 18759 RVA: 0x00176F2C File Offset: 0x0017532C
		public bool AllowSwitchOff
		{
			get
			{
				return this.m_AllowSwitchOff;
			}
			set
			{
				this.m_AllowSwitchOff = value;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06004948 RID: 18760 RVA: 0x00176F35 File Offset: 0x00175335
		// (set) Token: 0x06004949 RID: 18761 RVA: 0x00176F3D File Offset: 0x0017533D
		public ExtensionsToggle SelectedToggle { get; private set; }

		// Token: 0x0600494A RID: 18762 RVA: 0x00176F46 File Offset: 0x00175346
		private void ValidateToggleIsInGroup(ExtensionsToggle toggle)
		{
			if (toggle == null || !this.m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[]
				{
					toggle,
					this
				}));
			}
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x00176F84 File Offset: 0x00175384
		public void NotifyToggleOn(ExtensionsToggle toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				if (this.m_Toggles[i] == toggle)
				{
					this.SelectedToggle = toggle;
				}
				else
				{
					this.m_Toggles[i].IsOn = false;
				}
			}
			this.onToggleGroupChanged.Invoke(this.AnyTogglesOn());
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x00176FFA File Offset: 0x001753FA
		public void UnregisterToggle(ExtensionsToggle toggle)
		{
			if (this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Remove(toggle);
				toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x00177031 File Offset: 0x00175431
		private void NotifyToggleChanged(bool isOn)
		{
			this.onToggleGroupToggleChanged.Invoke(isOn);
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x0017703F File Offset: 0x0017543F
		public void RegisterToggle(ExtensionsToggle toggle)
		{
			if (!this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Add(toggle);
				toggle.onValueChanged.AddListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00177075 File Offset: 0x00175475
		public bool AnyTogglesOn()
		{
			return this.m_Toggles.Find((ExtensionsToggle x) => x.IsOn) != null;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x001770A5 File Offset: 0x001754A5
		public IEnumerable<ExtensionsToggle> ActiveToggles()
		{
			return from x in this.m_Toggles
			where x.IsOn
			select x;
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x001770D0 File Offset: 0x001754D0
		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = this.m_AllowSwitchOff;
			this.m_AllowSwitchOff = true;
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				this.m_Toggles[i].IsOn = false;
			}
			this.m_AllowSwitchOff = allowSwitchOff;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00177120 File Offset: 0x00175520
		public void HasTheGroupToggle(bool value)
		{
			global::UnityEngine.Debug.Log("Testing, the group has toggled [" + value + "]");
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0017713C File Offset: 0x0017553C
		public void HasAToggleFlipped(bool value)
		{
			global::UnityEngine.Debug.Log("Testing, a toggle has toggled [" + value + "]");
		}

		// Token: 0x04006FBE RID: 28606
		[SerializeField]
		private bool m_AllowSwitchOff;

		// Token: 0x04006FBF RID: 28607
		private List<ExtensionsToggle> m_Toggles = new List<ExtensionsToggle>();

		// Token: 0x04006FC0 RID: 28608
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x04006FC1 RID: 28609
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupToggleChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x02000C21 RID: 3105
		[Serializable]
		public class ToggleGroupEvent : UnityEvent<bool>
		{
		}
	}
}
