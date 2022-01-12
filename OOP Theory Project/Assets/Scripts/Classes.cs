using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classes : MonoBehaviour
{
   public class WorldGenSpecs
    {
        public Vector3 seedVector;

        public float scale;

        public int divisions;

        public float terrainFrequency;

        public Texture2D gradient;

        public float forestThreshold;  //value between 0 and 1 above which forest will be generated in empty land
        public float forestFrequency;
        public float berryThreshold;
        public float berryFrequency;

        public float waterLevel;

        public float sandLevel;

        public float grassLevel;

        public float mountainLevel;

        public float[] bottomTopLevel = new float[3]; //heights between bottom[0] and top[1] will be set to level[2]

        public int numberOfStarterVillagers;

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
    }


}
