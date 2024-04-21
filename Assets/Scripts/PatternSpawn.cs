using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PatternSpawn : MonoBehaviour
{
    public GameObject[] Patterns;
    public GameObject PreviousPattern;
    
    private bool isFirst = true;

    private int patternRandom = 0;
    // Start is called before the first frame update
    void Start()
    {
        patternRandom = Random.Range(0, Patterns.Length);
        SpawnPattern();
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPattern()
    {
        if (isFirst == true)
        {
            PreviousPattern = Instantiate(Patterns[patternRandom], new Vector3(-1.939f, -5.063f, 0), Quaternion.identity);
            isFirst = false;
        }
        else
        {
            PreviousPattern = Instantiate(Patterns[patternRandom], new Vector3(-1.939f, PreviousPattern.GetComponent<Pattern>().top.position.y, 0), Quaternion.identity);
        }

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(4f);
        patternRandom = Random.Range(0, Patterns.Length);
        SpawnPattern();
        StartCoroutine(wait());
    }
}
