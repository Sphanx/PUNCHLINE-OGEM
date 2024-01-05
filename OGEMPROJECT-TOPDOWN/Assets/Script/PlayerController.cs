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
    public float hungerVar;
    public float staminaVar;

    [Space(20)]
    [Tooltip("Health")]
    [SerializeField] int Health = 100;
    [Tooltip("Hunger")]
    [SerializeField] int Hunger = 100;
    [Tooltip("Stamina")]
    [SerializeField] int Stamina = 100;
    public Slider staminaSlider;
    public Slider hungerSlider;
    public Slider healthSlider;


    public Rigidbody2D rb;
    #endregion

    #region private fields
    private bool isDashing = false;
    public Vector2 movementInput
    { get; set; }

    private Vector2 scale;
    SurvivalBar health;
    SurvivalBar hunger;
    SurvivalBar stamina;

    private float dashCooldown = 2f; 
    private float lastDashTime;
    private Animator playerAnimator;
    
    #endregion

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();

        health = new SurvivalBar();
        hunger = new SurvivalBar();
        stamina = new SurvivalBar();    

        health.FullValue = Health;
        hunger.FullValue = Hunger;
        stamina.FullValue = Stamina;

        scale = transform.localScale;

    }


    void Update()
    {
        Vector2 movement = new Vector2(movementInput.x, movementInput.y) * moveSpeed * Time.deltaTime;  // assign movement info to vector2 movement
        transform.Translate(movement);

        DecreaseValue();

        staminaSlider.value = stamina.Bar;
        hungerSlider.value = hunger.Bar;
        healthSlider.value = health.Bar;
        
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
       if (context.started && !isDashing && Time.time - lastDashTime > dashCooldown)
        {
            Debug.Log("DASH ATII");
            DecreaseValue(stamina, reduceStmOnDash);
            Debug.Log("stamina: " + stamina.Bar);
            StartCoroutine(StaminaRecovery());
            lastDashTime = Time.time;
            stamina.Bar = stamina.IncreaseBar(1f);

            //add an instant speed
            Vector2 dashDirection = new Vector2(movementInput.x, movementInput.y).normalized;
            rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

            //play anim
            playerAnimator.SetTrigger("isDashed");
        }
    }

    private void DecreaseValue(SurvivalBar survivalBar, float decreaseAmount)
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

        DecreaseValue(hunger, hungerVar * Time.deltaTime);

        if (isDashing)
        {
            DecreaseValue(stamina, staminaVar * Time.deltaTime);
        }

    }
    private IEnumerator StaminaRecovery()
    {
        while (stamina.Bar < Stamina)
        {
            stamina.Bar += 1f * Time.deltaTime;
            yield return null;
        }
        stamina.Bar = Mathf.Clamp(stamina.Bar, 0f, Stamina);
    }

}
