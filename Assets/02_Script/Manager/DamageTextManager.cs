using UnityEngine;
using TMPro;
using System.Collections;

public class DamageTextManager : Singleton<DamageTextManager>
{
    public TextMeshProUGUI damageTextPrefab;
    public Transform canvasTransform; // UI 캔버스 참조

    private void Start()
    {
        PoolManager.Instance.CreatePool(damageTextPrefab, 100);
    }
    public void ShowDamage(int damage, Vector3 worldPosition)
    {
        // 월드 좌표 → 화면 좌표 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        TextMeshProUGUI textObj = PoolManager.Instance.GetFromPool(damageTextPrefab); 
        textObj.transform.SetParent(canvasTransform, false);

        textObj.transform.position = screenPos;
        textObj.text = damage.ToString();

        StartCoroutine(FadeOut(textObj));
    }

    private IEnumerator FadeOut(TextMeshProUGUI text)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color startColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 알파값 줄이기
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);

            // 위로 살짝 이동
            text.transform.position += Vector3.up * Time.deltaTime * 20f;

            yield return null;
        }

        PoolManager.Instance.ReturnPool(text);
    }
}