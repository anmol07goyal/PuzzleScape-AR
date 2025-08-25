using UnityEngine;

public class PointToObject : MonoBehaviour
{
    [SerializeField] private Transform _3dArrow;
    [SerializeField] private Transform _currentModel;

    private bool _objectVisibility;

    private void OnEnable()
    {
        CheckVisibility.ObjectVisibility += ObjectVisible;
    }

    private void ObjectVisible(bool visible, Transform objectT)
    {
        _objectVisibility = visible;
        _currentModel = objectT;
    }

    private void Update()
    {
        // show the 3d object
        if (!_objectVisibility && _currentModel != null)
            _3dArrow.LookAt(_currentModel.transform);
    }

    private void OnDisable()
    {
        CheckVisibility.ObjectVisibility -= ObjectVisible;
    }
}