using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkillBoxUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] Image[] star;
    [SerializeField] Sprite yellowStar;
    [SerializeField] Sprite grayStar;
    [SerializeField] Button btn;
    private IngameItemData _item;

    protected void Start()
    {
        btn.onClick.AddListener(OnClickGetSkill);
    }
    public virtual void SetupSkillUIData(IngameItemData item)
    {
        _item = item;
        nameText.text = item.name;
        _icon.sprite = item.icon;
        info.text = DataManager.Instance.GetSkillInfo(item.id);
        for (int i = 0; i < star.Length; i++)
        {
            if ( i < item.level)
            {
                star[i].sprite = yellowStar;
            }
            else
            {
                star[i].sprite = grayStar;
            }
        }
    }
    protected void OnClickGetSkill()
    {
        SkillSystem.Instance.SelectSkill(_item.id);
    }
}
