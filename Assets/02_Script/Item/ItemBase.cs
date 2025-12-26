using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected WaitForSeconds duration = new WaitForSeconds(20f);
    protected void OnEnable()
    {
        StartCoroutine(Duration());
    }
    protected IEnumerator Duration()
    {
        yield return duration;
        Managers.Instance.Pool.ReturnPool(this);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            Activate(player);
        }
    }
    public abstract void Activate(Player player);
}
