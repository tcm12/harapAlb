﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
