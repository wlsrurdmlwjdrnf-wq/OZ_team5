using UnityEngine.EventSystems;
using UnityEngine;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private bool isDragging = false;
    private Vector2 inputVector;

    public Vector2 GetInput() => inputVector;

    private void Update()
    {
        //PC 마우스 입력
        if (Input.GetMouseButtonDown(0))
        {
            ShowJoystick(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            DragJoystick(Input.mousePosition, null);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ResetJoystick();
        }
    }
    //모바일 터치 입력(PC에서도 입력가능하나 그렇게 하려면 조이스틱이 항시 보여야함)
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowJoystick(eventData.position);
        DragJoystick(eventData.position, eventData.pressEventCamera);
    }
    public void OnDrag(PointerEventData eventData) 
    {
        DragJoystick(eventData.position, eventData.pressEventCamera);
    }
    public void OnPointerUp(PointerEventData eventData) 
    {
        ResetJoystick();
    }

    //조이스틱 제어
    private void ShowJoystick(Vector2 position)
    {
        background.position = position;
        background.gameObject.SetActive(true);
        isDragging = true;
    }
    private void DragJoystick(Vector2 position, Camera cam)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, position, cam, out pos);//화면상의 터치/마우스 위치를 조이스틱 배경 내부 좌표로 바꿔줌

        pos = Vector2.ClampMagnitude(pos, background.sizeDelta.x * 0.5f);//조이스틱 핸들이 배경 원 밖으로 안 나가게 해줌
        handle.localPosition = pos;
        inputVector = pos / (background.sizeDelta.x * 0.5f);
    }
    private void ResetJoystick()
    {
        inputVector = Vector2.zero;
        handle.localPosition = Vector2.zero;
        background.gameObject.SetActive(false);
        isDragging = false;
    }
}
