using UnityEngine;
using System.Collections;

public class jetskiAnimation : MonoBehaviour {

	public GameObject water;
	private float modifier = 4.6f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = new Vector3 (this.transform.position.x, water.transform.position.y + modifier, this.transform.position.z);
		this.transform.position = newPosition;
	}
}
