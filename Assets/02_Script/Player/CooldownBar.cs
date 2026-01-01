using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField] private Image cooldownBar;
    public static float cooldownTime = 0f;
    public static float timer = 0f;

    private void Update()
    {
        if (cooldownTime <= 0f) return;
        timer += Time.deltaTime;
        cooldownBar.fillAmount = timer / cooldownTime;

        if (timer >= cooldownTime)
        {
            timer = 0f;
            cooldownBar.fillAmount = 0f;
        }
    }
}
