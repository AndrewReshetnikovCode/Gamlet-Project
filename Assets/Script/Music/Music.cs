using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
    public AudioClip BackgroundMusic;

    private float GameMusicVolume;
	// Use this for initialization
	void Start () {
        GameMusicVolume = PlayerPrefs.GetInt("Music") * 0.05f;
        
        this.GetComponent<AudioSource>().volume = GameMusicVolume;
        this.GetComponent<AudioSource>().clip = BackgroundMusic;
        this.GetComponent<AudioSource>().playOnAwake = true;
        this.GetComponent<AudioSource>().Play();
        //this.audio.PlayOneShot(BackgroundMusic);

	
	}
	
	// Update is called once per frame
	void Update () {
        GameMusicVolume = PlayerPrefs.GetInt("Music") * 0.05f;

        this.GetComponent<AudioSource>().volume = GameMusicVolume;
	}
}
