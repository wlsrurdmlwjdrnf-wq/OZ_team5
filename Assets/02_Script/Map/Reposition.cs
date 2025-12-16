using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //자동 계산된 Ground의 월드 단위 크기
    float groundWidth;
    float groundHeight;

    void Awake()
    {
        //1) Renderer(스프라이트/타일맵/메시 등) 기준으로 크기 계산 시도
        Renderer r = GetComponentInChildren<Renderer>();
        if (r != null)
        {
            groundWidth = r.bounds.size.x;
            groundHeight = r.bounds.size.y;
            return;
        }

        //2) Renderer가 없으면 Collider2D 기준으로 크기 계산 시도
        Collider2D c = GetComponentInChildren<Collider2D>();
        if (c != null)
        {
            groundWidth = c.bounds.size.x;
            groundHeight = c.bounds.size.y;
            return;
        }

        //3) 둘 다 없으면 크기 계산 불가
        Debug.LogError($"Reposition({name}): ground size를 계산할 Renderer/Collider2D가 없습니다.");
        groundWidth = groundHeight = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Area 태그가 아닌 트리거는 무시
        if (!collision.CompareTag("Area"))
            return;

        //크기 계산 실패했으면 동작 중단
        if (groundWidth <= 0f || groundHeight <= 0f)
            return;

        //플레이어 위치
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        //현재 Ground 위치
        Vector3 myPos = transform.position;

        //플레이어가 Ground의 어느 방향에 있는지 (+1 / -1)
        float dirX = playerPos.x > myPos.x ? 1f : -1f;
        float dirY = playerPos.y > myPos.y ? 1f : -1f;

        //플레이어가 어느 축으로 더 멀리 넘어갔는지 판단용 거리
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        //더 멀리 벗어난 축으로 Ground를 "자기 크기만큼" 이동
        if (diffX > diffY)
        {
            // 좌우로 넘어감 → 가로 크기만큼 이동
            transform.Translate(Vector3.right * dirX * groundWidth);
        }
        else
        {
            //상하로 넘어감 → 세로 크기만큼 이동
            transform.Translate(Vector3.up * dirY * groundHeight);
        }
    }
}
