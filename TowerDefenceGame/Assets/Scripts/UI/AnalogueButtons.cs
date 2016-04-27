/*
    Script: AnalogueButtons.cs
    Author: Frederic Babord
    This script is an extention of the Unity UI system that handles custom axis based
    analogue inputs instead of the generic mouse click functionality.
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/Analogue Button", 30)]
public class AnalogueButtons : Button, ISelectHandler, IDeselectHandler {

    [HideInInspector] public bool isSelected = false;
	[HideInInspector] public bool clicked = false;
    
    // The input AXIS that acts as a click check
    public string clickAnalogueAxis = "TriggerSelect";

    // The amount of time in seconds to wait before OnClick can be called again
	private float clickTimeout = 0.5f;

    // This checks if the analogue "button" has been released
    private bool clickReset = true;

    // Checks to see if the button is selectedand the analogue click is pressed
	void Update ()
    {
        if (this.interactable)
        {
            if (this.isSelected)
            {
                if (clickAnalogueAxis != "")
                {
                    if (Input.GetAxis(clickAnalogueAxis) >= 1 && clickReset)
                        this.OnClick();
                    if (Input.GetAxis(clickAnalogueAxis) <= 0 && !clickReset)
                        clickReset = false;
                }
            }
        }
        else if (this.currentSelectionState != SelectionState.Disabled)
            this.DoStateTransition(SelectionState.Disabled, false);
	}

    // Called through user created code when the button is 
    public void OnSelect()
    {
        if (this.interactable)
        {
            this.isSelected = true;
            this.DoStateTransition(SelectionState.Highlighted, false);
        }
    }

    // Called by Unitys UI Event System
    private void OnSelect(BaseEventData eventData) 
	{
        if (this.interactable)
        {
            this.isSelected = true;
            this.DoStateTransition(SelectionState.Highlighted, false);
        }
	}

    // Called through user created code
    public void OnDeselect()
    {
        if (this.interactable)
        {
            this.isSelected = false;
            this.DoStateTransition(SelectionState.Normal, false);
        }
    }

    // Called by Unitys UI Event System
    private void OnDeselect(BaseEventData data) 
	{
        if (this.interactable)
        {
            this.isSelected = false;
            this.DoStateTransition(SelectionState.Normal, false);
        }
	}

    // Call the normal click handler through user created code
    public void OnClick()
	{
        if (this.interactable)
        {
            if (!clicked)
            {
                this.onClick.Invoke();
                clickReset = false;
                this.DoStateTransition(SelectionState.Pressed, false);
                StartCoroutine(clickTimeOut());
            }
        }        
	}

    // Click sleep timer
    private IEnumerator clickTimeOut()
	{
        if (this.interactable)
        {
            this.clicked = true;
            yield return new WaitForSeconds(clickTimeout);
            OnDeselect();
            this.clicked = false;
        }
	}

}
