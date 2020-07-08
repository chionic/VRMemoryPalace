using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class AddText : MonoBehaviour
{
    private string jsonString; //stores the plaintext version of the json file
    public textLoops textLoops; //list of all the different text Arrays
    public textLoop currentLoop; //the current text array (eg tutorial, peacePrize etc)
    public Text UiText = null; //The unity gameobject component that displays the text on screen
    public int arrayIndex = 0; //the current element index of the curretn text array
    private int runOrder = 0; //where in the order of runnable arrays the programme currently is
    private int minRandomValue = 10000; //the point beyond which there is no set order for arrays to run and the next array is randomly selected
    private string pathToFile2 = "";
    private string fileName2 = "";
    private filePathText fp;

    void Awake()
    {
        if (File.Exists("Assets/JSON_Files/tabletText.json"))
        {
            string jsonString = File.ReadAllText("Assets/JSON_Files/tabletText.json");
            fp = JsonUtility.FromJson<filePathText>(jsonString);
            fileName2 = fp.fileName;
            pathToFile2 = fp.pathToFile;
        }
        else
        {
            Debug.LogError("The specified json file was not found at the file path ");
            return;
        }
        Debug.Log(pathToFile2 + " + " + fileName2);
        if(pathToFile2 != "" && fileName2 != "")
        {
            jsonString = File.ReadAllText(pathToFile2 + "/" + fileName2); //imports the json assets as plain text
        }
        else
        {
            jsonString = File.ReadAllText(Application.dataPath + "/JSON_Files/sampleText"); //imports the json assets as plain text
        }
        textLoops = JsonUtility.FromJson<textLoops>(jsonString); //turns the plaintext into a list of arrays with certain properties
        getNextLoop(runOrder); //picks which of the text arrays should be run first
        UiText = this.GetComponent<Text>();

    }

    // Update is called once per frame
   /* void Update()
    {
        //helper function while VR headset and controllers not available -> press space to change text.
        if (Input.GetKeyDown("space"))
        {
            updateUIText();
        }
    }*/

    public void updateUIText()
    {
        changeText(currentLoop);
    }

    //Change text to next piece of text
    private void changeText(textLoop currentLoop)
    {
        //if the end of the array is reached, go to the next array or go back to the start of the current one
        if (arrayIndex >= currentLoop.Values.Length)
        {
            arrayIndex = 0;
            if (currentLoop.loop == false)
            {
                UiText.text = "Starting next set of instructions";
                //find next array in execution order (or a random array if none specified) and start that
                runOrder++;
                if (runOrder < textLoops.textLooper.Count)
                {
                    getNextLoop(runOrder);
                }
                else
                {
                    getNextLoop(minRandomValue);
                }
            }
            else
            {
                UiText.text = "Repeating current set of instructions";
            }
            
        }
        //otherwise pick the next piece of text in the current text array
        else
        {
            UiText.text = currentLoop.Values[arrayIndex];
            arrayIndex++;
        }
         
    }

    //Decides which text array to display next
    private void getNextLoop(int runOrder)
    {
        int temp = Random.Range(runOrder, textLoops.textLooper.Count); //picks a random array somewhere between the start of the random ones and the size of the list
        foreach (textLoop textloop in textLoops.textLooper) //loops through all the possible text arrays
        {
            if (textloop.runOrder == runOrder) //picks the next one in the runOrder
            {
                currentLoop = textloop;
                return; //and returns the function
            }
            else if(textloop.id == temp) //if the next one in the set run order is never reached, picks the next one based on a random number
            {
                currentLoop = textloop;
            }
        }
        if(minRandomValue == 10000) //the first time a random array is selected, sets the start of the random array numbers
        {
            minRandomValue = runOrder;
        }
        
    }
}
