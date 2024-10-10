using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private Color _defaultColor;
    private Renderer _renderer;
    private Rigidbody _rigitbody;

    public bool IsColorChanged { get; private set; } = false;

    public event Action<Cube> BarrierTouched;

    public void SetColorChangedStatus(bool isChanged)
    {
        IsColorChanged = isChanged;
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void SetDefaultColor()
    {
        SetColor(_defaultColor);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigitbody.velocity = velocity;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigitbody = GetComponent<Rigidbody>();
        _defaultColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Barrier barrier) && IsColorChanged == false)
        {
            BarrierTouched?.Invoke(this);
        }
    }
}