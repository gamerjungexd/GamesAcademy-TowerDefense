using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDestroyTurret : MonoBehaviour
{
    private Button button = null;
    private UserInput userInput = null;
    void Awake()
    {
        userInput = FindObjectOfType<UserInput>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(DestroyTurret);
    }

    private void DestroyTurret()
    {
        userInput.RemoveTurret(userInput.LastClickedTurret);
        Destroy(userInput.LastClickedTurret);
    }
}
