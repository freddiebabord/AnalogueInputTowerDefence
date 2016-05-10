using UnityEngine;
using System.Collections;

public class AnalogueInput : MonoBehaviour {

    public static bool invertHorisontal = false;
    public static bool invertVertical = false;

    public static float GetLeftTrigger()
    {
        return Input.GetAxis("TriggerSelectLeft");
    }

    public static float GetRightTrigger()
    {
        return Input.GetAxis("TriggerSelectRight");
    }

    public static float GetLeftHorizontal()
    {
        return invertHorisontal ? -Input.GetAxis("HorizontalLeft") : Input.GetAxis("HorizontalLeft");
    }

    public static float GetRightHorizontal()
    {
        return invertHorisontal ? -Input.GetAxis("HorizontalRight") : Input.GetAxis("HorizontalRight");
    }

    public static float GetLeftVertical()
    {
        return invertHorisontal ? -Input.GetAxis("VerticalLeft") : Input.GetAxis("VerticalLeft");
    }

    public static float GetRightVertical()
    {
        return invertHorisontal ? -Input.GetAxis("VerticalRight") : Input.GetAxis("VerticalRight");
    }
}
