using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainManager : MonoBehaviour {
    private static TrainManager instance;
    public static TrainManager Instance {
        get {
            if(instance == null) {
                instance = GameObject.FindGameObjectWithTag("TrainManager").GetComponent<TrainManager>();
            }
            return instance;
        }
    }


    public List<AwareAlien> Unused;
    public List<AwareAlien> InTrain;    
    public List<AwareAlien> Used;

    public List<GameObject> BackgroundTiles;
    private int backgroundIndex;
    public float BackgroundSpeed;

    public float TripDuration;
    private float elapsedTime;
	// Use this for initialization
	void Start () {
        // Reset background index
        backgroundIndex = 0;
        // Reset timer
        elapsedTime = 0;
        // Pick initial monsters from unused list
        // First alien
        int index = Random.Range(0, Unused.Count);
        AwareAlien alien1 = Unused[index];
        alien1.gameObject.SetActive(true);
        Unused.RemoveAt(index);
        InTrain.Add(alien1);
        // Second alien
        index = Random.Range(0, Unused.Count);
        AwareAlien alien2 = Unused[index];
        alien2.gameObject.SetActive(true);
        Unused.RemoveAt(index);
        InTrain.Add(alien2);
    }
	
	// Update is called once per frame
	void Update () {
        // Keep track of time
        elapsedTime += Time.deltaTime;
        // When the next station is reached
        if(elapsedTime >= TripDuration) {
            // Select alien to get off
            //int index = Random.Range(0, InTrain.Count);
            int index = SelectAlienToGetOff();
            AwareAlien outAlien = InTrain[index];
            outAlien.gameObject.SetActive(false);
            InTrain.RemoveAt(index);
            Used.Add(outAlien);
            // Select alien to get in
            index = Random.Range(0, Unused.Count);
            AwareAlien inAlien = Unused[index];
            inAlien.gameObject.SetActive(true);
            Unused.RemoveAt(index);
            InTrain.Add(inAlien);
            // Move used to unused unused is empty
            if(Unused.Count == 0) {
                Unused.AddRange(Used);
                Used.Clear();
            }
            // Reset timer
            elapsedTime = 0;
            //Debug.Log("Station reached");
        }
    }

    private int SelectAlienToGetOff() {
        int index = 0;
        float minTime = Mathf.Infinity;
        for(int i = 0; i < InTrain.Count; ++i) {
            if(InTrain[i].lookedAtTime < minTime) {
                minTime = InTrain[i].lookedAtTime;
                index = i;
            }
            // Reset aliens' interaction time every time you pick someone to get off
            InTrain[i].lookedAtTime = 0;
        }
        return index;
    }
}
