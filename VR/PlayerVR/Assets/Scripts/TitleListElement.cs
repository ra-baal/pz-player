using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TitleListElement : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TitleListController tlc;

    public void SetText(string textString)
    {
        text.text = textString;
    }

    public void OnClick()
    {
        tlc.TitleClicked(text.text);
    }
}
