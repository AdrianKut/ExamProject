using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[SerializeField]
public class Exams
{
    public string description;
    public string courseId;

    public Exams(string description, string courseId)
    {
        this.description = description;
        this.courseId = courseId;
    }
}

public class Exam : InputFields
{
    [Header("Input Fields")]
    public TMP_InputField inputFieldIdExam;
    public TMP_InputField inputFieldIdCourse;
    public TMP_InputField inputFieldDescription;

    [Header("")]
    public TextMeshProUGUI textInfoResult;

    [Header("")]
    public GameObject gameObjectAllExams;

    [Header("")]
    public TextMeshProUGUI examsName;

    public override void InitializeInputFields()
    {
        listTMPInputFields = new List<TMP_InputField>();
        listTMPInputFields.Add(inputFieldIdExam);
        listTMPInputFields.Add(inputFieldIdCourse);
        listTMPInputFields.Add(inputFieldDescription);
    }

    void Start()
    {
        InitializeInputFields();
    }

    public void DeleteExam() => StartCoroutine(DeleteExam(UrlApi.urlExams));
    IEnumerator DeleteExam(string url)
    {
        var request = new UnityWebRequest(url + @"/" + inputFieldIdExam.text, "DELETE");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));

        yield return request.SendWebRequest();
        JSONNode jsonInfo = JSON.Parse(request.downloadHandler.text);

        ResetInputFields(ref listTMPInputFields);

        if (request.isNetworkError || request.isHttpError)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            string message = jsonInfo["message"];

            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
        }
    }

    public void PostExam() => StartCoroutine(PostExam(UrlApi.urlExams));
    IEnumerator PostExam(string url)
    {
        Exams exam = new Exams(inputFieldDescription.text, inputFieldIdExam.text);

        var request = new UnityWebRequest(url, "POST");
        var jsonString = JsonUtility.ToJson(exam);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (request.isNetworkError || request.isHttpError)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            JSONNode jsonInfo = JSON.Parse(request.downloadHandler.text);
            string message = jsonInfo["message"];

            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
        }
    }

    public void GetNotPassed() => StartCoroutine(GetNotPassed(UrlApi.urlExams));
    IEnumerator GetNotPassed(string url)
    {
        var request = new UnityWebRequest(url + @"/notpassed", "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
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

            if (counterCourse > 0)
            {
                gameObjectAllExams.SetActive(true);
                for (int i = 0; i < counterCourse; i++)
                {
                    titles += (i + 1) + ". " + registerInfo["records"][i]["description"] + "\nPassed: " + registerInfo["records"][i]["isPassed"];
                    titles += "\n";
                }
                examsName.text = titles;
            }

            if (isSuccess.Equals(false))
            {
                textInfoResult.color = Color.red;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
            }
            else
            {
                textInfoResult.color = Color.green;
                textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message + "\nCount: " + counterCourse;
            }
        }
    }

    public void GetExam() => StartCoroutine(GetExam(UrlApi.urlExams));
    IEnumerator GetExam(string url)
    {
        string path = url + @"/course=" + inputFieldIdCourse.text + "?plan=" + inputFieldIdCourse.text;
        var request = new UnityWebRequest(path, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
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

            inputFieldIdExam.text = jsonInfo["records"][0]["id"];
            inputFieldIdCourse.text = jsonInfo["records"][0]["courseId"];
            inputFieldDescription.text = jsonInfo["records"][0]["description"] + ", Passed: " + jsonInfo["records"][0]["isPassed"];

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

    public void PutExam() => StartCoroutine(PutExam(UrlApi.urlExams));
    IEnumerator PutExam(string url)
    {
        var request = new UnityWebRequest(url + @"/" + inputFieldIdExam.text, "PUT");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("authorization", string.Format("Bearer {0}", LoginToken.GetToken()));
        yield return request.SendWebRequest();

        ResetInputFields(ref listTMPInputFields);

        if (request.isNetworkError || request.isHttpError)
        {
            textInfoResult.color = Color.red;
            textInfoResult.text = request.error.ToString();
        }
        else
        {
            JSONNode jsonInfo = JSON.Parse(request.downloadHandler.text);
            string message = jsonInfo["message"];

            textInfoResult.color = Color.green;
            textInfoResult.text = "Status Code: " + request.responseCode + "\n" + message;
        }
    }


    public void CloseWindowCourses()
    {
        gameObjectAllExams.SetActive(false);
    }
}
