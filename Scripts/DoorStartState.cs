using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStartState : MonoBehaviour {

    [SerializeField] private bool startOpen;
    [SerializeField] private DoorAnims.ColorName doorColor;

    private void Start() {
    DoorAnims doorAnims = GetComponent<DoorAnims>();
    if (startOpen) {
        doorAnims.OpenDoorSilent(); // no sound on scene start
    } else {
        doorAnims.CloseDoor();
    }
    doorAnims.SetColor(doorColor);
}

}