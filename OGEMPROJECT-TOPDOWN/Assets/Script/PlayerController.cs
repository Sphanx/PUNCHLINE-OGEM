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
    public float thirstVar;
    public float hungerVar;
    public float exhaustVar;
    public float staminaVar;

    [Space(20)]
    [Tooltip("Health")]
    [SerializeField] int Health = 100;
    [Tooltip("Hunger")]
    [SerializeField] int Hunger = 100;
    [Tooltip("Thirst")]
    [SerializeField] int Thirst = 100;
    [Tooltip("Exhaust")]
    [SerializeField] int Exhaust = 100;
    [Tooltip("Stamina")]
    [SerializeField] int Stamina = 100;
    public Slider staminaSlider;
    public Slider thirstSlider;
    public Slider hungerSlider;
    public Slider exhaustSlider;
    public Slider healthSlider;


    public Rigidbody2D rb;
    #endregion

    #region private fields
    private bool isDashing = false;
    private Vector2 movementInput;
    SurvivalBar health;
    SurvivalBar hunger;
    SurvivalBar thirst;
    SurvivalBar exhaust;
    SurvivalBar stamina;

    private float dashCooldown = 2f; 
    private float lastDashTime;
    #endregion

    private void Start()
    {
        health = new SurvivalBar();
        hunger = new SurvivalBar();
        thirst = new SurvivalBar();
        exhaust = new SurvivalBar();
        stamina = new SurvivalBar();    

        health.FullValue = Health;
        hunger.FullValue = Hunger;
        thirst.FullValue = Thirst;
        exhaust.FullValue = Exhaust;
        stamina.FullValue = Stamina;

    }


    void Update()
    {
        // Hareket giriþini kullanarak karakteri hareket ettir
        Vector2 movement = new Vector2(movementInput.x, movementInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        DecreaseValue();

        staminaSlider.value = stamina.Bar;
        thirstSlider.value = thirst.Bar;
        hungerSlider.value = hunger.Bar;
        exhaustSlider.value = exhaust.Bar;
        healthSlider.value = health.Bar;

        //Debug.Log("thirst: " + thirst.Bar + "hunger: " + hunger.Bar + "exhaust: " + exhaust.Bar);

    }

    // Input System'un "Move" aksiyonunu dinlemek için bu metodunu kullanabiliriz
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
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
        DecreaseValue(thirst, thirstVar * Time.deltaTime);
        DecreaseValue(exhaust, exhaustVar * Time.deltaTime);

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
