using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairCursor : MonoBehaviour
{
    //[SerializeField] private Texture2D[] cursorTextureArray;
    //[SerializeField] private int frameCount;
    //[SerializeField] private float frameRate;

    //private int currentFrame;
    //private float frameTimer;

    private void Awake()
    {
        Cursor.visible = false;
    }

    //private void Start()
    //{
    //    Cursor.SetCursor(cursorTextureArray[0], new Vector2(10, 10), CursorMode.Auto);
    //}

    // Update is called once per frame
    private void Update()
    {
        //frameTimer -= Time.deltaTime;
        //if (frameTimer <= 0f) {
        //    frameTimer += frameRate;
        //    currentFrame = (currentFrame + 1) % frameCount;
        //    Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(10, 10), CursorMode.Auto);
        //}

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.forceRenderingOff = true;
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseCursorPosDef = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
        if((mouseCursorPosDef.y < 0 || mouseCursorPosDef.y > 1 || mouseCursorPosDef.x < 0 || mouseCursorPosDef.x > 1) || OptionSettings.GameisPaused == true)
        {

            Cursor.visible = true;
            sr.forceRenderingOff = true;
            //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
        else
        {
            Cursor.visible = false;
            //if(Input.GetKey(KeyCode.Mouse1))
            sr.forceRenderingOff = false;
        }
    }

}
