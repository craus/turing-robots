using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextMeshEditing : MonoBehaviour {
	public TMP_InputField field;

	bool onTextChangedWorking = false;

	public void OnTextChanged() {
		if (onTextChangedWorking) {
			return;
		}
		onTextChangedWorking = true;
		Debug.LogFormat("field.text = {0}", field.text);
		var s = field.text;
		field.text = "abacaba";
		field.text = s;
		onTextChangedWorking = false;
	}
}
