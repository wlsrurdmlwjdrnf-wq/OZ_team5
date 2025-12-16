using System;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///범용 무한 Ground 재배치(리포지션)
///동작 방식
///-현재 Ground들의 중심점들을 보고
/// 1) 열(cols)과 행(rows)을 자동 추정
/// 2) 타일 간격(spacingX, spacingY)을 자동 추정
/// 3) 전체 폭(width = cols*spacingX), 전체 높이(height = rows*spacingY)를 계산
/// 각 타일에 대해:플레이어가 타일 중심에서 width/2 이상 멀어지면 타일을 width만큼 반대편으로 이동(y도 동일)
///</summary>
public class Reposition : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] grounds;

    [Header("Auto Find (optional)")]
    [SerializeField] private bool autoFindPlayer = true;

    [Header("Tolerance")]
    [Tooltip("타일 중심 x/y 값들을 같은 열/행으로 묶을 때 사용하는 허용 오차")]
    [SerializeField] private float clusterEpsilon = 0.2f;

    //계산된 값들
    private int cols, rows;
    private float spacingX, spacingY;   //타일 중심 간격(가로/세로)
    private float width, height;        //전체 폭/높이 = cols*spacingX, rows*spacingY

    private bool initialized;

    private void Awake()
    {
        if (autoFindPlayer && player == null)
        {
            var p = FindAnyObjectByType<Player>();
            if (p != null) player = p.transform;
        }
    }

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("[Reposition] player가 비어 있습니다.");
            enabled = false;
            return;
        }

        if (grounds == null || grounds.Length < 2)
        {
            Debug.LogError("[Reposition] grounds는 최소 2개 이상 넣어주세요.");
            enabled = false;
            return;
        }

        if (!TryComputeGridInfo())
        {
            Debug.LogError("[Reposition] 그리드 정보 계산 실패. Ground들이 격자 형태로 배치돼 있는지 확인하세요.");
            enabled = false;
            return;
        }

        initialized = true;
    }

    private void LateUpdate()
    {
        if (!initialized) return;

        Vector3 p = player.position;

        //각 타일을 검사해서, 플레이어가 너무 멀어지면 반대편으로 순간이동
        for (int i = 0; i < grounds.Length; i++)
        {
            Transform g = grounds[i];
            if (g == null) continue;

            Vector3 gp = g.position;

            //X 방향: 플레이어가 타일 중심에서 width/2보다 오른쪽에 있으면 타일을 오른쪽으로 width만큼 이동
            //        반대로 너무 왼쪽이면 왼쪽으로 width만큼 이동
            float dx = p.x - gp.x;
            if (dx > width * 0.5f) gp.x += width;
            else if (dx < -width * 0.5f) gp.x -= width;

            // Y 방향도 동일
            float dy = p.y - gp.y;
            if (dy > height * 0.5f) gp.y += height;
            else if (dy < -height * 0.5f) gp.y -= height;

            g.position = gp;
        }
    }

    //현재 grounds 배치로부터 cols/rows, spacingX/Y, width/height를 자동 추정
    private bool TryComputeGridInfo()
    {
        //1) 각 타일의 중심 좌표 수집
        List<float> xs = new();
        List<float> ys = new();

        for (int i = 0; i < grounds.Length; i++)
        {
            if (grounds[i] == null) continue;
            xs.Add(grounds[i].position.x);
            ys.Add(grounds[i].position.y);
        }

        if (xs.Count < 2 || ys.Count < 2) return false;

        //2) x/y를 "비슷한 값끼리" 묶어서 고유 열/행 개수 추정
        List<float> uniqueX = ClusterUnique(xs, clusterEpsilon);
        List<float> uniqueY = ClusterUnique(ys, clusterEpsilon);

        cols = uniqueX.Count;
        rows = uniqueY.Count;

        if (cols < 1 || rows < 1) return false;

        //3) spacing(타일 간격) 추정: 정렬 후 인접 차이의 중앙값(median)
        spacingX = EstimateSpacing(uniqueX);
        spacingY = EstimateSpacing(uniqueY);

        //cols나 rows가 1이면 spacing이 0이 될 수 있음 → bounds로 보정 시도
        if (cols == 1) spacingX = EstimateSizeFromBoundsX();
        if (rows == 1) spacingY = EstimateSizeFromBoundsY();

        if (spacingX <= 0f || spacingY <= 0f) return false;

        width = cols * spacingX;
        height = rows * spacingY;

        //디버그 로그
        //Debug.Log($"[Reposition] cols={cols}, rows={rows}, spacingX={spacingX}, spacingY={spacingY}, width={width}, height={height}");

        return true;
    }

    //값들을 epsilon 기준으로 군집화해서 "고유 값들" 리스트를 만듦.
    //예: [0.01, -0.02, 10.0, 9.95] -> [0.0 근처, 10.0 근처]
    private static List<float> ClusterUnique(List<float> values, float epsilon)
    {
        values.Sort();
        List<float> result = new();

        float cur = values[0];
        int count = 1;

        for (int i = 1; i < values.Count; i++)
        {
            if (Mathf.Abs(values[i] - cur) <= epsilon)
            {
                //같은 군집
                cur = (cur * count + values[i]) / (count + 1);
                count++;
            }
            else
            {
                //새 군집 시작
                result.Add(cur);
                cur = values[i];
                count = 1;
            }
        }

        result.Add(cur);
        return result;
    }

    //unique 좌표들 간 인접 차이들의 중앙값을 spacing으로 사용
    private static float EstimateSpacing(List<float> uniqueSorted)
    {
        uniqueSorted.Sort();
        if (uniqueSorted.Count < 2) return 0f;

        List<float> diffs = new();
        for (int i = 1; i < uniqueSorted.Count; i++)
        {
            float d = uniqueSorted[i] - uniqueSorted[i - 1];
            if (d > 1e-5f) diffs.Add(d);
        }

        if (diffs.Count == 0) return 0f;

        diffs.Sort();
        return diffs[diffs.Count / 2]; //median
    }

    //cols==1인 경우, Renderer/Collider bounds에서 가로 크기 추정
    private float EstimateSizeFromBoundsX()
    {
        //첫 ground 기준으로 전체 bounds 합산 (프리팹이 복합 구조여도 OK)
        Bounds? b = GetCombinedBounds(grounds[0]);
        return b.HasValue ? Mathf.Max(b.Value.size.x, 0.0001f) : 0f;
    }

    //rows==1인 경우, Renderer/Collider bounds에서 세로 크기 추정
    private float EstimateSizeFromBoundsY()
    {
        Bounds? b = GetCombinedBounds(grounds[0]);
        return b.HasValue ? Mathf.Max(b.Value.size.y, 0.0001f) : 0f;
    }

    //Transform 아래 모든 Renderer(우선) 또는 Collider2D의 bounds를 합산해서 가져옴.
    private static Bounds? GetCombinedBounds(Transform root)
    {
        if (root == null) return null;

        var rs = root.GetComponentsInChildren<Renderer>(true);
        if (rs.Length > 0)
        {
            Bounds b = rs[0].bounds;
            for (int i = 1; i < rs.Length; i++) b.Encapsulate(rs[i].bounds);
            return b;
        }

        var cs = root.GetComponentsInChildren<Collider2D>(true);
        if (cs.Length > 0)
        {
            Bounds b = cs[0].bounds;
            for (int i = 1; i < cs.Length; i++) b.Encapsulate(cs[i].bounds);
            return b;
        }

        return null;
    }

    //맵을 바꿔서 ground배치가 달라졌을 때, 런타임에 다시 계산하고 싶으면 호출
    public void Rebuild()
    {
        initialized = false;
        if (!TryComputeGridInfo())
        {
            Debug.LogError("[Reposition] Rebuild 실패: Ground 배치가 격자가 아닌지 확인");
            return;
        }
        initialized = true;
    }
}
