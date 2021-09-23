using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;
    private Vector3 targetposition;
    private RectTransform pointerRectTransform;
    public RectTransform ArrowTransform;
    public float ArrowSize;
    public Player player;
    public float BorderSize;

    private void Awake()
    {
        
    }

    private void Update()
    {
        targetposition = player.Stats.Position;
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        Vector3 toPosition = targetposition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        float angle = player.Stats.Angle;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetposition);
        bool isOffScreen = targetPositionScreenPoint.x < BorderSize || 
            targetPositionScreenPoint.x > Screen.width - BorderSize || 
            targetPositionScreenPoint.y < BorderSize || 
            targetPositionScreenPoint.y > Screen.height - BorderSize;
        //Debug.Log(player.Stats.Position);
        //Debug.Log(ArrowTransform.position);
        //Debug.Log(targetPositionScreenPoint);

        if (isOffScreen && OptionSettings.GameisPaused == false)
        {
            ArrowTransform.localScale = new Vector3(ArrowSize, ArrowSize, 1);
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= BorderSize)
            {
                cappedTargetScreenPosition.y -= (- BorderSize + cappedTargetScreenPosition.x) * Mathf.Tan(angle / Mathf.Rad2Deg);
                cappedTargetScreenPosition.x = BorderSize;
            }
            if (cappedTargetScreenPosition.x >= Screen.width - BorderSize)
            {
                cappedTargetScreenPosition.y += (- BorderSize + Screen.width - cappedTargetScreenPosition.x) * Mathf.Tan(angle / Mathf.Rad2Deg);
                cappedTargetScreenPosition.x = Screen.width - BorderSize;
            }
            //Debug.Log(Mathf.Tan(angle / Mathf.Rad2Deg));


            if (cappedTargetScreenPosition.y <= BorderSize)
            {
                cappedTargetScreenPosition.x -= (-BorderSize + cappedTargetScreenPosition.y) / Mathf.Tan(angle / Mathf.Rad2Deg);
                cappedTargetScreenPosition.y = BorderSize;
            }
            if (cappedTargetScreenPosition.y >= Screen.height - BorderSize)
            {
                cappedTargetScreenPosition.x += (- BorderSize + Screen.height - cappedTargetScreenPosition.y) / Mathf.Tan(angle / Mathf.Rad2Deg);
                cappedTargetScreenPosition.y = Screen.height - BorderSize;
            }
            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y);
        }
        else
        {
            ArrowTransform.localScale = new Vector3(ArrowSize, ArrowSize, 0);
        }
    }
}
