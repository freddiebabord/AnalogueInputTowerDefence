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

    protected WorldSpaceCanvasScaler() { }

    protected override void OnEnable()
    {
        base.OnEnable();
        scalar = GetComponent<CanvasScaler>();
    }
    protected virtual void Update()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;

        GetComponent<RectTransform>().sizeDelta = new Vector2(width * scalar.scaleFactor, height * scalar.scaleFactor);

    }
    
}
