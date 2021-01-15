using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Login :  InputFields
{
    [Header("Input Fields")]
    public TMP_InputField inputFieldEmail;
    public TMP_InputField inputFieldPassword;

    public TextMeshProUGUI textInfoResult;

    [Header("")]
    public TextMeshProUGUI logged;

    private string UserInfo { get;  set; }
    public static bool isLogged = false;

    private void CharacterLimit()
    {
        inputFieldEmail.characterLimit = 50;
        inputFieldEmail.characterValidation = TMP_InputField.CharacterValidation.EmailAddress;

        inputFieldPassword.characterLimit = 50;
        inputFieldPassword.asteriskChar = '*';
        inputFieldPassword.inputType = TMP_InputField.InputType.Password;
    }

    public override void InitializeInputFields()
    {
        listTMPInputFields = new List<TMP_InputField>();
        listTMPInputFields.Add(inputFieldEmail);
        listTMPInputFields.Add(inputFieldPassword);
    }

    private void Start()
    {
        InitializeInputFields();
        UserInfo = "";
        CharacterLimit();
    }

    public void DoLogin()
    {
        CharacterLimit();

        Person PersonToRegister = new Person();
        PersonToRegister.email = inputFieldEmail.text;
        PersonToRegister.password = inputFieldPassword.text;

        StartCoroutine(Post(UrlApi.urlLogin, PersonToRegister));
    }

    IEnumerator Post(string url, Person person)
    {
        var request = new UnityWebRequest(url, "POST");
        var jsonString = JsonUtility.ToJson(person);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
       
        yield return request.SendWebRequest();

        JSONNode registerInfo = JSON.Parse(request.downloadHandler.text);
        string message = registerInfo["message"];
        UserInfo = registerInfo["userInfo"]["FirstName"];
        UserInfo += " ";
        UserInfo += registerInfo["userInfo"]["Email"];
        bool isSuccess = registerInfo["isSuccess"];

        LoginToken.SetToken(message);

        ResetInputFields(ref listTMPInputFields);
        if (isSuccess.Equals(false))
        {
            isLogged = false;
            logged.text = "Zalogowany: ";

            textInfoResult.color = Color.red;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
        }
        else
        {
            isLogged = true;
            logged.text = "Zalogowany: " + UserInfo ;
            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + request.responseCode + "\nLoggged in";
        }
    }
}
