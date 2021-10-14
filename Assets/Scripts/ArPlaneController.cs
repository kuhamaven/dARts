using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ArPlaneController : MonoBehaviour
{
    private ARPlaneManager _mArPlaneManager;

    private void Awake()
    {
        _mArPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        PlaceObjectOnPlane.onPlacedObject += DisablePlaneDetection;
    }

    private void OnDisable()
    {
        PlaceObjectOnPlane.onPlacedObject -= DisablePlaneDetection;
    }

    private void DisablePlaneDetection()
    {
        SetAllPlanesActive(false);
        _mArPlaneManager.enabled = !_mArPlaneManager.enabled;
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in _mArPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}