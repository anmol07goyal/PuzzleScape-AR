using UnityEngine;
using System;

public class CameraCollider : MonoBehaviour
{
    public static CameraCollider instance;
    public static Action CameraInRangeToView;

    [SerializeField] private LayerMask _modelLayer;

    private bool _triggerHit, _raycastHit, _once;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _raycastHit = false;
        _triggerHit = false;
        _once = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalCheck"))
        {
            _triggerHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FinalCheck"))
        {
            _triggerHit = false;
        }
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15, _modelLayer))
        {
            _raycastHit = hit.collider.CompareTag("FinalCheck");
        }

        if (_triggerHit && _raycastHit && !_once)
        {
            _once = true;
            CameraInRangeToView?.Invoke();
        }
    }

    public void BackToSearch()
    {
        Start();
    }
}