using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCylinder : MonoBehaviour
{
    // Reference to the material of the cylinder
    private Material cylinderMaterial;

    // Transparency value (0 for fully transparent, 1 for fully opaque)
    [Range(0.0f, 1.0f)]
    public float transparency = 0.0f;

    void Start()
    {
        // Ensure there is a MeshRenderer component on the GameObject
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on this GameObject.");
            return;
        }

        // Get the material from the MeshRenderer
        cylinderMaterial = meshRenderer.material;

        // Set the initial transparency
        SetTransparency(transparency);
    }

    void Update()
    {
        // You can update transparency dynamically if needed
        // For example, you could make it change over time.
        // transparency = Mathf.PingPong(Time.time * 0.5f, 1.0f);
        // SetTransparency(transparency);
    }

    // Function to set transparency
    void SetTransparency(float alpha)
    {
        // Ensure the material is not null
        if (cylinderMaterial != null)
        {
            // Set the alpha value of the material color
            Color materialColor = cylinderMaterial.color;
            materialColor.a = alpha;
            cylinderMaterial.color = materialColor;
        }
    }
}
