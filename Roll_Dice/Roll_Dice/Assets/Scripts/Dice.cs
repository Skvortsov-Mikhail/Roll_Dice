using DG.Tweening;
using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Serializable]
    private class DiceNumber
    {
        public int number;
        public Vector3 rotation;
    }

    public Action<bool> DiceRolling;

    [SerializeField] private DiceNumber[] m_numbers;
    [SerializeField] private Transform m_pathPointsContainer;

    private int _targetNumber;
    private Vector3 _targetRotation;

    private Tween _tween;
    private Sequence _sequence;
    private Vector3[] _pathPositions;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;

        _pathPositions = new Vector3[m_pathPointsContainer.childCount + 1];

        for (int i = 0; i < _pathPositions.Length - 1; i++)
        {
            _pathPositions[i] = m_pathPointsContainer.GetChild(i).position;
        }

        _pathPositions[_pathPositions.Length - 1] = _startPosition;

        _sequence = DOTween.Sequence();
    }
    private void OnDestroy()
    {
        _sequence.Kill();
        _tween.Kill();
    }

    public void StartRollDice()
    {
        _targetNumber = UnityEngine.Random.Range(1, 21);

        foreach (var target in m_numbers)
        {
            if (target.number == _targetNumber)
            {
                _targetRotation = target.rotation;
            }
        }

        AnimateRolling(_targetRotation);

        DiceRolling?.Invoke(true);
    }

    private void AnimateRolling(Vector3 target)
    {
        print(_targetNumber);

        _tween.Kill();
        _sequence.Kill();
        _sequence = DOTween.Sequence();

        transform.DOPath(_pathPositions, 2f);

        _sequence.Append(transform.DORotate(new Vector3(10000f, 10000f, 10000f), 1f, RotateMode.FastBeyond360))
            .Append(transform.DORotate(target, 1f, RotateMode.FastBeyond360))
            .SetEase(Ease.OutSine)
            .OnComplete(() => DiceRolling?.Invoke(false));
    }
}
