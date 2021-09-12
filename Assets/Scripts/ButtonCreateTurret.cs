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
    void Awake()
    {
        userInput = FindObjectOfType<UserInput>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CreateTurret);
    }

    public void CreateTurret()
    {
        userInput.AddTurret(Instantiate<GameObject>(turretType, userInput.LastClickedCell, Quaternion.identity));
    }

}
