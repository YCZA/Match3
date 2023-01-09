using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C11 RID: 3089
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Tab Navigation Helper")]
	public class TabNavigationHelper : MonoBehaviour
	{
		// Token: 0x060048D2 RID: 18642 RVA: 0x001743E4 File Offset: 0x001727E4
		private void Start()
		{
			this._system = base.GetComponent<EventSystem>();
			if (this._system == null)
			{
				global::UnityEngine.Debug.LogError("Needs to be attached to the Event System component in the scene");
			}
			if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
			{
				this.StartingObject = this.NavigationPath[0].gameObject.GetComponent<Selectable>();
			}
			if (this.StartingObject == null && this.CircularNavigation)
			{
				this.SelectDefaultObject(out this.StartingObject);
			}
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x00174474 File Offset: 0x00172874
		public void Update()
		{
			Selectable selectable = null;
			if (this.LastObject == null && this._system.currentSelectedGameObject != null)
			{
				selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				while (selectable != null)
				{
					this.LastObject = selectable;
					selectable = selectable.FindSelectableOnDown();
				}
			}
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Tab) && global::UnityEngine.Input.GetKey(KeyCode.LeftShift))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
				{
					for (int i = this.NavigationPath.Length - 1; i >= 0; i--)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[i].gameObject))
						{
							selectable = ((i != 0) ? this.NavigationPath[i - 1] : this.NavigationPath[this.NavigationPath.Length - 1]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.LastObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (global::UnityEngine.Input.GetKeyDown(KeyCode.Tab))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
				{
					for (int j = 0; j < this.NavigationPath.Length; j++)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[j].gameObject))
						{
							selectable = ((j != this.NavigationPath.Length - 1) ? this.NavigationPath[j + 1] : this.NavigationPath[0]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.StartingObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (this._system.currentSelectedGameObject == null)
			{
				this.SelectDefaultObject(out selectable);
			}
			if (this.CircularNavigation && this.StartingObject == null)
			{
				this.StartingObject = selectable;
			}
			this.selectGameObject(selectable);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0017471F File Offset: 0x00172B1F
		private void SelectDefaultObject(out Selectable next)
		{
			if (this._system.firstSelectedGameObject)
			{
				next = this._system.firstSelectedGameObject.GetComponent<Selectable>();
			}
			else
			{
				next = null;
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00174750 File Offset: 0x00172B50
		private void selectGameObject(Selectable selectable)
		{
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this._system));
				}
				this._system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(this._system));
			}
		}

		// Token: 0x04006F6E RID: 28526
		private EventSystem _system;

		// Token: 0x04006F6F RID: 28527
		private Selectable StartingObject;

		// Token: 0x04006F70 RID: 28528
		private Selectable LastObject;

		// Token: 0x04006F71 RID: 28529
		[Tooltip("The path to take when user is tabbing through ui components.")]
		public Selectable[] NavigationPath;

		// Token: 0x04006F72 RID: 28530
		[Tooltip("Use the default Unity navigation system or a manual fixed order using Navigation Path")]
		public NavigationMode NavigationMode;

		// Token: 0x04006F73 RID: 28531
		[Tooltip("If True, this will loop the tab order from last to first automatically")]
		public bool CircularNavigation;
	}
}
