using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumData;

/*
 UI 전체를 관리하는 매니저
 - Screen: 로비/배틀HUD/결과 등 "큰 화면"(보통 1개 유지)
 - Popup: 일시정지/상점/아이템상세/레벨업 등(스택으로 쌓고 닫기)
 - System: 로딩/페이드 같은 시스템 UI
 - Toast: 잠깐 뜨는 알림

 원칙
 1) GameManager는 "게임 상태"를 관리
 2) UIManager는 "보이기/숨기기/스택 관리"만 담당
*/
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas Layer Roots")]
    [SerializeField] private Transform screenRoot;
    [SerializeField] private Transform popupRoot;
    [SerializeField] private Transform systemRoot;
    [SerializeField] private Transform toastRoot;

    private readonly Stack<UIScreen> screenStack = new();
    private readonly Stack<UIPopup> popupStack = new();

    private readonly Dictionary<PopupId, UIPopup> popupTable = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //Screen
    public void ShowScreen(UIScreen screen, bool clearStack = true)
    {
        if (screen == null) return;

        if (screenRoot != null && screen.transform.parent != screenRoot)
        {
            screen.transform.SetParent(screenRoot, false);
        }

        if (clearStack)
        {
            while (screenStack.Count > 0)
            {
                UIScreen top = screenStack.Pop();
                if (top != null)
                {
                    top.Hide();
                }
            }
        }
        else
        {
            if (screenStack.Count > 0)
            {
                UIScreen top = screenStack.Peek();
                if (top != null)
                {
                    top.Hide();
                }
            }
        }

        screenStack.Push(screen);
        screen.Show();
    }

    //Popup(Register)
    public void RegisterPopup(UIPopup popup)
    {
        if (popup == null) return;

        PopupId id = popup.PopupId;

        popupTable[id] = popup;
        popup.gameObject.SetActive(false);

        if (popupRoot != null && popup.transform.parent != popupRoot)
        {
            popup.transform.SetParent(popupRoot, false);
        }

        Debug.Log($"//Popup Registered: {id} -> {popup.name}");
    }

    //버튼용
    public void ShowShopPopup()
    {
        ShowPopup(PopupId.Shop);
    }

    //Popup(Open)
    public void ShowPopup(PopupId id)
    {
        if (popupTable.TryGetValue(id, out UIPopup popup) == false)
        {
            Debug.LogError($"//등록되지않은팝업:{id}");
            return;
        }

        ShowPopupInternal(popup);
    }

    private void ShowPopupInternal(UIPopup popup)
    {
        if (popup == null)
        {
            return;
        }

        if (popupStack.Contains(popup))
        {
            return;
        }

        popupStack.Push(popup);
        popup.Open();
    }

    //Popup(Close)
    //스택 최상단 팝업 닫기
    public void CloseTopPopup()
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        UIPopup top = popupStack.Pop();
        if (top != null)
        {
            top.Close();
        }
    }

    //지정된 팝업이 "스택 최상단"일 때만 닫기
    //ItemPopup은 Item만 닫고 싶을 때 이걸 써야 Shop이 같이 안 닫힘
    public void ClosePopup(PopupId id)
    {
        if (popupStack.Count == 0)
        {
            return;
        }

        UIPopup top = popupStack.Peek();
        if (top == null)
        {
            return;
        }

        if (top.PopupId != id)
        {
            return;
        }

        popupStack.Pop();
        top.Close();
    }

    //모든 팝업 닫기
    public void CloseAllPopup()
    {
        while (popupStack.Count > 0)
        {
            UIPopup top = popupStack.Pop();
            if (top != null)
            {
                top.Close();
            }
        }
    }

    //System
    public void AttachSystemUI(MonoBehaviour systemUI)
    {
        if (systemUI == null) return;
        if (systemRoot == null) return;

        systemUI.transform.SetParent(systemRoot, false);
    }

    //Toast
    public void ShowToast(MonoBehaviour toastUI)
    {
        if (toastUI == null) return;
        if (toastRoot == null) return;

        toastUI.transform.SetParent(toastRoot, false);
        toastUI.gameObject.SetActive(true);
    }

    //코루틴 러너
    //팝업 오브젝트가 inactive일 수 있어서 UIManager가 대신 코루틴을 돌려줄 때 사용
    public Coroutine Run(IEnumerator routine)
    {
        if (routine == null) return null;

        return StartCoroutine(routine);
    }

    public void Stop(Coroutine coroutine)
    {
        if (coroutine == null) return;

        StopCoroutine(coroutine);
    }
}
