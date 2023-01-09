using System;
using System.Collections;
using System.Reflection;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

// Token: 0x0200088B RID: 2187
namespace Match3.Scripts1
{
	public abstract class AUiAdjuster : MonoBehaviour
	{
		// Token: 0x060035A7 RID: 13735 RVA: 0x000BEDFF File Offset: 0x000BD1FF
		static AUiAdjuster()
		{
			AUiAdjuster.OnScreenOrientationChange = new Signal<ScreenOrientation>();
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x000BEE20 File Offset: 0x000BD220
		public static bool IsIPhoneX
		{
			get
			{
				if (Application.platform != RuntimePlatform.IPhonePlayer && !Application.isEditor)
				{
					return false;
				}
				switch (AUiAdjuster.Orientation)
				{
					case ScreenOrientation.Portrait:
					case ScreenOrientation.PortraitUpsideDown:
						return AUiAdjuster.CurrentAspectRatio < AUiAdjuster.lowestAspect;
					case ScreenOrientation.LandscapeLeft:
					case ScreenOrientation.LandscapeRight:
						return AUiAdjuster.CurrentAspectRatio > 1f / AUiAdjuster.lowestAspect;
					default:
						return false;
				}
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000BEE88 File Offset: 0x000BD288
		protected T GetMatchingSetting<T>(T[] settings) where T : AUiAdjuster.UiAdjusterSetting
		{
			bool isIPhoneX = AUiAdjuster.IsIPhoneX;
			foreach (T t in settings)
			{
				if (t.isIphoneX == isIPhoneX && t.orientation == AUiAdjuster.Orientation)
				{
					return t;
				}
			}
			foreach (T t2 in settings)
			{
				if (t2.isIphoneX == isIPhoneX && t2.orientation == AUiAdjuster.SimilarOrientation)
				{
					return t2;
				}
			}
			foreach (T t3 in settings)
			{
				if (t3.orientation == AUiAdjuster.SimilarOrientation)
				{
					return t3;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x000BEF79 File Offset: 0x000BD379
		public static ScreenOrientation Orientation
		{
			get
			{
				// if (AUiAdjuster.CurrentAspectRatio > 1f)
				// {
				// 	if (Screen.orientation == ScreenOrientation.LandscapeRight || AUiAdjuster.EditorUseRightOrientation)
				// 	{
				// 		return ScreenOrientation.LandscapeRight;
				// 	}
				// 	return ScreenOrientation.LandscapeLeft;
				// }
				// else
				// {
				// 	if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
				// 	{
				// 		return ScreenOrientation.PortraitUpsideDown;
				// 	}
				// 	return ScreenOrientation.Portrait;
				// }
				// 只使用横屏
				return ScreenOrientation.LandscapeLeft;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060035AC RID: 13740 RVA: 0x000BEFB1 File Offset: 0x000BD3B1
		public static ScreenOrientation SimilarOrientation
		{
			get
			{
				return AUiAdjuster.GetSimilarOrientation(AUiAdjuster.Orientation);
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000BEFC0 File Offset: 0x000BD3C0
		public static AUiAdjuster.VideoMode GetVideoMode()
		{
			AUiAdjuster.VideoMode videoMode = new AUiAdjuster.VideoMode();
			float num = 0.625f;
			bool flag = AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft;
			if (flag)
			{
				num = 1.6f;
			}
			videoMode.isTablet = ((!flag) ? (AUiAdjuster.CurrentAspectRatio >= num) : (AUiAdjuster.CurrentAspectRatio <= num));
			if (flag || videoMode.isTablet)
			{
				videoMode.scalingMode = FullScreenMovieScalingMode.AspectFit;
			}
			return videoMode;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000BF02E File Offset: 0x000BD42E
		private static bool Approximately(float a, float b, float threshold)
		{
			return Mathf.Abs(a - b) <= threshold;
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x060035AF RID: 13743 RVA: 0x000BF040 File Offset: 0x000BD440
		protected static float CurrentAspectRatio
		{
			get
			{
				Vector2 screenDimensions = AUiAdjuster.ScreenDimensions;
				return screenDimensions.x / screenDimensions.y;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x060035B0 RID: 13744 RVA: 0x000BF062 File Offset: 0x000BD462
		private static Vector2 ScreenDimensions
		{
			get
			{
				if (!Application.isPlaying)
				{
					return AUiAdjuster.GetMainGameViewSize();
				}
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000BF088 File Offset: 0x000BD488
		private static Vector2 GetMainGameViewSize()
		{
			Type type = Type.GetType("UnityEditor.GameView,UnityEditor");
			MethodInfo method = type.GetMethod("GetSizeOfMainGameView", BindingFlags.Static | BindingFlags.NonPublic);
			object obj = method.Invoke(null, null);
			return (Vector2)obj;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000BF0BD File Offset: 0x000BD4BD
		protected void Start()
		{
			this.currentOrientation = AUiAdjuster.Orientation;
			this.AdjustValues();
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000BF0D0 File Offset: 0x000BD4D0
		protected void Update()
		{
			if (this.currentOrientation != AUiAdjuster.Orientation)
			{
				this.currentOrientation = AUiAdjuster.Orientation;
				this.AdjustValues();
				if ((long)Time.frameCount != AUiAdjuster.lastOrientationChangeFrame)
				{
					AUiAdjuster.OnScreenOrientationChange.Dispatch(AUiAdjuster.Orientation);
					AUiAdjuster.lastOrientationChangeFrame = (long)Time.frameCount;
				}
			}
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000BF128 File Offset: 0x000BD528
		public static ScreenOrientation GetSimilarOrientation(ScreenOrientation orientation)
		{
			switch (orientation)
			{
				case ScreenOrientation.Portrait:
				case ScreenOrientation.PortraitUpsideDown:
					return ScreenOrientation.Portrait;
				case ScreenOrientation.LandscapeLeft:
				case ScreenOrientation.LandscapeRight:
					return ScreenOrientation.LandscapeLeft;
				case ScreenOrientation.AutoRotation:
					return ScreenOrientation.AutoRotation;
				default:
					return ScreenOrientation.Unknown;
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000BF152 File Offset: 0x000BD552
		public static void SetOrientationLock(bool locked)
		{
			AUiAdjuster.orientationLocked = locked;
			AUiAdjuster.CheckForOsOrientationLock();
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000BF160 File Offset: 0x000BD560
		public static IEnumerator ForceScreenSimilarOrientation(ScreenOrientation screenOrientation)
		{
			if (!AUiAdjuster.orientationChangesEnabled)
			{
				yield break;
			}
			// switch (screenOrientation)
			// {
			// case ScreenOrientation.Portrait:
			// case ScreenOrientation.PortraitUpsideDown:
			// 	Screen.autorotateToPortrait = true;
			// 	Screen.autorotateToPortraitUpsideDown = true;
			// 	Screen.autorotateToLandscapeLeft = false;
			// 	Screen.autorotateToLandscapeRight = false;
			// 	if (Screen.orientation != ScreenOrientation.PortraitUpsideDown)
			// 	{
			// 		Screen.orientation = ScreenOrientation.Portrait;
			// 	}
			// 	AUiAdjuster.orientationLocked = true;
			// 	yield return null;
			// 	AUiAdjuster.SetOrientationLock(true);
			// 	break;
			// case ScreenOrientation.LandscapeLeft:
			// case ScreenOrientation.LandscapeRight:
			// 	Screen.autorotateToPortrait = false;
			// 	Screen.autorotateToPortraitUpsideDown = false;
			// 	Screen.autorotateToLandscapeLeft = true;
			// 	Screen.autorotateToLandscapeRight = true;
			// 	if (Screen.orientation != ScreenOrientation.LandscapeRight)
			// 	{
			// 		Screen.orientation = ScreenOrientation.LandscapeLeft;
			// 	}
			// 	AUiAdjuster.orientationLocked = true;
			// 	yield return null;
			// 	AUiAdjuster.SetOrientationLock(true);
			// 	break;
			// }
			yield break;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000BF17C File Offset: 0x000BD57C
		private static void CheckForOsOrientationLock()
		{
			// eli key point 屏幕不能旋转
			return;
			if (!AUiAdjuster.orientationChangesEnabled)
			{
				return;
			}
			// bool flag = true;
			// if (!AUiAdjuster.orientationLocked)
			// {
			// 	flag = AndroidRotationLockUtil.AllowAutorotation();
			// 	if (!flag)
			// 	{
			// 		Screen.orientation = ScreenOrientation.Portrait;
			// 	}
			// 	else
			// 	{
			// 		Screen.orientation = ScreenOrientation.AutoRotation;
			// 	}
			// }
			// if (AUiAdjuster.orientationLocked)
			// {
			// 	if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
			// 	{
			// 		Screen.autorotateToPortrait = false;
			// 		Screen.autorotateToPortraitUpsideDown = false;
			// 		Screen.autorotateToLandscapeLeft = true;
			// 		Screen.autorotateToLandscapeRight = true;
			// 	}
			// 	else
			// 	{
			// 		Screen.autorotateToPortrait = true;
			// 		Screen.autorotateToPortraitUpsideDown = true;
			// 		Screen.autorotateToLandscapeLeft = false;
			// 		Screen.autorotateToLandscapeRight = false;
			// 	}
			// }
			// else
			// {
			// 	Screen.autorotateToPortrait = true;
			// 	Screen.autorotateToPortraitUpsideDown = flag;
			// 	Screen.autorotateToLandscapeLeft = flag;
			// 	Screen.autorotateToLandscapeRight = flag;
			// }
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000BF224 File Offset: 0x000BD624
		private void OnApplicationFocus(bool hasFocus)
		{
			if (!AUiAdjuster.orientationLocked)
			{
				AUiAdjuster.CheckForOsOrientationLock();
			}
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000BF238 File Offset: 0x000BD638
		[ContextMenu("Apply")]
		protected void Apply()
		{
			AUiAdjuster[] array = global::UnityEngine.Object.FindObjectsOfType<AUiAdjuster>();
			foreach (AUiAdjuster auiAdjuster in array)
			{
				auiAdjuster.AdjustValues();
			}
		}

		// Token: 0x060035BA RID: 13754
		protected abstract void AdjustValues();

		// Token: 0x04005DB0 RID: 23984
		public static bool EditorUseRightOrientation;

		// Token: 0x04005DB1 RID: 23985
		public static Signal<ScreenOrientation> OnScreenOrientationChange;

		// Token: 0x04005DB2 RID: 23986
		public static bool orientationChangesEnabled;

		// Token: 0x04005DB3 RID: 23987
		private static bool orientationLocked;

		// Token: 0x04005DB4 RID: 23988
		private static long lastOrientationChangeFrame;

		// Token: 0x04005DB5 RID: 23989
		protected ScreenOrientation currentOrientation;

		// Token: 0x04005DB6 RID: 23990
		private static float lowestAspect = 0.4864865f;

		// Token: 0x0200088C RID: 2188
		[Serializable]
		public class UiAdjusterSetting
		{
			// Token: 0x04005DB7 RID: 23991
			public ScreenOrientation orientation;

			// Token: 0x04005DB8 RID: 23992
			public bool isIphoneX;
		}

		// Token: 0x0200088D RID: 2189
		public class VideoMode
		{
			// Token: 0x04005DB9 RID: 23993
			public FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFill;

			// Token: 0x04005DBA RID: 23994
			public bool isTablet;
		}
	}
}
