using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAtkHpUI : MonoBehaviour
{
    public TextMeshProUGUI hp;
    public TextMeshProUGUI atk;

    private void Start()
    {
        PlayerManager.Instance.OnItemUpdata += ShowPlayerStatView;
        hp.text = $"{PlayerManager.Instance.playerData.playerMaxHp}";
        atk.text = $"{PlayerManager.Instance.playerData.playerAtk}";
    }
    private void ShowPlayerStatView()
    {
        hp.text = $"{PlayerManager.Instance.playerData.playerMaxHp}";
        atk.text = $"{PlayerManager.Instance.playerData.playerAtk}";
    }
    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnItemUpdata -= ShowPlayerStatView;
        }
    }
}
