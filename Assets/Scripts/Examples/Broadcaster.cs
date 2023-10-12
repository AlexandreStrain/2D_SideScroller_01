using System.Collections.Generic;
using UnityEngine;

public class Broadcaster : Singleton<Broadcaster>
{
    //For example purposes, let's say the broadcaster has a list of their videos
    private List<Video> channel = new List<Video>();

    // Events and Delegates
    public delegate void AddVideoHandler(Video newVideo);
    public event AddVideoHandler OnUploadToWebsite;

    void Start()
    {
        PublishVideo();
    }

    void PublishVideo()
    {
        Video v = new Video("What are Events?");
        channel.Add(v);

        //Here we pass into our event the newest video being added to our channel
        //A broadcaster wants to notify their subscribers about the video, so an event gets called
        //upon uploading to the website 
        OnUploadToWebsite(v);
    }

}

public class Video
{
    public string name;

    public Video(string name)
    {
        this.name = name;
    }
}
