using UnityEngine;
using System.Collections.Generic;

public class PlatformMovement : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] List<Transform> points = new List<Transform>();
    [SerializeField] float platformSpeed;
    [SerializeField] LoopType loopType = LoopType.Normal;

    int currentPointIndex = 0;
    int nextPointIndex = 1;
    bool isReversing;

    void OnDrawGizmos()
    {
        if (points == null || points.Count < 2)
        {
            return;
        }

        Gizmos.color = Color.green;
        if (loopType == LoopType.Normal || loopType == LoopType.PingPong)
        {
            for (int i = 0; i < points.Count - 1; ++i)
            {
                if (points[i] != null && points[i + 1] != null)
                {
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }
            }
        }
        else if (loopType == LoopType.Random)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    if (points[i] != null && points[j] != null)
                    {
                        Gizmos.DrawLine(points[i].position, points[j].position);
                    }
                }
            }
        }


        if ((loopType == LoopType.Normal) && points[0] != null && points[points.Count - 1] != null)
        {
            Gizmos.DrawLine(points[points.Count - 1].position, points[0].position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if (points.Count < 2)
        {
            return;
        }

        Vector3 targetPosition = points[nextPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, platformSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            SwitchToNextPoint();
        }
    }

    private void SwitchToNextPoint()
    {
        switch (loopType)
        {
            case LoopType.Normal:
                HandleNormalLoop();
                break;

            case LoopType.PingPong:
                HandlePingPongLoop();
                break;

            case LoopType.Random:
                HandleRandomLoop();
                break;
        }
    }

    private void HandleNormalLoop()
    {
        currentPointIndex = nextPointIndex;
        nextPointIndex = (nextPointIndex + 1) % points.Count;
    }

    private void HandlePingPongLoop()
    {
        if (isReversing)
        {
            --nextPointIndex;

            if (nextPointIndex <= 0)
            {
                isReversing = false;
            }
        }
        else
        {
            ++nextPointIndex;

            if (nextPointIndex >= points.Count - 1)
            {
                isReversing = true;
            }
        }
    }

    private void HandleRandomLoop()
    {
        int newIndex;

        do
        {
            newIndex = Random.Range(0, points.Count);
        } while (newIndex == currentPointIndex);

        currentPointIndex = nextPointIndex;
        nextPointIndex = newIndex;
    }
}