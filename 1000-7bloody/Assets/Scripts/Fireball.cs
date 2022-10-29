using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _speed = 5f;
 

    [SerializeField] private GameObject _target;
    private Rigidbody2D _fireballRb;

    void Awake()
    {
        _fireballRb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (_target.transform.position - transform.position).normalized * _speed;
        _fireballRb.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerScript.Instance.gameObject)
        {
            PlayerScript.Instance.TakeDamage(_damage);
        }
        Destroy(gameObject, 0f);
    }
}
