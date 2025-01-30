using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float platformSpeed = 1f;

    
    void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time * platformSpeed, 1);
        transform.position = Vector3.Lerp(point1.position, point2.position, t);
    }
}
