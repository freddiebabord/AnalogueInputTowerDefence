using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

    public CanvasGroup transitionCanvas;
    public float transitionDuration = 5.0f;
    bool transition = false;
    float startTranstionAlpha;
    bool wasEnabledOnStart;
    bool reverse_ = false;
    float transtionTimeEnd;

    public bool TransitionComplete
    {
        get { return transition; }
    }

	// Use this for initialization
	void Start () {
        startTranstionAlpha = transitionCanvas.alpha;
        wasEnabledOnStart = transitionCanvas.interactable;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (transition)
        {
            if (reverse_)
            {
                if (transitionCanvas.alpha > 0.0f)
                    transitionCanvas.alpha = Mathf.Lerp(1, 0, Mathf.Tan(Time.time / transtionTimeEnd));
                else
                {
                    SetCanvas(false);
                    transition = false;
                }
            }
            else
            {
                if (transitionCanvas.alpha < 1.0f)
                    transitionCanvas.alpha = Mathf.Lerp(0, 1, Mathf.Tan(Time.time / transitionDuration));
                else
                {
                    SetCanvas(true);
                    transition = false;
                }
            }
        }
	}

    public void StartTranstion(bool reverse = false)
    {
        transition = true;
        reverse_ = reverse;
        transtionTimeEnd = Time.time + transitionDuration;
    }

    public void StopTransition(bool reset = false)
    {
        if(reset)
        {
            transitionCanvas.alpha = startTranstionAlpha;
            SetCanvas(wasEnabledOnStart);
        }
    }

    void SetCanvas(bool val)
    {
        transitionCanvas.interactable = val;
        transitionCanvas.blocksRaycasts = val;
    }

	public IEnumerator Wait()
	{
		while (transition)
			yield return null;
	}
}
