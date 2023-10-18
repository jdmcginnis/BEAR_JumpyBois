using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data {
    public class DataPoint {
        public string timeStamp {get; private set;}
        public int majority {get; private set;}

        public DataPoint(string time, int majorityNum) {
            timeStamp = time;
            majority = majorityNum;
        }


    }
}

