using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AwareAlien : MonoBehaviour {
    public GazeAwareComponent gazeAware;
    public Animator Anim;
    private AudioSource audioSource;

    public List<AudioClip> UnusedLines;
    public List<AudioClip> UsedLines;
    public float LineDelay;

    private bool looking;

    void Start() {
        looking = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if(!looking && gazeAware.HasGaze) {
            // Get next sound and play it
            PopLine();
            audioSource.PlayDelayed(LineDelay);
            // Look animation
            Anim.SetBool("Look", true);
            looking = true;
            Debug.Log("has gaze");
        } else if(looking && !gazeAware.HasGaze) {
            // Stop sound
            //audioSource.Stop();
            // Look away
            Anim.SetBool("Look", false);
            looking = false;
            Debug.Log("no gaze");
        }
    }


    void PopLine() {
        int index = Random.Range(0, UnusedLines.Count);
        audioSource.clip = UnusedLines[index];
        UsedLines.Add(UnusedLines[index]);
        UnusedLines.RemoveAt(index);
        // Reset unused sounds if unused is empty
        if(UnusedLines.Count == 0) {
            UnusedLines.AddRange(UsedLines);
            UsedLines.Clear();
        }
    }
}
