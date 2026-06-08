using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsInit : MonoBehaviour
{
    private void Start()
    {
        UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }
}
