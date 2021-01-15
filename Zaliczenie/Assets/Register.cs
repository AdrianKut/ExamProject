using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;

public class Register : InputFields
{
    [Header("Input Fields")]
    public TMP_InputField inputFieldEmail;
    public TMP_InputField inputFieldFirstName;
    public TMP_InputField inputFieldIndexNumber;
    public TMP_InputField inputFieldPassword;
    public TMP_InputField inputFieldConfirmPassword;

    [Header("")]
    public TextMeshProUGUI textInfoResult;

    public override void InitializeInputFields()
    {
        listTMPInputFields = new List<TMP_InputField>();
        listTMPInputFields.Add(inputFieldEmail);
        listTMPInputFields.Add(inputFieldFirstName);
        listTMPInputFields.Add(inputFieldIndexNumber);
        listTMPInputFields.Add(inputFieldPassword);
        listTMPInputFields.Add(inputFieldConfirmPassword);
    }
        
    private void CharacterLimit()
    {
        inputFieldEmail.characterLimit = 50;
        inputFieldEmail.characterValidation = TMP_InputField.CharacterValidation.EmailAddress;

        inputFieldFirstName.characterLimit = 25;
        inputFieldIndexNumber.characterLimit = 25;

        inputFieldPassword.characterLimit = 50;
        inputFieldPassword.asteriskChar = '*';
        inputFieldPassword.inputType = TMP_InputField.InputType.Password;

        inputFieldConfirmPassword.characterLimit = 50;
        inputFieldConfirmPassword.asteriskChar = '*';
        inputFieldConfirmPassword.inputType = TMP_InputField.InputType.Password;
    }


    public void Start()
    {
        InitializeInputFields();
        CharacterLimit();
    }

    
    public void DoRegister()
    {  
        Person PersonToRegister = new Person();
        PersonToRegister.email = inputFieldEmail.text;
        PersonToRegister.firstName = inputFieldFirstName.text;
        PersonToRegister.indexNumber = inputFieldIndexNumber.text;
        PersonToRegister.password = inputFieldPassword.text;
        PersonToRegister.confirmPassword = inputFieldConfirmPassword.text;

        StartCoroutine(Post(UrlApi.urlRegister, PersonToRegister));
    }

    IEnumerator Post(string url, Person person)
    {

        var request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes("{'email': '" + person.email + "', 'firstName': '" +
            person.firstName + "','indexNumber': '" + person.indexNumber + "', 'password': '" +
            person.indexNumber + "', 'confirmPassword': '" + person.confirmPassword + "'}");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        
        JSONNode registerInfo = JSON.Parse(request.downloadHandler.text);
        string message = registerInfo["message"];
        bool isSuccess = registerInfo["isSuccess"];

        ResetInputFields(ref listTMPInputFields);
        if (isSuccess.Equals(false))
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" +message + "\nisSuccess: " +isSuccess;
        }
        else
        {
            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
        }
    }

}
