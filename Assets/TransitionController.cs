using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TransitionController : MonoBehaviour
{
    public DynamicController DynamicController;
    public RenderTexture screencopy;
    private Camera _camera;

    private CommandBuffer _commandBuffer;
    private static readonly int ID_ScreenCopy = Shader.PropertyToID("_ScreenCopyTexture");

    private Renderer _renderer;

    private bool _bridge;
    // Start is called before the first frame update
    void OnEnable()
    {
        _camera = Camera.main;
        if (!_camera)
        {
            Debug.Log("No camera");
            this.enabled = false;
            return;
        }
        _camera.depthTextureMode |= DepthTextureMode.Depth;
        _renderer = GetComponent<Renderer>();
        _commandBuffer = new CommandBuffer(){ name = "Grab Before Transparent"};
        
    }

    // Update is called once per frame
    void Update()
    {
        _bridge = DynamicController.StartButton;
        cmdRun();
    }
    
    private void cmdRun()
    {
        if (_commandBuffer != null)
        {
            _camera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha,_commandBuffer);
        }
        if (!_bridge)
        {
            int _srcW = _camera.pixelWidth;
            int _srcH = _camera.pixelHeight;
             _commandBuffer.ClearRenderTarget(true, true, Color.black);
            _commandBuffer.Clear();
            _commandBuffer.GetTemporaryRT(ID_ScreenCopy, _srcW, _srcH, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.sRGB);
        
            _commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, screencopy);
            
            _commandBuffer.SetGlobalTexture(ID_ScreenCopy, screencopy);
            _camera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, _commandBuffer);
        }
        
        // _commandBuffer.DrawRenderer(_renderer, _renderer.sharedMaterial);
       
    }

    // private void OnRenderImage(RenderTexture src, RenderTexture dest)
    // {
    //     int _srcW = _camera.pixelWidth;
    //     int _srcH = _camera.pixelHeight;
    //     _commandBuffer.Clear();
    //     _commandBuffer.GetTemporaryRT(ID_ScreenCopy, _srcW, _srcH, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.sRGB);
    //     if (!DynamicController.StartButton)
    //     {
    //         _commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, screencopy);
    //     }
    //     Shader.SetGlobalTexture(ID_ScreenCopy, screencopy);
    //     //_commandBuffer.Blit(screencopy, ID_ScreenCopy);
    //     _commandBuffer.Blit(screencopy, dest);
    // }
    
    private void OnDisable()
    {
        if (_commandBuffer != null)
        {
            _camera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, _commandBuffer);
            _commandBuffer.Dispose();
        }
    }
}
