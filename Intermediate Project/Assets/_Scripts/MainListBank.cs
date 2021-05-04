using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainListBank : BaseListBank
{
    public string[] contents = {
        "1", "2", "3", "4", "5"
    };

    public override string GetListContent(int index)
    {
        return contents[index].ToString();
    }

    public override int GetListLength()
    {
        return contents.Length;
    }
}
