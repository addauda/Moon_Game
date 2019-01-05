using System;
using UnityEngine;
using System.Collections;

public class Space_Daddy : MonoBehaviour {
    private Animator anim;
    private Animation animate;
    private Rigidbody rigidBody;
    private CharacterController controller;
    [SerializeField] float jumpForce = 20f;

    public float speed = 600.0f;
    public float turnSpeed = 400.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 20.0f;
    public Vector3 jump;
    private bool groundedState;
    public Collider[] colliderBoxes;
    private static readonly int AnimationPar = Animator.StringToHash("AnimationPar");

    private void Start () {
        controller = GetComponent <CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        animate = gameObject.GetComponentInChildren<Animation>();
        groundedState = CheckGroundCollision(colliderBoxes[0]);
    }

    private void Update (){

        if (Input.GetKey ("w") || Input.GetKey("s"))
        {
            anim.SetInteger(AnimationPar, anim.GetCurrentAnimatorStateInfo(0).IsName("Run") ? 0 : 1);
        }
        else
        {
            anim.SetInteger(AnimationPar, 0);
        }
        if(groundedState)
        {
            moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
            Jumping();
        }
            Flip();
        if (!groundedState)
        {
            anim.Play("Jump_loop");
        }
        var turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;
    
    }
    private void FixedUpdate()
    {
        groundedState = CheckGroundCollision(colliderBoxes[0]);
        CheckAttackCollision(colliderBoxes[1]);
        Debug.Log(groundedState);
    }

    private void SetGroundedState()
    {
        groundedState = CheckGroundCollision(colliderBoxes[0]);
        CheckAttackCollision(colliderBoxes[1]);
        Debug.Log(groundedState);
    }


    private bool CheckGroundCollision(Collider col)
    {
        var transform1 = col.transform;
        Collider[] cols = Physics.OverlapBox(transform1.position, transform1.localScale / 2, Quaternion.identity, LayerMask.GetMask("Terrain", "Base"));
        {
            foreach (Collider c in cols)
            {
                Debug.Log(c.name + gameObject);
            }

        }
        if (cols.Length > 0 && !groundedState)
        {
            anim.Play("Jump_end");
        }
        return cols.Length > 0;
    }


    private void CheckAttackCollision(Collider col)
    {

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var transform1 = transform;
        Gizmos.DrawWireCube(transform1.position, transform1.localScale / 2);
    }

    private void Jumping()
    {
        if (!Input.GetKeyDown("space")) return;
        anim.Play("Jump_start");
        moveDirection.y += jumpForce;
    }
    private void Flip()
    {
        if (Input.GetKeyDown("f") && !groundedState)
        {
            anim.Play("Flip");
        }
    }
    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
