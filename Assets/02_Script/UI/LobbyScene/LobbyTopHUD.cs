using TMPro;
using UnityEngine;

public class LobbyTopHUD : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;

    //에너지 설정
    private const int MaxEnergy = 60;
    private const float EnergyRegenInterval = 60f;//1분당 1회복

    private int energy = MaxEnergy;
    private float energyTimer;

    private float uiRefreshTimer;

    private void OnEnable()
    {
        energyTimer = 0f;
        uiRefreshTimer = 0f;
        RefreshUI();
    }

    private void Update()
    {
        UpdateEnergy();
        UpdateUIRefresh();
    }

    private void UpdateEnergy()
    {
        if (energy >= MaxEnergy)
        {
            return;
        }

        energyTimer += Time.unscaledDeltaTime;

        if (energyTimer >= EnergyRegenInterval)
        {
            int addCount = (int)(energyTimer / EnergyRegenInterval);
            energyTimer -= addCount * EnergyRegenInterval;

            energy += addCount;

            if (energy > MaxEnergy)
            {
                energy = MaxEnergy;
            }
        }
    }

    private void UpdateUIRefresh()
    {
        uiRefreshTimer += Time.unscaledDeltaTime;

        if (uiRefreshTimer >= 0.5f)
        {
            uiRefreshTimer = 0f;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (GameManager.Instance != null)
        {
            if (goldText != null)
            {
                goldText.text = GameManager.Instance.gameGold.ToString();
            }
        }

        if (energyText != null)
        {
            energyText.text = $"{energy}/{MaxEnergy}";
        }

        if (DataManager.Instance != null)
        {
            var playerData = DataManager.Instance.GetPlayerBaseStat();

            if (playerLevelText != null)
            {
                playerLevelText.text = $"Lv.{playerData.playerLevel}";
            }

            if (playerNameText != null)
            {
                playerNameText.text = "Player";
            }
        }
    }
}
