using UnityEngine;

public class Fixed : MonoBehaviour
{
    //세로 기준 해상도
    [SerializeField] private int setWidth = 720;
    [SerializeField] private int setHeight = 1280;

    private void Start()
    {
        SetResolution();//초기에게임해상도고정
    }

    //해상도설정함수(세로720x1280기준으로고정+레터박스처리)
    public void SetResolution()
    {
        //현재기기해상도
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        //목표비율(세로)
        float targetAspect = (float)setWidth / setHeight;
        //현재비율
        float deviceAspect = (float)deviceWidth / deviceHeight;

        //전체화면(권장:Windowed로테스트하려면false로바꿔도됨)
        Screen.SetResolution(setWidth, setHeight, true);

        //메인카메라없으면예외방지
        if (Camera.main == null) return;

        //기기비율이더가로로긴경우(좌우레터박스)
        if (deviceAspect > targetAspect)
        {
            float newWidth = targetAspect / deviceAspect;
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        //기기비율이더세로로긴경우(상하레터박스)
        else if (deviceAspect < targetAspect)
        {
            float newHeight = deviceAspect / targetAspect;
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
        //비율이같으면그대로
        else
        {
            Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}
