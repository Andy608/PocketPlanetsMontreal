using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAnimation : MonoBehaviour
{
    [SerializeField] private GameObject animationPrefab;
    [SerializeField] private int overlaps;

    private GameObject[] animations;

    private void Start()
    {
        animations = new GameObject[overlaps];
        GameObject currentAnimation;
        float normalizedTimeDelta = 1.0f / overlaps;

        for (int i = 0; i < overlaps; ++i)
        {
            currentAnimation = animations[i];
            currentAnimation = Instantiate(animationPrefab, transform);
            currentAnimation.transform.SetParent(transform);
            currentAnimation.GetComponent<Animator>().SetFloat("CycleOffset", i * normalizedTimeDelta);
        }
    }
}
