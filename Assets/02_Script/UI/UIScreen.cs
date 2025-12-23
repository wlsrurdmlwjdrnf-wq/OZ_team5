using UnityEngine;

//모든 화면 UI의 베이스 클래스
// - 로비, 배틀 HUD, 타이틀 화면 등
// - UIManager가 Show / Hide로 제어
public abstract class UIScreen : MonoBehaviour
{
    //최초 1회 초기화 여부
    private bool initialized;

    //UIScreen 표시(UIManager에서 호출)
    public void Show()
    {
        //최초 1회만 초기화
        if (!initialized)
        {
            initialized = true;
            OnInit();
        }

        gameObject.SetActive(true);
        OnShow();
    }

    //UIScreen 숨김(UIManager에서 호출)
    public void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    //Override 포인트

    //최초 1회만 호출
    //버튼 바인딩, 참조 캐싱
    protected virtual void OnInit() { }

    //화면이 열릴 때마다 호출
    protected virtual void OnShow() { }

    //화면이 닫힐 때마다 호출
    protected virtual void OnHide() { }
}
