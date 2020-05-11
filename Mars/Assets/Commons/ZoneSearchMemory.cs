using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Commons
{
    public class ZoneSearchMemory
    {
        private readonly Dictionary<Type, MemoryElement> elements = new Dictionary<Type, MemoryElement>();

        public void Save<T>(ICollection<T> zones) where T : Zone
        {
            var currentFrame = Time.frameCount;
            var position = new MemoryElement(currentFrame, zones);
            if (!elements.ContainsKey(typeof(T)))
            {
                elements.Add(typeof(T), position);
                return;
            }

            if (elements[typeof(T)].frameNumber == currentFrame) return;

            elements[typeof(T)] = position;
        }

        public IList<T> Get<T>() where T : Zone
        {
            var currentFrame = Time.frameCount;
            if (!elements.ContainsKey(typeof(T)) || elements[typeof(T)].frameNumber != currentFrame) return null;

            return elements[typeof(T)].zonesFound.Cast<T>().ToList();
        }

        public class MemoryElement
        {
            public int frameNumber;
            public IEnumerable<Zone> zonesFound;

            public MemoryElement(int frame, IEnumerable<Zone> zones)
            {
                frameNumber = frame;
                zonesFound = zones;
            }
        }
    }
}