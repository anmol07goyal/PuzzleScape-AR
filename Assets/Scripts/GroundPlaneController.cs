using UnityEngine;
using Vuforia;

public class GroundPlaneController : MonoBehaviour
{
    [SerializeField] private PlaneFinderBehaviour _planeFinder;

    private bool placed = false;

    public void OnContentPlaced(GameObject obj)
    {
        if (!placed)
        {
            placed = true;

            // Disable plane finder so it stops searching
            if (_planeFinder != null)
                _planeFinder.gameObject.SetActive(false);
        }
    }

    public void ResetPlaneFinder()
    {
        placed = false;
        // Enable plane finder to start searching again
        if (_planeFinder != null)
            _planeFinder.gameObject.SetActive(true);
    }
}
