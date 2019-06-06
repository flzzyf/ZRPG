using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    Actor actor;

    public Actor target;

    //距离路径点多远算到达
    public float nextWaypointDistance = 1;

    Path path;
    int currentWaypointIndex;
    bool reachedEndOfPath;
    Seeker seeker;

    void Start()
    {
        actor = GetComponent<Actor>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("FindPath", 0f, .5f);
    }

    void FindPath()
    {
        if (!seeker.IsDone())
            return;

        seeker.StartPath(transform.position, target.transform.position + Vector3.up * .5f, HandleOnPathDelegate);
    }

    void HandleOnPathDelegate(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypointIndex = 0;

            reachedEndOfPath = false;
        }
    }


    void Update()
    {
        if (target == null)
            return;

        if (path == null)
            return;

        if(currentWaypointIndex >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;

            actor.Move(0);

            return;
        }

        Vector2 nextWaypointPos = path.vectorPath[currentWaypointIndex];
        Vector2 offset = nextWaypointPos - (Vector2)transform.position;
        Vector2 dir = offset.normalized;

        //移动
        actor.Move(Mathf.Sign(dir.x));

        if (offset.y > 2)
            actor.Jump();
        //(actor.controller2D.collisions.right || actor.controller2D.collisions.left)

        float distance = offset.magnitude;
        if(distance < nextWaypointDistance)
        {
            currentWaypointIndex++;
        }
    }
}
