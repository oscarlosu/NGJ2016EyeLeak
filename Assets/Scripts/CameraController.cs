using UnityEngine;

[RequireComponent(typeof(GazePointDataComponent), typeof(UserPresenceComponent))]
public class CameraController : MonoBehaviour {
    private GazePointDataComponent _gazePointDataComponent;
    private UserPresenceComponent _userPresenceComponent;
    //private Rigidbody rb;

    // Exponential smoothing parameters, alpha must be between 0 and 1.
    [Range(0.1f, 1.0f)]
    public float alpha = 0.3f;
    private Vector2 _historicPoint;
    private bool _hasHistoricPoint;

    public Vector2 Margins;
    public float Speed;
    public AnimationCurve EyeAcceleration;
    [Range(0.1f, 90.0f)]
    public float HorizontalMaxAngle;
    [Range(0.1f, 90.0f)]
    public float VerticalMaxAngle;

    private Vector2 lookingPos;


    public enum LookingState {
        Right,
        Left,
        Up,
        Down,
        Center
    }
    public LookingState CurrentState;
    private float elapsedTime;

    void Start() {
        _gazePointDataComponent = GetComponent<GazePointDataComponent>();
        _userPresenceComponent = GetComponent<UserPresenceComponent>();

        CurrentState = LookingState.Center;
    }


    private void HandleState() {
        LookingState newState = GetNewLookingState();
        if(CurrentState == LookingState.Center || newState == LookingState.Center) {
            elapsedTime = 0;
        }
        CurrentState = newState;
        elapsedTime += Time.deltaTime;
    }

    private LookingState GetNewLookingState() {
        if(lookingPos.x > 1.0f - Margins.x) {
            return LookingState.Right;
        } else if(lookingPos.x < Margins.x) {
            return LookingState.Left;
        } else if (lookingPos.y > 1.0f - Margins.y) {
            return LookingState.Up;
        } else if (lookingPos.y < Margins.y) {
            return LookingState.Down;
        } else {
            return LookingState.Center;
        }
    }

    public bool IsLookingRight() {
        return lookingPos.x > 1.0f - Margins.x;
    }
    public bool IsLookingLeft() {
        return lookingPos.x < Margins.x;
    }
    public bool IsLookingUp() {
        return lookingPos.y > 1.0f - Margins.y;
    }
    public bool IsLookingDown() {
        return lookingPos.y < Margins.y;
    }

    private void LookAround() {        
        // Horizontal
        if(IsLookingRight()) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(HorizontalMaxAngle, Vector2.up), Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)) * Time.deltaTime);
            //rb.angularVelocity = new Vector3(rb.angularVelocity.x, Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)), rb.angularVelocity.z);
        } else if(IsLookingLeft()) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-HorizontalMaxAngle, Vector2.up), Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)) * Time.deltaTime);
            //rb.angularVelocity = new Vector3(rb.angularVelocity.x, -Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)), rb.angularVelocity.z);
        }
        // Vertical
        if (IsLookingUp()) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-VerticalMaxAngle, transform.right), Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)) * Time.deltaTime);
            //rb.angularVelocity = new Vector3(-Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)), rb.angularVelocity.y, rb.angularVelocity.z);
        } else if (IsLookingDown()) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(VerticalMaxAngle, transform.right), Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)) * Time.deltaTime);
            //rb.angularVelocity = new Vector3(Speed * EyeAcceleration.Evaluate(Mathf.Clamp(elapsedTime, 0.0f, 1.0f)), rb.angularVelocity.y, rb.angularVelocity.z);
        }
    }


    void Update() {
        EyeXGazePoint gaze = _gazePointDataComponent.LastGazePoint;
        if (_userPresenceComponent.IsValid && _userPresenceComponent.IsUserPresent && gaze.IsValid) {
            // Save gaze position
            lookingPos = GetNormalizedGazePosition(gaze);
            // Change state and handle elapsed time
            HandleState();
            // Change camera rotation
            LookAround();
        } else {
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
