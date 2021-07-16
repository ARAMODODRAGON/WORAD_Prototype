using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextItem : MonoBehaviour {

	[Header("The Value")]
	[SerializeField] private Sprite m_itemSprite;
	[SerializeField] private string m_itemName;
	[SerializeField] private IntValue m_itemValue;

	[Header("References")]
	[SerializeField] private Image m_image;
	[SerializeField] private Text m_text;

	private int m_lastValue = 0;

	private void Start() {
		UpdateText();
	}

	private void UpdateText() {
		// update text
		m_lastValue = m_itemValue.value;
		m_text.text = $"{m_itemValue.value} \u00D7 {m_itemName}";
	}

	private void Update() {
		// check if text can or should update
		if (m_lastValue != m_itemValue.value && m_text) UpdateText();

		// check if image can or should update
		if (m_image && m_itemSprite != m_image.sprite) {
			// update image
			m_image.sprite = m_itemSprite;
		}
	}

}
