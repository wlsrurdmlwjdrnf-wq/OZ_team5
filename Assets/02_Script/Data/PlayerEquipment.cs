using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player 장착아이템 관리
public class PlayerEquipment : MonoBehaviour
{
    [Tooltip("장착 아이템 컨테이너")]
    private Dictionary<EnumData.EquipmentType, int> currentEquipment = new Dictionary<EnumData.EquipmentType, int>();

}
