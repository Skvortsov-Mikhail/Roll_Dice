using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    private Dice _dice;

    private TMP_Text _text;

    private Tween _tween;
    private Sequence _sequence;

    private float _hidingDuration = 0.2f;
    private float _winkingDuration = 0.5f;

    private Vector3 _winkingTextScale = new Vector3(0.9f, 0.8f, 1f);
    private string _changedText = "Попробуй еще раз";

    private void Start()
    {
        _dice = transform.parent.GetComponentInChildren<Dice>();
        _text = GetComponent<TMP_Text>();

        _dice.StartRolling += HideText;
        _dice.ReadyToNextRoll += ShowText;

        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(_winkingTextScale, _winkingDuration))
            .Append(transform.DOScale(Vector3.one, _winkingDuration))
            .SetLoops(-1);
    }

    private void OnDestroy()
    {
        _tween.Kill();
        _sequence.Kill();

        _dice.StartRolling -= HideText;
        _dice.ReadyToNextRoll -= ShowText;
    }

    private void HideText()
    {
        _tween.Kill();

        _text.DOColor(new Color(_text.color.r, _text.color.g, _text.color.b, 0f), _hidingDuration)
            .OnComplete(() => _text.text = _changedText);
    }

    private void ShowText()
    {
        _tween.Kill();

        _text.DOColor(new Color(_text.color.r, _text.color.g, _text.color.b, 1f), _hidingDuration);
    }
}