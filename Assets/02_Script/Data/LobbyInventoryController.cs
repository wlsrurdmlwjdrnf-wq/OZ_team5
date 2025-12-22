using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInventoryController : BaseInventoryController
{
    [SerializeField] private InventorySlotEquipUI[] equipUI;
    [SerializeField] private InventorySlotGeneralUI[] generalUI;

    protected override void LoadInven()
    {
    }
}
