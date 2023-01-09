using System;
using System.Collections;
using System.IO;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using MyFile.Level.Standard_Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A67 RID: 2663
namespace Match3.Scripts1
{
	public class TownTropicamUIRoot : APtSceneRoot, IPersistentDialog
	{
		// Token: 0x06003FBF RID: 16319 RVA: 0x001468B8 File Offset: 0x00144CB8
		public void Init()
		{
			base.gameObject.SetActive(true);
			this.canvasTakePicture.gameObject.SetActive(true);
			this.canvasTakePicture.GetComponent<AnimatedUi>().Show();
			this.canvasShare.gameObject.SetActive(false);
			this.postcardObject.SetActive(false);
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x00146910 File Offset: 0x00144D10
		private void Start()
		{
			this.buttonTakePicture.onClick.AddListener(new UnityAction(this.HandleTakePicture));
			this.buttonShare.onClick.AddListener(new UnityAction(this.HandleShare));
			foreach (Button button in this.closeButtons)
			{
				button.onClick.AddListener(new UnityAction(this.HandleClose));
			}
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0014698B File Offset: 0x00144D8B
		private void HandleClose()
		{
			this.tracking.TrackUi("tropicam", "player_triggered", "close", null, new object[0]);
			this.Close();
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x001469B4 File Offset: 0x00144DB4
		protected override void OnEnable()
		{
			base.OnEnable();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.Init();
			this.SetEnabled();
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x001469DE File Offset: 0x00144DDE
		private void SetEnabled()
		{
			if (this.tracking == null)
			{
				return;
			}
			this.gameStateService.isInteractable = false;
			this.tracking.TrackUi("tropicam", "player_triggered", "open", null, new object[0]);
		}

		// Token: 0x06003FC4 RID: 16324 RVA: 0x00146A1C File Offset: 0x00144E1C
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.rt)
			{
				global::UnityEngine.Object.Destroy(this.rt);
			}
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.gameStateService.isInteractable = true;
		}

		// Token: 0x06003FC5 RID: 16325 RVA: 0x00146A6C File Offset: 0x00144E6C
		private void HandleTakePicture()
		{
			this.texture = PtScreenshot.TakeScreenshot(-1, this.frame, new PtScreenshot.RenderCallback(this.Render));
			if (this.DEBUG)
			{
				File.WriteAllBytes(Application.persistentDataPath + "/crop.png", this.texture.EncodeToPNG());
			}
			this.tracking.TrackUi("tropicam", "player_triggered", "capture_photo", null, new object[0]);
			base.StartCoroutine(this.SwitchToShareScreen());
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x00146AEF File Offset: 0x00144EEF
		private void Render(RenderTexture rt)
		{
			PtScreenshot.RenderToTexture(Camera.main, rt);
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x00146AFC File Offset: 0x00144EFC
		private void HandleShare()
		{
			base.StartCoroutine(this.CreatePostcardRoutine());
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00146B0C File Offset: 0x00144F0C
		private IEnumerator CreatePostcardRoutine()
		{
			this.postcardObject.gameObject.SetActive(true);
			yield return null;
			Texture2D card = this.CreatePostcard(this.texture);
			this.tracking.TrackUi("tropicam", "player_triggered", "share", null, new object[0]);
			string path = this.SaveFileToShare(card);
			this.CreateNativeShareObject(path).Share();
			this.Close();
			yield break;
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x00146B28 File Offset: 0x00144F28
		private NativeShare CreateNativeShareObject(string path)
		{
			string text = this.locaService.GetText("ui.tropicam.share.subject", new LocaParam[0]);
			string text2 = this.locaService.GetText("ui.tropicam.share.text", new LocaParam[0]);
			string text3 = this.locaService.GetText("ui.tropicam.share.title", new LocaParam[0]);
			string text4 = this.locaService.GetText("tropicam.share", new LocaParam[0]);
			string text5 = string.Format("{0}\n\n{1}\n{2}", text3, text2, "#Tropicats");
			return new NativeShare().AddFile(path, null).SetSubject(text).SetText(text5).SetTitle(text4);
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x00146BC4 File Offset: 0x00144FC4
		private void Close()
		{
			Canvas canvas = (!this.canvasShare.isActiveAndEnabled) ? this.canvasTakePicture : this.canvasShare;
			canvas.GetComponent<AnimatedUi>().Hide();
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x00146C00 File Offset: 0x00145000
		private IEnumerator SwitchToShareScreen()
		{
			this.canvasTakePicture.gameObject.SetActive(false);
			this.canvasShare.gameObject.SetActive(true);
			this.texture.Apply();
			this.photo.texture = this.texture;
			yield return null;
			this.canvasShare.GetComponent<AnimatedUi>().Show();
			yield break;
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x00146C1C File Offset: 0x0014501C
		private Texture2D CreatePostcard(Texture2D photo)
		{
			this.cardPhoto.texture = photo;
			RectTransform component = this.cardPhoto.transform.parent.parent.GetComponent<RectTransform>();
			Texture2D texture2D = PtScreenshot.TakeScreenshot(750, component, new Camera[]
			{
				this.cameraPostcard
			});
			if (this.DEBUG)
			{
				File.WriteAllBytes(Application.persistentDataPath + "/card.jpg", texture2D.EncodeToJPG());
			}
			return texture2D;
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00146C94 File Offset: 0x00145094
		private string SaveFileToShare(Texture2D photo)
		{
			string text = Application.persistentDataPath + "/share.jpg";
			WoogaDebug.Log(new object[]
			{
				text
			});
			File.WriteAllBytes(Application.persistentDataPath + "/share.jpg", photo.EncodeToJPG());
			return text;
		}

		// Token: 0x04006963 RID: 26979
		public const string HASHTAG_TROPICATS = "#Tropicats";

		// Token: 0x04006964 RID: 26980
		private bool DEBUG = true;

		// Token: 0x04006965 RID: 26981
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04006966 RID: 26982
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04006967 RID: 26983
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x04006968 RID: 26984
		[SerializeField]
		private Canvas canvasTakePicture;

		// Token: 0x04006969 RID: 26985
		[SerializeField]
		private Canvas canvasShare;

		// Token: 0x0400696A RID: 26986
		[SerializeField]
		private GameObject postcardObject;

		// Token: 0x0400696B RID: 26987
		[SerializeField]
		private Camera cameraPostcard;

		// Token: 0x0400696C RID: 26988
		[SerializeField]
		private Button buttonTakePicture;

		// Token: 0x0400696D RID: 26989
		[SerializeField]
		private RectTransform frame;

		// Token: 0x0400696E RID: 26990
		[SerializeField]
		private RawImage photo;

		// Token: 0x0400696F RID: 26991
		[SerializeField]
		private RawImage cardPhoto;

		// Token: 0x04006970 RID: 26992
		[SerializeField]
		private Button buttonShare;

		// Token: 0x04006971 RID: 26993
		[SerializeField]
		private RenderTexture rt;

		// Token: 0x04006972 RID: 26994
		[SerializeField]
		private Texture2D texture;

		// Token: 0x04006973 RID: 26995
		[SerializeField]
		private Button[] closeButtons;

		// Token: 0x04006974 RID: 26996
		private const int HEIGHT = 750;

		// Token: 0x04006975 RID: 26997
		private const string UI = "tropicam";
	}
}
