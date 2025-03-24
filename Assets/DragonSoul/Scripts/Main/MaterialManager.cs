using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    List<Material> materialList = new List<Material>();

    public static MaterialManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMaterial(string materialName)
    {
        var material = materialList.FirstOrDefault(effect => effect.name == materialName);

        if (material == null) { return; }

        
    }
}
