using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.UI;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000868 RID: 2152
	[LoadOptions(true, false, false)]
	public class PopupDialogRoot : APtSceneRoot<object[]>, IHandler<Action>, IDisposableDialog
	{
		// Token: 0x06003517 RID: 13591 RVA: 0x000FE618 File Offset: 0x000FCA18
		protected override void Awake()
		{
			base.Awake();
			this.prototypeCells = base.GetComponentsInChildren<APopupCell>(true);
			this.prototypeCells.ForEach(delegate(APopupCell x)
			{
				x.gameObject.SetActive(false);
			});
			Color color = this.background.color;
			color.a = 0f;
			this.background.color = color;
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000FE684 File Offset: 0x000FCA84
		protected override void Go()
		{
			if (this.parameters == null)
			{
				return;
			}
			TownResourceElement townResourceElement = (TownResourceElement)0;
			object[] parameters = this.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				object data = parameters[i];
				APopupCell apopupCell = Array.Find<APopupCell>(this.prototypeCells, (APopupCell x) => x.CanPresent(data));
				if (!apopupCell)
				{
					WoogaDebug.Log(new object[]
					{
						"has sorting"
					});
					if (data is PopupSortingOrder)
					{
						WoogaDebug.Log(new object[]
						{
							"update sorting"
						});
						PopupSortingOrder popupSortingOrder = data as PopupSortingOrder;
						base.GetComponentInChildren<PlaceOnTop>().UpdateOrder(popupSortingOrder.layer);
					}
					else
					{
						global::UnityEngine.Debug.LogWarningFormat(this, "Can't find prototype cell for {0}", new object[]
						{
							data
						});
					}
				}
				else
				{
					APopupCell apopupCell2 = global::UnityEngine.Object.Instantiate<APopupCell>(apopupCell);
					apopupCell2.Show(data);
					apopupCell2.gameObject.SetActive(true);
					apopupCell2.transform.SetParent(this.targetLayout.transform, false);
					if (data is IMaterialAmountBasedElement)
					{
						IMaterialAmountBasedElement materialAmountBasedElement = data as IMaterialAmountBasedElement;
						TownResourceElement townResourceElement2;
						if (EnumExtensions.EnumTryParse<TownResourceElement>(materialAmountBasedElement.GetMaterialAmount().type, out townResourceElement2, true))
						{
							townResourceElement |= townResourceElement2;
						}
					}
					else if (data is MaterialAmount)
					{
						TownResourceElement townResourceElement2;
						if (EnumExtensions.EnumTryParse<TownResourceElement>(((MaterialAmount)data).type, out townResourceElement2, true))
						{
							townResourceElement |= townResourceElement2;
						}
					}
					else if (data is IEnumerable<MaterialAmount>)
					{
						IEnumerable<MaterialAmount> enumerable = (IEnumerable<MaterialAmount>)data;
						foreach (MaterialAmount materialAmount in enumerable)
						{
							TownResourceElement townResourceElement2;
							if (EnumExtensions.EnumTryParse<TownResourceElement>(materialAmount.type, out townResourceElement2, true))
							{
								townResourceElement |= townResourceElement2;
							}
						}
					}
					else if (data is Bundle)
					{
						IEnumerable<MaterialAmount> materials = ((Bundle)data).materials;
						foreach (MaterialAmount materialAmount2 in materials)
						{
							TownResourceElement townResourceElement2;
							if (EnumExtensions.EnumTryParse<TownResourceElement>(materialAmount2.type, out townResourceElement2, true))
							{
								townResourceElement |= townResourceElement2;
							}
						}
					}
				}
			}
			base.GetComponent<TownResourcePanelLoader>().VisibleElements = townResourceElement;
			this.dialog.Show();
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.UpdateOrder();
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000FE968 File Offset: 0x000FCD68
		public void Handle(Action action)
		{
			if (action != null)
			{
				action();
			}
			this.Close();
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000FE97C File Offset: 0x000FCD7C
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000FE9CC File Offset: 0x000FCDCC
		private void UpdateOrder()
		{
			PlaceOnTop componentInChildren = base.GetComponentInChildren<PlaceOnTop>();
			componentInChildren.UpdateOrder(componentInChildren.layer);
			base.GetComponentInChildren<TownResourcePanelLoader>().UpdateOrder();
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000FE9F7 File Offset: 0x000FCDF7
		public static Wooroutine<PopupDialogRoot> Show(params object[] rows)
		{
			return SceneManager.Instance.LoadSceneWithParams<PopupDialogRoot, object[]>(rows, null);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000FEA05 File Offset: 0x000FCE05
		public static Coroutine ShowOkDialog(string title, string body, Action callback = null, PopupSortingOrder order = null)
		{
			return WooroutineRunner.StartCoroutine(PopupDialogRoot.ShowOkDialogRoutine(title, body, callback, order), null);
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000FEA18 File Offset: 0x000FCE18
		private static IEnumerator ShowOkDialogRoutine(string title, string body, Action callback, PopupSortingOrder order)
		{
			Wooroutine<ILocalizationService> locaService = ServiceLocator.Instance.Await<ILocalizationService>(true);
			yield return locaService;
			string okayText = locaService.ReturnValue.GetText("ui.shared.button.ok", new LocaParam[0]);
			List<object> p = new List<object>
			{
				new CloseButton(null),
				TextData.Title(title),
				TextData.Content(body),
				new LabeledButtonWithCallback(okayText, callback)
			};
			p.AddIfNotNull(order);
			Wooroutine<PopupDialogRoot> scene = PopupDialogRoot.Show(p.ToArray());
			yield return scene;
			yield return scene.ReturnValue.onClose;
			yield break;
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x000FEA48 File Offset: 0x000FCE48
		private static IEnumerator ShowYesNoDialogRoutine(string title, string body, bool showCloseButton = true, PopupSortingOrder order = null)
		{
			AwaitSignal<bool> onClose = new AwaitSignal<bool>();
			Wooroutine<ILocalizationService> locaService = ServiceLocator.Instance.Await<ILocalizationService>(true);
			yield return locaService;
			string yesText = locaService.ReturnValue.GetText("ui.yes", new LocaParam[0]);
			string noText = locaService.ReturnValue.GetText("ui.no", new LocaParam[0]);
			List<object> p = new List<object>
			{
				TextData.Title(title),
				TextData.Content(body),
				new LabeledButtonWithCallback(yesText, delegate()
				{
					onClose.Dispatch(true);
				}),
				new LabeledButtonWithCallback(noText, delegate()
				{
					onClose.Dispatch(false);
				})
			};
			p.AddIfNotNull(order);
			if (showCloseButton)
			{
				p.Add(new CloseButton(delegate()
				{
					onClose.Dispatch(false);
				}));
			}
			PopupDialogRoot.Show(p.ToArray());
			yield return onClose;
			yield return onClose.Dispatched;
			yield break;
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x000FEA78 File Offset: 0x000FCE78
		public static Wooroutine<bool> ShowYesNoDialog(string title, string body, bool showCloseButton = true, PopupSortingOrder order = null)
		{
			return WooroutineRunner.StartWooroutine<bool>(PopupDialogRoot.ShowYesNoDialogRoutine(title, body, showCloseButton, order));
		}

		// Token: 0x04005CF3 RID: 23795
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005CF4 RID: 23796
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005CF5 RID: 23797
		private APopupCell[] prototypeCells;

		// Token: 0x04005CF6 RID: 23798
		public LayoutGroup targetLayout;

		// Token: 0x04005CF7 RID: 23799
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005CF8 RID: 23800
		public AnimatedUi dialog;

		// Token: 0x04005CF9 RID: 23801
		public Image background;
	}
}
