using UnityEngine;
using UnityEngine.UI;

public class SliderFill : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject player;
    [SerializeField] int kill;

    private void Start()
    {
        kill = player.GetComponent<PlayerAttributes>().magicReset;
        // Set the initial value of the slider to zero
        slider.maxValue= kill;
        slider.value = 0f;
    }

    private void Update()
    {
        slider.value = player.GetComponent<PlayerAttributes>().GetKillCount();
    }
}