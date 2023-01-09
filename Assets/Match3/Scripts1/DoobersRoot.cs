using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Flows;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

// Token: 0x0200085C RID: 2140
namespace Match3.Scripts1
{
	public class DoobersRoot : APtSceneRoot
	{
		// Token: 0x060034E1 RID: 13537 RVA: 0x000FD9B1 File Offset: 0x000FBDB1
		protected override void Awake()
		{
			base.Awake();
			base.GetComponentsInChildren<Doober>().ForEach(delegate(Doober b)
			{
				b.gameObject.SetActive(false);
			});
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000FD9E4 File Offset: 0x000FBDE4
		protected override IEnumerator GoRoutine()
		{
			Wooroutine<SpriteManager> spriteManagerFlow = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			yield return spriteManagerFlow;
			SpriteManager spriteManager = spriteManagerFlow.ReturnValue;
			if (spriteManager)
			{
				this.imageSources.Add(spriteManager);
			}
			yield break;
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000FD9FF File Offset: 0x000FBDFF
		public void SetCanvasSortingOrder(Transform a, Transform b)
		{
			this.SetCanvasSortingOrder(a);
			this.SetCanvasSortingOrder(b);
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x000FDA10 File Offset: 0x000FBE10
		public void SetCanvasSortingOrder(Transform t)
		{
			Canvas componentInParent = t.GetComponentInParent<Canvas>();
			if (!componentInParent)
			{
				return;
			}
			if (this.doobersCanvas.sortingOrder <= componentInParent.sortingOrder)
			{
				this.doobersCanvas.sortingOrder = componentInParent.sortingOrder + 1;
			}
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000FDA5C File Offset: 0x000FBE5C
		public Sprite GetSimilar(string name)
		{
			Sprite sprite = null;
			foreach (SpriteManager spriteManager in this.imageSources)
			{
				sprite = spriteManager.GetSimilar(name, false);
				if (sprite)
				{
					break;
				}
			}
			return sprite;
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000FDAD0 File Offset: 0x000FBED0
		public float SpawnDoobers(MaterialAmount mat, Transform start, Transform target, Action<int> onDooberArrival = null)
		{
			this.SetCanvasSortingOrder(start, target);
			DooberDesc dooberDesc = this.FindDesc(mat.type);
			if (dooberDesc == null)
			{
				dooberDesc = this.FindDesc("default");
			}
			if (dooberDesc == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find a setup for doober type",
					mat.type
				});
				return 0f;
			}
			Sprite similar = this.GetSimilar(mat.type);
			if (!similar)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find an image for doober type",
					mat.type
				});
				return 0f;
			}
			IMaterialAmountDisplay componentInParent = target.GetComponentInParent<IMaterialAmountDisplay>();
			Action<int> callback = onDooberArrival;
			if (componentInParent != null)
			{
				componentInParent.ReserveAmount(mat.amount);
				callback = new Action<int>(componentInParent.AcceptDoober);
			}
			int count = Mathf.Min(Mathf.CeilToInt((float)mat.amount / (float)dooberDesc.unitsPerInstance), 15);
			base.StartCoroutine(this.Spawn(similar, dooberDesc, count, mat.amount, start, target, callback, 0f));
			return 2f;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000FDBD8 File Offset: 0x000FBFD8
		public void SpawnDoober(string type, Sprite sprite, Transform start, Transform target, float delay = 0f)
		{
			DooberDesc dooberDesc = this.FindDesc(type);
			if (dooberDesc == null)
			{
				dooberDesc = this.FindDesc("default");
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find a setup for doober type",
					type,
					"Trying to use default"
				});
			}
			if (dooberDesc == null)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Could not find a setup for doober type nor a default",
					type
				});
				return;
			}
			IMaterialAmountDisplay componentInParent = target.GetComponentInParent<IMaterialAmountDisplay>();
			Action<int> callback = null;
			if (componentInParent != null)
			{
				componentInParent.ReserveAmount(1);
				callback = new Action<int>(componentInParent.AcceptDoober);
			}
			base.StartCoroutine(this.Spawn(sprite, dooberDesc, 1, 1, start, target, callback, delay));
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000FDC78 File Offset: 0x000FC078
		private IEnumerator Spawn(Sprite image, DooberDesc desc, int count, int totalDoobers, Transform start, Transform target, Action<int> callback, float delay = 0f)
		{
			yield return new WaitForSeconds(delay);
			WaitForSeconds wait = new WaitForSeconds(Mathf.Min(this.maxSpawnPeriod, this.spawnSpeed / (float)count));
			int givenAmount = 0;
			for (int i = 0; i < count; i++)
			{
				int dooberAmount = (totalDoobers - givenAmount) / (count - i);
				givenAmount += dooberAmount;
				if (!start)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Object SPAWNING doobers is destroyed"
					});
					if (callback != null)
					{
						callback(dooberAmount);
					}
				}
				else if (!target)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Object RECEIVING doobers is destoyed"
					});
					if (callback != null)
					{
						callback(dooberAmount);
					}
				}
				else
				{
					Doober doober = this.pool.Get(desc.doober.gameObject).GetComponent<Doober>();
					doober.transform.SetParent(this.doobersCanvas.transform, false);
					doober.gameObject.SetActive(true);
					Vector3 scale = start.localScale;
					if (image != null)
					{
						doober.image.sprite = image;
					}
					if (doober.animatedView != null)
					{
						doober.animatedView.AnimationState.SetAnimation(0, "Doober", true);
						scale = doober.animatedView.transform.localScale;
					}
					doober.amount = dooberAmount;
					doober.onFinished.RemoveAllListeners();
					doober.onFinished.AddListener(callback);
					Vector3 startPos = (!(start is RectTransform)) ? this.RandomWithin(start) : this.CenterInWorld(start as RectTransform);
					Vector3 targetPos = (!(target is RectTransform)) ? this.RandomWithin(target) : this.CenterInWorld(target as RectTransform);
					doober.ApplyTrajectory(new Doober.Location(startPos, start), new Doober.Location(targetPos, target), scale);
					yield return wait;
				}
			}
			yield break;
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000FDCD0 File Offset: 0x000FC0D0
		private Vector3 RandomWithin(Transform tr)
		{
			Collider component = tr.GetComponent<Collider>();
			Vector3 position = tr.position;
			if (component)
			{
				Bounds bounds = component.bounds;
				position = new Vector3(global::UnityEngine.Random.Range(bounds.min.x, bounds.max.x), global::UnityEngine.Random.Range(bounds.min.y, bounds.max.y), global::UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
			}
			return position;
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000FDD74 File Offset: 0x000FC174
		private Vector3 CenterInWorld(RectTransform rt)
		{
			return rt.TransformPoint(rt.rect.center);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000FDD9C File Offset: 0x000FC19C
		private DooberDesc FindDesc(string name)
		{
			return Array.Find<DooberDesc>(this.doobers, (DooberDesc d) => d.name == name);
		}

		// Token: 0x04005CDD RID: 23773
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005CDE RID: 23774
		public const float FLY_TIME = 2f;

		// Token: 0x04005CDF RID: 23775
		private const int MAX_DOOBERS = 15;

		// Token: 0x04005CE0 RID: 23776
		public DooberDesc[] doobers;

		// Token: 0x04005CE1 RID: 23777
		public global::Shared.Pooling.ObjectPool pool;

		// Token: 0x04005CE2 RID: 23778
		public float spawnSpeed = 1f;

		// Token: 0x04005CE3 RID: 23779
		public float maxSpawnPeriod = 0.25f;

		// Token: 0x04005CE4 RID: 23780
		public Canvas doobersCanvas;

		// Token: 0x04005CE5 RID: 23781
		public List<SpriteManager> imageSources;
	}
}
