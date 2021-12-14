using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /// <summary>移動する時にかける力</summary>
    [SerializeField] float _movePower = 3f;
    /// <summary>ジャンプする力</summary>
    [SerializeField] float _jumpPower = 15f;
    Rigidbody2D _rb = default;
    /// <summary>接地フラグ</summary>
    bool _isGrounded = true;

    Vector3 _initialPosition = default;
    Animator _anim = default;
    SpriteRenderer _sprite = default;
    float _h = 0;
    int _JumpCount = 0;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _initialPosition = this.transform.position;
    }

    void Update()
    {
        _h = Input.GetAxis("Horizontal");


        // ジャンプ処理
        if (_JumpCount < 2 && Input.GetButtonDown("Jump"))
        {
            _isGrounded = false;

            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _JumpCount++;

        }

        //画面外に落ちたら初期位置に戻す
        if (this.transform.position.y < -15)
        {
            this.transform.position = _initialPosition;
        }
    }

    void FixedUpdate()
    {
        _rb.AddForce(_h * _movePower * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _JumpCount = 0;
            _isGrounded = true;
        }
    }


    private void LateUpdate()
    {
        // キャラクターの左右の向きを制御する
        if (_h != 0)
        {
            _sprite.flipX = (_h > 0);
        }
        // アニメーションを制御する
        if (_anim)
        {
            _anim.SetFloat("SpeedX", Mathf.Abs(_rb.velocity.x));
            //m_anim.SetFloat("SpeedY", m_rb.velocity.y);
            _anim.SetBool("IsGround", _isGrounded);
        }
    }

}
