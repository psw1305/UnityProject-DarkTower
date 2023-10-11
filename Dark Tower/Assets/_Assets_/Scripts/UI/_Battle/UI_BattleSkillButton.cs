using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class UI_BattleSkillButton : MonoBehaviour
{
    [TabGroup("Skill", "정보")] public Image icon;
    [TabGroup("Skill", "정보")] public Image caseImage;
    private SkillSystem skillSystem;

    [TabGroup("Skill", "UI")][BoxGroup("Skill/UI/Base")] public GameObject info;
    [TabGroup("Skill", "UI")][BoxGroup("Skill/UI/Base")] public TextMeshProUGUI skillName, skillTarget, skillSTG;
    [TabGroup("Skill", "UI")][BoxGroup("Skill/UI/Text")] public TextMeshProUGUI textHIT, textCRIT, textSTR;

    private Toggle toggle;
    private SkillData skillData;

    public SkillData Data
    {
        get
        { 
            return skillData;
        }
        set
        {
            skillData = value;
        }
    }

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(SkillSelect);
        skillSystem = SkillSystem.Instance;
    }

    public void SkillSelect(bool isOn)
    {
        if (skillSystem != null)
        {
            if (isOn)
            {
                toggle.isOn = true;
                skillSystem.isToggle = true;
                skillSystem.selectSkillBtn = this;
                info.SetActive(true);
            }
            else
            {
                toggle.isOn = false;
                skillSystem.isToggle = false;
                skillSystem.selectSkillBtn = null;
                info.SetActive(false);
            }
        }
    }

    public void SkillToggleKey()
    {
        bool isCheck = toggle.isOn ? false : true;
        SkillSelect(isCheck);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///-------------------------------------------- UI -----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void SetUISkill()
    {
        skillName.text = skillData.textName;
        SetTarget();
        SetText();
    }

    void SetTarget()
    {
        switch (skillData.targetType)
        {
            case TargetType.Self:
                skillTarget.text = "자신";
                skillTarget.color = new Color32(255, 255, 255, 255);
                break;

            case TargetType.Player:
                skillTarget.text = "동료 단일";
                skillTarget.color = new Color32(0, 150, 255, 255);
                break;

            case TargetType.PlayerAll:
                skillTarget.text = "동료 전체";
                skillTarget.color = new Color32(0, 150, 255, 255);
                break;

            case TargetType.Enemy:
                skillTarget.text = "적 단일";
                skillTarget.color = new Color32(255, 50, 0, 255);
                break;

            case TargetType.EnemyAll:
                skillTarget.text = "적 전체";
                skillTarget.color = new Color32(255, 50, 0, 255);
                break;

            case TargetType.All:
                skillTarget.text = "전체";
                skillTarget.color = new Color32(255, 50, 0, 255);
                break;
        }
    }

    void SetText()
    {
        textHIT.text = skillData.editHIT + "%";
        textCRIT.text = skillData.editCRIT + "%";
        textSTR.text = skillData.editSTR + "%";
    }
}
