using UnityEngine;
using CodeMonkey.Utils;

public class MainMenu : MonoBehaviour {

    private AudioSource menuMusicSource;

    private void Start() {
        Sound_Manager.Init();
        menuMusicSource = Sound_Manager.PlaySound(Sound_Manager.Sound.BGM_Menu);
    }

    private void Awake() {
        transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => {
            // Stop music before scene loads
            if (menuMusicSource != null) {
                Sound_Manager.StopSound(menuMusicSource);
            }
            Loader.Load(Loader.Scene.WeaponSelect);
        };
    }

 
    // private void Awake() {
    //     transform.Find("playBtn").GetComponent<Button_UI>().ClickFunc = 
    //         () => Loader.Load(Loader.Scene.WeaponSelect);
    // }
}
