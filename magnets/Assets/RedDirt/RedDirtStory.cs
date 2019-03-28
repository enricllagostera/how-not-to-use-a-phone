using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class RedDirtStory : MonoBehaviour
{
    public TextAsset inkAsset;
    Story inkStory;
    bool waitingForChoice = false;

    private void Awake()
    {
        inkStory = new Story(inkAsset.text);
    }

    private void Start()
    {
        StartCoroutine(ShowKnot());
    }

    private void Update()
    {
        if (waitingForChoice)
        {
            int choice = -1;
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                choice = 1;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                choice = 1;
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                choice = 3;
            }
            if (choice > 0)
            {
                inkStory.ChooseChoiceIndex(choice - 1);
                StartCoroutine(ShowKnot());
            }
        }
    }

    IEnumerator ShowKnot()
    {
        while (inkStory.canContinue)
        {
            waitingForChoice = false;
            Debug.Log(inkStory.Continue());
            yield return new WaitForSeconds(5f);
        }
        if (inkStory.currentChoices.Count > 0)
        {
            for (int i = 0; i < inkStory.currentChoices.Count; ++i)
            {
                Choice choice = inkStory.currentChoices[i];
                Debug.Log("Choice " + (i + 1) + ". " + choice.text);
            }
            waitingForChoice = true;
        }
    }
}
