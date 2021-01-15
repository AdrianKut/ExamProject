using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoginToken : MonoBehaviour
{
    private static string token;
    public static void SetToken(string message) => token = message;
    public static string GetToken() => token;
}
