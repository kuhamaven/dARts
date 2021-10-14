using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DartController : MonoBehaviour
{
    public GameObject dartPrefab;
    public Transform dartThrowPoint;
    private ARSessionOrigin _arSessionOrigin;
    private GameObject _arCam;
    private GameObject _dartTemp;
    private Rigidbody _rigidBody;

    
    private Transform _dartBoardObj;
    private bool _isDartBoardSearched;
    private float _distanceFromBoard;
    public TMP_Text textDistance;
    
    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        _arCam = _arSessionOrigin.transform.Find("AR Camera").gameObject;
    }

    private void OnEnable()
    {
        PlaceObjectOnPlane.onPlacedObject += DartsInit;
    }

    private void OnDisable()
    {
        PlaceObjectOnPlane.onPlacedObject -= DartsInit;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("dart"))
                {
                    raycastHit.collider.enabled = false;
                    _dartTemp.transform.parent = _arSessionOrigin.transform;

                    Dart currentDartScript = _dartTemp.GetComponent<Dart>();
                    currentDartScript.isForceOk = true;
                    
                    DartsInit();
                }
            }
        }

        if (_isDartBoardSearched)
        {
            _distanceFromBoard = Vector3.Distance(_dartBoardObj.position, _arCam.transform.position);
            textDistance.text = _distanceFromBoard.ToString().Substring(0, 3);
        }
    }

    void DartsInit()
    {
        _dartBoardObj = GameObject.FindWithTag("dart_board").transform;
        if (_dartBoardObj)
        {
            _isDartBoardSearched = true;
        }
        StartCoroutine(WaitAndSpawnDart());
    }

    public IEnumerator WaitAndSpawnDart()
    {
        yield return new WaitForSeconds(0.1f);
        _dartTemp = Instantiate(dartPrefab, dartThrowPoint.position, _arCam.transform.localRotation);
        _dartTemp.transform.parent = _arCam.transform;
        _rigidBody = _dartTemp.GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
    }
}