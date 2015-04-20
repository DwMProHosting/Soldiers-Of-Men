using UnityEngine;
using System.Collections;
public class HUD : MonoBehaviour
{

    public PlayerVitals pv;
    public Weapon wep;

    // Use this for initialization
    void Start()
    {
        if (!networkView.isMine)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (networkView.isMine)
        {
			GUI.color = Color.green;
			GUI.Box (new Rect (Screen.width / 4 - 300, Screen.height / 6 + 445, 100, 50), "");
            GUI.Label(new Rect(50, Screen.height - 40, 100, 40), "+ " + pv.hitPoints.ToString("F0"));
			GUI.Box (new Rect (Screen.width / 4 + 730, Screen.height / 6 + 445, 200, 50), "");
           GUI.Label(new Rect(1075, Screen.height - 40, 150, 40), "Ammo: " + wep.bulletsLeftRead + " / " + wep.bulletsPerMagRead + " | " + wep.magsLeftRead);

        }
        else
        {
            this.enabled = false;
        }
    }
}
