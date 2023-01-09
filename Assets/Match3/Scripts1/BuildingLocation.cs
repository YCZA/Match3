using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.UI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000960 RID: 2400
namespace Match3.Scripts1
{
	public class BuildingLocation : MonoBehaviour, IDataView<BuildingInstance>, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x00121F77 File Offset: 0x00120377
		// (set) Token: 0x06003A8D RID: 14989 RVA: 0x00121F80 File Offset: 0x00120380
		public static BuildingInstance Selected
		{
			get
			{
				return BuildingLocation.m_selected;
			}
			set
			{
				if (BuildingLocation.m_selected == value)
				{
					return;
				}
				if (BuildingLocation.m_selected != null && BuildingLocation.m_selected.mustBePlaced)
				{
					return;
				}
				if (BuildingLocation.m_selected != null && BuildingLocation.m_selected.view && BuildingLocation.m_selected.view.transform)
				{
					BuildingInstance selected = BuildingLocation.m_selected;
					IntVector2 location = BuildingLocation.GetLocation(selected);
					BuildingLocation.m_selected = null;
					if (location != selected.position)
					{
						if (BuildingLocation.IsValidPlacement(location, selected))
						{
							selected.position = location;
						}
						selected.onPositionChanged.Dispatch(selected);
					}
					selected.onSelected.Dispatch(selected, false);
					BuildingState state = selected.State;
					if (state != BuildingState.NotSaved)
					{
						if (state == BuildingState.ReadyToPlace)
						{
							selected.onTimer.Dispatch(selected);
						}
					}
					else
					{
						selected.controller.DestroyBuilding(selected);
						selected.controller.onPurchaseCancelled.Dispatch();
						global::UnityEngine.Object.Destroy(selected.view.gameObject);
					}
				}
				BuildingLocation.m_selected = value;
				if (BuildingLocation.m_selected != null)
				{
					BuildingLocation.m_selected.mapDataProvider.onAreaClick.Dispatch(0);
					BuildingLocation.m_selected.onSelected.Dispatch(BuildingLocation.m_selected, true);
				}
				BuildingAssetConnector.UpdateAllConfigurations();
			}
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x001220D8 File Offset: 0x001204D8
		protected void OnDrawGizmos()
		{
			Vector3 b = new Vector3(0.5f, 0f, 0.5f) * (float)this.data.blueprint.size;
			Vector3 size = new Vector3(1f, 0f, 1f) * (float)this.data.blueprint.size;
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawWireCube(base.transform.position + b, size);
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x00122170 File Offset: 0x00120570
		public static bool IsValidPlacement(IntVector2 position, BuildingInstance building)
		{
			int size = building.blueprint.size;
			IntRect buildingArea = new IntRect(position, IntVector2.One * size);
			IEnumerable<BuildingInstance> buildings = building.controller.Buildings;
			return building.mapDataProvider.IsBuildingAreaFree(buildingArea) && !BuildingLocation.BlockedByAnotherBuilding(position, size, building, buildings);
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001221C8 File Offset: 0x001205C8
		public static bool BlockedByAnotherBuilding(IntVector2 position, int buildingSize, BuildingInstance ignoreBuilding, IEnumerable<BuildingInstance> otherBuildings)
		{
			IntRect other = new IntRect(position, IntVector2.One * buildingSize);
			foreach (BuildingInstance buildingInstance in otherBuildings)
			{
				if (buildingInstance != ignoreBuilding && !buildingInstance.blueprint.IsRubble())
				{
					if (buildingInstance.Area.Overlaps(other))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x00122264 File Offset: 0x00120664
		public void Show(BuildingInstance building)
		{
			this.data = building;
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.boxCollider.isTrigger = true;
			this.boxCollider.size = new Vector3(1f, 0f, 1f) * (float)this.data.blueprint.size;
			this.boxCollider.center = new Vector3(0.5f, 0f, 0.5f) * (float)this.data.blueprint.size;
			this.Position = building.position;
			building.onSelected.AddListener(new Action<BuildingInstance, bool>(this.OnSelected));
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00122320 File Offset: 0x00120720
		private void OnSelected(BuildingInstance building, bool selected)
		{
			this.Position = building.position;
			this.boxCollider.size = new Vector3(1f, 0f, 1f) * ((float)this.data.blueprint.size + ((!selected) ? 0f : 2f));
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003A93 RID: 14995 RVA: 0x00122384 File Offset: 0x00120784
		// (set) Token: 0x06003A94 RID: 14996 RVA: 0x00122398 File Offset: 0x00120798
		public IntVector2 Position
		{
			get
			{
				return IntVector2.ProjectToGridXZ(base.transform.position);
			}
			set
			{
				base.transform.position = value.ProjectToVector3XZ();
				if (BuildingLocation.Selected == this.data)
				{
					base.transform.position += Vector3.up * this.data.view.SelectedElevation;
				}
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x001223F7 File Offset: 0x001207F7
		private bool IsLocatedInLockedArea()
		{
			return this.data.mapDataProvider.HasRubble(this.data.position);
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00122414 File Offset: 0x00120814
		private void Select()
		{
			if (this.data.State == BuildingState.ReadyToPlace || this.IsLocatedInLockedArea() || !this.data.sv.IsRepaired)
			{
				return;
			}
			BuildingLocation.Selected = this.data;
			BuildingOperation buildingOperation = BuildingOperation.Cancel | BuildingOperation.ConfirmDisabled;
			if (this.data.CanBeStored())
			{
				buildingOperation |= BuildingOperation.Store;
			}
			this.data.view.controlButtons.Show(this.data, buildingOperation);
			this.dragState = BuildingLocation.DragState.Dragging;
			this.tapPosition = CameraInputController.MousePosition;
			CameraInputController.currentDragObject = base.gameObject;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x001224B0 File Offset: 0x001208B0
		public void OnPointerDown(PointerEventData evt)
		{
			if (TownCheatsRoot.EditMode && evt.button == PointerEventData.InputButton.Right)
			{
				this.data.sv.SetTimer(BuildingTimer.Placed, 0);
				this.data.controller.DestroyBuilding(this.data);
				this.data.controller.RefreshMap();
				global::UnityEngine.Object.Destroy(this.data.view.gameObject);
				return;
			}
			this.tapPosition = CameraInputController.MousePosition;
			this.dragState = BuildingLocation.DragState.Holding;
			this.tryingToDeselect = (BuildingLocation.Selected == this.data);
			CameraInputController.currentDragObject = null;
			if (BuildingLocation.Selected == this.data)
			{
				this.Select();
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00122564 File Offset: 0x00120964
		public IntVector2 SnapToValidSpot()
		{
			foreach (IntVector2 b in IntVector2.SelfSides)
			{
				if (BuildingLocation.IsValidPlacement(this.Position + b, this.data))
				{
					return this.Position + b;
				}
			}
			return this.Position;
		}

		// eli key point 建筑点击相关
		public void OnPointerUp(PointerEventData evt)
		{
			if (this.tryingToDeselect)
			{
				if (this.data.sv.IsSaved)
				{
					BuildingLocation.Selected = null;
				}
				else if (BuildingLocation.IsValidPlacement(this.Position, this.data))
				{
					this.data.Handle(BuildingOperation.Confirm);
				}
				this.dragState = BuildingLocation.DragState.None;
				CameraInputController.currentDragObject = null;
				CameraInputController.current.scrollDirection = Vector3.zero;
				return;
			}
			this.Position = this.SnapToValidSpot();
			BuildingLocation.DragState dragState = this.dragState;
			if (dragState != BuildingLocation.DragState.Holding)
			{
				if (dragState == BuildingLocation.DragState.Dragging)
				{
					this.data.onPositionChanged.Dispatch(this.data);
				}
			}
			else if (BuildingLocation.Selected == this.data || BuildingLocation.Selected == null || BuildingLocation.Selected.sv.IsSaved)
			{
				// onclick
				this.data.OnClick(this.data);
			}
			else if (BuildingLocation.Selected != null)
			{
				BuildingLocation.Selected = null;
			}
			if (BuildingLocation.Selected == this.data && BuildingLocation.GetScrollDirection(evt.position).magnitude > 0.33f)
			{
				CameraInputController.current.SetPositionAnchor(this.data.view.FocusPosition);
			}
			this.dragState = BuildingLocation.DragState.None;
			CameraInputController.currentDragObject = null;
			CameraInputController.current.scrollDirection = Vector3.zero;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x0012273C File Offset: 0x00120B3C
		public void OnDrag(PointerEventData evt)
		{
			this.tryingToDeselect = false;
			BuildingLocation.DragState dragState = this.dragState;
			if (dragState != BuildingLocation.DragState.Holding)
			{
				if (dragState == BuildingLocation.DragState.Dragging)
				{
					base.transform.position += CameraInputController.MousePosition - this.tapPosition;
					this.tapPosition = CameraInputController.MousePosition;
					BuildingLocation.ScrollCamera(evt);
				}
			}
			else
			{
				CameraInputController.currentDragObject = null;
				this.dragState = BuildingLocation.DragState.Cancelled;
			}
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x001227B8 File Offset: 0x00120BB8
		private void OnDestroy()
		{
			BuildingLocation.m_selected = null;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x001227C0 File Offset: 0x00120BC0
		private static Vector2 GetScrollDirection(Vector2 pointer)
		{
			Vector2 result = new Vector2(pointer.x / (float)Screen.width - CameraInputController.SCREEN_CENTER.x, pointer.y / (float)Screen.height - CameraInputController.SCREEN_CENTER.y);
			if (result.y < 0f)
			{
				result.y *= 1.75f;
			}
			return result;
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00122834 File Offset: 0x00120C34
		private static void ScrollCamera(PointerEventData evt)
		{
			Vector3 a = new Vector3(1f, 0f, -1f);
			Vector3 a2 = new Vector3(1f, 0f, 1f);
			Vector2 scrollDirection = BuildingLocation.GetScrollDirection(evt.position);
			Vector2 vector = scrollDirection.normalized * Mathf.Max(0f, scrollDirection.magnitude - 0.33f) * 20f;
			CameraInputController.current.scrollDirection = a * vector.x + a2 * vector.y;
			CameraInputController.current.lastPointerEvent = evt;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001228DB File Offset: 0x00120CDB
		public static IntVector2 GetLocation(BuildingInstance building)
		{
			if (BuildingLocation.Selected == building)
			{
				return BuildingLocation.Selected.view.Position;
			}
			return building.position;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00122900 File Offset: 0x00120D00
		public static Bounds RealignBounds(Bounds bounds, Vector3 center)
		{
			Vector3 a = Vector3.Max(bounds.max - center, center - bounds.min);
			return new Bounds(center, a * 2f);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00122940 File Offset: 0x00120D40
		public static Bounds RealignBounds(Bounds bounds)
		{
			Vector3 center = new Vector3(0.5f, 0f, 0.5f);
			return BuildingLocation.RealignBounds(bounds, center);
		}

		// Token: 0x0400626B RID: 25195
		private const float SELECTED_COLLIDER_PADDING = 2f;

		// Token: 0x0400626C RID: 25196
		private BuildingLocation.DragState dragState;

		// Token: 0x0400626D RID: 25197
		private Vector3 tapPosition;

		// Token: 0x0400626E RID: 25198
		private BuildingInstance data;

		// Token: 0x0400626F RID: 25199
		private static BuildingInstance m_selected;

		// Token: 0x04006270 RID: 25200
		private bool tryingToDeselect;

		// Token: 0x04006271 RID: 25201
		private BoxCollider boxCollider;

		// Token: 0x04006272 RID: 25202
		private const float SCROLL_CROP_VALUE = 0.33f;

		// Token: 0x04006273 RID: 25203
		private const float SCROLL_SPEED = 20f;

		// Token: 0x04006274 RID: 25204
		private const float CAMERA_CENTER_THRESHOLD = 0.33f;

		// Token: 0x04006275 RID: 25205
		private const float SCROLL_BOTTOM_DISTORTION = 1.75f;

		// Token: 0x02000961 RID: 2401
		private enum DragState
		{
			// Token: 0x04006277 RID: 25207
			None,
			// Token: 0x04006278 RID: 25208
			Holding,
			// Token: 0x04006279 RID: 25209
			Dragging,
			// Token: 0x0400627A RID: 25210
			Cancelled
		}
	}
}
