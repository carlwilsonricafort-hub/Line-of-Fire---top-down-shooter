using UnityEngine;
using CodeMonkey.Utils;

public class WeaponSelectMenu : MonoBehaviour {
    private void Awake() {
    GameObject ui = GameObject.Find("weaponSelectUI");

    ui.transform.Find("punchBtn").GetComponent<Button_UI>().ClickFunc =
        () => SelectWeapon(Weapon.WeaponType.Punch);

    ui.transform.Find("pistolBtn").GetComponent<Button_UI>().ClickFunc =
        () => SelectWeapon(Weapon.WeaponType.Pistol);
}

    private void SelectWeapon(Weapon.WeaponType weaponType) {
    WeaponSelection.SelectedWeapon = weaponType;
    
    switch (weaponType) {
        case Weapon.WeaponType.Punch:
            Loader.Load(Loader.Scene.GameScene_Punch); // ← punch scene
            break;
        default:
            Loader.Load(Loader.Scene.GameScene); // ← original scene
            break;
    }
}
}