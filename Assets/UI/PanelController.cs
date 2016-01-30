using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {
	private bool _open;
	public Animator PanelAnimator;

	// Use this for initialization
	void Start () {
		PanelAnimator.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TogglePanel() {
		PanelAnimator.SetBool("Open", !_open);
		PanelAnimator.SetBool("Close", _open);
		_open = !_open;
	}
}
