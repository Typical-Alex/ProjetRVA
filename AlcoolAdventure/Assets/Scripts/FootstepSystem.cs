using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootstepSystem : MonoBehaviour {

    [Range(0,20f)]
    public float frequency = 10.0f;
    public UnityEvent onFootStep;
    
    float Sin;
    
    bool isTriggered = false;
    private FirstPersonController playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }
    // Update is called once per frame
    void Update() {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")).magnitude;
        
        if (inputMagnitude > 0 & playerMovement.CanMove & !playerMovement.Paused & !playerMovement.Transition) {
        	StartFootsteps();
        } 
    }
    
    private void StartFootsteps() {
    	Sin = Mathf.Sin(Time.time * frequency);
    	
    	if (Sin > 0.97f && isTriggered == false) {
    		isTriggered = true;
    		onFootStep.Invoke();
    	} else if (isTriggered == true && Sin < -0.97f) {
    		isTriggered = false;
    	}
    }
}
