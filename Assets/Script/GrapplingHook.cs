using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    DistanceJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;

    CharacterController2D carController;

    public float distance = 10f;
    public float step = 0.2f;
    public bool release = false;

    public bool avvolgi = false;
    public bool allunga = false;

    public LineRenderer Line;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        carController = GetComponent<CharacterController2D>();
        joint.enabled = false;
        Line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            avvolgi = true;
        }else if (Input.GetMouseButtonUp(0))
        {
            avvolgi = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            allunga = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            allunga = false;
        }

        if (avvolgi)
        {
            if (joint.distance > 0.1f)
            {
              
                joint.distance -= step * Time.deltaTime * 5;                  
                carController.m_AirControl = false;
            }
            else
            {
                Line.enabled = false;
                joint.enabled = false;
            }
        }

        if (allunga)
        {
            if (joint.distance > 0.1f)
            {
                if (!carController.m_Grounded) {
                    joint.distance += step * Time.deltaTime * 5;
                }
                    
            }
            else
            {
                Line.enabled = false;
                joint.enabled = false;
            }
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            hit = Physics2D.Raycast(transform.position,targetPos-transform.position,distance,mask);

            if(hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                joint.enabled = true;
                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                joint.distance = Vector2.Distance(transform.position, hit.point);
                carController.m_AirControl = false;




                Line.enabled = true;
                Line.SetPosition(0, transform.position);
                Line.SetPosition(1, hit.point);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Line.SetPosition(0, transform.position);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            joint.enabled = false;
            Line.enabled = false;            
        }
       
    }
}
