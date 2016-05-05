using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    // Simple selectable object - derived from to create a control.
    [AddComponentMenu("UI/Analogue Selectable", 70)]
    [ExecuteInEditMode]
    [SelectionBase]
    [DisallowMultipleComponent]
    public class AnalogueUIElement : Selectable, ISelectHandler, IDeselectHandler
    {

        [HideInInspector]
        public bool isSelected = false;
        [HideInInspector]
        public bool clicked = false;

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        protected ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        // The input AXIS that acts as a click check
        public string clickAnalogueAxis = "TriggerSelectRight";

        // The amount of time in seconds to wait before OnClick can be called again
        protected float clickTimeout = 0.5f;

        // This checks if the analogue "button" has been released
        protected bool clickReset = true;

        // Checks to see if the button is selectedand the analogue click is pressed
        protected virtual void Update()
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
        public virtual void OnSelect()
        {
            if (this.interactable)
            {
                this.isSelected = true;
                this.DoStateTransition(SelectionState.Highlighted, false);
            }
        }

        // Called by Unitys UI Event System
        public override void OnSelect(BaseEventData eventData)
        {
            if (this.interactable)
            {
                this.isSelected = true;
                this.DoStateTransition(SelectionState.Highlighted, false);
            }
        }

        // Called through user created code
        public virtual void OnDeselect()
        {
            if (this.interactable)
            {
                this.isSelected = false;
                this.DoStateTransition(SelectionState.Normal, false);
            }
        }

        // Called by Unitys UI Event System
        public override void OnDeselect(BaseEventData data)
        {
            if (this.interactable)
            {
                this.isSelected = false;
                this.DoStateTransition(SelectionState.Normal, false);
            }
        }

        // Call the normal click handler through user created code
        public virtual void OnClick()
        {
            if (this.interactable)
            {
                if (!clicked)
                {
                    this.onClick.Invoke();
                    clickReset = false;
                    this.DoStateTransition(SelectionState.Pressed, false);
                    if (this.gameObject.activeInHierarchy)
                        StartCoroutine(clickTimeOut());
                }
            }
        }

        // Click sleep timer
        protected IEnumerator clickTimeOut()
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
}