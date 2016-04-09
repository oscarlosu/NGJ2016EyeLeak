using UnityEngine;

[RequireComponent(typeof(GazePointDataComponent), typeof(UserPresenceComponent), typeof(Light))]
public class CameraController : MonoBehaviour {
    private GazePointDataComponent _gazePointDataComponent;
    private UserPresenceComponent _userPresenceComponent;
    private Light _lightComponent;

    // Exponential smoothing parameters, alpha must be between 0 and 1.
    [Range(0.1f, 1.0f)]
    public float alpha = 0.3f;
    private Vector2 _historicPoint;
    private bool _hasHistoricPoint;

    public Vector2 Margins;
    public float Speed;

    void Start() {
        _gazePointDataComponent = GetComponent<GazePointDataComponent>();
        _userPresenceComponent = GetComponent<UserPresenceComponent>();
        _lightComponent = GetComponent<Light>();
        Debug.Log(Screen.width + " " + Screen.height);
    }

    void Update() {
        var lastGazePoint = _gazePointDataComponent.LastGazePoint;

        if (_userPresenceComponent.IsValid && _userPresenceComponent.IsUserPresent && lastGazePoint.IsValid) {
            var gazePointInScreenSpace = GetNormalizedGazePosition(lastGazePoint);
            Debug.Log(gazePointInScreenSpace);

            //Vector3 cameraRotation = transform.localEulerAngles;
            // Look right
            if (gazePointInScreenSpace.x > 1.0f - Margins.x) {
                //transform.RotateAround(transform.position, Vector3.up, Speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(90, Vector3.up), Speed * Time.deltaTime);

            }
            // Look left
            else if (gazePointInScreenSpace.x < Margins.x) {
                //transform.RotateAround(transform.position, Vector3.up, -Speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-90, Vector3.up), Speed * Time.deltaTime);
            }
            // Look up
            if (gazePointInScreenSpace.y > 1.0f - Margins.y) {
                //transform.RotateAround(transform.position, Vector3.right, -Speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-45, Vector3.right), Speed * Time.deltaTime);
                //cameraRotation.x = Mathf.Clamp(cameraRotation.x, -45, 45);
            } 
            // Look down
            else if(gazePointInScreenSpace.y < Margins.y) {
                //transform.RotateAround(transform.position, Vector3.right, Speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(45, Vector3.right), Speed * Time.deltaTime);
                //cameraRotation.x = Mathf.Clamp(cameraRotation.x, -45, 45);
            }
            // Clamp
            //transform.localEulerAngles = cameraRotation;
            

            _lightComponent.enabled = true;
        } else {
            _lightComponent.enabled = false;
            _hasHistoricPoint = false;
        }
    }

    private Vector2 Smoothify(Vector2 point) {
        if (!_hasHistoricPoint) {
            _historicPoint = point;
            _hasHistoricPoint = true;
        }

        var smoothedPoint = new Vector2(point.x * alpha + _historicPoint.x * (1.0f - alpha),
            point.y * alpha + _historicPoint.y * (1.0f - alpha));

        _historicPoint = smoothedPoint;

        return smoothedPoint;
    }

    private Vector2 GetNormalizedGazePosition(EyeXGazePoint lastGazePoint) {
        // Get position in screen space
        Vector2 gazePointInScreenSpace = lastGazePoint.Viewport;
        // Smoothen
        Vector2 smoothedGazePoint = Smoothify(gazePointInScreenSpace);
        return smoothedGazePoint;
    }
}
