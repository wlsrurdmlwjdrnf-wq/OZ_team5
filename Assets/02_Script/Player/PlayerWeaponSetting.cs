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

    [Tooltip("임시로 무기 타입 결정을 위한 직렬화( 0 -> 쿠나이 , 1-> 샷건, 2-> 드릴샷, 3-> 축구공," +
        "4-> 방어막, 5-> 화염병, 6->수호자)")]
    [SerializeField] private int weaponType;

    private void Start()
    {
        switch (weaponType)
        {
            case 0:
                kunai.SetActive(true);
                break;
            case 1:
                shotgun.SetActive(true);
                break;
            case 2:
                drillshot.SetActive(true);
                break;
            case 3:
                football.SetActive(true);
                break;
            case 4:
                barrier.SetActive(true);
                break;
            case 5:
                fireBomb.SetActive(true);
                break;
            case 6:
                defender.SetActive(true);
                break;
        }
    }
}
