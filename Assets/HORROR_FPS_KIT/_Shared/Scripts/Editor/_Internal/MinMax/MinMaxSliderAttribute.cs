using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MinMaxSliderAttribute : PropertyAttribute {

    public float Min { get; set; }
    public float Max { get; set; }
    public bool DataFields { get; set; } = true;
    public bool FlexibleFields { get; set; } = true;
    public bool Bound { get; set; } = true;
    public bool Round { get; set; } = true;

    public MinMaxSliderAttribute() : this(0, 1){}//MinMaxSliderAttribute

    public MinMaxSliderAttribute(float min, float max) {
    
        Min = min;
        Max = max;
    
    }//MinMaxSliderAttribute
    
}//MinMaxSliderAttribute