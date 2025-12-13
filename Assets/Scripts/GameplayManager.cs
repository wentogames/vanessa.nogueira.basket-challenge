using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject basketBall;
    [SerializeField] private CapsuleCollider netCollider;

    private readonly Vector3 PlayerCenterInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerLeftInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerRightInitialPos = new Vector3(0, 0, 3.85f);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
