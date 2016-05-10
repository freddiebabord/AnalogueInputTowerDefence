using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Word : MonoBehaviour {

    public List<Character> characters;
    int m_currentSelcted = 0;
    bool m_changingCharacter = false;
    bool m_selectingNewCharacetr = false;

	void Start()
    {
        FindObjectOfType<CameraController>().shouldBeActive = false;
        characters = new List<Character>();
        var chars = GetComponentsInChildren<Character>();
        int size = chars.Length;
        for (int i = 0; i < size; ++i)
            characters.Add(chars[i]);

        characters[0].SelectCharacter();
    }

    void Update()
    {
        if(AnalogueInput.GetRightHorizontal() >= 1.0f)
        {
            if (!m_selectingNewCharacetr)
                StartCoroutine(SelectNextCharacter(true));
        }
        if (AnalogueInput.GetRightHorizontal() <= -1.0f)
        {
            if (!m_selectingNewCharacetr)
                StartCoroutine(SelectNextCharacter(false));
        }
        if (AnalogueInput.GetRightVertical() >= 1.0f)
        {
            if (!m_changingCharacter)
                StartCoroutine(ChangeCharacter(true));
        }
        if (AnalogueInput.GetRightVertical() <= -1.0f)
        {
            if (!m_changingCharacter)
                StartCoroutine(ChangeCharacter(false));
        }
    }

    void OnDisable()
    {
        FindObjectOfType<CameraController>().shouldBeActive = true;
    }

    public string GetWord()
    {
        int size = characters.Count;
        char[] word = new char[size];
        for(int i = 0; i < size; ++i)
        {
            word[i] = characters[i].Char;
        }

        return word.ToString();
    }

    IEnumerator SelectNextCharacter(bool increment)
    {
        m_selectingNewCharacetr = true;
        yield return new WaitForSeconds(0.1f);
        if (increment)
        {
            if((m_currentSelcted + 1) < characters.Count)
            {
                characters[m_currentSelcted].DeselectCharacter();
                m_currentSelcted++;
                characters[m_currentSelcted].SelectCharacter();
            }
        }
        else
        {
            if ((m_currentSelcted - 1) >= 0)
            {
                characters[m_currentSelcted].DeselectCharacter();
                m_currentSelcted--;
                characters[m_currentSelcted].SelectCharacter();
            }
        }

        m_selectingNewCharacetr = false;
    }

    IEnumerator ChangeCharacter(bool increment)
    {
        m_changingCharacter = true;
        yield return new WaitForSeconds(0.1f);

        if (increment)
            characters[m_currentSelcted].IncreaseCharacter();
        else
            characters[m_currentSelcted].DecreaseCharacter();

        m_changingCharacter = false;
    }
}
