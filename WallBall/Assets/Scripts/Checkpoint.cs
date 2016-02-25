using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	bool restrt = false;
	int numCheckpoints = 4;

	float randx;
	float randxmin;
	float randxmax;
	float ballx;

	float randy;
	float randymin;
	float randymax;
	float bally;

	bool numIsGood = false;
	float magnitude;

	public static ArrayList checkpoints = new ArrayList();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (restrt) {
			// create a new set of things
			for (int i = 0; i < numCheckpoints; i++) {
				while (!numIsGood) {
					randxmin = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x;
					randxmax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth,0,0)).x;
					randx = Random.Range(randxmin, randxmax);
					ballx = GameObject.Find("Ball").transform.position.x;

					randymin = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y;
					randymax = Camera.main.ScreenToWorldPoint(new Vector3(0,Camera.main.pixelHeight,0)).y;
					randy = Random.Range(randymin, randymax);
					bally = GameObject.Find("Ball").transform.position.y;
				
					magnitude = Mathf.Sqrt((randx-ballx)*(randx-ballx)+(randy-bally)*(randy-bally));

					if (magnitude >= 1) {
						foreach (GameObject check in checkpoints) {
							magnitude = Mathf.Sqrt((check.transform.position.x-randx)*(check.transform.position.x-randx)+(check.transform.position.y-randy)*(check.transform.position.y-randy)); 
							if (magnitude <= 1) {
								numIsGood = false;
								break;
							} else {
								numIsGood = true;
							}
						} 
					}
				}

				checkpoints.Add((GameObject)Instantiate(gameObject, new Vector2(randx, randy), Quaternion.Euler(0,0,0)));
			}

			restrt = false;
		}

	}

	public void restart() {
		restrt = true;
	}
}
