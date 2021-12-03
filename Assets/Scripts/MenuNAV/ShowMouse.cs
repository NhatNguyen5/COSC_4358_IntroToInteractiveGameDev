using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMouse : MonoBehaviour
{
    // Start is called before the first frame update

    public Image ForeGround;
    private Color FGC;

    private void Start()
    {
        Cursor.visible = true;
        GlobalPlayerVariables.GameOver = false;
        RememberLoadout.penalty = false;
    }

    private void Update()
    {
        FGC = ForeGround.color;
        if(FGC != null)
        {
            if (FGC.a <= 0)
            {
                Destroy(ForeGround);
            }
            else
            {
                FGC.a -= Time.deltaTime;
                ForeGround.color = FGC;
            }
        }
    }

}
