using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
   
    #region public fields
    [Header("variable values")]
    public float reduceStmOnDash;
    public float moveSpeed = 5f;
    public float dashForce;
    public float dashCooldown = 2f; 
    public float staminaRecoverySpeed;
    public float timeRemaining;
    public int potionNumber;
    public bool isMoving;
    public bool isTimerRunning;
    public IEnumerator currentStaminaRecovery;


    [Space(20)]
    [Tooltip("Health")]
    [SerializeField] int Health = 100;
    [Tooltip("Stamina")]
    [SerializeField] int Stamina = 100;
    public Slider staminaSlider;
    public Slider healthSlider;

    [Space(20)]
    [SerializeField] PlayerAttack playerAttackScript;
    [SerializeField] Potions potionsScript;
    [SerializeField] PotionCounter potionCounterScript;
    public Transform attackPoint;
    public Rigidbody2D rb;
    #endregion

    #region private fields
    private bool isDashing = false;
    public Vector2 movementInput
    { get; set; }

    private Vector2 scale;
    SurvivalBar health;
    public SurvivalBar stamina;

    private float lastDashTime;
    private Animator playerAnimator;


    #endregion

    private void Awake()
    {
        potionsScript = GetComponent<Potions>();
        health = new SurvivalBar();
        stamina = new SurvivalBar();    
    }
    private void Start()
    {
        health.FullValue = Health;
        stamina.FullValue = Stamina;

        //set potions
        potionsScript.numberOfPotions = potionNumber;
        potionCounterScript.setPotionTextNumber(potionsScript);

        playerAnimator = GetComponent<Animator>();

        scale = transform.localScale;
        
        isTimerRunning = false;

    }


    void Update()
    {
        Vector2 movement = new Vector2(movementInput.x, movementInput.y) * moveSpeed * Time.deltaTime;  // assign movement info to vector2 movement
        transform.Translate(movement);
        LookDirection();
        
        DecreaseValue();

        staminaSlider.value = stamina.Bar;
        healthSlider.value = health.Bar;

        if(movementInput.x != 0 || movementInput.y != 0)
        {
            isMoving = true;
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
            isMoving = false;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(31);
        }
    }

    //Player movement
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        //set movement/idle animation
        playerAnimator.SetFloat("Xinput", movementInput.x);
        playerAnimator.SetFloat("Yinput", movementInput.y);
    }

    public void Dash(InputAction.CallbackContext context)
    {
       if (context.started && !isDashing && Time.time - lastDashTime > dashCooldown && (stamina.Bar > 0))
        {
            Debug.Log("DASH ATII");
            DecreaseValue(stamina, reduceStmOnDash);
            Debug.Log("stamina: " + stamina.Bar);
            isTimerRunning = true;
            
            //get dash time
            lastDashTime = Time.time;
            

            //add an instant speed
            Vector2 dashDirection = new Vector2(movementInput.x, movementInput.y).normalized;
            rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

            //play anim
            playerAnimator.SetTrigger("isDashed");

            StopStaminaRecovery();
            currentStaminaRecovery = StaminaRecovery();
            StartCoroutine(currentStaminaRecovery);
        }
    }

    public void HealSelf(InputAction.CallbackContext context)
    {
        if(context.performed && potionsScript.numberOfPotions != 0)
        {
            if(health.Bar > health.FullValue)
            {
                health.Bar = health.FullValue;
            }
            else
            {
                health.IncreaseBar(potionsScript.healAmount);
                potionsScript.numberOfPotions--;
                potionCounterScript.setPotionTextNumber(potionsScript);
                Debug.Log("health increased: " + health.Bar + "full health: " + health.FullValue);
            }
        }
    }
    public void TakeDamage(float damageAmount)
    {
        health.DecreaseBar(damageAmount);
    }
    public void DecreaseValue(SurvivalBar survivalBar, float decreaseAmount)
    {
        float barPlaceholder = survivalBar.Bar;

        
        barPlaceholder -= decreaseAmount;

        if(barPlaceholder < 0)
        {
            survivalBar.Bar = 0;
        }
        else
        {
            survivalBar.Bar = barPlaceholder;
        }

    }
    private void DecreaseValue()
    {
    }
    public IEnumerator StaminaRecovery()
    {
        while (stamina.Bar < Stamina)
        {
            stamina.Bar += staminaRecoverySpeed * Time.deltaTime;
            yield return null;
        }
        stamina.Bar = Mathf.Clamp(stamina.Bar, 0f, Stamina);
    }
    public void StartStaminaRecovery()
    {
        StartCoroutine(StaminaRecovery());
    }
    public void StopStaminaRecovery()
    {
        if (currentStaminaRecovery != null)
        {
            StopAllCoroutines();
        }
    }
    public void LookDirection()
    {
        attackPoint.position = transform.position;
        if (movementInput.x > 0)
        {
            attackPoint.position += new Vector3(playerAttackScript.attackDistance, 0);
            playerAnimator.SetFloat("Xinput", 1);
        }
        else if (movementInput.x < 0)
        {
            attackPoint.position += new Vector3(-playerAttackScript.attackDistance, 0);
            playerAnimator.SetFloat("Xinput", -1);

        }
        else if (movementInput.y > 0)
        {
            attackPoint.position += new Vector3(0, playerAttackScript.attackDistance);
            playerAnimator.SetFloat("Yinput", 1);

        }
        else if (movementInput.y < 0)
        {
            attackPoint.position += new Vector3(0, -playerAttackScript.attackDistance);
            playerAnimator.SetFloat("Yinput", -1);
        }
    }

}
