using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum DamageType
{
    NORMAL,
    MISS,
    HEAL,
    SHIELD,
}

public class UI_TextDamage : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI textDamage;
    private Image iconDamage;
    private Tween tween;

    void DamageToText(DamageType type, string text)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        textDamage = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        iconDamage = textDamage.transform.Find("Icon").GetComponent<Image>();
        textDamage.text = text;

        switch (type)
        {
            case DamageType.NORMAL:
                textDamage.color = new Color32(255, 87, 0, 255);
                iconDamage.color = Color.clear;
                iconDamage.sprite = null;
                break;

            case DamageType.MISS:
                textDamage.color = new Color32(136, 140, 141, 255);
                iconDamage.color = Color.clear;
                iconDamage.sprite = null;
                break;

            case DamageType.HEAL:
                textDamage.color = new Color32(111, 196, 135, 255);
                iconDamage.color = Color.white;
                iconDamage.sprite = Resources.Load<Sprite>("Images/Icon/Attribute/text-attribute-heal");
                break;

            case DamageType.SHIELD:
                textDamage.color = new Color32(0, 255, 42, 255);
                iconDamage.color = Color.white;
                iconDamage.sprite = Resources.Load<Sprite>("Images/Icon/Attribute/text-attribute-heal");
                break;
        }

        // DOTween 실행, 애니메이션 종료시 gameObject 삭제
        tween = canvasGroup.DOFade(0, 0.4f).SetEase(Ease.OutSine).SetDelay(1f).OnComplete(() => Destroy(gameObject));
    }

    public void TextShow(DamageType type, string text, Transform textTransform)
    {
        GameObject textClone = Instantiate(gameObject, textTransform);

        UI_TextDamage textCloneScript = textClone.GetComponent<UI_TextDamage>();
        textCloneScript.DamageToText(type, text);

        Canvas.ForceUpdateCanvases();

        // DOTween 실행, 애니메이션 종료시 textClone 삭제
        //_tween = textClone.GetComponent<TextMeshProUGUI>().DOFade(0, 0.4f).SetEase(Ease.OutSine).SetDelay(1f).OnComplete(() => Destroy(textClone));
    }

    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            tween?.Kill();
        }
    }
}
