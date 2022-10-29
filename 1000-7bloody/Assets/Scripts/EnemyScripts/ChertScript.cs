using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChertScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private AudioSource _fireballCastSound;

    [SerializeField] private GameObject _fireball;
    [SerializeField] private float _nextAttackTimer;
    [SerializeField] private float _attackTime;

    [SerializeField] private float _fireballSize; 

    [SerializeField] private GameObject _target;

    [SerializeField] private GameObject _fireballStartPosition;

    private Animator _animator;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }



    private void FixedUpdate()
    {
        if (transform.position.x < _target.transform.position.x)
        {
            transform.localScale = new Vector3(-0.7f, 0.7f, 1f);
        }
        else if (transform.position.x > _target.transform.position.x)
        {
            transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }

    }

    private void Update()
    {
        if (Time.time >= _nextAttackTimer) PreFireball();
    }



    private void PreFireball()
    {
        _animator.SetTrigger("AttackTrigger");
        _nextAttackTimer = Time.time + _attackTime;
    }

    private void CreateFireball()
    {

        _fireballCastSound.Play();
        GameObject newFireball = Instantiate(_fireball, _fireballStartPosition.transform.position, Quaternion.identity);
        newFireball.transform.localScale = transform.localScale;
    }
}
