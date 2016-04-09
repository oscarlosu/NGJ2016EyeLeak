using UnityEngine;
using System.Collections;

public class AwareAlien : MonoBehaviour {
    public GazeAwareComponent gazeAware;
    public Animator Anim;

    private bool lookingBack;

    void Start() {
        lookingBack = false;
    }

    void Update() {
        if (gazeAware.HasGaze) {
            //transform.Rotate(Vector3.forward);
            Anim.SetBool("Look", true);
            lookingBack = true;
            Debug.Log("has gaze");
        } else if(lookingBack) {
            Anim.SetBool("Look", false);
            lookingBack = false;
            Debug.Log("no gaze");
        }
    }
}
