using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    private Transform _cameraTransform;

    private Vector3 _previousCameraPosition;
    [SerializeField] private bool _infiniteScrollingX;
    [SerializeField] private bool _infiniteScrollingY;

    [SerializeField] private Vector2 _scrollingSpeed;

    //The scrolling scales with the size of the background image
    private Vector2 _textureSize;

    void Start()
    {
        SetCameraVariables();
        SetTextureSize();
    }

    private void SetCameraVariables()
    {
        _cameraTransform = Camera.main.transform;
        _previousCameraPosition = _cameraTransform.position;
    }

    private void SetTextureSize()
    {
        //As we need to scroll the background, we need to know which sprite&image we are working with
        UnityEngine.Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        //how far in the sprite do we have to move before we begin to scroll
        _textureSize.x = texture.width * transform.localScale.x / sprite.pixelsPerUnit;
        _textureSize.y = texture.height * transform.localScale.y / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        ScrollBackground();


        //if we enable infinite scrolling in the X direction
        if (_infiniteScrollingX)
        {
            //calculate if the distance travelled exceeds the image size
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureSize.x)
            {
                //shift the image over by an offset so the image appears endless
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureSize.x;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y, 0f);
            }
        }

        if (_infiniteScrollingY)
        {
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureSize.y)
            {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureSize.y;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY, 0f);
            }
        }
    }


    private void ScrollBackground()
    {
        //how far did we travel from the last frame?
        Vector3 deltaMovement = _cameraTransform.position - _previousCameraPosition;

        //Scroll background based on speed
        transform.position += new Vector3(deltaMovement.x * _scrollingSpeed.x, deltaMovement.y * _scrollingSpeed.y, 0f);

        _previousCameraPosition = _cameraTransform.position;
    }
}
