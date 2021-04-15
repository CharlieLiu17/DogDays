using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindMenuHandler : MonoBehaviour
{
    [SerializeField]
    private KeybindButtonHandler[] keybindButtons;

    private void Start()
    {
        keybindButtons = GetComponentsInChildren<KeybindButtonHandler>();
        foreach (KeybindButtonHandler button in keybindButtons)
        {
            button.menuHandler = this;
        }
    }

    public void OnKeybindButtonPressed(KeybindButtonHandler handler)
    {
        foreach (KeybindButtonHandler button in keybindButtons)
        {
            // Only one button can be waiting for a new binding at a time
            if (button != handler)
            {
                button.isAwaitingInput = false;
                button.UpdateText();
            }
        }
    }

    public void OnKeybindChanged(KeybindButtonHandler handler)
    {
        // Each key can only be bound to one control
        foreach (KeybindButtonHandler button in keybindButtons)
        {
            if (button != handler && button.binding == handler.binding)
            {
                button.binding = KeyCode.None;
                button.UpdateText();
            }
        }
        WriteControlsToPlayerPrefs();
        Reference.Instance.GetControlsFromPlayerPrefs();
    }

    public void WriteControlsToPlayerPrefs()
    {
        foreach (KeybindButtonHandler button in keybindButtons)
        {
            PlayerPrefs.SetString(button.Control, button.binding.ToString());
        }
    }
}