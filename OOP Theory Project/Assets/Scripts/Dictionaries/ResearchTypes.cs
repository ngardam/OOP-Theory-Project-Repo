using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResearchTypes

{

    public static Dictionary<string, Research> ResearchDictionary { get; private set; } = new Dictionary<string, Research>
    {
        {
            "Village", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    { "Ponder", 20 }
                },
                prerequisites = new string[1]
                {
                    "Research Button"
                }
            }
        },
        {
            "Farming", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 35 }
                },
                prerequisites = new string[]
                {
                    "Village"
                }
            }
        },
        {
            "Research Button", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {

                }
            }
        },
        {
            "Fire", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {
                    "Village"
                }
            }
        },
        {
            "Mining", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {
                    "Village"
                }
            }
        },
        {
            "Woodcutting", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {
                    "Village"
                }
            }
        },
        {
            "Storage", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {
                    "Village"
                }
            }
        },
        {
            "Cooking", new Research
            {
                RequiredForResearch = new Dictionary<string, int>
                {
                    {"Ponder", 8 }
                },
                prerequisites = new string[]
                {
                    "Fire"
                }
            }

        },
        {
            "None Selected", new Research
            {
                RequiredForResearch = new Dictionary<string, int>{ },
                prerequisites = new string[0]
            }

        }
    };
    
}


