using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AwareAlien : MonoBehaviour {
    public List<GazeAwareComponent> _gazeAware;
    public Animator Anim;

    private bool lookingBack;

    void Start() {
        lookingBack = false;
    }

    void Update() {
        bool hasGaze = false;
        foreach(GazeAwareComponent part in _gazeAware) {
            if(part.HasGaze) {
                hasGaze = true;
                break;
            }
        }
        if (hasGaze) {
            //transform.Rotate(Vector3.forward);
            Anim.SetBool("Look", true);
            lookingBack = true;
            Debug.Log("has gaze");
        } else if(lookingBack) {
            Anim.SetBool("Look", false);
            lookingBack = false;
        }

    }
}
