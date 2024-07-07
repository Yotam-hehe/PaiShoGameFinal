using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distanceToGround = 0;

            // Ensure the object is not dragged below the ground
            if (new Plane(Vector3.up, Vector3.zero).Raycast(ray, out distanceToGround))
            {
                Vector3 newPosition = ray.GetPoint(distanceToGround);
                transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}