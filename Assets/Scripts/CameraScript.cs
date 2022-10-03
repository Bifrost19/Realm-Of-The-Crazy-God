using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform characterTransform;
    void Update()
    {
        cameraTransform.position = new Vector3(characterTransform.position.x, characterTransform.position.y, characterTransform.position.z - 10f);
    }
}
