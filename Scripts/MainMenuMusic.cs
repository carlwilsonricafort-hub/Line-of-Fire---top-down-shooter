using UnityEngine;

public class MainMenuMusic : MonoBehaviour {

    private AudioSource musicSource;

    private void Start() {
        Sound_Manager.Init();
        musicSource = Sound_Manager.PlaySound(Sound_Manager.Sound.BGM_Menu);
    }

    private void OnDestroy() {
        if (musicSource != null) {
            Sound_Manager.StopSound(musicSource);
        }
    }
}