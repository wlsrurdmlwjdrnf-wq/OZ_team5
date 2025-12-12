using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private const string DATA_PATH = "Data"; // 모든 csv파일 데이터들이 모여있는 폴더명


    private const string ITEM_DATA_PATH = "ItemData"; // 아이템 데이터 csv파일명
    private List<ItemData_> itemData = new List<ItemData_>(); // 아이템 데이터를 저장할 컨테이너



    protected override void Init()
    {
        base.Init();
    }

    private void LoadItemData()
    {
       
    }
}

