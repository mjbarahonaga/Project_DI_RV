using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeEnemyStyle : MonoBehaviour
{
    public List<GameObject> ParentRandomizedComponents;

#if UNITY_EDITOR
    [SerializeField] private bool _testRandomize = false;
#endif
    public void Randomize()
    {
        int length = ParentRandomizedComponents.Count;
        for (int i = 0; i < length; ++i)
        {
            int lengthChilds = ParentRandomizedComponents[i].transform.childCount;
            var parentTransform = ParentRandomizedComponents[i].transform;
            for(int j = 0; j < lengthChilds; ++j)
            {
                parentTransform.GetChild(j).gameObject.SetActive(false);
            }
            parentTransform.GetChild(Random.Range(0,lengthChilds)).gameObject.SetActive(true);
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_testRandomize)
        {
            Randomize();
            _testRandomize = false;
        }
    }
#endif
}
