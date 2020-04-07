using System;
using System.Collections;
using UnityEngine;

public class RecordStarter : MonoBehaviour
{
    // ======================================================================================================= variables
    public AdvancedTimelineSetter advancedTimelineSetter;
    public GameObject image1;
    public GameObject image2;

    private bool update = true;
    // =========================================================================================================== Start
    private void Start()
    {
        image2.SetActive(false);
        StartCoroutine(UpdateNOW());
    }


    // ========================================================================================================== Update
    IEnumerator UpdateNOW()
    {
        while (update)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                advancedTimelineSetter.StartRecording();
                update = false;
                image1.SetActive(false);
                image2.SetActive(true);
                yield return 0;
            }

            yield return 0;
        }
    }
}
