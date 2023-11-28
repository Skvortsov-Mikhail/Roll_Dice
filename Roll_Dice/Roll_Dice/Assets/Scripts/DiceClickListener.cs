using UnityEngine;

public class DiceClickListener : MonoBehaviour
{
    private Dice _dice;

    private Vector3 _startScale;
    private Vector3 _reducedScale;
    private float _reduceRatio = 0.9f;

    private void Start()
    {
        _dice = transform.parent.GetComponent<Dice>();

        _startScale = transform.localScale;
        _reducedScale = _startScale * _reduceRatio;
    }

    private void OnMouseUpAsButton()
    {
        _dice.StartRollDice();
    }

    private void OnMouseDown()
    {
        transform.localScale = _reducedScale;
    }

    private void OnMouseUp()
    {
        transform.localScale = _startScale;
    }
}