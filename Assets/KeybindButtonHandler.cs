using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindButtonHandler : MonoBehaviour
{
    [SerializeField]
    private string _control; // The control this button corresponds to
    public string Control { get { return _control; } }

    public bool isAwaitingInput;

    public KeyCode binding;

    [SerializeField]
    private Text text;

    public KeybindMenuHandler menuHandler;

    private void Start()
    {
        LoadKeybindFromPlayerPrefs();
        UpdateText();
    }

    private void OnGUI()
    {
        if (isAwaitingInput && (Event.current.isKey || Event.current.shift))
        {
            binding = Event.current.keyCode;
            if (Event.current.shift)
            {
                binding = KeyCode.LeftShift;
            }
            isAwaitingInput = false;
            menuHandler.OnKeybindChanged(this);
            UpdateText();
        }
    }

    public void OnClick()
    {
        isAwaitingInput = true;
        menuHandler.OnKeybindButtonPressed(this);
        UpdateText();
    }

    private void LoadKeybindFromPlayerPrefs()
    {
        binding = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(_control, "None"));
    }

    public void UpdateText()
    {
        if (isAwaitingInput)
        {
            text.text = "Select New Keybind...";
        }
        else
        {
            text.text = _control.Substring(0, 1).ToUpper() + _control.Substring(1, _control.Length - 1) + ": "
                + binding.ToString();
        }
    }
}
