using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairCursor : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseCursorPosDef = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
        if((mouseCursorPosDef.y < 0 || mouseCursorPosDef.y > 1 || mouseCursorPosDef.x < 0 || mouseCursorPosDef.x > 1) || OptionSettings.GameisPaused == true)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Cursor.visible = true;
            sr.forceRenderingOff = true;
            //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
        else
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Cursor.visible = false;
            sr.forceRenderingOff = false;
        }
    }

}
