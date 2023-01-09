using System.Collections;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
// using New.Plugins.IllegalWordsDetection;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000846 RID: 2118
namespace Match3.Scripts1
{
	public class EnterNameDialogRoot : APtSceneRoot, IHandler<PopupOperation>
	{
		// Token: 0x06003479 RID: 13433 RVA: 0x000FB1CC File Offset: 0x000F95CC
		protected override void Go()
		{
			this.dialog.Show();
			if (!string.IsNullOrEmpty(this.gameState.PlayerName))
			{
				this.nameInput.text = this.gameState.PlayerName;
				// this.placeHolderText.text = this.gameState.PlayerName;
			}
			else
			{
				this.gameState.PlayerName = this.localizationService.GetText("name.dialog.box.default.text", new LocaParam[0]);
				this.nameInput.text = this.gameState.PlayerName;
			}

			this.nameInput.onValueChanged.RemoveAllListeners();
			this.nameInput.onValueChanged.AddListener(ValidateInput);
			this.nameInput.MoveTextEnd(false);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000FB284 File Offset: 0x000F9684
		public void Handle(PopupOperation op)
		{
			if (string.IsNullOrEmpty(this.nameInput.text))
			{
				if (!string.IsNullOrEmpty(this.gameState.PlayerName))
				{
					this.nameInput.text = this.gameState.PlayerName;
				}
				else
				{
					// this.nameInput.text = this.placeHolderText.text;
				}
			}
			if (op != PopupOperation.OK)
			{
				if (op != PopupOperation.Close)
				{
					if (op == PopupOperation.Details)
					{
						this.nameInput.text = string.Empty;
						this.nameInput.ActivateInputField();
					}
				}
				else
				{
					if (string.IsNullOrEmpty(this.gameState.PlayerName))
					{
						this.SetPlayerName(this.nameInput.text);
					}
					base.StartCoroutine(this.Hide());
				}
			}
			else if (!string.IsNullOrEmpty(this.nameInput.text))
			{
				this.SetPlayerName(this.nameInput.text);
				base.StartCoroutine(this.Hide());
			}
		}

		// private char ValidateInput(string input, int length, char desiredCharacter)
		private void ValidateInput(string input)
		{
			// 	if (this.font != null)
			// 	{
			// 		return (!this.font.HasCharacter(desiredCharacter)) ? '\0' : desiredCharacter;
			// 	}
			// 	return desiredCharacter;
			okButton.interactable = false;
			if (nameInput.text == "")
			{
				info.text = this.localizationService.GetText("name.dialog.box.change.empty");
				return;
			}
			// var isExist = BadWordsDetection.IsExistBadWords(input);
			// if (isExist)
			// {
				// info.text = this.localizationService.GetText("name.dialog.box.change.retry");
			// }
			// else
			// {
				info.text = "";
				okButton.interactable = true;
			// }
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000FB3C3 File Offset: 0x000F97C3
		private void SetPlayerName(string name)
		{
			this.gameState.PlayerName = name;
			this.localizationService.SetGlobalReplaceKeys(this.gameState.GlobalReplaceKeys);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000FB3E8 File Offset: 0x000F97E8
		private IEnumerator Hide()
		{
			this.dialog.Hide();
			yield return new WaitForSecondsRealtime(this.dialog.close.length);
			base.Destroy();
			yield break;
		}

		// Token: 0x04005C6C RID: 23660
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005C6D RID: 23661
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005C6E RID: 23662
		public Image backgroundImage;

		// Token: 0x04005C6F RID: 23663
		public InputField nameInput;

		// Token: 0x04005C70 RID: 23664
		// public TextMeshProUGUI placeHolderText;

		// Token: 0x04005C71 RID: 23665
		public AnimatedUi dialog;

		// Token: 0x04005C72 RID: 23666
		// public Font font;
		public Text info;
		public Button okButton;
	}
}
