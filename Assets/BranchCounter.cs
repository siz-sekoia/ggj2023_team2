using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BranchCounter : MonoBehaviour
{
    private TextMeshPro textMesh;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCounter(int count)
    {
        if (textMesh)
            textMesh.SetText(string.Format("残り{0}回", count));
    }

}
