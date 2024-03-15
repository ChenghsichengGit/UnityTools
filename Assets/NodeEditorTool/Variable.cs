using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
[System.Serializable]
public class Variable
{
    public List<Int> ints;
    public List<Float> floats;
    public List<Bool> bools;
    public List<String> strings;

    public object FindByName(string name)
    {
        var i = ints.FirstOrDefault(i => i.name == name);

        if (i != null)
        {
            return i.value;
        }

        var f = floats.FirstOrDefault(f => f.name == name);

        if (f != null)
        {
            return f.value;
        }

        var b = bools.FirstOrDefault(b => b.name == name);

        if (b != null)
        {
            return b.value;
        }

        var s = strings.FirstOrDefault(s => s.name == name);

        if (s != null)
        {
            return s.value;
        }

        return null;
    }
}

[System.Serializable]
public class Int
{
    public string name;
    public int value;
}

[System.Serializable]
public class Float
{
    public string name;
    public float value;
}

[System.Serializable]
public class Bool
{
    public string name;
    public bool value;
}

[System.Serializable]
public class String
{
    public string name;
    public string value;
}

