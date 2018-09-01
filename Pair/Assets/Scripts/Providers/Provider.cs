using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public abstract class Provider<T> : MonoBehaviour {
	public abstract T Get();

	public static implicit operator T(Provider<T> provider) {
		return provider.Get();
	}
}
