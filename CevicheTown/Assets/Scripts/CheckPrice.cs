using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPrice : MonoBehaviour
{
    [SerializeField]
    GameObject selledBuild;
    [SerializeField]
    TextMeshProUGUI currencyText;

    public GameObject SelledBuild { get => selledBuild; set => selledBuild = value; }
    public TextMeshProUGUI CurrencyText { get => currencyText; set => currencyText = value; }
    Image img;
    TextMeshProUGUI text;
    float currency;
    // Start is called before the first frame update
    private void Start()
    {
        img = GetComponent<Image>();
        currency = ShopController.instance.currency;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        currency = ShopController.instance.currency;
        text.alignment = TextAlignmentOptions.Bottom;
        float cost = SelledBuild.GetComponent<Building>().cost;
        text.text = cost.ToString();
        if (cost > currency)
        {
            img.color = new Color(189 / 255f, 81 / 255f, 15 / 255f);
            text.color = Color.red;
            Button button = GetComponent<Button>();
            button.interactable = false;
        } else 
        {
            img.color = Color.white;
            text.color = Color.white;
            Button button = GetComponent<Button>();
            button.interactable = true;
        }
    }
}
