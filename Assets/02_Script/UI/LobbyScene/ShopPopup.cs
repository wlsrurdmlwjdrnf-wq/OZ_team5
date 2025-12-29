using System.Collections;
using UnityEngine;

//상점 팝업(로비 전용)
//- 상점 버튼 클릭으로 열림
//- 2초 이후부터 화면 클릭하면 닫힘
public class ShopPopup : MonoBehaviour
{
    [SerializeField] private GameObject root;   //팝업 루트(켜기/끄기)
    [SerializeField] private float closeDelay = 1f; //1초 후 닫기 허용

    private bool canClose;
    private Coroutine delayCo;

    private void Awake()
    {
        if (root != null) root.SetActive(false);
    }

    public void Open()
    {
        if (root != null) root.SetActive(true);

        canClose = false;

        if (delayCo != null) StopCoroutine(delayCo);
        delayCo = StartCoroutine(CoEnableClose());
    }

    public void Close()
    {
        if (root != null) root.SetActive(false);

        canClose = false;

        if (delayCo != null) StopCoroutine(delayCo);
        delayCo = null;
    }

    private IEnumerator CoEnableClose()
    {
        yield return new WaitForSecondsRealtime(closeDelay);
        canClose = true;
    }

    private void Update()
    {
        if (root == null || !root.activeInHierarchy) return;
        if (!canClose) return;

        if (Input.GetMouseButtonDown(0)) Close();
    }
}
