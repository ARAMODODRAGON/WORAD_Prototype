using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Value", menuName = "Values/Int Value")]
public class IntValue : ScriptableObject {

	// the value
	public int value = 0;

	// implicit conversion to int
	public static implicit operator int(IntValue intValue) => intValue.value;
}
