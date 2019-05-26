using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Compiler : Singletone<Compiler> {
	public TMPro.TMP_InputField input;

	public PairProgram program;

	public void Start() {
		input.scrollSensitivity = 20;
		program = new PairProgram();
		program.Compile(input.text);
		program.Run();
	}

	[ContextMenu("Run")] 
	public void RunCurrent() {
		Clear();
		program.Compile(input.text);
		program.Run();
	}

	#if UNITY_EDITOR
	[MenuItem("Pair/Run From Clipboard %r")]
	public static void StaticRunCurrent()
	{
		instance.input.text = EditorGUIUtility.systemCopyBuffer;
		instance.RunCurrent();
	}
	#endif

	[ContextMenu("Clear")]
	public void Clear() {
		program.functions.Clear();
	}


}
