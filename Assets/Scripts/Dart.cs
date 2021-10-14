using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Dart : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private GameObject _dartThrowPoint;
    public bool isForceOk;
    private bool _isDartRotating;
    private bool _isDartReadyToShoot = true;
    private bool _isDartOnBoard;

    private ARSessionOrigin _arSessionOrigin;
    private GameObject _arCam;

    public Collider dartFrontCollider;

    private SoundManager _soundManager;

    private void Start()
    {
        _arSessionOrigin = GameObject.FindGameObjectWithTag("AR Session Origin").GetComponent<ARSessionOrigin>();
        _arCam = _arSessionOrigin.transform.Find("AR Camera").gameObject;

        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _dartThrowPoint = GameObject.FindGameObjectWithTag("Dart Throw Point");

        _soundManager = GameObject.FindGameObjectWithTag("Sound Manager").GetComponent<SoundManager>();
        if(_soundManager) _soundManager.PlayDartReady();
    }

    private void FixedUpdate()
    {
        if (isForceOk)
        {
            dartFrontCollider.enabled = true;
            StartCoroutine(InitDartDestroyVFX());
            GetComponent<Rigidbody>().isKinematic = false;
            isForceOk = false;
            _isDartRotating = true;
        }

        _rigidBody.AddForce(_dartThrowPoint.transform.forward * ((12f + 6f) * Time.deltaTime), ForceMode.VelocityChange);

        if (_isDartReadyToShoot)
        {
            transform.Rotate(Vector3.forward * (Time.deltaTime * 20f));
        }

        if (_isDartRotating)
        {
            _isDartReadyToShoot = false;
            transform.Rotate(Vector3.forward * (Time.deltaTime * 400f));
        }
    }

    private IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(5f);
        if (!_isDartOnBoard)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dart_board") && !_isDartOnBoard)
        {
            Handheld.Vibrate();
            if(_soundManager) _soundManager.PlayDartHit();

            GetComponent<Rigidbody>().isKinematic = true;
            _isDartRotating = false;

            _isDartOnBoard = true;
            other.GetComponent<DartBoardSpace>().ResolveScore(this);
        }
    }
}