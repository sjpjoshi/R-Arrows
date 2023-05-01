using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour {

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    int inputIndex = 0;

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array) {

        foreach (var note in array) {

            if (note.NoteName == noteRestriction)  {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongM.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);

            } // if

        } // foreach

    } // setTimeStamps

    // Update is called once per frame
    void Update() {

        if (spawnIndex < timeStamps.Count) {

            if (SongM.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongM.Instance.noteTime) {

                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;

            } // if

        } // if

        if (inputIndex < timeStamps.Count) {

            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongM.Instance.marginOfError;
            double audioTime = SongM.GetAudioSourceTime() - (SongM.Instance.inputDelay / 1000.0);

            if (Input.GetKeyDown(input)) {

                if (Math.Abs(audioTime - timeStamp) < marginOfError) {

                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;

                } else
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                

            } // if
            if (timeStamp + marginOfError <= audioTime)  {

                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;

            } // if 

        } // if

    } // Update

    private void Hit()  { ScoreM.Hit(); } // hit
    private void Miss() { ScoreM.Miss(); } // Miss

} // lane
