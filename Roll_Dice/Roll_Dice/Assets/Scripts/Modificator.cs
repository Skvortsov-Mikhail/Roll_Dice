using DG.Tweening;
using TMPro;
using UnityEngine;

public class Modificator : MonoBehaviour
{
    [SerializeField] private int m_modificatorValue = 1;

    [SerializeField] private TMP_Text m_valueText;

    private Dice _dice;

    private Tween _tween;
    private Sequence _sequence;

    private Vector3 _startModificatorPanelScale;
    private Vector3 _startTextPosition;
    private Color _startTextColor;

    private float _showModificatorDuration = 0.5f;
    private float _hideModificatorDuration = 0.2f;
    private float _moveValueTextDuration = 0.5f;

    private void Start()
    {
        m_valueText.text = "+" + m_modificatorValue.ToString();
        _startTextPosition = m_valueText.transform.localPosition;
        _startTextColor = m_valueText.color;

        _dice = transform.parent.GetComponentInChildren<Dice>();

        _dice.FinishRolling += ShowModificator;
        _dice.StartRolling += HideModificator;

        _startModificatorPanelScale = transform.localScale;

        transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        _tween.Kill();
        _sequence.Kill();

        _dice.FinishRolling -= ShowModificator;
        _dice.StartRolling -= HideModificator;
    }

    private void ShowModificator()
    {
        m_valueText.transform.localPosition = _startTextPosition;
        m_valueText.color = _startTextColor;

        _tween.Kill();
        _sequence.Kill();
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOScale(_startModificatorPanelScale, _showModificatorDuration))
            .SetEase(Ease.OutBack)
            .Append(m_valueText.transform.DOMove(_dice.transform.position, _moveValueTextDuration))
            .Append(m_valueText.DOColor(Vector4.zero, 0f))
            .SetEase(Ease.Linear)
            .OnComplete(ModificateDiceValue);
    }

    private void HideModificator()
    {
        _tween.Kill();
        _sequence.Kill();

        transform.DOScale(Vector3.zero, _hideModificatorDuration)
            .SetEase(Ease.InBack);
    }

    private void ModificateDiceValue()
    {
        _dice.ApplyModificator(m_modificatorValue);
    }
}