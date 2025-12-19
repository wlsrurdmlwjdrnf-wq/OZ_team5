using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{
    //팝업이 화면에 표시될 때 호출됨
    public virtual void OnOpen(object param = null) 
    {

    }
    //팝업이 닫힐 때 호출됨
    //이벤트 구독 해제, 코루틴 정리 같은 마무리 작업용
    public virtual void OnClose() 
    {

    }
}
