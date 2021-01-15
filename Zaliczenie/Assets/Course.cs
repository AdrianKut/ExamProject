using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Course : InputFields
{
    [Header("Input Fields")]
    public TMP_InputField inputFieldIdCourse;
    public TMP_InputField inputFIeldIdExam;
    public TMP_InputField inputFieldTitle;
    public TMP_InputField inputFieldDescription;
    public TMP_InputField inputFieldExams;

    [Header("")]
    public TextMeshProUGUI textInfoResult;

    [Header("")]
    public GameObject gameObjectAllCourses;

    [Header("")]
    public TextMeshProUGUI courseName;

    public override void InitializeInputFields()
    {
        listTMPInputFields = new List<TMP_InputField>();
        listTMPInputFields.Add(inputFieldIdCourse);
        listTMPInputFields.Add(inputFIeldIdExam);
        listTMPInputFields.Add(inputFieldTitle);
        listTMPInputFields.Add(inputFieldDescription);
        listTMPInputFields.Add(inputFieldExams);
    }

    void Start()
    {
        InitializeInputFields();
    }

    public void PostCourse() => StartCoroutine(PostCourse(UrlApi.urlCourse));
    IEnumerator PostCourse(string url)
    {
        var formData = new WWWForm();
        formData.AddField("Title", inputFieldTitle.text);
        formData.AddField("Description", inputFieldDescription.text);

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        www.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));
        yield return www.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (www.isNetworkError || www.isHttpError)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = www.error.ToString();
        }
        else
        {
            JSONNode registerInfo = JSON.Parse(www.downloadHandler.text);
            bool isSuccess = registerInfo["isSuccess"];
            string message = registerInfo["message"];

            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + www.responseCode + "\n" + message;
        }
    }

    public void GetCourses() => StartCoroutine(Get(UrlApi.urlCourse));  
    IEnumerator Get(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));

        yield return request.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (request.responseCode != 200)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            JSONNode registerInfo = JSON.Parse(request.downloadHandler.text);
            bool isSuccess = registerInfo["isSuccess"];
            string message = registerInfo["message"];

            int counterCourse = registerInfo["count"];
            string titles = "";
            for (int i = 0; i < counterCourse; i++)
            {
                titles += (i + 1) + ". " + registerInfo["records"][i]["title"];
                titles += "\n";
            }

            courseName.text = titles;

            if (isSuccess.Equals(false))
            {
                textInfoResult.color = Color.red;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
            else
            {
                gameObjectAllCourses.SetActive(true);
                textInfoResult.color = Color.green;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
        }
    }

    public void DeleteCourse() => StartCoroutine(Delete(UrlApi.urlCourse));
    IEnumerator Delete(string url)
    {
        string id = inputFieldIdCourse.text;
        var request = new UnityWebRequest(url + @"/" + id, "DELETE");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));

        yield return request.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (request.responseCode != 200)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            JSONNode jsonInfo = JSON.Parse(request.downloadHandler.text);
            bool isSuccess = jsonInfo["isSuccess"];
            string message = jsonInfo["message"];

            if (isSuccess.Equals(false))
            {
                textInfoResult.color = Color.red;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
            else
            {
                textInfoResult.color = Color.green;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
        }
    }

    public void GetCourse() => StartCoroutine(GetOne(UrlApi.urlCourse));

    private bool isGetID = false;
    IEnumerator GetOne(string url)
    {
        string idCourse, idExam, descriptionCourse, descriptionExam, title;
        UnityWebRequest request;
        if (inputFieldIdCourse.text != "")
        {
            request = new UnityWebRequest(url + @"/" + inputFieldIdCourse.text, "GET");
            isGetID = true;
        }
        else
        {
            request = new UnityWebRequest(url + @"/query=" + inputFieldTitle.text, "GET");
            isGetID = false;
        }

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));

        yield return request.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (request.responseCode != 200)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            JSONNode jsonInfo = JSON.Parse(request.downloadHandler.text);
            bool isSuccess = jsonInfo["isSuccess"];
            string message = jsonInfo["message"];

            if (isGetID)
            {
                idCourse = jsonInfo["record"]["id"];
                idExam = jsonInfo["record"]["exams"][0]["id"];
                descriptionCourse = jsonInfo["record"]["description"];
                descriptionExam = jsonInfo["record"]["exams"][0]["description"];
                title = jsonInfo["record"]["title"];

            }
            else
            {
                idCourse = jsonInfo["records"][0]["id"];
                idExam = jsonInfo["records"][0]["exams"][0]["id"];
                descriptionCourse = jsonInfo["records"][0]["description"];
                descriptionExam = jsonInfo["records"][0]["exams"][0]["description"];
                title = jsonInfo["records"][0]["title"];
            }


            if (isSuccess.Equals(false))
            {
                textInfoResult.color = Color.red;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
            else
            {
                inputFieldIdCourse.text = idCourse;
                inputFIeldIdExam.text = idExam;
                inputFieldDescription.text = descriptionCourse;
                inputFieldTitle.text = title;

                if (descriptionExam != null)
                {
                    inputFieldExams.text = descriptionExam;
                    inputFieldExams.text += ", Passed: ";
                    inputFieldExams.text += jsonInfo["records"][0]["exams"][0]["isPassed"];
                    inputFieldExams.text += jsonInfo["record"]["exams"][0]["isPassed"];
                }

                textInfoResult.color = Color.green;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
        }
    }

    public void PutCourse() => StartCoroutine(PutOne(UrlApi.urlCourse));
    IEnumerator PutOne(string url)
    {
        var formData = new WWWForm();
        formData.AddField("Id", inputFieldIdCourse.text);
        formData.AddField("Title", inputFieldTitle.text);
        formData.AddField("Description", inputFieldDescription.text);

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        www.method = "PUT";
        www.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));
        yield return www.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (www.isNetworkError || www.isHttpError)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = www.error.ToString();
        }
        else
        {
            JSONNode registerInfo = JSON.Parse(www.downloadHandler.text);
            bool isSuccess = registerInfo["isSuccess"];
            string message = registerInfo["message"];

            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + www.responseCode + "\n" + message;
        }
    }

    public void CloseWindowCourses()
    {
        gameObjectAllCourses.SetActive(false);
    }
}
