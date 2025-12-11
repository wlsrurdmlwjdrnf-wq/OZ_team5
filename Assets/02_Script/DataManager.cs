using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ItemID 규칙
//   종류 등급 넘버링(2자리)
//ex)  1    1   0    0   1


public enum ItemType
{
    Weapon = 1,
    Armor = 2,
    Pants = 3,
    Gloves = 4,
    Boots = 5
    
}



public class DataManager : Singleton<DataManager>
{
    private const string DATA_PATH = "GameData";

    protected override void Init()
    {
        base.Init();
    }


}

public class ItemData
{

}
