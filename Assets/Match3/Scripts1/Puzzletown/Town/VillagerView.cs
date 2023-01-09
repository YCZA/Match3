using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Match3.Scripts1.Spine.Unity;
using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Town
{
	// Token: 0x020008D2 RID: 2258
	[RequireComponent(typeof(Animator))]
	public class VillagerView : AVisibleGameObject, IHandler<VillagerInteractionTrigger>
	{
		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060036EE RID: 14062 RVA: 0x0010C540 File Offset: 0x0010A940
		public Vector3 pivot
		{
			get
			{
				return base.transform.position + Vector3.up * this.pivotHeight;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060036EF RID: 14063 RVA: 0x0010C562 File Offset: 0x0010A962
		// (set) Token: 0x060036F0 RID: 14064 RVA: 0x0010C56C File Offset: 0x0010A96C
		public DedicatedPosition DedicatedPosition
		{
			get
			{
				return this.dedicatedPosition;
			}
			set
			{
				this.dedicatedPosition = value;
				base.transform.position = value.transform.position;
				this.LookAt(value.transform.localToWorldMatrix.MultiplyPoint(Vector3.forward));
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x0010C5B4 File Offset: 0x0010A9B4
		// (set) Token: 0x060036F2 RID: 14066 RVA: 0x0010C5BC File Offset: 0x0010A9BC
		public string villagerName { get; private set; }

		// Token: 0x060036F3 RID: 14067 RVA: 0x0010C5C8 File Offset: 0x0010A9C8
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.skeleton = base.GetComponent<SkeletonRendererCPU>();
			if (this.createColliders)
			{
				this.CreateTrigger(VillagerInteraction.LookAtVillager, 2f);
				this.CreateTrigger(VillagerInteraction.LookAtDeco, 1f);
				Transform transform = base.transform.CreateChild("Interaction Trigger");
				BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
				transform.gameObject.AddComponent<VillagerTapBox>();
				transform.gameObject.layer = 16;
				boxCollider.isTrigger = true;
				boxCollider.size = new Vector3(1f, this.pivotHeight / 2f, 1f);
				transform.localPosition = new Vector3(0f, this.pivotHeight / 4f, 0f);
			}
			// this.skeleton.GetComponent<MeshRenderer>().sortingLayerName = CameraExtensions.ABOVE_FORSHADOWING_LAYER;
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x0010C6A4 File Offset: 0x0010AAA4
		private void CreateTrigger(VillagerInteraction type, float radius)
		{
			Transform transform = base.transform.CreateChild("Trigger");
			SphereCollider sphereCollider = transform.gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = radius;
			sphereCollider.isTrigger = true;
			transform.gameObject.AddComponent<VillagerInteractionTrigger>().type = type;
			transform.gameObject.layer = 2;
			transform.localPosition = new Vector3(0f, 0f, 0f);
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x0010C714 File Offset: 0x0010AB14
		public void Handle(VillagerOperation op)
		{
			if (op != VillagerOperation.FocusCamera)
			{
				if (op == VillagerOperation.ShowInformation)
				{
					SceneManager.TryLoadQuestPopupOnIsland();
				}
			}
			else
			{
				Vector3 b = base.transform.position - CameraInputController.CameraPosition;
				Camera.main.transform.DOMove(Camera.main.transform.position + b, 0.5f, false);
			}
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0010C784 File Offset: 0x0010AB84
		public void Handle(VillagerInteractionTrigger trigger)
		{
			VillagerInteraction type = trigger.type;
			if (type != VillagerInteraction.LookAtDeco)
			{
				if (type == VillagerInteraction.LookAtVillager || type == VillagerInteraction.TalkToVillager)
				{
					VillagerView componentInParent = trigger.GetComponentInParent<VillagerView>();
					if (componentInParent)
					{
						this.onVillagerInteraction.Dispatch(this, componentInParent, trigger.type);
					}
				}
			}
			else
			{
				BuildingMainView component = trigger.GetComponent<BuildingMainView>();
				if (component)
				{
					this.onBuildingInteraction.Dispatch(this, component.data);
				}
			}
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x0010C80E File Offset: 0x0010AC0E
		public bool Init(QuestData quest, QuestProgress progress)
		{
			if (progress.status > this.Status && this.Status != QuestProgress.Status.undefined)
			{
				return false;
			}
			this.progress = progress;
			this.villagerName = quest.character;
			return true;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x0010C842 File Offset: 0x0010AC42
		public bool Init(string name)
		{
			this.villagerName = name;
			this.progress = null;
			return true;
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x0010C854 File Offset: 0x0010AC54
		private float CalculatePathLength(Vector3[] path)
		{
			float num = 0f;
			int num2 = 0;
			for (int i = 1; i < path.Length; i++)
			{
				num += (path[num2] - path[i]).magnitude;
				num2++;
			}
			return num;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x0010C8AA File Offset: 0x0010ACAA
		public void MoveTo(Vector2 position, IPathfinder pathfinder)
		{
			this.MoveTo(position, pathfinder, this.moveSpeed);
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x0010C8BC File Offset: 0x0010ACBC
		public void MoveTo(Vector2 position, IPathfinder pathfinder, float speed)
		{
			// position: 世界坐标下的目标位置
			Vector2[] pathArray;
			if (!pathfinder.GetPath(base.transform.position.xz(), position, out pathArray))
			{
				return;
			}
			if (VillagerView._003C_003Ef__mg_0024cache0 == null)
			{
				VillagerView._003C_003Ef__mg_0024cache0 = new Converter<Vector2, Vector3>(VectorExtensions.x0y);
			}
			Vector3[] path = Array.ConvertAll<Vector2, Vector3>(pathArray, VillagerView._003C_003Ef__mg_0024cache0);
			this.mover = base.transform.DOPath(path, this.CalculatePathLength(path) / speed, PathType.Linear, PathMode.Full3D, 10, null).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.isMoving = false;
			});
			this.isMoving = true;
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x0010C952 File Offset: 0x0010AD52
		public void StopMovement()
		{
			base.transform.DOKill(false);
			this.isMoving = false;
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x0010C968 File Offset: 0x0010AD68
		public QuestProgress.Status Status
		{
			get
			{
				return (this.progress == null) ? QuestProgress.Status.undefined : this.progress.status;
			}
		}

		// 角色转向
		public void LookAt(Vector3 position)
		{
			var sprite = transform.GetComponent<SpriteRenderer>();
			if (sprite == null)
			{
				return;
			}
			
			float num = Vector3.Dot((position - base.transform.position).normalized, new Vector3(-1f, 0f, 1f));
			if (num < -0.05f)
			{
				// this.skeleton.flipped = true;
				sprite.flipX = true;
			}
			else if (num > 0.05f)
			{
				// this.skeleton.flipped = false;
				sprite.flipX = false;
			}
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x0010C9FC File Offset: 0x0010ADFC
		private void FixedUpdate()
		{
			if (this.isMoving && (base.transform.position - this.lastPosition).sqrMagnitude > 0.01f)
			{
				this.LookAt(base.transform.position * 2f - this.lastPosition);
				this.lastPosition = base.transform.position;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x0010CA73 File Offset: 0x0010AE73
		// (set) Token: 0x06003701 RID: 14081 RVA: 0x0010CA7C File Offset: 0x0010AE7C
		public bool isMoving
		{
			get
			{
				return this._isMoving;
			}
			private set
			{
				if (!value && this.isMoving)
				{
					this.onMovementFinished.Dispatch(this);
					this.mover = null;
				}
				this.lastPosition = base.transform.position;
				this.animator.SetBool(this.WALK, value);
				this._isMoving = value;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x0010CAD7 File Offset: 0x0010AED7
		public bool CanMove
		{
			get
			{
				return base.gameObject.activeInHierarchy && !this.isMoving && this.CanInteract;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x0010CAFD File Offset: 0x0010AEFD
		public bool CanInteract
		{
			get
			{
				return !this.isLocked;
			}
		}

		// eli todo 角色方向暂时不管
		public bool isFlipped
		{
			get
			{
				// return this.skeleton.flipped;
				return true;
			}
			set
			{
				// this.skeleton.flipped = value;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x0010CB23 File Offset: 0x0010AF23
		public SBOrientation Orientation
		{
			get
			{
				return (!this.isFlipped) ? SBOrientation.Left : SBOrientation.Right;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x0010CB38 File Offset: 0x0010AF38
		public Vector2 Position
		{
			get
			{
				return new Vector2(base.transform.position.x, base.transform.position.z);
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x0010CB70 File Offset: 0x0010AF70
		private void OnEnable()
		{
			if (this.isMoving && this.mover != null)
			{
				this.animator.SetBool(this.WALK, true);
				this.mover.Play<Tweener>();
			}
			else
			{
				this.animator.SetTrigger(this.lastTrigger);
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0010CBC7 File Offset: 0x0010AFC7
		private void OnDisable()
		{
			if (this.mover != null)
			{
				this.mover.Pause<Tweener>();
			}
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0010CBE0 File Offset: 0x0010AFE0
		private void OnDestroy()
		{
			if (this.mover != null)
			{
				this.mover.Kill(false);
				this.mover = null;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0010CC00 File Offset: 0x0010B000
		public void SetTrigger(string name)
		{
			this.lastTrigger = name;
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.animator.SetTrigger(name);
		}

		// Token: 0x04005F15 RID: 24341
		private const float FLIP_SQR_EPS = 0.01f;

		// Token: 0x04005F16 RID: 24342
		private const float FLIP_THRESHOLD = 0.05f;

		// Token: 0x04005F17 RID: 24343
		private const float VILLAGER_TRIGGER_RADIUS = 2f;

		// Token: 0x04005F18 RID: 24344
		private const float DECO_TRIGGER_RADIUS = 1f;

		// Token: 0x04005F19 RID: 24345
		public readonly int WALK = Animator.StringToHash("walk");

		// Token: 0x04005F1A RID: 24346
		private DedicatedPosition dedicatedPosition;

		// Token: 0x04005F1B RID: 24347
		private QuestProgress progress;

		// Token: 0x04005F1C RID: 24348
		private SkeletonRendererCPU skeleton;

		// Token: 0x04005F1D RID: 24349
		private Vector3 lastPosition;

		// Token: 0x04005F1E RID: 24350
		private Tweener mover;

		// Token: 0x04005F1F RID: 24351
		private string lastTrigger = "idle";

		// Token: 0x04005F20 RID: 24352
		public float moveSpeed = 1f;

		// Token: 0x04005F21 RID: 24353
		public float pivotHeight = 2f;

		// Token: 0x04005F22 RID: 24354
		public bool createColliders = true;

		// Token: 0x04005F23 RID: 24355
		private Animator animator;

		// Token: 0x04005F25 RID: 24357
		public readonly Signal<VillagerView> onMovementFinished = new Signal<VillagerView>();

		// Token: 0x04005F26 RID: 24358
		public readonly Signal<VillagerView, BuildingInstance> onBuildingInteraction = new Signal<VillagerView, BuildingInstance>();

		// Token: 0x04005F27 RID: 24359
		public readonly Signal<VillagerView, VillagerView, VillagerInteraction> onVillagerInteraction = new Signal<VillagerView, VillagerView, VillagerInteraction>();

		// Token: 0x04005F28 RID: 24360
		[NonSerialized]
		public bool isLocked;

		// Token: 0x04005F29 RID: 24361
		private bool _isMoving;

		// Token: 0x04005F2A RID: 24362
		[CompilerGenerated]
		private static Converter<Vector2, Vector3> _003C_003Ef__mg_0024cache0;
	}
}
