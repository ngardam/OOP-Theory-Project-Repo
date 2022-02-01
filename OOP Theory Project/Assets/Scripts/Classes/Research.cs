using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research
{
    // Start is called before the first frame update
    //private string m_name;
    //
    //public string name
    //{
    //    get { return m_name; }
    //    set { m_name = value; }
    //}

    public Dictionary<string, int> RequiredForResearch { get;  set; }

    public Dictionary<string, int> completedResearch { get; set; }

    public string[] prerequisites { get; set; }

    public bool isComplete { get; set; }

}
