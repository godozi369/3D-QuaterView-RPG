using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElementStrategy 
{
    ElementType ElementType { get; }
    void Apply(PlayerController player);
}
