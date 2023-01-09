using System.Collections.Generic;
using Shared.Pooling;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006A5 RID: 1701
	public class DirtView : ABlinkingAnimatedView, IGemModifierView, IModifierViewWithLanding, ITintableView
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000C20A0 File Offset: 0x000C04A0
		public Transform ViewTransform
		{
			get
			{
				if (this._viewTransform == null)
				{
					this._viewTransform = base.transform;
				}
				return this._viewTransform;
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000C20C8 File Offset: 0x000C04C8
		public void ShowModifier(Gem gem)
		{
			bool isCoveredByDirt = gem.IsCoveredByDirt;
			this.dirtSprite.gameObject.SetActive(isCoveredByDirt);
			if (isCoveredByDirt)
			{
				this.dirtSprite.sprite = this.dirtSpriteManager.GetSimilar(gem.modifier.ToString());
			}
			else
			{
				base.transform.localScale = Vector3.one;
				this.dirtSprite.transform.localScale = Vector3.one;
			}
			this.ReleaseTreasure();
			bool flag = gem.color == GemColor.Treasure && gem.modifier < GemModifier.DirtHp3;
			if (flag)
			{
				this.currentTreasure = this.AddAnimatedTreasure(this.treasurePrefabs[(int)((int)gem.modifier % (int)GemModifier.IceHp3)]);
				this.animator = this.currentTreasure.GetComponent<Animator>();
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000C219C File Offset: 0x000C059C
		public void UpdateBorder(HashSet<IntVector2> dirtViewPositions)
		{
			IntVector2 a = (IntVector2)this.ViewTransform.position;
			this.borderLeft.gameObject.SetActive(!dirtViewPositions.Contains(a + IntVector2.Left));
			this.borderRight.gameObject.SetActive(!dirtViewPositions.Contains(a + IntVector2.Right));
			this.borderTop.gameObject.SetActive(!dirtViewPositions.Contains(a + IntVector2.Up));
			this.borderBottom.gameObject.SetActive(!dirtViewPositions.Contains(a + IntVector2.Down));
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000C224A File Offset: 0x000C064A
		public void ShowModifierLanding()
		{
			if (this.currentTreasure != null)
			{
				this.animator.SetTrigger("TrickleEnded");
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000C226D File Offset: 0x000C066D
		protected override void Awake()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
			base.Awake();
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000C2280 File Offset: 0x000C0680
		protected override void OnEnable()
		{
			base.OnEnable();
			this.boardView.AddDirtView(this);
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000C2294 File Offset: 0x000C0694
		private void OnDisable()
		{
			this.boardView.RemoveDirtView(this);
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000C22A2 File Offset: 0x000C06A2
		protected override void Update()
		{
			if (this.currentTreasure != null)
			{
				base.Update();
			}
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000C22BB File Offset: 0x000C06BB
		private void ReleaseTreasure()
		{
			if (this.currentTreasure != null)
			{
				this.currentTreasure.transform.SetParent(null);
				this.currentTreasure.Release();
				this.currentTreasure = null;
			}
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000C22F1 File Offset: 0x000C06F1
		protected override void Blink()
		{
			if (this.currentTreasure != null)
			{
				base.Blink();
			}
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000C230C File Offset: 0x000C070C
		private GameObject AddAnimatedTreasure(GameObject prefab)
		{
			GameObject gameObject = this.boardView.objectPool.Get(prefab);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.position = base.transform.position;
			gameObject.transform.localPosition = DirtView.TREASURE_POSITION;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			return gameObject;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000C2384 File Offset: 0x000C0784
		public void ApplyTintColor(Color tint)
		{
			this.dirtSprite.color = tint;
			this.borderTop.color = tint;
			this.borderRight.color = tint;
			this.borderBottom.color = tint;
			this.borderLeft.color = tint;
			if (this.currentTreasure != null)
			{
				MeshRenderer component = this.currentTreasure.GetComponent<MeshRenderer>();
				if (component != null && this.materialPropertyBlock != null)
				{
					component.GetPropertyBlock(this.materialPropertyBlock);
					this.materialPropertyBlock.SetColor("_Tint", tint);
					component.SetPropertyBlock(this.materialPropertyBlock);
				}
			}
		}

		// Token: 0x040053B8 RID: 21432
		private const string TRICKLE_END_EVENT_NAME = "TrickleEnded";

		// Token: 0x040053B9 RID: 21433
		private static readonly Vector3 TREASURE_POSITION = new Vector3(0f, -0.5f, 0f);

		// Token: 0x040053BA RID: 21434
		[SerializeField]
		private SpriteRenderer dirtSprite;

		// Token: 0x040053BB RID: 21435
		[SerializeField]
		private SpriteManager dirtSpriteManager;

		// Token: 0x040053BC RID: 21436
		[SerializeField]
		private GameObject[] treasurePrefabs;

		// Token: 0x040053BD RID: 21437
		[SerializeField]
		private SpriteRenderer borderLeft;

		// Token: 0x040053BE RID: 21438
		[SerializeField]
		private SpriteRenderer borderRight;

		// Token: 0x040053BF RID: 21439
		[SerializeField]
		private SpriteRenderer borderBottom;

		// Token: 0x040053C0 RID: 21440
		[SerializeField]
		private SpriteRenderer borderTop;

		// Token: 0x040053C1 RID: 21441
		private Transform _viewTransform;

		// Token: 0x040053C2 RID: 21442
		private GameObject currentTreasure;

		// Token: 0x040053C3 RID: 21443
		private MaterialPropertyBlock materialPropertyBlock;
	}
}
