using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText; // ✅ Display best time

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;
    private static float bestTime = Mathf.Infinity; // store shortest time

    void Update() {

        // tell time to increase / decrease depending on flag
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
        // show
        timerText.text = currentTime.ToString("0.00");
    }
    // ✅ Call this when player reaches WinTile
    public void CheckForBestTime()
    {
        if (currentTime < bestTime) // ✅ If this is the shortest time
        {
            bestTime = currentTime;
            bestTimeText.text = $"Best Time: {bestTime:0.00}"; // ✅ Update UI
            Debug.Log($"🏆 New Best Time: {bestTime:0.00} seconds!");
        }
    }

    // reset timer
    public void ResetTimer() {
        Debug.Log("⏳ Timer Reset!");
        currentTime = 0;
        timerText.text = "0.00";
    }
}
