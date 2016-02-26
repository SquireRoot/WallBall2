using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	const float colisionRadius = 1.0f;
	float radius;
	//const float threshold = 0.3f;
	ArrayList lines = new ArrayList();
	ArrayList checkpoints = new ArrayList();

	Vector2 projection;
	Vector2 normal;
	float xPointOnLine;
	float yPointOnLine;
	float distance; // distance from ball to line
	float magnitude1; // distance from ball to point 1
	float magnitude2; // distance from ball to point 2
	float magnitude; // distance between the ball and a checkpoint

	bool shouldStart = false;

	bool colisionRegistered;
	
	Vector2 velocity = new Vector2(0,2);

	// Use this for initialization
	void Start () {
		radius = (transform.localScale.x*3)/2;
		Debug.Log(radius);

	}

	// Update is called once per frame
	void Update () {
		//physics stuff, if it hits a wall or anything
		//get an array of all the line game objects
		lines = WallCreator.pointSets;
		this.checkpoints = Checkpoint.checkpoints;
			
		foreach (Vector4 ln in lines) { 
			// gets normal vector to the line created by ln
			//if (transform.position.y < ((ln.z - ln.x)/(ln.w-ln.y))*(transform.position.x - ln.x) + ln.y) {
			//	normal = (new Vector2(-(ln.w - ln.y), (ln.z - ln.x))).normalized; //normal vector to the line
			//} else {
				normal = (new Vector2((ln.w - ln.y), -(ln.z - ln.x))).normalized;
			//}

			//calcululates the shortest vector between the center of the ball and the line created by ln
			projection = ((ln.x - transform.position.x)*(normal.x) + (ln.y - transform.position.y)*(normal.y))*normal;
			distance = projection.magnitude; // distance between point and line
	
			// distance between center of the ball and point 1 of line
			magnitude1 = Mathf.Sqrt((ln.x - transform.position.x)*(ln.x - transform.position.x) + (ln.y - transform.position.y)*(ln.y - transform.position.y));
			// distance between center of the ball and point 2 of the line
			magnitude2 = Mathf.Sqrt((ln.z - transform.position.x)*(ln.z - transform.position.x) + (ln.w - transform.position.y)*(ln.w - transform.position.y));

			// line colision stuff
			if (ln.z < ln.x && distance <= radius) { // line checking
				xPointOnLine = transform.position.x + projection.x;
				if (!colisionRegistered && (xPointOnLine <= ln.x && xPointOnLine >= ln.z)) {
					lineRebound(normal);
					colisionRegistered = true;
				}
			} else if (ln.z >= ln.x && distance <= radius) { // line checking
				xPointOnLine = transform.position.x + projection.x;
				if (!colisionRegistered && (xPointOnLine >= ln.x && xPointOnLine <= ln.z)) {
					lineRebound(normal);
					colisionRegistered = true;
				}
			} else if (magnitude1 <= radius) { // point 1 checking
				if (!colisionRegistered) {
					pointRebound(new Vector2(ln.x, ln.y));
					colisionRegistered = true;
				}
			} else if (magnitude2 <= radius) { // point 2 checking
				if (!colisionRegistered) {
					pointRebound(new Vector2(ln.z, ln.w));
					colisionRegistered = true;
				}
			} else {
				colisionRegistered = false;
			}
		}

		foreach (GameObject chk in checkpoints) {
			magnitude = Mathf.Sqrt((chk.transform.position.x-transform.position.x)*(chk.transform.position.x-transform.position.x)+(chk.transform.position.y-transform.position.y)*(chk.transform.position.y-transform.position.y));

			if (magnitude < radius+50) {
				//Destroy(chk);
			}
		}
	}
		//if the sum of the two is within a threshold of the length of c then the objects have colided

	void FixedUpdate() { // called every 0.01 secs
		if (shouldStart) {
			transform.Translate(new Vector2(velocity.x/100, velocity.y/100));
		}
	}

	public void start() {
		shouldStart = true;
	}
	private void lineRebound(Vector2 normal) {
		velocity = velocity - 2 * ((velocity.x * normal.x + velocity.y * normal.y) / ((normal.x) * (normal.x) + (normal.y) * (normal.y)))*normal;

	}

	private void pointRebound(Vector2 point) {
		lineRebound(new Vector2(point.x - transform.position.x, point.y - transform.position.y));
	}
}
