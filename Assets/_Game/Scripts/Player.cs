using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 0.9f;
    [SerializeField] private float jumpForce = 100;
    [SerializeField] private float lazer = 0.578f;
    [SerializeField] private Animator anim;
    private string currentAnim;
    private bool isGrounded = true;
    private bool isJumping= false;
    private bool isAttack = false;
    private bool isDeath = false;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded();
        /*Movement*/
        horizontal = Input.GetAxisRaw("Horizontal");
        if(isGrounded && !isAttack)
        {
            /*Jump*/
            if (Input.GetKey(KeyCode.UpArrow))
            {
                isJumping = true;
                ChangeAnim("jumpIn");
                rb.AddForce(jumpForce * Vector2.up);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                ChangeAnim("crouch");
            }
            //Run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            //Attack
            if (Input.GetKey(KeyCode.Space))
            {
                Attack();
            }
            //Throw
            if (Input.GetKey(KeyCode.C))
            {
                FastAttack();
        }
}
        if (!isGrounded && rb.velocity.y < 0)
        {
            isJumping = false;
            ChangeAnim("jumpOut");
        }
        if (!isAttack)
        { 
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            }
            else if (isGrounded)
            {
                ChangeAnim("idle");
                rb.velocity = Vector3.zero;
            }
        }
    }
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * lazer, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, lazer, groundLayer);
        return hit.collider != null;
    }
    private void ChangeAnim(string animName)
    {
        if(currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
    private void ResetAttack()
    {
        isAttack = false;
        rb.isKinematic = false;
        ChangeAnim("Idle");
    }
    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void FastAttack()
    {
        ChangeAnim("fastAttack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 1f);
    }
    //private void Jump()
    //{
    //    if (isGrounded)
    //    {
    //        isJumping = true;
    //        ChangeAnim("jumpIn");
    //        rb.AddForce(jumpForce * Vector2.up);
    //    }
    //}
}
