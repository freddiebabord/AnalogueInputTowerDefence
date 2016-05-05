using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Canvas))]
[ExecuteInEditMode]
[AddComponentMenu("Layout/Canvas Scaler", 101)]
public class WorldSpaceCanvasScaler : UIBehaviour
{
    float height, width;
    CanvasScaler scalar;
    Pointer pointer;

    protected WorldSpaceCanvasScaler() { }

    protected override void OnEnable()
    {
        base.OnEnable();
        scalar = GetComponent<CanvasScaler>();
    }
    public virtual void UpdateSize()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        GetComponent<RectTransform>().sizeDelta = new Vector2(width * scalar.scaleFactor, height * scalar.scaleFactor);
        if (pointer == null)
            GameObject.FindObjectOfType<Pointer>();
        else
        {
            pointer.maxHorizontal = width * scalar.scaleFactor;
            pointer.maxVertical = height * scalar.scaleFactor;
        }
    }
    
}
