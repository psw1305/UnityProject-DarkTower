using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TurnUnit : MonoBehaviour
{
    public UnitType unitType;
    public UnitSystem unit;
    public Image icon;
    public TextMeshProUGUI nickName;
    public CanvasGroup turnCase;
    public Image caseLight, pointLight;
    public float spd, spdLevel, detailSpd;
    public bool isCheck = false;

    private Tween _case = null, _light = null;

    public void Init(UnitSystem _unit)
    {
        unit = _unit;
        unit.turnUnits.Add(this);

        spd = unit.SPD;
        unit.SPD -= 6;

        nickName.text = unit.nickName;
        gameObject.name = Naming();

    }

    string Naming()
    {
        if (spd >= 18)
        {
            spdLevel = 5;
            return "[5] " + DetailSpeed();
        }
        else if (spd >= 12)
        {
            spdLevel = 4;
            return "[4] " + DetailSpeed();
        }
        else if (spd >= 6)
        {
            spdLevel = 3;
            return "[3] " + DetailSpeed();
        }
        else if (spd >= 0)
        {
            spdLevel = 2;
            return "[2] " + DetailSpeed();
        }
        else 
        {
            spdLevel = 1;
            return "[1] " + DetailSpeed();
        }
    }

    public float DetailSpeed()
    {
        detailSpd = spd + Random.Range(0, 11);
        return detailSpd;
    }

    public void Active()
    {
        _light = caseLight.DOFade(0.5f, 0.3f).SetEase(Ease.OutSine);
    }

    public void Deactive()
    {
        isCheck = true;
        _case = turnCase.DOFade(0.2f, 0.3f).SetEase(Ease.OutSine);
        _light = caseLight.DOFade(0f, 0.3f).SetEase(Ease.OutSine);
    }

    public void ImageFadeOn()
    {
        if (!isCheck)
        {
            _light = pointLight.DOFade(1f, 0.3f).SetEase(Ease.OutSine);
        }
    }

    public void ImageFadeOff()
    {
        if (!isCheck)
        {
            _light = pointLight.DOFade(0f, 0.3f).SetEase(Ease.OutSine);
        }
    }

    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            _case?.Kill();
            _light?.Kill();
        }
    }
}
