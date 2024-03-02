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
    // Start is called before the first frame update
    void Update()
    {
        Image img = GetComponent<Image>();
        int currency = int.Parse(currencyText.text);
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.alignment = TextAlignmentOptions.Bottom;
        float cost = selledBuild.GetComponent<Building>().cost;
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
