using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Person
{
    public string email;
    public string firstName;
    public string indexNumber;
    public string password;
    public string confirmPassword;

    public Person()
    {
    }

    public Person(string email, string firstName, string indexNumber, string password, string confirmPassword)
    {
        this.email = email;
        this.firstName = firstName;
        this.indexNumber = indexNumber;
        this.password = password;
        this.confirmPassword = confirmPassword;
    }

}