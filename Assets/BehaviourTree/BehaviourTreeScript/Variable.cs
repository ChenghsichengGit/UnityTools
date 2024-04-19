using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;

public enum VariableType
{
    Int, Float, String, Bool
}

[System.Serializable]
public class Variables
{
    public List<Int> ints;
    public List<Float> floats;
    public List<Bool> bools;
    public List<String> strings;

    public void SetInt(string name, int value)
    {
        Int variable = ints.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            variable.value = value;
        }
    }

    public int GetInt(string name)
    {
        Int variable = ints.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            return variable.value;
        }

        UnityEngine.Debug.Log("找不到此變數");
        return 0;
    }

    public void SetFloat(string name, float value)
    {
        Float variable = floats.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            variable.value = value;
        }
    }

    public float GetFloat(string name)
    {
        Float variable = floats.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            return variable.value;
        }

        UnityEngine.Debug.Log("找不到此變數");
        return 0;
    }

    public void SetBool(string name, bool value)
    {
        Bool variable = bools.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            variable.value = value;
        }
    }

    public bool GetBool(string name)
    {
        Bool variable = bools.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            return variable.value;
        }

        UnityEngine.Debug.Log("找不到此變數");
        return false;
    }

    public void SetString(string name, string value)
    {
        String variable = strings.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            variable.value = value;
        }
    }

    public string GetString(string name)
    {
        String variable = strings.FirstOrDefault(i => i.name == name);

        if (variable != null)
        {
            return variable.value;
        }

        UnityEngine.Debug.Log("找不到此變數");
        return "";
    }
}

public class Variable
{
    public string name;
}

[System.Serializable]
public class Int : Variable
{
    public int value;
}

[System.Serializable]
public class Float : Variable
{
    public float value;
}

[System.Serializable]
public class Bool : Variable
{
    public bool value;
}

[System.Serializable]
public class String : Variable
{
    public string value;
}

