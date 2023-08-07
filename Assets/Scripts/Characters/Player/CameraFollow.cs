using UnityEngine; 

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _maxDistance = 1.5f;
    [SerializeField] private float _zoom = 8f;
    [SerializeField] [Range(0, 1)] private float rotation;

    private float _defaultDist;
    private Vector3 _dest;
    private float _previousRotation;
    private float _previousZoom;

    private Quaternion targetRot;

    private void Update()
    {
        ApplyRotation();
        SmoothFollow();
    }

    private void SmoothFollow()
    {
        if (_dest == transform.position)
            return;

        _defaultDist = Vector3.Distance(_dest, _player.position);

        float dist = Mathf.Abs(Vector3.Distance(transform.position, _player.position) - _defaultDist);
        float cameraSpeed = CustomMathFunctions.remap(0f, _maxDistance, _minSpeed, _maxSpeed, dist);

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _dest, step);

        if (Vector3.Distance(transform.position, _dest) <= 0.001f)
        {
            transform.position = _dest;
        }
    }

    private void ApplyRotation()
    {

        float degPos = rotation * CustomMathFunctions.Tau;
        _dest = new Vector3(Mathf.Cos(degPos) * _zoom + _player.position.x, transform.position.y, Mathf.Sin(degPos) * _zoom + _player.position.z);

        if ((_previousRotation != rotation || targetRot != transform.rotation) || _zoom != _previousZoom)
        {
            SmoothLookAt();
            _previousRotation = rotation;
            _previousZoom = _zoom;
        }

    }

    private void SmoothLookAt()
    {
        targetRot = Quaternion.LookRotation(_player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
    }
}
