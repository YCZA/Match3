using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BAA RID: 2986
	[RequireComponent(typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Extensions/Re-orderable list")]
	public class ReorderableList : MonoBehaviour
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x00162715 File Offset: 0x00160B15
		public RectTransform Content
		{
			get
			{
				if (this._content == null)
				{
					this._content = this.ContentLayout.GetComponent<RectTransform>();
				}
				return this._content;
			}
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00162740 File Offset: 0x00160B40
		private Canvas GetCanvas()
		{
			Transform transform = base.transform;
			Canvas canvas = null;
			int num = 100;
			int num2 = 0;
			while (canvas == null && num2 < num)
			{
				canvas = transform.gameObject.GetComponent<Canvas>();
				if (canvas == null)
				{
					transform = transform.parent;
				}
				num2++;
			}
			return canvas;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00162798 File Offset: 0x00160B98
		private void Awake()
		{
			if (this.ContentLayout == null)
			{
				global::UnityEngine.Debug.LogError("You need to have a child LayoutGroup content set for the list: " + base.name, base.gameObject);
				return;
			}
			if (this.DraggableArea == null)
			{
				this.DraggableArea = base.transform.root.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
			}
			if (this.IsDropable && !base.GetComponent<Graphic>())
			{
				global::UnityEngine.Debug.LogError("You need to have a Graphic control (such as an Image) for the list [" + base.name + "] to be droppable", base.gameObject);
				return;
			}
			this._listContent = this.ContentLayout.gameObject.AddComponent<ReorderableListContent>();
			this._listContent.Init(this);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x0016285C File Offset: 0x00160C5C
		public void TestReOrderableListTarget(ReorderableList.ReorderableListEventStruct item)
		{
			global::UnityEngine.Debug.Log("Event Received");
			global::UnityEngine.Debug.Log("Hello World, is my item a clone? [" + item.IsAClone + "]");
		}

		// Token: 0x04006D70 RID: 28016
		[Tooltip("Child container with re-orderable items in a layout group")]
		public LayoutGroup ContentLayout;

		// Token: 0x04006D71 RID: 28017
		[Tooltip("Parent area to draw the dragged element on top of containers. Defaults to the root Canvas")]
		public RectTransform DraggableArea;

		// Token: 0x04006D72 RID: 28018
		[Tooltip("Can items be dragged from the container?")]
		public bool IsDraggable = true;

		// Token: 0x04006D73 RID: 28019
		[Tooltip("Should the draggable components be removed or cloned?")]
		public bool CloneDraggedObject;

		// Token: 0x04006D74 RID: 28020
		[Tooltip("Can new draggable items be dropped in to the container?")]
		public bool IsDropable = true;

		// Token: 0x04006D75 RID: 28021
		[Header("UI Re-orderable Events")]
		public ReorderableList.ReorderableListHandler OnElementDropped = new ReorderableList.ReorderableListHandler();

		// Token: 0x04006D76 RID: 28022
		public ReorderableList.ReorderableListHandler OnElementGrabbed = new ReorderableList.ReorderableListHandler();

		// Token: 0x04006D77 RID: 28023
		public ReorderableList.ReorderableListHandler OnElementRemoved = new ReorderableList.ReorderableListHandler();

		// Token: 0x04006D78 RID: 28024
		public ReorderableList.ReorderableListHandler OnElementAdded = new ReorderableList.ReorderableListHandler();

		// Token: 0x04006D79 RID: 28025
		private RectTransform _content;

		// Token: 0x04006D7A RID: 28026
		private ReorderableListContent _listContent;

		// Token: 0x02000BAB RID: 2987
		[Serializable]
		public struct ReorderableListEventStruct
		{
			// Token: 0x06004600 RID: 17920 RVA: 0x00162888 File Offset: 0x00160C88
			public void Cancel()
			{
				this.SourceObject.GetComponent<ReorderableListElement>().isValid = false;
			}

			// Token: 0x04006D7B RID: 28027
			public GameObject DroppedObject;

			// Token: 0x04006D7C RID: 28028
			public int FromIndex;

			// Token: 0x04006D7D RID: 28029
			public ReorderableList FromList;

			// Token: 0x04006D7E RID: 28030
			public bool IsAClone;

			// Token: 0x04006D7F RID: 28031
			public GameObject SourceObject;

			// Token: 0x04006D80 RID: 28032
			public int ToIndex;

			// Token: 0x04006D81 RID: 28033
			public ReorderableList ToList;
		}

		// Token: 0x02000BAC RID: 2988
		[Serializable]
		public class ReorderableListHandler : UnityEvent<ReorderableList.ReorderableListEventStruct>
		{
		}
	}
}
