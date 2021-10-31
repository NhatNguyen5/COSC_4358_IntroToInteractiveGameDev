using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using System.Linq;

public class ThymusScript : MonoBehaviour
{
    private Transform Thymus;
    private Transform DialogBox;
    private Text DialogText;
    private TextWriter.TextWriterSingle textWriterSingle;
    private bool dialogSequenceFinish = false;
    private int currDialogIdx = 0;
    private float timer = 0;
    private bool waitNext = false;
    private bool isShown = true;
    private float zoom = 0;
    private string[] currEyesSprite;
    private string[] currBrowsSprite;

    [Header("Thymus")]
    [Range(-500, 500)]
    public float ThymusAppearPosX;
    [Range(-300, 300)]
    public float ThymusAppearPosY;
    public bool Appear;
    [Range(1, 5)]
    public float zoomSpeed;

    [SerializeField]
    private SpriteLibrary ThymusSpriteLibrary;
    [SerializeField]
    private SpriteResolver ThymusEyesSpriteResolver;
    [SerializeField]
    private SpriteResolver ThymusBrowsSpriteResolver;

    private Vector3 ThymusOgScale;
    private Vector3 DialogBoxOgScale;

    [System.Serializable]
    public struct ThymusDialog
    {
        [Header("General Setting")]
        public float WaitTimeAfterFinish;
        public bool EnablePlayerControl;
        public bool EnableAI;
        public bool HideThymus;
        public float ShowAgainAfter;
        [Header("Thymus")]
        [Range(-500, 500)]
        public float ThymusPositionX;
        [Range(-250, 250)]
        public float ThymusPositionY;
        [Range(0.1f, 3)]
        public float ThymusScale;
        [StringInList("SeriousEyes", "ChillEyes")] public string EyesCategory;
        [StringInList("Forward", "Down", "Up", "Left", "Right", "TopLeft", "TopRight", "BotLeft", "BotRight")] public string EyesDirection;
        [StringInList("Normal", "RaiseBoth", "RaiseLeft", "RaiseRight", "Angry", "Sad")] public string BrowsExpression;

        [Header("DialogBox")]
        [Range(-500, 500)]
        public float DialogBoxPositionX;
        [Range(-250, 250)]
        public float DialogBoxPositionY;
        [Range(0.1f, 3)]
        public float DialogBoxScale;
        public string DialogContent;
        [Range(0, 50)]
        public float FontSize;
        [Range(1, 20)]
        public float DialogSpeed;
    }

    public ThymusDialog[] ThymusDialogSequence;

    // Start is called before the first frame update
    private void Awake()
    {
        Thymus = transform.Find("ThymusHimself");
        ThymusOgScale = Thymus.localScale;
        DialogBox = transform.Find("DialogBox");
        DialogBoxOgScale = DialogBox.localScale;
        DialogText = DialogBox.Find("Dialog").GetComponent<Text>();
        currEyesSprite = ThymusSpriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(ThymusDialogSequence[0].EyesCategory).ToArray();
        currBrowsSprite = ThymusSpriteLibrary.spriteLibraryAsset.GetCategoryLabelNames("EyeBrows").ToArray();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!Appear && isShown)
        {
            if (zoom > 0)
            {
                zoom -= Time.deltaTime * zoomSpeed;
                transform.localPosition = new Vector3(ThymusAppearPosX * (1 - zoom), ThymusAppearPosY * (1 - zoom), -1);
                transform.localScale = new Vector3(zoom, zoom, 1);
            }
            else
            {
                zoom = 0;
                transform.localScale = new Vector3(zoom, zoom, 1);
            }

            if (zoom == 0)
            {
                Thymus.gameObject.SetActive(false);
                DialogBox.gameObject.SetActive(false);
                isShown = false;
            }

            if(GlobalPlayerVariables.EnablePlayerControl == false || GlobalPlayerVariables.EnableAI == false)
            {
                GlobalPlayerVariables.EnablePlayerControl = true;
                GlobalPlayerVariables.EnableAI = true;
            }
        }


        if(Appear)
        {
            if (zoom == 0)
            {
                Thymus.localPosition = new Vector3(ThymusDialogSequence[currDialogIdx].ThymusPositionX, ThymusDialogSequence[currDialogIdx].ThymusPositionY);
                DialogBox.localPosition = new Vector3(ThymusDialogSequence[currDialogIdx].DialogBoxPositionX, ThymusDialogSequence[currDialogIdx].DialogBoxPositionY);
                Thymus.localScale = new Vector3(ThymusOgScale.x * ThymusDialogSequence[currDialogIdx].ThymusScale, ThymusOgScale.y * ThymusDialogSequence[currDialogIdx].ThymusScale, ThymusOgScale.z);
                DialogBox.localScale = new Vector3(DialogBoxOgScale.x * ThymusDialogSequence[currDialogIdx].DialogBoxScale, DialogBoxOgScale.y * ThymusDialogSequence[currDialogIdx].DialogBoxScale, DialogBoxOgScale.z);
            }

            if (zoom < 1)
            {
                if(textWriterSingle != null)
                    textWriterSingle.Stop();
                zoom += Time.deltaTime * zoomSpeed;
                transform.localPosition = new Vector3(ThymusAppearPosX * (1 - zoom), ThymusAppearPosY * (1 - zoom), -1);
                transform.localScale = new Vector3(zoom, zoom, 1);
            }
            else
            {
                zoom = 1;
                if (textWriterSingle != null)
                    textWriterSingle.Play();
                transform.localScale = new Vector3(zoom, zoom, 1);
            }

            if (!isShown)
            {
                Thymus.gameObject.SetActive(true);
                DialogBox.gameObject.SetActive(true);
                isShown = true;
            }

            if (zoom == 1)
            {
                if (currDialogIdx == ThymusDialogSequence.Length && !textWriterSingle.IsActive())
                    dialogSequenceFinish = true;

                if (timer > 0)
                    timer -= Time.deltaTime;

                if (textWriterSingle != null && timer <= 0 && waitNext)
                    if (!textWriterSingle.IsActive())
                    {
                        timer = ThymusDialogSequence[currDialogIdx - 1].WaitTimeAfterFinish;
                        waitNext = false;
                    }

                if (textWriterSingle == null && !dialogSequenceFinish && Appear)
                {
                    ThymusDialogPlay(ThymusDialogSequence[currDialogIdx]);
                    waitNext = true;
                }
                else if (!textWriterSingle.IsActive() && !dialogSequenceFinish && timer <= 0)
                {
                    ThymusDialogPlay(ThymusDialogSequence[currDialogIdx]);
                    waitNext = true;
                }

                if (dialogSequenceFinish && timer <= 0)
                    Appear = false;

                //Debug.Log(timer);
                //Debug.Log(currDialogIdx + " " + textWriterSingle.IsActive());


                if (Input.GetKeyDown(KeyCode.Mouse0) && (!dialogSequenceFinish || timer > 0) && textWriterSingle != null)
                {
                    //Debug.Log("Push Button");
                    if (textWriterSingle.IsActive() && waitNext)
                    {
                        textWriterSingle.WriteAllAndDestroy();
                        timer = ThymusDialogSequence[currDialogIdx - 1].WaitTimeAfterFinish;
                        waitNext = false;
                    }
                    else
                    {
                        timer = 0;
                    }
                    if (dialogSequenceFinish)
                        Appear = false;
                }
                //Debug.Log("dialogSequenceFinish " + dialogSequenceFinish);
            }
        }
    }

    private void ThymusDialogPlay(ThymusDialog Dialog)
    {
        int tempEyesIdx = 0;
        int tempBrowsIdx = 0;
        switch (Dialog.EyesDirection)
        {
            case "Forward":
                tempEyesIdx = 0;
                break;
            case "Left":
                tempEyesIdx = 1;
                break;
            case "Right":
                tempEyesIdx = 2;
                break;
            case "Down":
                tempEyesIdx = 3;
                break;
            case "TopLeft":
                tempEyesIdx = 4;
                break;
            case "TopRight":
                tempEyesIdx = 5;
                break;
            case "BotLeft":
                tempEyesIdx = 6;
                break;
            case "BotRight":
                tempEyesIdx = 7;
                break;
            case "Up":
                tempEyesIdx = 8;
                break;

        }

        switch (Dialog.BrowsExpression)
        {
            case "Normal":
                tempBrowsIdx = 0;
                break;
            case "RaiseBoth":
                tempBrowsIdx = 1;
                break;
            case "RaiseLeft":
                tempBrowsIdx = 2;
                break;
            case "RaiseRight":
                tempBrowsIdx = 3;
                break;
            case "Sad":
                tempBrowsIdx = 4;
                break;
            case "Angry":
                tempBrowsIdx = 5;
                break;
        }

        textWriterSingle = TextWriter.AddWriter_static(DialogText, Dialog.DialogContent + " ", 1 / Dialog.DialogSpeed, true, true);

        GlobalPlayerVariables.EnablePlayerControl = ThymusDialogSequence[currDialogIdx].EnablePlayerControl;
        GlobalPlayerVariables.EnableAI = ThymusDialogSequence[currDialogIdx].EnableAI;

        if (Dialog.HideThymus)
        {
            StartCoroutine(ShowThymusAfter(Dialog.ShowAgainAfter));
        }

        Debug.Log(tempEyesIdx + " " + tempBrowsIdx);
        ThymusEyesSpriteResolver.SetCategoryAndLabel(Dialog.EyesCategory, currEyesSprite[tempEyesIdx]);
        ThymusBrowsSpriteResolver.SetCategoryAndLabel("EyeBrows", currBrowsSprite[tempBrowsIdx]);
        DialogText.fontSize = (int)(Dialog.FontSize);
        Thymus.localPosition = new Vector3(Dialog.ThymusPositionX, Dialog.ThymusPositionY);
        Thymus.localScale = new Vector3(ThymusOgScale.x * Dialog.ThymusScale, ThymusOgScale.y * Dialog.ThymusScale, ThymusOgScale.z);
        DialogBox.localPosition = new Vector3(Dialog.DialogBoxPositionX, Dialog.DialogBoxPositionY);
        DialogBox.localScale = new Vector3(DialogBoxOgScale.x * Dialog.DialogBoxScale, DialogBoxOgScale.y * Dialog.DialogBoxScale, DialogBoxOgScale.z);
        
        //DialogText.text = Dialog.DialogContent;
        currDialogIdx++;
    }

    private IEnumerator ShowThymusAfter(float dur)
    {
        Appear = false;
        textWriterSingle.Stop();
        yield return new WaitForSeconds(dur);
        GlobalPlayerVariables.EnablePlayerControl = ThymusDialogSequence[currDialogIdx].EnablePlayerControl;
        GlobalPlayerVariables.EnableAI = ThymusDialogSequence[currDialogIdx].EnableAI;
        Appear = true;
        //textWriterSingle.Play();
    }
}
