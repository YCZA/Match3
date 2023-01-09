using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts2.Start
{
    public class NetworkTip : MonoBehaviour
    {
        public TextMeshProUGUI contentTxt;
        public Button ok;

        private void Awake()
        {
            ok.onClick.AddListener(Hide);
        }

        public void Show(string content)
        {
            gameObject.SetActive(true);
            contentTxt.text = content;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
