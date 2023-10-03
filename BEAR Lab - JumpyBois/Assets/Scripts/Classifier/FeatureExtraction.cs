using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureExtraction : MonoBehaviour
{

    // TODO: Below is the plan for getting the classifier from MatLab to Unity (C#)

        // Custom Input Profile: Delsys/FMG -> Unity
        // Setup Menu
            // Looks at grasps we are testing in Game Settings 
                // Loops through these grasps and instructs user to hold each grasp for x seconds
                // Records data in data structure (recSession)
            // Signal Treatment Step
                // Cleans up data gathered in recSession and stores in sigTreated
            // Feature Extraction
                // Looks at cleaned up data (sigTreated)
                // Extracts features (Look at Marcus's MatLab script)
            // Train classifier/pattern rec. algorithm
                // Look at Marcus's MatLab code (Matlab_Real_Time_Control)
        
         // Gameplay
            // Uses the previously created classifier to predict real-time data
                // Look at Marcus's MatLab code (Matlab_Real_Time_Control)








}
