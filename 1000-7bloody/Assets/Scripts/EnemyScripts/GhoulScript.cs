using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class GhoulScript : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;

    [SerializeField] private AudioSource _hurtSound;
    [SerializeField] private AudioSource _deathSound;

    [SerializeField] private GameObject _target;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;


    [Header("Hit Effects")]
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject bloodSplash;
    [SerializeField] private GameObject hitGlow;

    [Header("Death Effects")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject deathBloodSplash;
    [SerializeField] private GameObject deathGlow;

    [Header("CameraAnim")]
    [SerializeField] private Animator camAnim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _target = GameObject.FindGameObjectWithTag("Player");   
    }



    private void FixedUpdate()
    {
        ChaseTarget();

    }




    private void ChaseTarget()
    {
        if (transform.position.x < _target.transform.position.x -0.5f)
        {
            _sprite.flipX = true; 
        } 
        else if (transform.position.x > _target.transform.position.x + 0.5f)
        {
            _sprite.flipX = false;
        }

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_target.transform.position.x, transform.position.y), _speed * Time.deltaTime);
    }


}
