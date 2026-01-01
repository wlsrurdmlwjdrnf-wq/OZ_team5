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
    [SerializeField] Player player;

    private void OnEnable()
    {
        RefreshResult();
        GameManager.Instance.AddGold(player.PlayerStat().playerGold);
    }

    private void RefreshResult()
    {
        goldText.text = $"{player.PlayerStat().playerGold}";
        killText.text = $"{EnemyManager.Instance.enemyKillCount}";
    }
}
