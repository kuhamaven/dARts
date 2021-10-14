using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    private Pose _placementPose;
    private Transform _placementTransform;
    private bool _placementPoseIsValid;
    private bool _isObjectPlaced;
    private TrackableId _placedPlaneId = TrackableId.invalidId;

    private ARRaycastManager _mRayCastManager;
    private static List<ARRaycastHit> _sHits = new List<ARRaycastHit>();

    public static event Action onPlacedObject;

    private void Awake()
    {
        _mRayCastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (!_isObjectPlaced)
        {
            UpdatePlacementPosition();
            UpdatePlacementIndicator();

            if (_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void PlaceObject()
    {
        //Si definimos un objeto a colocar, lo instanciamos e invocamos su metodo.
        
        if (objectToPlace)
        {
            Instantiate(objectToPlace, _placementPose.position, _placementTransform.rotation);
            onPlacedObject?.Invoke(); //Llamamos a los que se suscribieron al metodo.
            _isObjectPlaced = true;
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPosition()
    {
        
        //Aca revisamos los planos, y si estamos mirando a almenos uno, tomamos sus datos para usarlos en el placement.
        
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (_mRayCastManager.Raycast(screenCenter, _sHits, TrackableType.PlaneWithinPolygon))
        {
            _placementPoseIsValid = _sHits.Count > 0;
            if (_placementPoseIsValid)
            {
                _placementPose = _sHits[0].pose;
                _placedPlaneId = _sHits[0].trackableId;

                var planeManager = GetComponent<ARPlaneManager>();
                ARPlane arPlane = planeManager.GetPlane(_placedPlaneId);
                _placementTransform = arPlane.transform;
            }
        }
    }

    private void UpdatePlacementIndicator()
    {
        
        //Si la ubicacion es valida, mostramos el indicador y actualizamos su posicipn. Si no, lo desactivamos.
        
        if (_placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementTransform.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }
}