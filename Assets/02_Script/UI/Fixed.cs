using UnityEngine;

public class Fixed : MonoBehaviour
{
    //세로 기준 해상도
    [SerializeField] private int setWidth = 720;
    [SerializeField] private int setHeight = 1280;

    //메인 카메라 참조(매번 Camera.main 호출 방지)
    [SerializeField] private Camera targetCamera;

    //이전 해상도/모드 저장용(변경 감지)
    private int lastWidth;
    private int lastHeight;
    private FullScreenMode lastMode;

    private void Awake()
    {
        //카메라가 지정되지 않았으면 MainCamera 사용
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Start()
    {
        //게임 시작 시 한 번 강제 적용
        ApplyResolution(true);
        CacheScreenState();
    }

    private void Update()
    {
        //창모드/전체화면 전환 또는 리사이즈 감지
        if (Screen.width != lastWidth ||
            Screen.height != lastHeight ||
            Screen.fullScreenMode != lastMode)
        {
            //전환 직후 다시 비율 계산
            ApplyResolution(false);
            CacheScreenState();
        }
    }

    //현재 화면 상태 저장
    private void CacheScreenState()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        lastMode = Screen.fullScreenMode;
    }

    //해상도 + 레터박스 적용 함수
    private void ApplyResolution(bool forceSetResolution)
    {
        //처음 실행 시 또는 필요할 때만 해상도 강제 지정
        if (forceSetResolution)
        {
            //true/false 대신 명시적 모드 사용이 안정적
            Screen.SetResolution(setWidth, setHeight, FullScreenMode.FullScreenWindow);
        }

        //카메라가 없으면 예외 방지
        if (targetCamera == null)
            return;

        //실제 적용된 현재 해상도
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        //목표 비율(720 / 1280)
        float targetAspect = (float)setWidth / setHeight;

        //현재 화면 비율
        float deviceAspect = (float)deviceWidth / deviceHeight;

        //현재 화면이 더 가로로 긴 경우(좌우 레터박스)
        if (deviceAspect > targetAspect)
        {
            float newWidth = targetAspect / deviceAspect;
            targetCamera.rect = new Rect(
                (1f - newWidth) / 2f, //좌우 중앙 정렬
                0f, newWidth, 1f 
                );
        }
        //현재 화면이 더 세로로 긴 경우(상하 레터박스)
        else if (deviceAspect < targetAspect)
        {
            float newHeight = deviceAspect / targetAspect;
            targetCamera.rect = new Rect(
                0f,
                (1f - newHeight) / 2f, //상하 중앙 정렬
                1f,
                newHeight
            );
        }
        //비율이 동일한 경우(레터박스 없음)
        else
        {
            targetCamera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}