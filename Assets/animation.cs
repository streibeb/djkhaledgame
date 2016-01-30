using UnityEngine;
using System.Collections;

public class animation : MonoBehaviour {

	public float maxWater = 5.0f;
	public float minWater = 1.5f;
	private bool raising = true;



	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		float currentY = this.transform.position.y;
		if (currentY >= maxWater) {
			this.raising = false;
		}
		else if(currentY <= minWater) {
			this.raising = true;
		}

		if (raising == true) {
			this.transform.Translate (Vector3.up * Time.deltaTime);
		} else {
			this.transform.Translate (Vector3.down * Time.deltaTime);
		}
	}
}
