using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    private Dice _dice;

    private TMP_Text _text;
    private Tween _tween;
    private Sequence _sequence;

    private void Start()
    {
        _dice = transform.parent.GetComponentInChildren<Dice>();
        _text = GetComponent<TMP_Text>();

        _dice.DiceRolling += ChangeTextVisibility;

        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(new Vector3(0.9f, 0.8f, 1f), 0.5f))
            .Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f))
            .SetLoops(-1);
    }

    private void OnDestroy()
    {
        _tween.Kill();
        _sequence.Kill();
        _dice.DiceRolling -= ChangeTextVisibility;
    }

    private void ChangeTextVisibility(bool state)
    {
        _tween.Kill();

        if(state)
        {
            _text.DOColor(new Color(_text.color.r, _text.color.g, _text.color.b, 0f), 0.2f);
        }
        else
        {
            _text.DOColor(new Color(_text.color.r, _text.color.g, _text.color.b, 1f), 0.2f);
        }
    }
}
