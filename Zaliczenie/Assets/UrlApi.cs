using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UrlApi : MonoBehaviour
{
    readonly public static string urlRegister = "https://zaliczenie.btry.eu/api/Auth/Register";
    readonly public static string urlLogin = "https://zaliczenie.btry.eu/api/Auth/Login";
    readonly public static string urlCourse = "https://zaliczenie.btry.eu/api/Course";
    readonly public static string urlExams = "https://zaliczenie.btry.eu/api/Exams";
}
