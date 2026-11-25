using CodeMonkey;
using CodeMonkey.MonoBehaviours;
using CodeMonkey.Utils;
using GridPathfindingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameHandler_Setup : MonoBehaviour {

    public static GameHandler_Setup Instance { get; private set; }
    public static GridPathfinding gridPathfinding;

    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform followTransform;
    [SerializeField] private bool cameraPositionWithMouse;
    [SerializeField] private UI_Weapon uiWeapon;

    [SerializeField] private Player player;

    [SerializeField] private DoorAnims doorAnims;
    [SerializeField] private Transform pfPathfindingUnWalkable;
    [SerializeField] private Transform pfPathfindingWalkable;

    private AudioSource bgmAudioSource; // store reference so you can stop/pause it later

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Sound_Manager.Init();

        // Play background music
        bgmAudioSource = Sound_Manager.PlaySound(Sound_Manager.Sound.BGM_Main);

        cameraFollow.Setup(GetCameraPosition, () => 60f, true, true);

        Vector3 pathfindingLowerLeft = transform.Find("PathfindingLowerLeft").position;
        Vector3 pathfindingUpperRight = transform.Find("PathfindingUpperRight").position;

        gridPathfinding = new GridPathfinding(pathfindingLowerLeft, pathfindingUpperRight, 5f);
        gridPathfinding.RaycastWalkable(1 << GameAssets.i.wallLayer);

        uiWeapon.SetWeapon(player.GetWeapon());
        player.OnWeaponChanged += Player_OnWeaponChanged;

        FunctionTimer.Create(() => doorAnims.OpenDoor(), 3f);
    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.tabKey.wasPressedThisFrame) {
            PauseGame();
            UI_Controls.Instance.Show();
        }
    }

    private void Player_OnWeaponChanged(object sender, System.EventArgs e) {
        uiWeapon.SetWeapon(player.GetWeapon());
    }

    private Vector3 GetCameraPosition() {
        if (cameraPositionWithMouse) {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 playerToMouseDirection = mousePosition - followTransform.position;
            return followTransform.position + playerToMouseDirection * .3f;
        } else {
            return followTransform.position;
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        // Pause BGM when game is paused
        if (bgmAudioSource != null && bgmAudioSource.isPlaying) {
            bgmAudioSource.Pause();
        }
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        // Resume BGM when game resumes
        if (bgmAudioSource != null && !bgmAudioSource.isPlaying) {
            bgmAudioSource.UnPause();
        }
    }

    public void StopBGM() {
        if (bgmAudioSource != null) {
            Sound_Manager.StopSound(bgmAudioSource);
            bgmAudioSource = null;
        }
    }
}