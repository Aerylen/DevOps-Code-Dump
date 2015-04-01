using UnityEngine;
using System.Collections;

public class CameraControllerGlobal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void CameraLerp(Vector3 toPos, Vector3 toEuler, float speed) {
		StartCoroutine(CameraDistLerp(toPos, speed));
		StartCoroutine(CameraAngleLerp(toEuler, speed));
	}

	// lerps the camera from current position to <toPos> with a speed of <speed>
	public IEnumerator CameraDistLerp(Vector3 toPos, float speed) {
		bool isLerping = true;
		bool checkDist = false;
		while (isLerping) {
			if (Vector3.Distance(transform.position, toPos) > 0.1f)
				transform.position = Vector3.Lerp(transform.position, toPos, speed / 2 * Time.deltaTime);
			else
				checkDist = true;

			if (checkDist)
				isLerping = false;

			yield return 0;
		}
	}

	// current bug: sometimes takes long to start moving the camera angle.
	// lerps the camera angle from current euler angle to <toEuler> with a speed of <speed>
	public IEnumerator CameraAngleLerp(Vector3 toEuler, float speed) {
		bool isLerping = true;
		bool checkAngle = false;
		while (isLerping) {
//			if (Vector3.Distance(transform.position, toEuler) > 0.1f)
//				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, toEuler, speed * Time.deltaTime);
			if (Mathf.Abs(toEuler.x - transform.eulerAngles.x) > 0.1f)
				transform.eulerAngles = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, toEuler.x, speed * Time.deltaTime), 0, 0);
			else
				checkAngle = true;
			
			if (checkAngle)
				isLerping = false;
			
			yield return 0;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
