using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Haunted Harbour/Level Information")]
public class LevelInformation : ScriptableObject
{
    [Header("Audio")]
    public AudioClip _defaultMusic;
    public AudioClip _bossMusic;


    [Header("Player Information")]
    public Vector2 _playerStartLocation;
}
