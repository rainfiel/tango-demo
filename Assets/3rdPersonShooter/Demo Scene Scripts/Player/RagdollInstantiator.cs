using UnityEngine;
using System.Collections;

public class RagdollInstantiator : MonoBehaviour {
	
	public GameObject deadReplacement;
	public string cameraTargetPath;
	
	public void Die () {
	
		// Replace ourselves with the dead body
		GameObject dead = null;
		if (deadReplacement) {
			// Create the dead body
			dead = (GameObject)Instantiate (deadReplacement, transform.position, transform.rotation);
  //    dead.transform.Find("Reference").transform.Find("Hips").gameObject.AddComponent<FoodScore>();
			
			Vector3 vel = Vector3.zero;
			if (GetComponent<Rigidbody>()) {
				vel = GetComponent<Rigidbody>().velocity;
			}
			else {
				CharacterController cc = GetComponent<CharacterController> ();
				vel = cc.velocity;
			}
			
			// Copy position & rotation from the old hierarchy into the dead replacement
			CopyTransformsRecurse (transform, dead.transform, vel);
			
			gameObject.SetActiveRecursively(false);
			
			// ShooterGameCamera cam = Camera.mainCamera.gameObject.GetComponent<ShooterGameCamera>();
			// cam.player = dead.transform.FindChild(cameraTargetPath);
		}
	}
	
	void CopyTransformsRecurse (Transform src, Transform dst, Vector3 velocity) {
		
		Rigidbody body = dst.GetComponent<Rigidbody>();
		if (body != null) {
			body.velocity = velocity;
			body.useGravity = true;
		}
		
		dst.position = src.position;
		dst.rotation = src.rotation;
		
		foreach (Transform child in dst) {
			// Match the transform with the same name
			Transform curSrc = src.Find (child.name);
			if (curSrc)
				CopyTransformsRecurse (curSrc, child, velocity);
		}
	}
}
