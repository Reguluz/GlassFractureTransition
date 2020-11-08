using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicController : MonoBehaviour
{
    public bool StartButton;
    public GameObject ParentNode;
    
    private Rigidbody[] breakblocks;

    private bool _iAnimation = false;

    private new Animator _animator;
    // public Renderer[] Renderers;
    // Start is called before the first frame update
    void Start()
    {
        breakblocks = ParentNode.GetComponentsInChildren<Rigidbody>();
        Debug.Log("BlockLength" + breakblocks.Length);
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartButton && !_iAnimation)
        {
            _iAnimation = true;
            Change();
        }
    }

    void Change()
    {
        // foreach (var VARIABLE in breakblocks)
        // {
        //     VARIABLE.isKinematic = false;
        // }
        _iAnimation = false;
        _animator.SetBool("Play", true);
        // for (int i = 0; i < Renderers.Length; i++)
        // {
        //     Renderers[i].enabled = false;
        // }
        
    }
}
