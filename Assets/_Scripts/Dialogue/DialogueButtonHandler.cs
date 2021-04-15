using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI text;

    public DialogueOption option;
    
    public void UpdateButton(bool active, DialogueOption option = null)
    {
        this.option = option;
        button.interactable = active;
        if (option != null)
        {
            text.text = option.displayText;
        }
        else
        {
            text.text = "";
        }
    }

    // Called when the button is pressed, set up on the button component in the inspector
    public void OnPress()
    {
        if (option == null)
        {
            Debug.LogError("Dialogue Button pressed while not having an associated DialogueOption");
            return;
        }

        option.OnSelect();
    }
}
