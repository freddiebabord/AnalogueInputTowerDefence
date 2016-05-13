using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TowerPlacement : MonoBehaviour {

	public bool isTrue = false;
    public Animation m_animation;

	GameObject panel;
    bool isPlaying = false;
    bool done = false;
    List<AnalogueButtons> listButtons;

	void Start()
	{
		panel = GameObject.FindGameObjectWithTag ("TowerSelect");
        AnalogueButtons[] buttons = GetComponentsInChildren<AnalogueButtons>();
        listButtons = buttons.ToList();
        listButtons.Remove(GetComponent<AnalogueButtons>());
	}

	void Update()
	{
        if (isTrue && !done)
        {
            ShowPanel();
        }
        else if (!isPlaying && !done)
        {
            HidePanel();
        }
        
	}

    public void HidePanel()
    {
        isTrue = false;
        done = false;
        if (m_animation)
            m_animation.Play();
        StartCoroutine(waitForAnimComplete(false));
        done = true;
    }

    public void ShowPanel()
    {
        isTrue = false;
        done = false;
        StartCoroutine(waitForAnimComplete(true));
        done = true;
    }


    IEnumerator WaitForAnimOpencomplete()
    {
        while (m_animation.isPlaying)
            yield return null;
    }


	public void Placement()
	{
		isTrue = !isTrue;
        done = false;
	}

    IEnumerator waitForAnimComplete(bool enable_)
    {
        isPlaying = true;
        foreach (var b in listButtons)
            b.interactable = false;
        if (enable_)
        {
            while (m_animation.isPlaying)
                yield return null;
            panel.SetActive(true);
        }
        else
        {
            yield return null;
            panel.SetActive(false);
        }
        foreach (var b in listButtons)
            b.interactable = true;
        isPlaying = false;
    }
}
