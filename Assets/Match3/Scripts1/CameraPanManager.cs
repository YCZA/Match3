using System.Collections;
using DG.Tweening;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using UnityEngine;

// Token: 0x02000902 RID: 2306
namespace Match3.Scripts1
{
	public class CameraPanManager : MonoBehaviour
	{
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06003840 RID: 14400 RVA: 0x00112EFA File Offset: 0x001112FA
		// (set) Token: 0x06003841 RID: 14401 RVA: 0x00112F02 File Offset: 0x00111302
		public bool isFollowing { get; private set; }

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x00112F0B File Offset: 0x0011130B
		// (set) Token: 0x06003843 RID: 14403 RVA: 0x00112F13 File Offset: 0x00111313
		public bool isPanning { get; private set; }

		// Token: 0x06003844 RID: 14404 RVA: 0x00112F1C File Offset: 0x0011131C
		private void Awake()
		{
			this.mainCamera = Camera.main;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x00112F29 File Offset: 0x00111329
		private void OnDestroy()
		{
			this.TryStopPanRoutine();
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00112F31 File Offset: 0x00111331
		public Coroutine PanTo(Vector3 target, bool shouldZoom = false, float zoomRatio = 0.5f)
		{
			this.TryStopPanRoutine();
			this.panRoutine = this.PanTo(target, 0.5f, shouldZoom, zoomRatio);
			return this.panRoutine;
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00112F53 File Offset: 0x00111353
		public Coroutine PanTo(Vector3 target, float time, bool shouldZoom = false, float zoomRatio = 0.5f)
		{
			this.TryStopPanRoutine();
			CameraInputController.current.ResetVelocity();
			this.panRoutine = WooroutineRunner.StartCoroutine(this.DoPanTo(target, time, shouldZoom, zoomRatio), null);
			return this.panRoutine;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00112F82 File Offset: 0x00111382
		private void TryStopPanRoutine()
		{
			if (this.panRoutine != null)
			{
				WooroutineRunner.Stop(this.panRoutine);
			}
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00112F9C File Offset: 0x0011139C
		private IEnumerator DoPanTo(Vector3 target, float time, bool shouldZoom = false, float zoomRatio = 0.5f)
		{
			if (!this.Enabled)
			{
				this.panRoutine = null;
				yield break;
			}
			if (shouldZoom)
			{
				target = this.CalculateTargetPanAndZoom(target, zoomRatio);
			}
			Vector3 delta = (!shouldZoom) ? (target - CameraInputController.CameraPosition) : (target - base.transform.position);
			yield return this.DoPanBy(delta, time, shouldZoom);
			yield break;
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00112FD4 File Offset: 0x001113D4
		private IEnumerator DoPanBy(Vector3 delta, float time, bool shouldZoom = false)
		{
			if (!this.Enabled)
			{
				this.panRoutine = null;
				yield break;
			}
			CameraInputController.current.enabled = false;
			this.isPanning = true;
			if (!shouldZoom)
			{
				delta.y = 0f;
			}
			Vector3 targetPosition = base.transform.position + delta;
			yield return base.transform.DOMove(targetPosition, time, false).SetEase(Ease.OutCubic).WaitForCompletion();
			if (CameraInputController.current != null)
			{
				CameraInputController.current.ResetVelocity();
				CameraInputController.current.enabled = true;
			}
			this.isPanning = false;
			this.panRoutine = null;
			yield break;
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00113004 File Offset: 0x00111404
		private Vector3 CalculateTargetPanAndZoom(Vector3 panTarget, float zoomInRatio)
		{
			CameraInputController current = CameraInputController.current;
			float d = current.CamDistanceMax - (current.CamDistanceMax - current.CamDistanceMin) * zoomInRatio;
			Vector3 a = Vector3.Normalize(this.mainCamera.transform.position - CameraInputController.CameraPosition);
			return panTarget + a * d;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x0011305B File Offset: 0x0011145B
		public void Follow(Transform target)
		{
			this.followTarget = target;
			this.followPosition = target.position;
			this.followVelocity = Vector3.zero;
			this.isFollowing = true;
			CameraInputController.current.enabled = false;
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0011308D File Offset: 0x0011148D
		public void Unfollow()
		{
			this.isFollowing = false;
			CameraInputController.current.ResetVelocity();
			CameraInputController.current.enabled = true;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x001130AC File Offset: 0x001114AC
		private void Update()
		{
			if (this.isFollowing)
			{
				if (this.followTarget)
				{
					this.followPosition = this.followTarget.position;
				}
				Vector3 b = this.mainCamera.transform.position - CameraInputController.CameraPosition;
				Vector3 position = this.mainCamera.transform.position;
				Vector3 vector = Vector3.SmoothDamp(position, this.followPosition + b, ref this.followVelocity, 0.3f);
				if ((position - vector).magnitude < 0.1f && !this.followTarget)
				{
					this.Unfollow();
				}
				this.mainCamera.transform.position = vector;
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x00113170 File Offset: 0x00111570
		public void SwitchDisplayMode(bool somethingSelected)
		{
			if (this.villagers)
			{
				foreach (VillagerView self in this.villagers.Villagers)
				{
					self.ExecuteOnChildren(delegate(Renderer r)
					{
						r.enabled = !somethingSelected;
					}, true);
					self.ExecuteOnChildren(delegate(Collider c)
					{
						c.enabled = !somethingSelected;
					}, true);
				}
			}
		}

		// Token: 0x04006050 RID: 24656
		public bool Enabled = true;

		// Token: 0x04006051 RID: 24657
		public const float PanTime = 0.5f;

		// Token: 0x04006054 RID: 24660
		private Transform followTarget;

		// Token: 0x04006055 RID: 24661
		private Vector3 followPosition;

		// Token: 0x04006056 RID: 24662
		private Vector3 followVelocity;

		// Token: 0x04006057 RID: 24663
		private const float smoothTime = 0.3f;

		// Token: 0x04006058 RID: 24664
		private Camera mainCamera;

		// Token: 0x04006059 RID: 24665
		private Coroutine panRoutine;

		// Token: 0x0400605A RID: 24666
		public VillagersControllerRoot villagers;
	}
}
