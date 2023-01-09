using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000718 RID: 1816
	public class BoostMaskUiHintDisplay : ABoostMaskListener
	{
		// Token: 0x06002CF8 RID: 11512 RVA: 0x000D0A84 File Offset: 0x000CEE84
		public override void HandleBoostOverlayStateChanged(BoostOverlayState newState)
		{
			if (this.goToActivate != null)
			{
				this.goToActivate.SetActive(newState.isOn);
				if (newState.isOn)
				{
					this.TrySetup(newState.boostType);
				}
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000D0AC2 File Offset: 0x000CEEC2
		private void TrySetup(Boosts boostType)
		{
			if (this.locaService != null)
			{
				this.SetupIcon(boostType);
				this.SetupLoca(boostType);
			}
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000D0AE0 File Offset: 0x000CEEE0
		private void SetupIcon(Boosts boostType)
		{
			string nameFor = MaterialName.GetNameFor(boostType);
			this.boostIcon.sprite = this.boostSpriteManager.GetSimilar(nameFor);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000D0B0C File Offset: 0x000CEF0C
		private void SetupLoca(Boosts boostType)
		{
			string locaKeyFor = this.GetLocaKeyFor(boostType);
			if (!string.IsNullOrEmpty(locaKeyFor))
			{
				this.label.text = this.locaService.GetText(locaKeyFor, new LocaParam[0]);
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000D0B49 File Offset: 0x000CEF49
		private string GetLocaKeyFor(Boosts boostType)
		{
			switch (boostType)
			{
			case Boosts.boost_hammer:
				return "ui.boosts.ingame.add.hammer.body";
			case Boosts.boost_star:
				return "ui.boosts.ingame.add.star.body";
			case Boosts.boost_rainbow:
				return "ui.boosts.ingame.add.rainbow.body";
			default:
				return string.Empty;
			}
		}

		// Token: 0x0400567D RID: 22141
		public GameObject goToActivate;

		// Token: 0x0400567E RID: 22142
		public SpriteManager boostSpriteManager;

		// Token: 0x0400567F RID: 22143
		public Image boostIcon;

		// Token: 0x04005680 RID: 22144
		public TextMeshProUGUI label;

		// Token: 0x04005681 RID: 22145
		[WaitForService(true, true)]
		protected ILocalizationService locaService;
	}
}
