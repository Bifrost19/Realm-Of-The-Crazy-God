using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationScript : MonoBehaviour
{
    public float rotAngle;

    void Update()
    {
        if (ShootingScript.isRotating) Rotate();    
    }

    public void Rotate()
    {
        gameObject.transform.Rotate(0, 0, -rotAngle);
        Invoke("RotateBackwards", 0.1f);
        ShootingScript.isRotating = false;
    }

    void RotateBackwards()
    {
        gameObject.transform.Rotate(0, 0, rotAngle);
    }
 
}
