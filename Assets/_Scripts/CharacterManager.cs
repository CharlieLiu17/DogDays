using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

// Singelton component for managing characters and input.
// Input is the main thing that belongs here.
public class CharacterManager : MonoBehaviour
{
    // This code allows us to access CharacterManager's methods and data by using CharacterManager.Instance.<method/data name> from anywhere
    #region Singleton Code
    private static CharacterManager _instance;

    public static CharacterManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Attempted to Instantiate multiple CharacterManagers in one scene!");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (this == _instance) { _instance = null; }
    }
    #endregion

    [SerializeField]
    private GameObject thirdPersonCamera = null;
    [SerializeField]
    private Transform kobeFollowTarget = null;
    [SerializeField]
    private ThirdPersonMovement kobeMovement = null;
    [SerializeField]
    private Transform kaliFollowTarget = null;
    [SerializeField]
    private ThirdPersonMovement kaliMovement = null;
    [SerializeField]
    private CinemachineFreeLook freeLookScript = null;

    private void Start()
    {
        GameManager.Instance.CursorLocked = true; // This also makes the cursor invisible
    }

    // Update is called once per frame
    void Update()
    {
        // Switch dogs when F is pressed
        if(Input.GetKeyDown(KeyCode.F)) {
            if (freeLookScript.m_Follow  == kobeFollowTarget)
            {
                freeLookScript.m_Follow = kaliFollowTarget;
                freeLookScript.m_LookAt = kaliFollowTarget;
                kaliMovement.enabled = true;
                kobeMovement.enabled = false;
            } else
            {
                freeLookScript.m_Follow = kobeFollowTarget;
                freeLookScript.m_LookAt = kobeFollowTarget;
                kobeMovement.enabled = true;
                kaliMovement.enabled = false;
            }
        }
        // Restart when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
