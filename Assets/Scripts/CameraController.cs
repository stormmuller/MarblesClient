using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public float speed;

    public float mapY = 100.0f;
    public float mapX = 100.0f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
     
    void Start()
    {
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - horzExtent;
        minY = vertExtent - mapY / 2.0f;
        maxY = mapY / 2.0f - vertExtent;

        print(minX);
    }

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * speed;
    }

    private void LateUpdate()
    {
        var v3 = transform.position;
        v3.x = Mathf.Clamp(v3.x, minX, maxX);
        v3.y = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = v3;
    }
}
