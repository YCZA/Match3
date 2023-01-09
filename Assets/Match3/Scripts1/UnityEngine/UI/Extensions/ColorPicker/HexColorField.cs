using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B93 RID: 2963
	[RequireComponent(typeof(InputField))]
	public class HexColorField : MonoBehaviour
	{
		// Token: 0x0600455F RID: 17759 RVA: 0x0015F504 File Offset: 0x0015D904
		private void Awake()
		{
			this.hexInputField = base.GetComponent<InputField>();
			this.hexInputField.onEndEdit.AddListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0015F555 File Offset: 0x0015D955
		private void OnDestroy()
		{
			this.hexInputField.onValueChanged.RemoveListener(new UnityAction<string>(this.UpdateColor));
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.UpdateHex));
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0015F58F File Offset: 0x0015D98F
		private void UpdateHex(Color newColor)
		{
			this.hexInputField.text = this.ColorToHex(newColor);
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x0015F5A8 File Offset: 0x0015D9A8
		private void UpdateColor(string newHex)
		{
			Color32 c;
			if (HexColorField.HexToColor(newHex, out c))
			{
				this.ColorPicker.CurrentColor = c;
			}
			else
			{
				global::UnityEngine.Debug.Log("hex value is in the wrong format, valid formats are: #RGB, #RGBA, #RRGGBB and #RRGGBBAA (# is optional)");
			}
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x0015F5E4 File Offset: 0x0015D9E4
		private string ColorToHex(Color32 color)
		{
			if (this.displayAlpha)
			{
				return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
				{
					color.r,
					color.g,
					color.b,
					color.a
				});
			}
			return string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0015F678 File Offset: 0x0015DA78
		public static bool HexToColor(string hex, out Color32 color)
		{
			if (Regex.IsMatch(hex, "^#?(?:[0-9a-fA-F]{3,4}){1,2}$"))
			{
				int num = (!hex.StartsWith("#")) ? 0 : 1;
				if (hex.Length == num + 8)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 6, 2), NumberStyles.AllowHexSpecifier));
				}
				else if (hex.Length == num + 6)
				{
					color = new Color32(byte.Parse(hex.Substring(num, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 2, 2), NumberStyles.AllowHexSpecifier), byte.Parse(hex.Substring(num + 4, 2), NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				else if (hex.Length == num + 4)
				{
					color = new Color32(byte.Parse(string.Empty + hex[num] + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 1] + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 2] + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 3] + hex[num + 3], NumberStyles.AllowHexSpecifier));
				}
				else
				{
					color = new Color32(byte.Parse(string.Empty + hex[num] + hex[num], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 1] + hex[num + 1], NumberStyles.AllowHexSpecifier), byte.Parse(string.Empty + hex[num + 2] + hex[num + 2], NumberStyles.AllowHexSpecifier), byte.MaxValue);
				}
				return true;
			}
			color = default(Color32);
			return false;
		}

		// Token: 0x04006CE2 RID: 27874
		public ColorPickerControl ColorPicker;

		// Token: 0x04006CE3 RID: 27875
		public bool displayAlpha;

		// Token: 0x04006CE4 RID: 27876
		private InputField hexInputField;

		// Token: 0x04006CE5 RID: 27877
		private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";
	}
}
