using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTagToChildren : MonoBehaviour
{
    // This function will copy the parent's tag to all child objects
    void Start()
    {
        CopyTagToAllChildren(transform);
    }

    private void CopyTagToAllChildren(Transform parent)
    {
        // Iterate through each child
        foreach (Transform child in parent)
        {
            // Assign the parent's tag to the child
            child.tag = parent.tag;

            // Recursively call this function if the child has more children
            if (child.childCount > 0)
            {
                CopyTagToAllChildren(child);
            }
        }
    }
}
