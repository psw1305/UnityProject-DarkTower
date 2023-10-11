using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExploreNpc : MonoBehaviour
{
    private Transform ui;
    private CanvasGroup doIcon;
    private Tween _tween = null;

    public Transform uiPanel;
    bool activePanel = false;

    void Start()
    {
        ui = transform.Find("UI");
        doIcon = ui.Find("Do Icon").GetComponent<CanvasGroup>();

        uiPanel.gameObject.SetActive(false);
    }

    public void Interact()
    {
        activePanel = !activePanel;
        GameSystem.Instance.State = (activePanel ? GameState.INTERACT : GameState.EXPLORE); 

        uiPanel.gameObject.SetActive(activePanel);
        UI_Character.Instance.inventoryPanel.SetActive(activePanel);

        if (!activePanel) UI_TooltipItem.Hide();
    }

    public void ContactUI(bool isCheck)
    {
        if (isCheck)
        {
            ImageTween(true);
        }
        else
        {
            ImageTween(false);
        }
    }

    void ImageTween(bool isOn)
    {
        if (isOn)
        {
            _tween = doIcon.DOFade(1f, 0.15f).SetEase(Ease.OutSine);
        }
        else
        {
            _tween = doIcon.DOFade(0f, 0.15f).SetEase(Ease.OutSine);
        }
    }

    void OnDisable()
    {
        if (DOTween.instance != null) _tween?.Kill();
    }
}
