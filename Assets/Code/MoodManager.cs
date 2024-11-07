using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : MonoBehaviour
{

    public bool IsHappy;
    public bool IsSad;
    public bool IsAngry;
    public bool IsUpset;
    public bool IsIndifference;
    public bool noMood;

    public GameObject mood;
    public GameObject happy;
    public GameObject sad;
    public GameObject angry;
    public GameObject upset;
    public GameObject indifference;

    private Coroutine moodCoroutine;

    public void SetMood(string mood)
    {
        // Stop any ongoing mood coroutine
        if (moodCoroutine != null)
        {
            StopCoroutine(moodCoroutine);
        }

        // Set all moods to false initially
        IsHappy = false;
        IsSad = false;
        IsAngry = false;
        IsUpset = false;
        IsIndifference = false;
        noMood = false;

        // Start the coroutine to reset the mood after 3 seconds
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(3f));
    }

    private IEnumerator ResetMoodAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset all moods to false and set noMood to true
        IsHappy = false;
        IsSad = false;
        IsAngry = false;
        IsUpset = false;
        IsIndifference = false;
        noMood = true;
    }

    private void Update()
    {
        // Check the current mood and display the corresponding UI
        if (IsHappy)
        {
            ShowHappy();
        }
        else if (IsSad)
        {
            ShowSad();
        }
        else if (IsAngry)
        {
            ShowAngry();
        }
        else if (IsUpset)
        {
            ShowUpset();
        }
        else if (IsIndifference)
        {
            ShowIndifference();
        }
        else if (noMood)
        {
            ShowNoMood();
        }
    }
    public void happymood()
    {
        IsHappy = true;
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(2f));

    }
    public void sadmood()
    {
        IsSad = true;
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(2f));

    }
    public void angrymood()
    {
        IsAngry = true;
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(2f));

    }
    public void upsetmood()
    {
        IsUpset = true;
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(2f));

    }
    public void indifferencemood()
    {
        IsIndifference = true;
        moodCoroutine = StartCoroutine(ResetMoodAfterDelay(2f));

    }
    public void nomood()
    {
        noMood = true;

    }



    private void ShowHappy()
    {
        mood.SetActive(true);
        happy.SetActive(true);
        sad.SetActive(false);
        angry.SetActive(false);
        upset.SetActive(false);
        indifference.SetActive(false);
    }

    private void ShowSad()
    {
        mood.SetActive(true);
        happy.SetActive(false);
        sad.SetActive(true);
        angry.SetActive(false);
        upset.SetActive(false);
        indifference.SetActive(false);
    }

    private void ShowAngry()
    {
        mood.SetActive(true);
        happy.SetActive(false);
        sad.SetActive(false);
        angry.SetActive(true);
        upset.SetActive(false);
        indifference.SetActive(false);
    }

    private void ShowUpset()
    {
        mood.SetActive(true);
        happy.SetActive(false);
        sad.SetActive(false);
        angry.SetActive(false);
        upset.SetActive(true);
        indifference.SetActive(false);
    }

    private void ShowIndifference()
    {
        mood.SetActive(true);
        happy.SetActive(false);
        sad.SetActive(false);
        angry.SetActive(false);
        upset.SetActive(false);
        indifference.SetActive(true);
    }

    private void ShowNoMood()
    {
        mood.SetActive(false);
        happy.SetActive(false);
        sad.SetActive(false);
        angry.SetActive(false);
        upset.SetActive(false);
        indifference.SetActive(false);
    }
}
