using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Character : MonoBehaviour {

    public Button m_upArrow;
    public Button m_downArrow;
    public Image m_BackgroundImage;
    public Text m_character;


    private char m_currentCharacter = 'A';

    public Color m_SelectedCharacterColour;
    public Color m_NormalCharacterColour;

    public char Char { get { return m_currentCharacter; } }

    public void IncreaseCharacter()
    {
		if (m_currentCharacter == '9')
			m_currentCharacter = ' ';
		else if (m_currentCharacter == ' ')
			m_currentCharacter = 'A';
		else if (m_currentCharacter < 'Z')
			m_currentCharacter++;

//		if (m_currentCharacter == '9')
//			m_currentCharacter = 'A';
//		else if (m_currentCharacter == ' ')
//            m_currentCharacter = 'A';
//        else if(m_currentCharacter < m_lastChar)
//            m_currentCharacter++;
        m_character.text = m_currentCharacter.ToString();
    }

    public void DecreaseCharacter()
    {
		if (m_currentCharacter == 'A')
			m_currentCharacter = ' ';
		else if (m_currentCharacter == ' ')
			m_currentCharacter = '9';
		else if (m_currentCharacter > '0')
			m_currentCharacter--;

//		if (m_currentCharacter >= '0')
//			m_currentCharacter = '9';
//		else if (m_currentCharacter > m_firstChar)
//            m_currentCharacter--;
//        else
//            m_currentCharacter = ' ';
        m_character.text = m_currentCharacter.ToString();
    }

    public void SelectCharacter()
    {
        m_BackgroundImage.color = m_SelectedCharacterColour;
    }

    public void DeselectCharacter()
    {
        m_BackgroundImage.color = m_NormalCharacterColour;
    }

}
