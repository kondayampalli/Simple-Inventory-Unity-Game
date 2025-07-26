using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5f, -7f);
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
