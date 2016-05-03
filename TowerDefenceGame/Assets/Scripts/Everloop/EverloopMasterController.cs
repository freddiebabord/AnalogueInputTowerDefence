using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class EverloopMasterController : MonoBehaviour {

    public List<EverloopTheme> everloops = new List<EverloopTheme>();
    public EverloopTheme currentLoop;

    void Start()
    {
        if(everloops.Count > 0)
        {
            foreach (EverloopTheme loop in everloops)
            {
                loop.everloopManager.StopAll();
            }
            currentLoop = everloops[0];
            currentLoop.everloopManager.volume = 1.0f;
            currentLoop.everloopManager.StartAutopilot();
        }
    }

	public void ChangeLoopBasedOnTheme(EverloopTheme.Theme targetTheme)
    {
        foreach (EverloopTheme loop in everloops)
        {
            if(loop.theme == targetTheme)
            {
                currentLoop.everloopManager.StopAll(5.0f);
                currentLoop = loop;
                currentLoop.everloopManager.StartAutopilot();
            }
        }
    }
}

[System.Serializable]
public struct EverloopTheme
{
    public enum Theme
    {
        Normal,
        Tense,
        Death
    }
    public EverloopController everloopManager;
    public Theme theme;
};
