using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subscriber : MonoBehaviour
{

    // Awake is called before the first frame update
    void Awake()
    {
        //We are communicating to Broadcaster that when the OnUploadToWebsite() event occurs,
        // we want the WatchVideo() method to be subscribed to it
        // in other terms, as soon as the video is uploaded, the subscriber gets notified
        Broadcaster.Instance.OnUploadToWebsite += WatchVideo;
    }

    //Our method that runs every time our OnUploadToWebsite() Event from Broadcaster gets called
    private void WatchVideo(Video v)
    {
        Debug.Log($"{gameObject.name} is Watching video: '{v.name}' from their favorite broadcaster...");
    }
}
