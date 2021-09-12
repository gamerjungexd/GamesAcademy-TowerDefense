using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUpgrade : MonoBehaviour
{
    private Button button = null;
    private UserInput userInput = null;
    private WaveManager waveManager = null;

    void Awake()
    {
        userInput = FindObjectOfType<UserInput>();
        waveManager = FindObjectOfType<WaveManager>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(UpgradeTurret);
    }
    public void UpgradeTurret()
    {
        Turret turret = userInput.LastClickedTurret.GetComponent<Turret>();
        GameObject upgradedTurret = waveManager.GetTurretUpgrade(turret.Type, turret.TypeLevel + 1);

        userInput.AddTurret(Instantiate<GameObject>(upgradedTurret, turret.transform.position, turret.transform.rotation));
        userInput.RemoveTurret(turret.gameObject);
        Destroy(turret.gameObject);
    }
}
