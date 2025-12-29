using System.Collections.Generic;
using UnityEngine;
using static EnumData;

//툴팁 표시용으로 필요한 값만 모은 뷰 데이터
public class ItemDataView
{
    public string itemName;                //이름
    public EquipmentType type;             //종류
    public EquipmentTier tier;             //등급
    public Sprite icon;                    //아이콘(없으면 null)

    public int statValue;                  //최종 증가 수치(테이블 값 그대로)
    public List<string> gradeSkills;       //등급 스킬 문구 리스트(테이블 값 그대로)
}
