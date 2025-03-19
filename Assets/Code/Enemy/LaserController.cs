using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Transform startPoint; // The starting point of the laser (orb)
    public Transform endPoint;   // The ending point of the laser (Lich)
    public float cubeSize = 0.1f; // Size of each cube in the laser
    public Material laserMaterial; // Material for the laser cubes

    private GameObject[] laserCubes; // Array to hold the laser cubes
    private bool isLaserActive = true; // Whether the laser is active

    void Start()
    {
        CreateLaser();
    }

    void Update()
    {
        if (isLaserActive)
        {
            UpdateLaser();
        }
    }

    // Create the laser using small cubes
    private void CreateLaser()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Laser start or end point is not set!");
            return;
        }

        // Calculate the distance between the start and end points
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // Calculate the number of cubes needed
        int cubeCount = Mathf.CeilToInt(distance / cubeSize);

        // Initialize the laserCubes array
        laserCubes = new GameObject[cubeCount];

        // Create the laser cubes
        for (int i = 0; i < cubeCount; i++)
        {
            laserCubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            laserCubes[i].transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
            laserCubes[i].GetComponent<Renderer>().material = laserMaterial;
            laserCubes[i].transform.parent = transform; // Parent the cubes to the laser object
        }

        // Update the laser cubes' positions
        UpdateLaser();
    }

    // Update the laser cubes' positions and scales
    private void UpdateLaser()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Laser start or end point is not set!");
            return;
        }

        // Calculate the direction and distance between the start and end points
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        // Debug laser path
        Debug.DrawLine(startPoint.position, endPoint.position, Color.red, 0.1f);

        // Update the position and scale of each cube
        for (int i = 0; i < laserCubes.Length; i++)
        {
            float t = (float)i / (laserCubes.Length - 1);
            Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, t) + Vector3.up * cubeSize * 0.5f;
            laserCubes[i].transform.position = position;

            // Rotate the cube to align with the laser direction
            laserCubes[i].transform.LookAt(endPoint);
        }
    }

    // Set the start and end points of the laser
    public void SetTargets(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
        UpdateLaser();
    }

    // Disable the laser
    public void DisableLaser()
    {
        isLaserActive = false;

        // Disable all laser cubes
        foreach (var cube in laserCubes)
        {
            cube.SetActive(false);
        }
    }

    // Enable the laser
    public void EnableLaser()
    {
        isLaserActive = true;

        // Enable all laser cubes
        foreach (var cube in laserCubes)
        {
            cube.SetActive(true);
        }

        // Update the laser to ensure it's correctly positioned
        UpdateLaser();
    }
}
