using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class UI_TooltipBuff : MonoBehaviour
{
    public static UI_TooltipBuff current { get; private set; }
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;
    private RectTransform rectTrnf;

    void Awake()
    {
        current = this;
        rectTrnf = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public static void Show(Vector3 pos, string buffName, string buffDesc)
    {
        current.rectTrnf.position = pos;
        current.gameObject.SetActive(true);

        current.buffName.text = buffName;
        current.buffDesc.text = buffDesc;
    }

    public static void Hide()
    {
        current.gameObject.SetActive(false);

        current.buffName.text = "아이템 설명";
        current.buffDesc.text = "아이템 스탯";
    }
}
