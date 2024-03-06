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
    public float speed;
    public float knockbackForce;

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
    [SerializeField] GameObject mouseObjectObj;
    public Transform aimPoint;
    public Rigidbody2D rb;

    #endregion

    #region private fields
    private bool isDashing = false;
    public Vector2 movementInput
    { get; set; }
    private Vector2 lookDir;
    private Vector2 aimDir;

    private Vector2 scale;
    SurvivalBar health;
    public SurvivalBar stamina;

    private float lastDashTime;
    private Animator playerAnimator;



    Vector2 movement;
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
        movement = new Vector2(movementInput.x, movementInput.y) * moveSpeed * Time.deltaTime;  // assign movement info to vector2 movement
        if (playerAttackScript.isAttacking)
        {
            transform.Translate(movement * playerAttackScript.slowOnAttack);
        }
        else
        {
            transform.Translate(movement);
        }
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
    

    public void GetLookMouse(InputAction.CallbackContext context)
    {
        //enable mouse cursor
        mouseObjectObj.SetActive(true);
        Cursor.visible = false;

        // Ekranýn geniþliði ve yüksekliðini al
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Fare pozisyonunu al
        Vector2 mousePosition = context.ReadValue<Vector2>();

        // Ekranýn sol üstünden alýnan pozisyonu -1 ile 1 arasýnda normalleþtir
        float x = (mousePosition.x / screenWidth) * 2 - 1;
        float y = (mousePosition.y / screenHeight) * 2 - 1;

        // Deðerleri -1 ile 1 arasýnda sýnýrla
        x = Mathf.Clamp(x, -1f, 1f);
        y = Mathf.Clamp(y, -1f, 1f);

        // Yeni yönu oluþtur
        float approachFactor = 1.5f; // Örneðin, 0.8 olarak ayarlayabilirsiniz

        // Yeni yönu oluþtur ve objeye atama yap
        Vector2 aimDirWithApproach = new Vector2(x * approachFactor, y * approachFactor);
        aimPoint.position = new Vector3(aimDirWithApproach.x, aimDirWithApproach.y) + transform.position;

        aimPoint.right = aimDirWithApproach;

    }
    public void GetLookGamepad(InputAction.CallbackContext context)
    {
        Vector2 aimDir = context.ReadValue<Vector2>().normalized;
        aimPoint.position = new Vector3(aimDir.x, aimDir.y) + transform.position;
        aimPoint.right = aimDir;
        mouseObjectObj.SetActive(false);
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
            if(health.Bar >= health.FullValue)
            {
                health.Bar = health.FullValue;
                Debug.Log("can full, heallanamaz: " + health.Bar);
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
        playerAnimator.SetTrigger("isHurt");
        
        if (health.Bar <= 0)
        {
            health.Bar = 0;
            Debug.Log("ded: " + health.Bar);
        }
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
        if (movementInput.x > 0)
        {
            playerAnimator.SetFloat("Xinput", 1);
            lookDir = new Vector2(1, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementInput.x < 0)
        {
            playerAnimator.SetFloat("Xinput", -1);
            lookDir = new Vector2(-1, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movementInput.y > 0)
        {
            playerAnimator.SetFloat("Yinput", 1);
            lookDir = new Vector2(0, 1);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movementInput.y < 0)
        {
            playerAnimator.SetFloat("Yinput", -1);
            lookDir = new Vector2(0, -1);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

}
