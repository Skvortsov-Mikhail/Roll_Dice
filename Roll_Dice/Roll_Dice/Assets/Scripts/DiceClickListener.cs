using UnityEngine;

public class DiceClickListener : MonoBehaviour
{
    private Dice _dice;
    private Vector3 _startScale;

    private void Start()
    {
        _dice = transform.parent.GetComponent<Dice>();

        _startScale = transform.localScale;
    }

    private void OnMouseUpAsButton()
    {
        _dice.StartRollDice();
    }

    private void OnMouseDown()
    {
        transform.localScale *= 0.9f; 
    }

    private void OnMouseUp()
    {
        transform.localScale = _startScale;
    }
}
