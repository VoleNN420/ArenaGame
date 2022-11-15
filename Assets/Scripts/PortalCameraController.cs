using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraController : MonoBehaviour
{	

	public PortalCameraController linkedPortal;
	public MeshRenderer screen;
	Camera playerCamera;
	Camera portalCamera;
	RenderTexture viewTexture;

    private void Awake()
    {
		playerCamera = Camera.main;
		portalCamera = GetComponentInChildren<Camera>();
		portalCamera.enabled = false;
		
	}

    void CreateViewTexture()
    {
		if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
		{
			if (viewTexture != null)
			{
				viewTexture.Release();
			}
			viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
			// Render the view from the portal camera to the view texture
			portalCamera.targetTexture = viewTexture;
			// Display the view texture on the screen of the linked portal
			linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
		}

	}

    public void Render()
    {
		screen.enabled = false;
		CreateViewTexture();
		
		var m = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCamera.transform.localToWorldMatrix;
		

		portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

		portalCamera.Render();

		screen.enabled = true;

    }





}
