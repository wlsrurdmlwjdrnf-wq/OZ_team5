using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    //화면(Screen)이 처음 표시될 때 호출됨
    //param으로 씬/상태에 따라 필요한 데이터 전달 가능
    public virtual void OnOpen(object param = null) 
    { 

    }
    //화면(Screen)이 교체되거나 제거될 때 호출됨
    //이벤트 해제, 리스너 정리, 데이터 초기화 용도
    public virtual void OnClose() 
    { 

    }
}
