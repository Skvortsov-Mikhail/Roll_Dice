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

    public Action StartRolling;
    public Action FinishRolling;
    public Action ReadyToNextRoll;

    [SerializeField] private DiceNumber[] m_numbers;
    [SerializeField] private Transform m_pathPointsContainer;

    private AudioSource _audio;

    private int _targetNumber;
    private Vector3 _targetRotation;

    private Tween _tween;
    private Sequence _sequence;

    private Vector3[] _pathPositions;
    private Vector3 _startPosition;

    private Vector3 _animateRotationVector = new Vector3(10000f, 10000f, 10000f);
    private Vector3 _shakePositionVector = new Vector3(20f, 20f, 20f);

    private float _pathDuration = 2f;
    private float _shakeDuration = 1f;

    private void Start()
    {
        _audio = GetComponentInChildren<AudioSource>();

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

        SetTargetRotation();

        AnimateRolling();

        _audio.Play();

        StartRolling?.Invoke();
    }

    public void ApplyModificator(int bonusValue)
    {
        _targetNumber = Mathf.Clamp(_targetNumber + bonusValue, 1, 20);

        SetTargetRotation();

        AnimateModification();
    }

    private void SetTargetRotation()
    {
        foreach (var target in m_numbers)
        {
            if (target.number == _targetNumber)
            {
                _targetRotation = target.rotation;
            }
        }
    }

    private void AnimateRolling()
    {
        _tween.Kill();
        _sequence.Kill();
        _sequence = DOTween.Sequence();

        transform.DOPath(_pathPositions, _pathDuration, PathType.CatmullRom);

        _sequence.Append(transform.DORotate(_animateRotationVector, _pathDuration / 2, RotateMode.FastBeyond360))
            .Append(transform.DORotate(_targetRotation, _pathDuration / 2, RotateMode.FastBeyond360))
            .SetEase(Ease.OutSine)
            .OnComplete(() => FinishRolling?.Invoke());
    }

    private void AnimateModification()
    {
        _tween.Kill();

        transform.DOShakePosition(_shakeDuration, _shakePositionVector);

        transform.rotation = Quaternion.Euler(_targetRotation);

        ReadyToNextRoll?.Invoke();
    }
}