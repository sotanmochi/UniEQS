using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnvQueryGenerator
{
    List<EnvQueryItem> GenerateItems(int numTests, Transform centerOfItems);
}