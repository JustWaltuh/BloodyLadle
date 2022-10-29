using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }
    public static bool gameIsPaused = false;

    public HealthbarScript healthBar;
    public BloodBottleScript bloodBottle;
    public CollectedBloodScript GameOverMenu;

    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _jumpPower = 16f;
    private float _attackRange = 1.5f;
    [SerializeField] private float _attackRate = 0.5f;
    [SerializeField] private float _nextAttackTime = 0f;
    private float _playerDamage = 10f;
    private float _horizontal;

    public float bloodFilled = 0f;
    [SerializeField] private float _maxBloodFilled = 25f;

    public float slowdownFactorLight = 0.05f;
    public float slowdowndLenght = 2f;


    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    private float _hurtTimer = 0f;
    private bool _invincibility = false;
    [SerializeField] private float _invincibilityTimer = 2f;
    private bool _isDead = false;

    private PlayerInput _input;

    private SpriteRenderer _sprite;

    private bool _isFacingRight = true;
    private bool _isAttacking = false;

    [SerializeField] private AudioSource _attackSound;
    [SerializeField] private AudioSource _footstepSound;
    [SerializeField] private AudioSource _hurtSound;
    [SerializeField] private AudioSource _deathSound;
    [SerializeField] private AudioSource _notEnoughBlood;
    [SerializeField] private AudioSource _healsUp;

    private Rigidbody2D _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _attackCheck;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Effects")]
    [SerializeField] private ParticleSystem dustEffects;
    [SerializeField] private ParticleSystem landingEffects;
    [SerializeField] private ParticleSystem hitEffects;
    [SerializeField] private ParticleSystem deathEffects;
    private bool spawnDust;


    private Animator _animator;

    private string _currentAnimation;

    private const string _player_idle = "Idle";
    private const string _player_run = "Run";
    private const string _player_jump = "Jump";
    private const string _player_hurt = "Hurt";
    private const string _player_melee = "Melee";
    private const string _player_jump_melee = "JumpMelee";
    private const string _player_fall = "Fall";

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        _health = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);
        Physics2D.IgnoreLayerCollision(8, 9);
    }

    private void Update()
    {
        Time.timeScale += (1f / slowdowndLenght) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        _rb.velocity = new Vector2(_horizontal * _speed, _rb.velocity.y);

        if (IsGrounded() && !_isAttacking && Time.time >= _hurtTimer && !_isDead)
        {
            if (_horizontal != 0) ChangeAnimationState(_player_run);
            else ChangeAnimationState(_player_idle);
        }

        if (!_isFacingRight && _horizontal > 0f) Flip();
        else if (_isFacingRight && _horizontal < 0f) Flip();


        if (!IsGrounded() && !_isAttacking && Time.time >= _hurtTimer && !_isDead)
        { 
        
            ChangeAnimationState(_player_fall);
        }

        //if (Input.GetKeyDown(KeyCode.E)) TakeDamage(1f);

        if (IsGrounded() && _rb.velocity.magnitude > 1f && !_footstepSound.isPlaying)
        {
            
            _footstepSound.volume = Random.Range(0.8f, 1);
            _footstepSound.pitch = Random.Range(0.8f, 1);
            _footstepSound.Play();

        }

        if((_horizontal != 0) && IsGrounded())  
        {
            CreateDust();
        } else 
        if(!IsGrounded())  OffDust();
            

        if(IsGrounded() == true)
        {
           
            if (spawnDust == true)
                {
                    Instantiate(dustEffects, _groundCheck.position, Quaternion.identity);
                    spawnDust = false;
                }
        }
        else
        {
            
            spawnDust = true;
        }
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
        CreateDust();
        if (context.performed && IsGrounded() && Time.time >= _hurtTimer && !_isDead)
        {
            
            ChangeAnimationState(_player_jump);
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
        }

        if (context.canceled && _rb.velocity.y > 0f)
        {   
            
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            
        }
        

    }

    private bool IsGrounded()
    {
        
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }

    private void Flip()
    {   
        CreateDust();
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;

    }

    public void MeleeStrike(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= _nextAttackTime )
        {   
            _isAttacking = true;
            if (_isAttacking)
            {
                _attackSound.Play();

                if (IsGrounded()) ChangeAnimationState(_player_melee);
                else ChangeAnimationState(_player_jump_melee);

                Invoke("AttackComplete", 0.3f);

                
            }

            

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackCheck.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                DoLightSlowmotion();

                float tmpBlood = enemy.GetComponent<Enemy>().bloodAmount;
                bloodFilled += tmpBlood;
                if (bloodFilled > _maxBloodFilled) bloodFilled = _maxBloodFilled;
                bloodBottle.UpdateBottle(tmpBlood);

                enemy.GetComponent<Enemy>().TakeDamage(_playerDamage);
            }
            _nextAttackTime = Time.time + _attackRate;
            
        }
        
    }

    private void AttackComplete()
    {
        _isAttacking = false;
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);

        _currentAnimation = newAnimation;
    }

    public void TakeDamage(float damage)
    {
        if (_invincibility == false && !_isDead)
        {
            _hurtSound.Play();

            CinemaMachineShake.Instance.ShakeCamera(5f, .1f);

            _health -= damage;
            healthBar.SetHealth(_health);

            Instantiate(hitEffects, transform.position, Quaternion.identity);

            ChangeAnimationState(_player_hurt);
            _hurtTimer = Time.time + 0.5f;
            if (_health <= 0) 
            {
                Instantiate(deathEffects, transform.position, Quaternion.identity);
                Death();
            }
            if (!_isDead) StartCoroutine(InvincibilityOnOff());
        }
    }



    public void HealthPlus(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (bloodBottle._bloodFilled >= 20)
            {

                if (_health < _maxHealth)
                {
                    _healsUp.Play();
                    RestoreHealth(15);
                    bloodBottle.UpdateBottle(-20);
                    bloodFilled -= 20;
                }

                else
                {
                    _notEnoughBlood.Play();
                }
            }

            if (bloodBottle._bloodFilled < 20)
            {
                _notEnoughBlood.Play();
            }

        }
    }


    public void RestoreHealth(float newHealth)
    {
        if (_health < _maxHealth)
        {
            _health += newHealth;
            healthBar.SetHealth(_health);
        }

    }

    private IEnumerator InvincibilityOnOff()
    {
        _invincibility = true;
        //�������� ����
        _sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(_invincibilityTimer);

        _invincibility = false;
        //������� �� ������� ����(���� ��� ������������)
        _sprite.color = new Color(1, 1, 1, 1);
    }

    private void Death()
    {
        _deathSound.Play();
        _isDead = true;
        _input.enabled = false;

        GameOverMenu.gameObject.SetActive(true);
        GameOverMenu.UpdateText();

    }

    private void CreateDust()
    {
        
        dustEffects.Play();
        spawnDust = true;
    }

    private void OffDust()
    {   
       
        dustEffects.Stop();
        spawnDust = false;
    }

    private void CreateLandingEffect()
    {
        landingEffects.Play();
       
    }

    private void OffLandingEffect()
    {
        landingEffects.Stop();
    }
  

    

    public void DoLightSlowmotion()
    {
        Time.timeScale = slowdownFactorLight;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    //public void DoHardSlowmotion()
   // {
   //     Time.timeScale = slowdownFactorHard;
    //    Time.fixedDeltaTime = Time.timeScale * .02f;
   // }
}