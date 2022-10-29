using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbsScript : MonoBehaviour
{
    public float forcePower;

    private Rigidbody2D _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(transform.up * forcePower * 100);
        _rb.AddForce(transform.right * Random.Range(-1f, 1f) * forcePower * 100);

        StartCoroutine(DestroyLimbs());
    }

    private IEnumerator DestroyLimbs()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
