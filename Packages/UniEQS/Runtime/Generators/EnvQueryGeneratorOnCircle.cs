using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniEQS.Generators
{
    [Serializable]
    public class EnvQueryGeneratorOnCircle : IEnvQueryGenerator
    {
        public Transform CenterPosition;
        public float Radius = 4.0f;
        public float SpaceBetween = 1.0f;
        
        public List<EnvQueryItem> GenerateItems(int numberOfQueryTests)
        {
            var items = new List<EnvQueryItem>();
            
            var relativePosition = Vector3.zero;
            items.Add(new EnvQueryItem(numberOfQueryTests, relativePosition, CenterPosition)); // Center
            
            var numOfStepsForRadialDirection = (int)Mathf.Ceil(Radius / SpaceBetween);
            for(var ri = 1; ri <= numOfStepsForRadialDirection; ri++)
            {
                for(var k = 0; k < ri * 8; k++)
                {
                    var theta = 1.0f / ri * k * Mathf.PI / 4.0f;
                    relativePosition.x = ri * SpaceBetween * Mathf.Sin(theta);
                    relativePosition.y = 0.0f;
                    relativePosition.z = ri * SpaceBetween * Mathf.Cos(theta);
                    items.Add(new EnvQueryItem(numberOfQueryTests, relativePosition, CenterPosition));
                }
            }
            
            return items;
        }
    }
}