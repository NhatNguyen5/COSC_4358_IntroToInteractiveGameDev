using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using TMPro;

public class ThymusScript : MonoBehaviour
{
    private Transform Thymus;
    private Transform DialogBox;
    private TextMeshProUGUI DialogText;
    private bool dialogSequenceFinish = false;
    private int currDialogIdx = 0;
    private int newDialogIdx = 1;
    private TextWriter.TextWriterSingle textWriterSingle;

    [Header("Thymus")]
    [SerializeField]
    private SpriteLibraryAsset ThymusSpriteLibrary;
    [SerializeField]
    private SpriteResolver ThymusEyesSpriteResolver;
    [SerializeField]
    private SpriteResolver ThymusBrowsSpriteResolver;
    
    [System.Serializable]
    public struct ThymusDialog
    {
        [Header("General Setting")]
        public float DialogDuration;
        [Header("Thymus")]
        public Vector2 ThymusPosition;
        [StringInList("SeriousEyes", "ChillEyes")] public string EyesCategory;
        [StringInList("Forward", "Down", "Left", "Right")] public string EyesDirection;
        [StringInList("Normal", "RaiseLeft", "Angry", "Sad")] public string BrowsExpression;

        [Header("DialogBox")]
        public Vector2 DialogBoxPosition;
        public string DialogContent;
        public float DialogSpeed;
    }

    public ThymusDialog[] ThymusDialogSequence;

    // Start is called before the first frame update
    private void Awake()
    {
        Thymus = transform.Find("ThymusHimself");
        DialogBox = transform.Find("DialogBox");
        DialogText = DialogBox.Find("Dialog").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Push Button");
            if (textWriterSingle != null && textWriterSingle.IsActive())
            {
                textWriterSingle.WriteAllAndDestroy();
                currDialogIdx++; 
                newDialogIdx++;
            }
        }

        Debug.Log(currDialogIdx + " " + newDialogIdx);
        if (textWriterSingle == null && !dialogSequenceFinish)
        {
            ThymusDialogPlay(ThymusDialogSequence[currDialogIdx]);
            currDialogIdx += 1;
        }
        else if(currDialogIdx != newDialogIdx && !textWriterSingle.IsActive())
        {
            ThymusDialogPlay(ThymusDialogSequence[currDialogIdx]);
            currDialogIdx += 1;
        }
        
        if (newDialogIdx < ThymusDialogSequence.Length && !textWriterSingle.IsActive())
        {
            newDialogIdx += 1;
        }
        else
            dialogSequenceFinish = true;

        Debug.Log(currDialogIdx + " " + newDialogIdx + " " + textWriterSingle.IsActive());
    }

    private void ThymusDialogPlay(ThymusDialog Dialog)
    {
        Thymus.localPosition = Dialog.ThymusPosition;
        DialogBox.localPosition = Dialog.DialogBoxPosition;
        textWriterSingle = TextWriter.AddWriter_static(DialogText, Dialog.DialogContent, 0.2f, true, true);
        DialogText.text = Dialog.DialogContent;
    }
}
