using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDestroyTurret : MonoBehaviour
{
    [Tooltip("Percentage of tower cost refund.\n[Min 0f, Max 1f]")]
    [Range(0f,1f)]
    [SerializeField] private float refundMultiply = 0.3f;

    private UserInput userInput = null;
    private Player player = null;
    void Awake()
    {
        player = FindObjectOfType<Player>();
        userInput = FindObjectOfType<UserInput>();

        gameObject.GetComponent<Button>().onClick.AddListener(DestroyTurret);
    }

    private void DestroyTurret()
    {
        player.EditResources((int)(userInput.LastClickedTurret.GetComponent<Turret>().Cost * refundMultiply));
        userInput.RemoveTurret(userInput.LastClickedTurret);

        Destroy(userInput.LastClickedTurret);
    }
}
