using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainManager : MonoBehaviour {

    public List<GameObject> Unused;
    public List<GameObject> InTrain;    
    public List<GameObject> Used;

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
        GameObject alien1 = Unused[index];
        alien1.SetActive(true);
        Unused.RemoveAt(index);
        InTrain.Add(alien1);
        // Second alien
        index = Random.Range(0, Unused.Count);
        GameObject alien2 = Unused[index];
        alien2.SetActive(true);
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
            int index = Random.Range(0, InTrain.Count);
            GameObject outAlien = InTrain[index];
            outAlien.SetActive(false);
            InTrain.RemoveAt(index);
            Used.Add(outAlien);
            // Select alien to get in
            index = Random.Range(0, Unused.Count);
            GameObject inAlien = Unused[index];
            inAlien.SetActive(true);
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
}
