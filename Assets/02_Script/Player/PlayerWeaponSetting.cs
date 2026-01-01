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
        Debug.Log($"1234");
        switch (player.PlayerStat().playerSkillInven[0].id)
        {
            case 0:
            case 10001:
                kunai.SetActive(true);
                break;
            case 10002:
                shotgun.SetActive(true);
                break;
        }
    }
}
