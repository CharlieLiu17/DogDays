using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Put this on a button used for changing controls and set it up - it should handle the rest.
public class ControlChangeButtonHandler : MonoBehaviour
{
    // You can change the type to TextMeshProUGUI if you want to use a TMP text component instead
    [SerializeField]
    private Text text;

    // What key this button affects. A full list can be found in Reference.
    [SerializeField]
    private string keyMapping;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
