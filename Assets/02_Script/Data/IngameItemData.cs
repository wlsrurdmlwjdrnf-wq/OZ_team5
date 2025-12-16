using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameItemData
{
    public int id;
    public string name;
    public EnumData.IngameItemType type;

    public int pairID; // 조합시 필요한 id
    public int evolutionID; // 진화됬을때 id
}
