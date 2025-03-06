using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{
    public GameObject mapObject; // Assign your map object in the Inspector
    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        AdjustCamera();
    }

    // Update is called once per frame
    void Update()
    {
        //Optional: Recalculate camera size on resize.
        if (Screen.width != mainCamera.pixelWidth || Screen.height != mainCamera.pixelHeight)
        {
            AdjustCamera();
        }
    }
    void AdjustCamera()
    {
        if (mapObject == null || mainCamera == null) return;

        BoxCollider2D mapCollider = mapObject.GetComponent<BoxCollider2D>();
        if (mapCollider == null)
        {
            Debug.LogError("Map object must have a BoxCollider2D component.");
            return;
        }

        Bounds mapBounds = mapCollider.bounds;
        float mapWidth = mapBounds.size.x;
        float mapHeight = mapBounds.size.y;

        float screenAspect = (float)Screen.width / (float)Screen.height;

        float cameraVerticalExtent = mapHeight / 2f;
        float cameraHorizontalExtent = mapWidth / 2f;

        float requiredVerticalSize = cameraVerticalExtent;
        float requiredHorizontalSize = cameraHorizontalExtent / screenAspect;

        mainCamera.orthographicSize = Mathf.Max(requiredVerticalSize, requiredHorizontalSize);

        mainCamera.transform.position = new Vector3(mapBounds.center.x, mapBounds.center.y, mainCamera.transform.position.z);
    }
}
