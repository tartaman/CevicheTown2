using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DatabaseLoader : MonoBehaviour
{
    [SerializeField] public string username;
    [SerializeField] public string password;
    [SerializeField] public string fileName;
    [SerializeField] public string fileContents;
    public static string direcciondelJSON;
    public IEnumerator getUserData(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CevicheTown/Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                //Show results as text
                Debug.LogWarning(www.downloadHandler.text);
                direcciondelJSON = www.downloadHandler.text;
                string[] result = www.downloadHandler.text.Split("<br>");
                foreach (string s in result)
                {
                    if (s.Contains(username) && s.Contains("User:"))
                    {
                        GameObject.Find("GameManager").GetComponent<SaveData>()._user.Name = username;
                        break;
                    }
                }
            }
        }
    }

    public IEnumerator signUpUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CevicheTown/SignUp.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                //Show results as text
                Debug.LogWarning(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator saveGame(string username, string nombrePartida, string partida)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("fileName", nombrePartida);
        form.AddField("fileContents", partida);
        
        GameObject.Find("GameManager").GetComponent<SaveData>()._user.grid.fileName = nombrePartida.Trim(new char[] {' ', '\\', '/', '@', '?', '=', '#', '$', '%' });

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CevicheTown/SaveGame.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                //Show results as text
                Debug.LogWarning(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator loadGame(string username, string nombrePartida)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("fileName", nombrePartida);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CevicheTown/LoadGame.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                //Transforms string to json.
                Debug.LogWarning("loading file data on: " + Application.persistentDataPath + "/GridData.json");
                System.IO.File.WriteAllText(Application.persistentDataPath + "/GridData.json", www.downloadHandler.text);

                //Show results as text
                Debug.LogWarning(www.downloadHandler.text);

                string[] result = www.downloadHandler.text.Split("<br>");
                foreach (string s in result)
                {
                    if(s.Contains(nombrePartida.Trim(' '))) { 
                        GameObject.Find("GameManager").GetComponent<SaveData>()._user.grid.fileName = nombrePartida;
                    }
                }

            }
        }
    }
}
