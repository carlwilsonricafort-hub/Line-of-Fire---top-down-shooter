using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon {

    public event EventHandler OnAmmoChanged;

    public enum WeaponType {
        Pistol,
        Shotgun,
        Rifle,
        Sword,
        Punch
    }

    private WeaponType weaponType;
    private int ammo;

    public Weapon(WeaponType weaponType) {
        this.weaponType = weaponType;
        ammo = GetAmmoMax();
    }

    public WeaponType GetWeaponType() {
        return weaponType;
    }

    public int GetAmmo() {
        return ammo;
    }

    public bool TrySpendAmmo() {
        if (ammo > 0) {
            ammo--;
            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
            return true;
        } else {
            return false;
        }
    }

    public void Reload() {
        ammo = GetAmmoMax();
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanReload() {
        return ammo < GetAmmoMax();
    }

    public int GetAmmoMax() {
        switch (weaponType) {
        default:
            return 99;
        case WeaponType.Pistol:
            return 6;
        case WeaponType.Shotgun:
            return 4;
        case WeaponType.Rifle:
            return 25;
        }
    }

    public float GetDamageMultiplier() {
        switch (weaponType) {
        default:
        case WeaponType.Pistol:     return 1.1f;//1.2f
        case WeaponType.Shotgun:    return 2.2f;
        case WeaponType.Rifle:      return 0.9f;
        }
    }

    public float GetFireRate() {
        switch (weaponType) {
        default:
        case WeaponType.Pistol:     return .10f;
        case WeaponType.Shotgun:    return .94f;
        case WeaponType.Rifle:      return .09f;
        }
    }

   public Sprite GetSprite() {
    switch (weaponType) {
    default:
    case WeaponType.Pistol:  return GameAssets.i.s_PistolIcon;
    case WeaponType.Shotgun: return GameAssets.i.s_ShotgunIcon;
    case WeaponType.Rifle:   return GameAssets.i.s_RifleIcon;
    case Weapon.WeaponType.Punch: return GameAssets.i.s_SwordIcon;
    }
}

}
