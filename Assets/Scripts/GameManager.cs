using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public GameObject Elephant;
    public GameObject Human;
    private CharacterStats elephantStats;
    private CharacterStats humanStats;
    private CharacterController elephantController;
    private CharacterController humanController;
    public UnityEngine.UI.Slider temperatureSlider;
    public UnityEngine.UI.Slider hungerSlider;

    private float elephantCurrentTemp;
    private float elephantCurrentHunger;
    private float humanCurrentTemp;
    private float humanCurrentHunger;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        elephantController = Elephant.GetComponent<CharacterController>();
        humanController = Human.GetComponent<CharacterController>();
        elephantStats = Elephant.GetComponent<CharacterStats>();
        humanStats = Human.GetComponent<CharacterStats>();

        humanCurrentTemp = humanStats.MaxTemperature;
        humanCurrentHunger = humanStats.MaxHunger;
        elephantCurrentTemp = elephantStats.MaxTemperature;
        elephantCurrentHunger = elephantStats.MaxHunger;
        StartCoroutine(TimerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if(humanController.enabled)
            {
                temperatureSlider.maxValue = humanStats.MaxTemperature;
                hungerSlider.maxValue = humanStats.MaxHunger;

                temperatureSlider.value = humanCurrentTemp;
                hungerSlider.value = humanCurrentHunger;

                --humanCurrentTemp;
                --humanCurrentHunger;
            }

            if(elephantController.enabled)
            {
                temperatureSlider.maxValue = elephantStats.MaxTemperature;
                hungerSlider.maxValue = elephantStats.MaxHunger;

                temperatureSlider.value = elephantCurrentTemp;
                hungerSlider.value = elephantCurrentHunger;

                --elephantCurrentTemp;
                --elephantCurrentHunger;
            }
        }
    }
}
