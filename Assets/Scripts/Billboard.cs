using UnityEditor.TerrainTools;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    private Transform attachedObject;
    public Vector3 Offset = new Vector3( 0f, 0f, 0f );
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attachedObject = transform.parent;
        mainCamera = Camera.main;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = attachedObject.position + Offset;
        transform.LookAt(transform.position + (transform.position - mainCamera.transform.position));
    }
}
