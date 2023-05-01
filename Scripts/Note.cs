using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    double timeInstantiated;
    public float assignedTime;
    void Start(){ timeInstantiated = SongM.GetAudioSourceTime(); } // Start

    // Update is called once per frame
    void Update() {

        double timeSinceInstantiated = SongM.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongM.Instance.noteTime * 2));


        if (t > 1)
            Destroy(gameObject);
        
        else {
            transform.localPosition = Vector3.Lerp(Vector3.up * SongM.Instance.noteSpawnY, Vector3.up * SongM.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;

        } // else

    } // Update

} // Note
