using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderProxy : MonoBehaviour
{
	public  Action<GameObject> onTrigger;
	private void       OnTriggerEnter(Collider other) => onTrigger?.Invoke(other.gameObject);
}
