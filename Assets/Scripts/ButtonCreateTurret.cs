using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonCreateTurret : MonoBehaviour
{
    [SerializeField] private GameObject turretType = null;

    private Button button = null;
    private UserInput userInput = null;
    private Player player = null;
    void Awake()
    {
        player = FindObjectOfType<Player>();

        userInput = FindObjectOfType<UserInput>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CreateTurret);
    }

    public void CreateTurret()
    {
        int cost = turretType.GetComponent<Turret>().Cost;
        if (player.Resources >= cost)
        {
            player.EditResources(-cost);
            userInput.AddTurret(Instantiate<GameObject>(turretType, userInput.LastClickedCell, Quaternion.identity));
        }
    }

}
