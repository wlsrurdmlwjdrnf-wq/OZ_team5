using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset;

    private Transform target;

    public void Init(Transform targetTransform)
    {
        target = targetTransform;
    }
    public void UpdateHp(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}