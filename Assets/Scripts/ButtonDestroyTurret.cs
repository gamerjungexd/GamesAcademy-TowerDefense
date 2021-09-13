using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDestroyTurret : MonoBehaviour
{
    private Button button = null;
    private UserInput userInput = null;
    private Player player = null;
    void Awake()
    {
        player = FindObjectOfType<Player>();

        userInput = FindObjectOfType<UserInput>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(DestroyTurret);
    }

    private void DestroyTurret()
    {
        player.EditResources(userInput.LastClickedTurret.GetComponent<Turret>().Cost / 3);
        userInput.RemoveTurret(userInput.LastClickedTurret);
        Destroy(userInput.LastClickedTurret);
    }
}
