using TMPro;
using UnityEngine;

//게임 클리어 팝업
//- 활성화될 때(OnEnable) 코인/처치 수만 갱신
//- 버튼 클릭(로비 이동/다음 등)은 인스펙터에서 다른 스크립트로 처리
public class GameClearPopup : UIPopup
{
    [Header("Result Texts")]
    [SerializeField] private TextMeshProUGUI goldText; //코인 표시(00000)
    [SerializeField] private TextMeshProUGUI killText; //처치 수 표시(00000)

    private BattleHUDNumbers hud; //상시 HUD에서 처치 수 가져오기
    private PlayerData playerData; //PlayerData에서 코인 가져오기

    private void OnEnable()
    {
        RefreshResult();
    }

    private void RefreshResult()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<BattleHUDNumbers>();
        }

        if (playerData == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerData = player.PlayerStat();
            }
        }
        //골드
        int gold = 0;
        if (playerData != null)
        {
            gold = (int)playerData.playerGold;
        }

        if (goldText != null)
        {
            goldText.text = $"{gold}";
        }
        //킬수
        int killCount = 0;
        if (hud != null)
        {
            killCount = hud.GetKillCount();
        }

        if (killText != null)
        {
            killText.text = $"{killCount}";
        }
    }
}
