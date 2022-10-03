using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public Transform character;
    public static float piRelation = 57.32484f;
    void Update()
    {
        if(Input.GetKey("w"))
        {
            Vector3 moveVec = findVec('w');
            character.position += moveVec * Time.deltaTime * Player.getSpeed() / 4;
            FixSlotPosVectors(moveVec);
            TreeGenerationScript.LayerTrees();
        }
        if (Input.GetKey("s"))
        {
            Vector3 moveVec = findVec('s');
            character.position += moveVec * Time.deltaTime * Player.getSpeed() / 4;
            FixSlotPosVectors(moveVec);
            TreeGenerationScript.LayerTrees();
        }
        if (Input.GetKey("a"))
        {
            Vector3 moveVec = findVec('a');
            character.position += moveVec * Time.deltaTime * Player.getSpeed() / 4;
            FixSlotPosVectors(moveVec);
            TreeGenerationScript.LayerTrees();
        }
        if (Input.GetKey("d"))
        {
            Vector3 moveVec = findVec('d');
            character.position += moveVec * Time.deltaTime * Player.getSpeed() / 4;
            FixSlotPosVectors(moveVec);
            TreeGenerationScript.LayerTrees();
        }
    }

    void FixSlotPosVectors(Vector3 offsetVec)
    {
        for (int i = 0; i < 10; i++)
        {
            EquippingScript.slotList[i].setPos(EquippingScript.slotList[i].getPos() + offsetVec * Time.deltaTime * Player.getSpeed() / 4);
        }
    }

    Vector3 findVec(char dir)
    {
        float x, y;
        float angle = character.eulerAngles.z / piRelation;
        switch (dir)
        {
            case 'w':
                y = Mathf.Cos(angle);
                if(Mathf.Sin(angle) < 0)
                x = Mathf.Sqrt(1 - y * y);
                else
                  x = -Mathf.Sqrt(1 - y * y);
                return new Vector3(x, y, 0);

            case 's':
                y = -Mathf.Cos(angle);
                if (Mathf.Sin(angle) >= 0)
                    x = Mathf.Sqrt(1 - y * y);
                else
                    x = -Mathf.Sqrt(1 - y * y);
                return new Vector3(x, y, 0);

            case 'a':
                x = -Mathf.Cos(angle);
                if (Mathf.Sin(angle) < 0)
                    y = Mathf.Sqrt(1 - x * x);
                else
                    y = -Mathf.Sqrt(1 - x * x);
                return new Vector3(x, y, 0);

            case 'd':
                x = Mathf.Cos(angle);
                if (Mathf.Sin(angle) >= 0)
                    y = Mathf.Sqrt(1 - x * x);
                else
                    y = -Mathf.Sqrt(1 - x * x);
                return new Vector3(x, y, 0);
        }
        return Vector3.zero;
    }
}
