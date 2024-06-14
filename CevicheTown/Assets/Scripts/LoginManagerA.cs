using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class LoginManagerA : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    Button loginButton;
    [SerializeField]
    Button InvitadoButton;
    [SerializeField]
    Button SignUpButton;
    DatabaseLoader databaseLoader;
    [SerializeField]
    TMP_InputField User;
    [SerializeField]
    TMP_InputField Password;
    [SerializeField]
    Canvas LoginCanvas;
    [SerializeField]
    LoadExistingGame loader;
    private void Start()
    {
        databaseLoader = gameManager.GetComponent<DatabaseLoader>();
        Time.timeScale = 0f;
    }
    public void Login()
    {
        databaseLoader.username = User.text;
        databaseLoader.password = Password.text;
        databaseLoader.StartCoroutine(databaseLoader.getUserData(databaseLoader.username,databaseLoader.password));
        if (databaseLoader.JSONDATA != null) 
        {
            LoginCanvas.gameObject.SetActive(false);
        }
        Time.timeScale = 1.0f;
    }
    public void Invitado()
    {
        Time.timeScale = 1.0f;
        LoginCanvas.gameObject.SetActive(false);
    }
    public void signUp()
    {
        databaseLoader.StartCoroutine(databaseLoader.signUpUser(User.text, Password.text));
        User.text = "";
        Password.text = "";
    }
}