using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSetting : MonoBehaviour
{
    [SerializeField] private GameObject kunai;
    [SerializeField] private GameObject shotgun;

    [Tooltip("임시로 무기 타입 결정을 위한 직렬화( 0 -> 쿠나이 , 1-> 샷건)")]
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
                break ;
        }
    }
}
