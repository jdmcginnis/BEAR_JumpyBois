using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data {
    public class DataPoint {
        public string timeStamp;
        public int majority;

        public DataPoint(string time, int majorityNum) {
            timeStamp = time;
            majority = majorityNum;
        }
    }
}

