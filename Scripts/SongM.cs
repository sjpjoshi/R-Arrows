using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongM : MonoBehaviour
{

    public static SongM Instance;
    public AudioSource audioSource;
    public float delayInSeconds;
    public int inputDelay;

    public string fileLocation;
    public float noteTime, noteSpawnY, noteTapY;

    public double marginOfError; // in seconds
    public Lane[] lanes;

    public float noteDespawnY { get { return noteTapY - (noteSpawnY - noteTapY);  } } // noteDespawnY


    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start() {

        Instance = this;

        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
            StartCoroutine(ReadFromWebsite());
        else
            ReadFromFile();
        

    } // Start

    private IEnumerator ReadFromWebsite() {


        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation)) {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.LogError(www.error);
            
            else {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results)) {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();

                } // using

            } // else

        } // using

    } // ReadFromWebsite

    private void ReadFromFile() {

        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();

    } // ReadFromFile

    public void GetDataFromMidi() {

        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), delayInSeconds);

    } // GetDataFromMidi

    public void StartSong() {

        audioSource.Play();

    } // StartSong

    public static double GetAudioSourceTime() {

        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;

    } // GetAudioSourceTime


} // SongM
