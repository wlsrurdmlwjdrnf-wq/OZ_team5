using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPopUIController : MonoBehaviour
{
    private SkillBarUI skUI;
    //플레이어 레벨 팝업
    [SerializeField] Image expBackground;
    [SerializeField] TextMeshProUGUI playerLv;

    //스킬선택 팝업
    [SerializeField] Image stringBackground;
    [SerializeField] TextMeshProUGUI stringText;

    //인벤토리 팝업
    [SerializeField] Transform skillTab;
    [SerializeField] GameObject atkSkillbar;
    [SerializeField] GameObject supSkillBar;
    [SerializeField] TextMeshProUGUI choiceText;

    //스킬 팝업
    [SerializeField] Transform skillRouletteTab;
    [SerializeField] GameObject atkSkillBox;
    [SerializeField] GameObject supSkillBox;

    //새로고침
    [SerializeField] Button refresh;

    private void Start()
    {
        refresh.onClick.AddListener(OnClickRefreshButton);      
        skUI = GetComponent<SkillBarUI>();
    }

    private void OnEnable()
    {
        ViewPlayerBattleInven();
        ResetUIData();
        refresh.gameObject.SetActive(true);
    }

    private void ResetUIData()
    {
        foreach (Transform child in skillRouletteTab)
        {
            Destroy(child.gameObject);
        }

        var skillList = GachaManager.Instance.SelectSkill();

        foreach (var data in skillList)
        {
            GameObject temp = null;

            if (data.type == EnumData.SkillType.Attack)
            {
                temp = atkSkillBox;
            }
            else if (data.type == EnumData.SkillType.Support)
            {
                temp = supSkillBox;
            }
            else continue;

            GameObject result = GameObject.Instantiate(temp, skillRouletteTab);

            BaseSkillBoxUI boxScript = result.GetComponent<BaseSkillBoxUI>();
            boxScript.SetupSkillUIData(data);            
        }
    }

    private void ViewPlayerBattleInven()
    {
        skUI.GetPlayerInvenData();
    }

    private void OnClickRefreshButton()
    {
        ResetUIData();
        refresh.gameObject.SetActive(false);
    }
}
