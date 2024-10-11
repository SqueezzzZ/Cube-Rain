using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private Color _defaultColor;
    private Renderer _renderer;
    private Rigidbody _rigitbody;
    private bool _isBarrierTouched = false;

    public event Action<Cube> BarrierTouched;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigitbody = GetComponent<Rigidbody>();
        _defaultColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        _rigitbody.velocity = Vector3.zero;
        _isBarrierTouched = false;
        SetColor(_defaultColor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isBarrierTouched == false)
        {
            if (collision.transform.TryGetComponent(out Barrier barrier))
            {
                BarrierTouched?.Invoke(this);
                _isBarrierTouched = true;
            }
        }
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}