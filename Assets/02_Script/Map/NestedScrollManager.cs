using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 수평 스크롤(탭) + 각 탭의 수직 스크롤을 함께 관리하는 매니저
public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;         // 메인 가로 Scrollbar (수평 페이지 이동)
    public Transform contentTr;         // 각 탭(컨텐츠)이 들어 있는 부모 Transform
    public Slider tabSlider;            // 탭의 이동 상태를 반영하는 UI 슬라이더 (하이라이트용)    
    public RectTransform[] BtnRect;     // 탭 버튼의 RectTransform 배열    
    public RectTransform[] BtnImageRect;// 각 버튼의 아이콘(RectTransform), 버튼 위에 위치한 이미지

    const int SIZE = 4;            // 탭 수
    float[] pos = new float[SIZE]; // 0~1 사이에 균등 배치된 위치 저장용

    float distance;   // 각 탭 사이 간격 (예: 0, 0.33, 0.66, 1)
    float curPos;     // 드래그 시작할 때 위치
    float targetPos;  // 스크롤이 도달해야 할 목표 위치

    bool isDrag;      // 현재 드래그 중인지 여부
    int targetIndex;  // 선택된 탭 인덱스

    void Start()
    {
        // 탭 사이의 간격 계산 (총 SIZE개 → 간격 = 1 / (SIZE - 1))
        distance = 1f / (SIZE - 1);

        // pos 배열에 균등 간격 위치 저장
        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
    }

    // 현재 스크롤 위치에서 가장 가까운 탭의 pos 값을 찾음
    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            // 절반 거리 기준 안에 들어오면 해당 탭을 선택한 것으로 간주
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        // 어느 것도 못 찾았다면 기본값(0) 반환 (첫 번째 탭)
        return 0;
    }

    // 드래그 시작 시 현재 위치 저장
    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    // 드래그 중 상태 표시
    public void OnDrag(PointerEventData eventData) => isDrag = true;

    // 드래그 종료 시 목표 위치 계산 + 빠르게 넘긴 경우 처리
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // 드래그 속도가 빠를 때는 절반 넘어가지 않아도 페이지 넘김 처리
        if (curPos == targetPos)
        {
            // 왼쪽으로 쓸기 (양수 → 왼쪽)
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }
            // 오른쪽으로 쓸기 (음수 → 오른쪽)
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }
        // 새 탭으로 이동했다면 해당 탭의 수직 스크롤을 맨 위로
        VerticalScrollUp();
    }

    // 이동한 탭의 수직 스크롤을 맨 위로 올리는 함수
    void VerticalScrollUp()
    {
        for (int i = 0; i < SIZE; i++)
        {
            // ScrollScript가 붙어 있는(수직 스크롤이 존재하는) 탭 찾기
            // 이전 탭과 다른 탭으로 이동했을 때만 동작
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
            {
                // 각 탭의 2번째 자식(1번 인덱스)에 있는 Scrollbar를 찾아 value = 1 (맨 위)
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
            }
        }
    }

    void Update()
    {
        // 탭 이동 상태를 슬라이더에 반영
        tabSlider.value = scrollbar.value;

        // 드래그 중이 아닐 때 자동으로 부드럽게 목표 pos로 이동
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            // 선택된 탭의 버튼만 크기 증가(기본 120 → 선택 시 240)
            for (int i = 0; i < SIZE; i++)
            {
                BtnRect[i].sizeDelta =
                    new Vector2(i == targetIndex ? 240 : 120, BtnRect[i].sizeDelta.y);
            }
        }

        // 초기 프레임에서는 튀는 연출을 방지하기 위해 0.1초 후부터 실행
        if (Time.time < 0.1f) return;

        // 버튼 아이콘 애니메이션 처리
        for (int i = 0; i < SIZE; i++)
        {
            // 기본 상태(중앙, 크기 1, 텍스트 비활성화)
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            // 선택한 탭은 아이콘 크기 확대 + 위로 올림 + 텍스트 활성화
            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }

            // 부드러운 이동 및 크기 변화
            BtnImageRect[i].anchoredPosition3D =
                Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);

            BtnImageRect[i].localScale =
                Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);

            // 아이콘 아래 텍스트 보여주기/숨기기
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    // 특정 탭 버튼 클릭 시 직접 이동하는 함수
    public void TabClick(int n)
    {
        curPos = SetPos();
        targetIndex = n;
        targetPos = pos[n];
        VerticalScrollUp();
    }
}
