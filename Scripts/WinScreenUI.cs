using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenUI : MonoBehaviour {

    public static WinScreenUI Instance { get; private set; }

    [SerializeField] private GameObject panel;

    private void Awake() {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show() {
        panel.SetActive(true);
        Time.timeScale = 0f;
        CinematicBars.Show_Static(150f, .6f);
    }

    public void OnRetryClicked() {
        Time.timeScale = 1f;
        Shoot_Flash.ResetStatic();
        Blood_Handler.ResetStatic();
        Enemy.ResetStatic();  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnWeaponSelectClicked() {
    Time.timeScale = 1f;
    Shoot_Flash.ResetStatic();
    Blood_Handler.ResetStatic();
    Enemy.ResetStatic();
    Loader.Load(Loader.Scene.WeaponSelect);
}
}