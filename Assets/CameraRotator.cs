using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float rotationStep = 1f;
    [SerializeField] private Vector2 xRotationClamp = new (30, 90);
    [SerializeField] private float zoomStep = 1f;
    [SerializeField] private Vector2 zoomClamp = new(3, 7);
    private GameObject _cameraChild;
    private Camera _cameraComponent;

    private void Start()
    {
        _cameraChild = gameObject.transform.Find("Main Camera").gameObject;
        _cameraComponent = _cameraChild.GetComponent<Camera>();
    }

    private void Update()
    {
        var cameraChildRotation = _cameraChild.transform.rotation.eulerAngles;
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            gameObject.transform.Rotate(0, rotationStep * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            gameObject.transform.Rotate(0, -rotationStep * Time.deltaTime, 0);
        
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            _cameraChild.transform.Rotate(
                ClampValue(
                    rotationStep * Time.deltaTime,
                    cameraChildRotation.x, 
                    xRotationClamp.x, 
                    xRotationClamp.y), 
                0, 
                0
                );
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            _cameraChild.transform.Rotate(
                ClampValue(
                    -rotationStep * Time.deltaTime, 
                    cameraChildRotation.x, 
                    xRotationClamp.x, 
                    xRotationClamp.y), 
                0, 
                0
            );

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
           _cameraComponent.orthographicSize += ClampValue(
               -Input.GetAxis("Mouse ScrollWheel") * zoomStep * Time.deltaTime,
               _cameraComponent.orthographicSize,
               zoomClamp.x,
               zoomClamp.y
               );
    }

    private float ClampValue(float value, float baseValue, float min, float max)
    {
        if (baseValue + value < min || baseValue + value > max)
            return 0;
        return value;
    }
}