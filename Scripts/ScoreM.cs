using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreM : MonoBehaviour
{
    public static ScoreM Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int comboScore;

    void Start() {
        Instance = this;
        comboScore = 0;

    } // Start

    public static void Hit()  {
        comboScore += 1;
        Instance.hitSFX.Play();

    } // Hit

    public static void Miss() {

        comboScore = 0;
        Instance.missSFX.Play();

    } // Miss

    private void Update() { scoreText.text = comboScore.ToString(); } // Update

} // ScoreM
