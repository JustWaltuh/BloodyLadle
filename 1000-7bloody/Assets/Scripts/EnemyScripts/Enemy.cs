using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float bloodAmount; 

    [SerializeField] private float _currentHealth = 20f;
    [SerializeField] private float _touchDamage = 10f;

    [SerializeField] private AudioSource _hurtSound;


    [Header("Hit Effects")]
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject bloodSplash;
    [SerializeField] private GameObject hitGlow;
    [SerializeField] private GameObject HitWhiteEffect;

    [Header("Death Effects")]
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject deathBloodSplash;
    [SerializeField] private GameObject deathGlow;

    [SerializeField] private GameObject[] _limbs; 


    [Header("CameraAnim")]
    [SerializeField] private Animator camAnim;

    public PlayerScript playerMovement;

    public void TakeDamage(float damage)
    {
        CinemaMachineShake.Instance.ShakeCamera(5f, .1f);
        _currentHealth -= damage;
        _hurtSound.Play();

        Instantiate(effect, transform.position, Quaternion.identity);
        Instantiate(hitGlow, transform.position, Quaternion.identity);
        Instantiate(bloodSplash, transform.position, Quaternion.identity);
        Instantiate(HitWhiteEffect, transform.position, Quaternion.identity);

        if (_currentHealth <= 0)
        {
            //playerMovement.DoHardSlowmotion();
            CinemaMachineShake.Instance.ShakeCamera(7f, .2f);

            for (int i = 0; i < _limbs.Length; i++)
            {
                Instantiate(_limbs[i], transform.position, Quaternion.identity);
            }
            
            Instantiate(deathGlow, transform.position, Quaternion.identity);
            Instantiate(deathBloodSplash, transform.position, Quaternion.identity);
            Instantiate(deathEffect, transform.position, Quaternion.identity);

            

            Death();
        }
    }

    private void Death()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerScript.Instance.gameObject)
        {
            PlayerScript.Instance.TakeDamage(_touchDamage);
        }
    }
}
