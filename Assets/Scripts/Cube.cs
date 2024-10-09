using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]

public class Cube : MonoBehaviour
{
    private readonly string _materialPropertyName = "_Color";

    private Color _defaultColor;

    public bool IsColorChanged { get; private set; } = false;

    public event Action<Cube> BarrierTouched;

    public void SetColorChangedStatus(bool isChanged)
    {
        IsColorChanged = isChanged;
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.SetColor(_materialPropertyName, color);
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
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
    }

    private void Start()
    {
        _defaultColor = gameObject.GetComponent<Renderer>().material.GetColor(_materialPropertyName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Barrier barrier) && IsColorChanged == false)
        {
            BarrierTouched?.Invoke(this);
            Debug.Log("Wall touched");
        }
    }
}