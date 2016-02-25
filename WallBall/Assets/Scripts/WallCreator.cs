using UnityEngine;
using System.Collections;

public class WallCreator : MonoBehaviour {
	Vector3 lastPoint = new Vector3(0,0,0);
	public static ArrayList pointSets = new ArrayList();
	GameObject newLine;
	float magnitude;
	float mouseX;
	float mouseY;
	float deltaX;
	float deltaY;
	
	float magnitudeThreshold = 0.1f; // length of line before a new line is created
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// get initial point when the mouse button is clicked
		if (Input.GetMouseButtonDown (0)) {
			lastPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log("Mouse Clicked at x: " + lastPoint.x + " y: " + lastPoint.y);
		}
		
		// finds the distance between the current mouse position and the prevous saved point to see if it should save a new point

		if (Input.GetMouseButton(0)) {
			mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
			deltaX = mouseX - lastPoint.x;
			deltaY = mouseY - lastPoint.y;
			magnitude = Mathf.Sqrt((deltaX)*(deltaX) + (deltaY)*(deltaY));
			//Debug.Log(magnitude);

			if (magnitude > magnitudeThreshold) {
				pointSets.Add(new Vector4(lastPoint.x, lastPoint.y, mouseX, mouseY));

				// instantiate a new line thing
				newLine = (GameObject)Instantiate(GameObject.Find("Line"), new Vector3((lastPoint.x + 0.5f*deltaX), (lastPoint.y + 0.5f*deltaY), 0), Quaternion.Euler(0.0f, 0.0f, (Mathf.Atan(deltaY/deltaX))*(180/Mathf.PI)));
				newLine.transform.localScale = new Vector3(magnitude*10,1,1); // multiplied by 10 because the image is 0.1 units wide
				newLine.GetComponent<SpriteRenderer>().enabled = true;
				lastPoint = new Vector3(mouseX, mouseY);
			}
		}
	}
}
