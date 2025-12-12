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
    private const string DATA_PATH = "06_Data"; // 모든 csv파일 데이터들이 모여있는 폴더명


    //private const string ITEM_DATA_PATH = "ItemData"; // 아이템 데이터 csv파일명
    //private const string CHAPTER_DATA_PATH = "ChapterData"; // 챕터 데이터 csv파일명

    //private List<ItemData> itemData = new List<ItemData>(); // 아이템 데이터를 저장할 컨테이너
    //private List<ChapterData> chapterData = new List<ChapterData>(); // 챕터 데이터를 저장할 컨테이너



    protected override void Init()
    {
        base.Init();
    }

    private void LoadItemData()
    {
        
    }

    private void LoadChapterData()
    {

    }
}

