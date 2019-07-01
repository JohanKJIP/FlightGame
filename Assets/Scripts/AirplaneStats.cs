using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneStats : MonoBehaviour
{
    public GameObject[] checkPoints;
    private int currentCheckpoint;
    
    void Start()
    {
        currentCheckpoint = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
        {
            // checkpoints left
            if (currentCheckpoint + 1 < checkPoints.Length)
            {
                checkPoints[currentCheckpoint + 1].SetActive(true);
                checkPoints[currentCheckpoint].SetActive(false);
            }
            currentCheckpoint++;

            // end of course/checkpoints
            if (currentCheckpoint == checkPoints.Length)
            {
                // TODO END game
                checkPoints[checkPoints.Length-1].SetActive(false);
                Debug.Log("END OF GAME!!!");
            }
            Debug.Log(currentCheckpoint);
        }
    }
}
