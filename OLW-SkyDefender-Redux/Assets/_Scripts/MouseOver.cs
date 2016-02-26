using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {

	public Sprite defaultImage;
	public Sprite highlightImage;

	private SpriteRenderer myRenderer;

	void Start () {
		myRenderer = GetComponent<SpriteRenderer>();
		myRenderer.sprite = highlightImage;
	}

	void OnMouseOver () { myRenderer.sprite = highlightImage; }
	void OnMouseExit () { myRenderer.sprite = defaultImage;  }
}
