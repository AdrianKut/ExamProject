using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

abstract public class InputFields : MonoBehaviour
{
    public List<TMP_InputField> listTMPInputFields;
    public abstract void InitializeInputFields();
    public virtual void ResetInputFields(ref List<TMP_InputField> tmpInputFields) => tmpInputFields.ForEach(x => x.text = "");
}
