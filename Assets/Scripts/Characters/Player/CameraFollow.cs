using UnityEngine; 

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] [Range(1f, 2f)] private float _zoom;
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 6f;
    [SerializeField] private float _maxDistance = 1.5f;

    private float _defaultDist;

    private void Start()
    {
        UpdateZoom();
    }

    private void Update()
    {
        Vector3 dest = _player.position + (_offset * _zoom);

        if (dest == transform.position)
            return;

        float dist = Mathf.Abs(Vector3.Distance(transform.position, _player.position) - _defaultDist);
        float cameraSpeed = CustomMathFunctions.remap(0f, _maxDistance, _minSpeed, _maxSpeed, dist);

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, dest, step);

        if(Vector3.Distance(transform.position, dest) <= 0.001f)
        {
            transform.position = dest;
        }
    }

    private void UpdateZoom()
    {
        transform.position = _player.position + (_offset * _zoom);
        _defaultDist = Vector3.Distance(transform.position, _player.position);
    }
}
