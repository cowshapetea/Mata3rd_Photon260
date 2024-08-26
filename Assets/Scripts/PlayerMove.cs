using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 2f;

    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.TransformDirection(new Vector3(h, 0, v).normalized);
        //Vector3 dir = new Vector3(h, 0, v);
        //transform.position += (dir * moveSpeed * Time.deltaTime);
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
