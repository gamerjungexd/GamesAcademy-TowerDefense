using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonCreateTurret : MonoBehaviour
{
    [SerializeField] private GameObject turretType = null;

    private UserInput userInput = null;
    private Player player = null;
    private Turret turret = null;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        userInput = FindObjectOfType<UserInput>();
        turret = turretType.GetComponent<Turret>();

        gameObject.GetComponent<Button>().onClick.AddListener(CreateTurret);
    }

    public void CreateTurret()
    {
        int cost = turret.Cost;
        if (player.Resources >= cost)
        {
            player.EditResources(-cost);
            userInput.AddTurret(Instantiate<GameObject>(turretType, userInput.LastClickedCell, Quaternion.identity));
        }
    }

}
