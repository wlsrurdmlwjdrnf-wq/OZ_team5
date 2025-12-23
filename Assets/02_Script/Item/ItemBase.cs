using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected void OnEnable()
    {
        StartCoroutine(Duration());
    }
    protected IEnumerator Duration()
    {
        yield return new WaitForSeconds(20f);
        Managers.Pool.ReturnPool(this);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            Activate(player);
        }
    }
    public abstract void Activate(Player player);
}
