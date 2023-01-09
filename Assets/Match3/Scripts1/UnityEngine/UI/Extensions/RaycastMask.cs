using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD5 RID: 3029
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Extensions/Raycast Mask")]
	public class RaycastMask : MonoBehaviour, ICanvasRaycastFilter
	{
		// Token: 0x0600470F RID: 18191 RVA: 0x00169FED File Offset: 0x001683ED
		private void Start()
		{
			this._image = base.GetComponent<Image>();
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00169FFC File Offset: 0x001683FC
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			this._sprite = this._image.sprite;
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, sp, eventCamera, out vector);
			Vector2 vector2 = new Vector2(vector.x + rectTransform.pivot.x * rectTransform.rect.width, vector.y + rectTransform.pivot.y * rectTransform.rect.height);
			Rect textureRect = this._sprite.textureRect;
			Rect rect = rectTransform.rect;
			Image.Type type = this._image.type;
			int x;
			int y;
			if (type != Image.Type.Sliced)
			{
				if (type != Image.Type.Simple)
				{
				}
				x = Mathf.FloorToInt(textureRect.x + textureRect.width * vector2.x / rect.width);
				y = Mathf.FloorToInt(textureRect.y + textureRect.height * vector2.y / rect.height);
			}
			else
			{
				Vector4 border = this._sprite.border;
				if (vector2.x < border.x)
				{
					x = Mathf.FloorToInt(textureRect.x + vector2.x);
				}
				else if (vector2.x > rect.width - border.z)
				{
					x = Mathf.FloorToInt(textureRect.x + textureRect.width - (rect.width - vector2.x));
				}
				else
				{
					x = Mathf.FloorToInt(textureRect.x + border.x + (vector2.x - border.x) / (rect.width - border.x - border.z) * (textureRect.width - border.x - border.z));
				}
				if (vector2.y < border.y)
				{
					y = Mathf.FloorToInt(textureRect.y + vector2.y);
				}
				else if (vector2.y > rect.height - border.w)
				{
					y = Mathf.FloorToInt(textureRect.y + textureRect.height - (rect.height - vector2.y));
				}
				else
				{
					y = Mathf.FloorToInt(textureRect.y + border.y + (vector2.y - border.y) / (rect.height - border.y - border.w) * (textureRect.height - border.y - border.w));
				}
			}
			bool result;
			try
			{
				result = (this._sprite.texture.GetPixel(x, y).a > 0f);
			}
			catch (UnityException)
			{
				global::UnityEngine.Debug.LogWarning("Mask texture not readable, set your sprite to Texture Type 'Advanced' and check 'Read/Write Enabled'");
				global::UnityEngine.Object.Destroy(this);
				result = false;
			}
			return result;
		}

		// Token: 0x04006E38 RID: 28216
		private Image _image;

		// Token: 0x04006E39 RID: 28217
		private Sprite _sprite;
	}
}
