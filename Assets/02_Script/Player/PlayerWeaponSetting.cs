using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSetting : MonoBehaviour
{
    [SerializeField] private GameObject kunai;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject drillshot;
    [SerializeField] private GameObject football;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject fireBomb;
    [SerializeField] private GameObject defender;

    [SerializeField] private Player player;
    
    private void Start()
    {
        switch (player.PlayerStat().playerSkillInven[0].id)
        {
            case 0:
            case 1010:
            case 1011:
            case 1012:
            case 1013:
            case 1014:
                kunai.SetActive(true);
                break;
            case 1000:
            case 1001:
            case 1002:
            case 1003:
            case 1004:
                shotgun.SetActive(true);
                break;
        }
    }
}
