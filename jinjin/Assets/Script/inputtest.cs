using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]
public class inputtest : MonoBehaviour
{


   

    [HideInInspector]public float horizontal;
    [HideInInspector]public bool jumpHeld;
    [HideInInspector]public bool jumpPressed;
    [HideInInspector]public bool crouchHeld;
    [HideInInspector]public bool crouchPressed;
    [HideInInspector]public bool keypost;
    [HideInInspector] public bool pauesmenu;
    private bool readyToClear;

    private void Start()
    {
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;
        crouchHeld = false;
        crouchPressed = false;
        readyToClear = false;
        pauesmenu = false;
    }

    void Update()
    {
        //PauseMenu();
        ClearInput();
       
        ProcessInput();
        
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        
    }
    private void FixedUpdate()
    {
        readyToClear = true;
       
    }
    void ClearInput()
    {
        if (!readyToClear)
            return;
            pauesmenu = false;
            horizontal = 0f;
            jumpPressed = false;
            jumpHeld = false;
            crouchHeld = false;
            crouchPressed = false;
            readyToClear = false;
        
       
    }
    void ProcessInput()
    {
        horizontal += Input.GetAxis("Horizontal");  
        jumpPressed = Input.GetButtonDown("Jump");
        //jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButtonDown("Jump");
       // crouchPressed = crouchPressed || Input.GetButton("Crouch");
        //crouchHeld = crouchPressed || Input.GetButton("Crouch");
            
    }
    /*
    void PauseMenu()
    {
        pauesmenu = Input.GetKeyDown(KeyCode.Escape);
        
        
    }
    */
}
