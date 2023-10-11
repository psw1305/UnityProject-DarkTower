using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum NoticeType
{
    NONE,
    NOTICE,
    BUFF,
    BURN,
    BLIGHT,
    BLEED,
}

public class UI_TextNotice : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textNotice;
    private Image iconNotice;
    private Tween tween;

    void NoticeToText(NoticeType type, string text)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        textNotice = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        iconNotice = textNotice.transform.Find("Icon").GetComponent<Image>();
        textNotice.text = text;

        switch (type)
        {
            case NoticeType.NOTICE:
                textNotice.color = new Color32(161, 161, 161, 255);
                iconNotice.color = Color.clear;
                iconNotice.sprite = null;
                break;
            case NoticeType.BUFF:
                textNotice.color = new Color32(0, 84, 119, 255);
                iconNotice.color = Color.clear;
                iconNotice.sprite = null;
                break;
            case NoticeType.BURN:
                textNotice.color = new Color32(226, 88, 34, 255);
                iconNotice.color = Color.white;
                iconNotice.sprite = Resources.Load<Sprite>("Images/Icon/Attribute/text-attribute-fire");
                break;

            case NoticeType.BLIGHT:
                textNotice.color = new Color32(0, 108, 0, 255);
                iconNotice.color = Color.white;
                iconNotice.sprite = Resources.Load<Sprite>("Images/Icon/Attribute/text-attribute-poison");
                break;

            case NoticeType.BLEED:
                textNotice.color = new Color32(138, 3, 3, 255);
                iconNotice.color = Color.white;
                iconNotice.sprite = Resources.Load<Sprite>("Images/Icon/Attribute/text-attribute-bleed");
                break;
        }

        // DOTween 실행, 애니메이션 종료시 gameObject 삭제
        tween = canvasGroup.DOFade(0, 0.4f).SetEase(Ease.OutSine).SetDelay(0.6f).OnComplete(() => Destroy(gameObject));
    }

    public void TextShow(NoticeType type, string text, Transform textTransform)
    {
        GameObject textClone = Instantiate(gameObject, textTransform);

        UI_TextNotice textCloneScript = textClone.GetComponent<UI_TextNotice>();
        textCloneScript.NoticeToText(type, text);
    }

    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            tween?.Kill();
        }
    }
}
