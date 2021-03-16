using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public Transform kobeFollowTarget;
    public ThirdPersonMovement kobeMovement;
    public Transform kaliFollowTarget;
    public ThirdPersonMovement kaliMovement;

    private CinemachineFreeLook freeLookScript;

    private void Start()
    {
        freeLookScript = thirdPersonCamera.GetComponent<CinemachineFreeLook>();
        GameManager.Instance.CursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
