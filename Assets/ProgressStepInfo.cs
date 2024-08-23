using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ProgressStepData
{
    public Sprite greenCheckbox;
    public Sprite redCheckbox;
    public Image checkbox;

    public TextMeshProUGUI progressText;

}
public class ProgressStepInfo : MonoBehaviour
{
    public ProgressStepData ProgressStepData;


    public void SetCheckbox(bool isTrue)
    {
        if (isTrue)
            ProgressStepData.checkbox.sprite = ProgressStepData.greenCheckbox;
        else
            ProgressStepData.checkbox.sprite = ProgressStepData.redCheckbox;
    }

    public void SetProgressText(string text)
    {
        ProgressStepData.progressText.text = text;
    }

}
