using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
public class CameraMovingScript : MonoBehaviour
{
    public float dragSpeed = 10; // Speed of camera movement when dragging
    public float zoomSpeed = 2;
    private bool isDragging = false;
    private Vector3 dragOrigin;
    private CinemachineVirtualCamera virtualCamera;
    private float zoomAmount;
    private void Start()
    {
        virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        zoomAmount = virtualCamera.m_Lens.OrthographicSize;
    }
    void Update()
    {
        //Obtener la scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        //Restar o sumar si se hace zoom
        virtualCamera.m_Lens.OrthographicSize -= scroll * zoomSpeed;
        //Que no se vaya alv
        virtualCamera.m_Lens.OrthographicSize = Mathf.Max(virtualCamera.m_Lens.OrthographicSize, 0.5f);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Min(virtualCamera.m_Lens.OrthographicSize, 12f);
        if (Input.GetMouseButtonDown(1))
        {
            GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineVirtualCamera>().Follow = null;
            isDragging = true;
            dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);
            transform.Translate(-move, Space.World);
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            Vector3 moveH = new Vector3(Input.GetAxis("Horizontal") * dragSpeed * Time.deltaTime, 0, 0);
            transform.Translate(moveH, Space.World);

            if (Input.GetAxis("Vertical") != 0)
            {
                Vector3 moveV = new Vector3(0, Input.GetAxis("Vertical") * dragSpeed * Time.deltaTime, 0);
                transform.Translate(moveV, Space.World);
            }
        }
    }
}
