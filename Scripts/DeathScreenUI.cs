using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenUI : MonoBehaviour {

    public static DeathScreenUI Instance { get; private set; }

    [SerializeField] private GameObject panel; // assign in Inspector

    private void Awake() {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show() {
        panel.SetActive(true);
        Time.timeScale = 0f; // freeze game
    }

    public void OnRetryClicked() {
    Time.timeScale = 1f;
    Shoot_Flash.ResetStatic(); 
    Blood_Handler.ResetStatic();
    Enemy.ResetStatic();        // ← add this
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


    // public void OnQuitClicked() {
    //     Time.timeScale = 1f;
    //     Application.Quit();
    // }

    public void OnWeaponSelectClicked() {
    Time.timeScale = 1f;
    Shoot_Flash.ResetStatic();
    Blood_Handler.ResetStatic();
    Enemy.ResetStatic();
    Loader.Load(Loader.Scene.WeaponSelect);
}
}