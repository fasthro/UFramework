using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    public static RTSCamera instance { get; private set; }

    public float followingSpeed = 30f;
    public Transform targetFollow;

    public bool isFollowingTarget => targetFollow != null;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        if (!isFollowingTarget) return;
        Vector3 targetPos = new Vector3(targetFollow.position.x, transform.position.y, targetFollow.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * followingSpeed);
    }
}