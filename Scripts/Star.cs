using UnityEngine;

public class Star : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player != null) {
            GameHandler_Setup.Instance.StopBGM();
            Sound_Manager.PlaySound(Sound_Manager.Sound.Victory_2);
            GetComponent<Collider2D>().enabled = false; // prevent double trigger
            GetComponent<SpriteRenderer>().enabled = false; // hide star visually

            // Run coroutine on GameHandler_Setup instead — it stays active
            GameHandler_Setup.Instance.StartCoroutine(ShowWinDelayed());
        }
    }

    private System.Collections.IEnumerator ShowWinDelayed() {
        yield return new WaitForSecondsRealtime(1.5f);
        WinScreenUI.Instance.Show();
    }
}