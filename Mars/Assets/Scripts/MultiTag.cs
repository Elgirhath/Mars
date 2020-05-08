﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class MultiTag : MonoBehaviour {
        public string[] tags;

        public bool Contains(string tag) {
            int pos = Array.IndexOf(tags, tag);
            return pos > -1;
        }
    }
}