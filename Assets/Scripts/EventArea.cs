using UnityEngine;
using UnityEngine.Events;

public class EventArea : MonoBehaviour
{
    public UnityEvent _allEvents;
    public UnityEvent _reset;

    private void Awake()
    {
        transform.parent = GameObject.FindGameObjectWithTag(Tags.events).transform;
    }

    //If Something runs into the Trigger on the EventArea's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*and if it collides with a sprite, and the script on its parent GameObject
         happens to be the player...*/
        if (collision.transform.parent.CompareTag(Tags.player))
        {
            //invoke or call all events listening to run
            _allEvents.Invoke();
        }
    }
}
