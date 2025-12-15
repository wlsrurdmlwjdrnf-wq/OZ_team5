using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Activate(player);
        }
    }
    public abstract void Activate(Player player);
}
