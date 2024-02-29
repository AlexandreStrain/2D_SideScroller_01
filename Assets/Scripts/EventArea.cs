using UnityEngine;
using UnityEngine.Events;

public class EventArea : MonoBehaviour
{
    public UnityEvent _allEvents;
    public UnityEvent _reset;

    private void Awake()
    {
        ParentToEventsGameObjectInScene();
    }

    private void ParentToEventsGameObjectInScene()
    {
        transform.parent = GameObject.FindGameObjectWithTag(Tags.events).transform;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CallAllEvents(collision);
    }

    private void CallAllEvents(Collider2D collisionObject)
    {
        if (CollisionObjectIsPlayer(collisionObject))
        {
            _allEvents.Invoke();
        }
    }

    private bool CollisionObjectIsPlayer(Collider2D collisionObject)
    {
        return collisionObject.transform.parent.CompareTag(Tags.player);
    }
}
