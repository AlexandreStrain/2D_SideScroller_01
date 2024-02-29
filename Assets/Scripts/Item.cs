using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //The type of item - Health, Money, or Other?
    public ItemType _type;
    //How much of the item we wish to give to the player
    public float _amount;

    public AudioClip _pickupSound;
    //the item that interacts with the world
    private GameObject _itemSprite;

    private AudioSource _audioSource;
    private Rigidbody2D _rigidbody;
    private bool _isPickedUp; // has this item been picked up


    void Awake()
    {
        //Parent this Item to the "Items" GameObject within the scene
        //this helps to organize our Hierarchy
        transform.parent = GameObject.FindGameObjectWithTag(Tags.items).transform;
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        //one can also get a GameObject attached to the parent GameObject
        _itemSprite = transform.GetChild(0).gameObject;
        GameManager.Instance.OnToggleGamePause += TogglePhysics;
    }

    private void TogglePhysics()
    {
        _rigidbody.simulated = !(GameManager.Instance._isPaused);
    }

    //check for a Triggered Collision on the CircleCollider2D component
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //we find the Sprite Script on we collided with from its parent
        Sprite sprite = collision.GetComponentInParent<Sprite>();

        //determine sprite script is found (not null) AND what we hit is the player (by tag)
        if (sprite && sprite.gameObject.CompareTag(Tags.player))
        {
            //depending on the Item, decide what to give
            //We use a switch case here because it helps to clarify what item we have
            switch (_type)
            {
                case ItemType.Health:
                    //We make sure we don't give too much health by clamping the values
                    //between a minimum and a maximum health
                    sprite._currentHealth = Mathf.Clamp(sprite._currentHealth + _amount, 0f, sprite._stats._maxHealth);
                    break;
                case ItemType.Money:
                    //do something with money here involving GameManager...
                    GameManager.Instance._playerCoins += _amount;
                    break;
            }
            _audioSource.PlayOneShot(_pickupSound);
            _isPickedUp = true;
            //make sprite disappear as if collected
            _itemSprite.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.Instance._isPaused)
        {
            return;
        }
 
        //if the item has been picked up and the pickup sound has finished playing
        if (_isPickedUp && !_audioSource.isPlaying)
        {
            //Destroy or remove the gameobject this script is attached to in the scene
            Destroy(this.gameObject);
        }
    }
}
