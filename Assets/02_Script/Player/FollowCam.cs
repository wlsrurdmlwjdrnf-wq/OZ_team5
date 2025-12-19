using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 0.125f;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, speed);
    }
}