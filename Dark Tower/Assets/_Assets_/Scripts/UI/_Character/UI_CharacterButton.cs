using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterButton : MonoBehaviour
{
    public TextMeshProUGUI characterName;  
    private UnitPlayer _unitPlayer;
    private Toggle characterBtn;
    private UI_Character characterInfo;

    void Start()
    {
        characterInfo = UI_Character.Instance;

        characterBtn = GetComponent<Toggle>();
        characterBtn.group = GetComponentInParent<ToggleGroup>();
        characterBtn.onValueChanged.AddListener(SelectCharacter);
    }

    public void Init(UnitPlayer unitPlayer)
    {
        _unitPlayer = unitPlayer;
        characterName.text = unitPlayer.nickName;
    }

    void SelectCharacter(bool isOn)
    {
        if (_unitPlayer == null) return;

        if (isOn) characterInfo.SetCharacter(_unitPlayer);
    }
}
