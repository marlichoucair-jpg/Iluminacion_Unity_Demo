using UnityEngine;

public class StickToTheGround : MonoBehaviour
{
    float GetHeightAtPosition(float x, float z)
    {
        return Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 5f;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = GetHeightAtPosition(pos.x, pos.z) + 0.1f;
        transform.position = pos;
    }
}
