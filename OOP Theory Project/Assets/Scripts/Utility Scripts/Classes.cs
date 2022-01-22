using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classes : MonoBehaviour
{
   public class WorldGenSpecs
    {
        public Vector3 seedVector { get; set; }

        public float scale { get; set; }

        public int divisions { get; set; }

        public float terrainFrequency { get; set; }

        public Texture2D gradient { get; set; }

        public float forestThreshold { get; set; }  //value between 0 and 1 above which forest will be generated in empty land
        public float forestFrequency { get; set; }
        public float berryThreshold { get; set; }
        public float berryFrequency { get; set; }

        public float waterLevel { get; set; }

        public float sandLevel { get; set; }

        public float grassLevel { get; set; }

        public float mountainLevel { get; set; }

        public float[] bottomTopLevel { get; set; } = new float[3]; //heights between bottom[0] and top[1] will be set to level[2]

        public int numberOfStarterVillagers { get; set; }

    }

    public class BuildingType
    {

    }

    public class TileLogisticsRequest
    {
        public string type;
        public int qty;
        public int requesterIndex;
        public int priority = 0;
        public int active = 0; // number of requests actively being filled
        public int supplierIndex;
        public bool isComplete = true;
    }


}
