using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Offset : MonoBehaviour
{

    private LineRenderer lineRenderer;
    public GameObject objectToMove;

    private Vector3[] positions = new Vector3[3];
    private Vector3[] pos;
    Vector3 newpos = new Vector3(0.0f, 1.0f, 0.0f);
    float lastXVal;
    float initialPos;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pos = GetLinePointsInWorldSpace();
        lastXVal = pos[1].z;
        initialPos = objectToMove.transform.localPosition.z;


        //  objectToMove.transform.position = new Vector3(pos[1].x, objectToMove.transform.position.y, pos[1].z);

    }

    Vector3[] GetLinePointsInWorldSpace()
    {
        //Get the positions which are shown in the inspector 
        lineRenderer.GetPositions(positions);

        //the points returned are in world space
        return positions;
    }

    // Update is called once per frame
    void Update()

        
    {
        Move();

    }

    void Move()
    {

          pos = GetLinePointsInWorldSpace();


        float offset = pos[1].z + 0.25f;
       
          Debug.Log(offset);



        lastXVal = pos[1].z;

        objectToMove.transform.localPosition = new Vector3(objectToMove.transform.localPosition.x, objectToMove.transform.localPosition.y, initialPos + offset);
       // objectToMove.transform.localPosition = new Vector3(objectToMove.transform.localPosition.x, objectToMove.transform.localPosition.y, objectToMove.transform.localPosition.z);


    }
}
