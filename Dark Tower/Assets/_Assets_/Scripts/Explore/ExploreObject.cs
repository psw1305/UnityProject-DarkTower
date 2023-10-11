using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExploreObject : MonoBehaviour
{
    private Transform ui;
    private CanvasGroup doIcon;
    private Image image;
    private bool isClosest;
    private Tween _tween = null;

    protected GameSystem gameSystem;
    protected ExploreSystem exploreSystem;

    void Start()
    {
        ui = transform.Find("UI");
        doIcon = ui.Find("Do Icon").GetComponent<CanvasGroup>();
        image = ui.Find("Do Icon").GetComponent<Image>();

        gameSystem = GameSystem.Instance;
        exploreSystem = ExploreSystem.Instance;
    }

    public void SetFillAmount(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }

    public void SetClosest(bool isCheck)
    {
        isClosest = isCheck;
    }

    public bool GetClosest()
    {
        return isClosest;
    }

    virtual public void Interact() {}

    virtual public void ContactUI(bool isCheck)
    {
        if (isCheck)
        {
            image.fillAmount = 0;
            ImageTween(true);
        }
        else
        {
            ImageTween(false);
        }
    }

    protected void ImageTween(bool isOn)
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
