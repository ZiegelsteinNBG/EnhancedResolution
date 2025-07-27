using Harmony;
using MelonLoader;
using Rotorz.Games;
using RwActions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MelonLoader.UnhollowerSupport;


namespace Enhanced_Resolution
{
    public class EnhancedResolution: MelonMod
    {
        // Camera
        List<Camera> camerasOrtho;
        RenderTexture newRT, oldRT;
        GameObject UI = new GameObject(), cameraTemp = new GameObject(), UIcopy, token;
        int counterUI = 0;
        List<float> ingameOrthoUI = new List<float>(), menuOrthoUI = new List<float>();
        float oldYScaleBar = -1;
        bool fixUIBool = false, fixUIBook = false, fixOrthoPlay = false;
        Transform DiagEffects;
        public override void OnUpdate()
        {
            try
            {
                if (GameObject.Find("__Prerequisites__/Character Origin/Character Root/Ellie_Default") != null && token == null)
                {
                    // Camera Testing
                    Camera main = GameObject.Find("__Prerequisites__/Angled Camera Rig/LocalSpace/Main Camera").GetComponent<Camera>();
                    camerasOrtho = new List<Camera>();
                    camerasOrtho.Add(main);
                    
                    oldRT = main.targetTexture;

                    
                    newRT = new RenderTexture();
                    if (oldRT != null)
                    {
                        // Create a new RenderTexture with the same format but higher resolution
                        newRT = new RenderTexture(1920, 1080, 24);

                        newRT.antiAliasing = oldRT.antiAliasing; // Keep same AA settings
                        newRT.filterMode = oldRT.filterMode; // Keep same filter mode
                        newRT.wrapMode = oldRT.wrapMode; // Keep same wrap mode
                        newRT.useMipMap = oldRT.useMipMap; // Keep mipmapping
                        newRT.Create();

                        // Assign the new RenderTexture to the camera
                        main.targetTexture = newRT;
                    }
                    GameObject.DontDestroyOnLoad(oldRT);
                    GameObject.DontDestroyOnLoad(newRT);
                    // Set RT for playerCameras
                    Renderer[] renderers = GameObject.Find("__Prerequisites__").GetComponentsInChildren<Renderer>(true);
                    foreach (Renderer renderer in renderers)
                    {
                        foreach (Material mat in renderer.materials)
                        {
                            if (mat.mainTexture == oldRT)
                            {
                                mat.mainTexture = newRT;
                            }
                        }
                    }

                    // Set RT for cutscenCameras
                    Camera[] cameras = GameObject.Find("Cutscenes").GetComponentsInChildren<Camera>(true);
                    foreach (Camera camera in cameras)
                    {
                        if (camera.targetTexture == oldRT)
                        {
                            camera.targetTexture = newRT;
                            camerasOrtho.Add(camera);
                        }

                    }
                    // Set RT for Events
                    cameras = GameObject.Find("Events").GetComponentsInChildren<Camera>(true);
                    foreach (Camera camera in cameras)
                    {
                        if (camera.targetTexture == oldRT)
                        {
                            camera.targetTexture = newRT;
                            camerasOrtho.Add(camera);
                        }

                    }


                    cameras = GameObject.Find("UI/").GetComponentsInChildren<Camera>(true);
                    switch(counterUI){
                        case 0:
                            UI = GameObject.Find("UI/");
                            DiagEffects = FindByNameWithSlashes(UI.transform, "Diag/Effects Camera");   
                            cameraTemp = GameObject.Instantiate(UI);
                            cameraTemp.name = "CameraTemp";
                            GameObject.Destroy(cameraTemp.transform.Find("Manager").GetComponent<PlayerState>());
                            cameraTemp.SetActive(false);
                            GameObject.DontDestroyOnLoad(cameraTemp); token = new GameObject();

                            //UIcopy = GameObject.Instantiate(cameraTemp);
                            //UIcopy.name = "UICopy";
                            string[] keywords = { "PauseMenu" };
                            for( int i = 0; i < UI.transform.childCount; i++)
                            {
                                if (keywords.Any(UI.transform.GetChild(i).name.Contains)) UI.transform.GetChild(i).gameObject.SetActive(false);
                            }
                            cameras = UI.GetComponentsInChildren<Camera>(true);
                            counterUI++;
                            break;
                        case 1:
                        //case 2:
                        //    if (cameraTemp == null)
                        //    {
                        //        MelonLogger.Msg("Searching cameraTemP");
                        //        cameraTemp = GameObject.Find("CameraTemp");
                        //        if (cameraTemp == null)
                        //        {
                        //            MelonLogger.Msg("cameraTemP not found");
                        //        }
                        //    }
                        //    if (UIcopy != null)
                        //    {
                        //        GameObject.Destroy(UIcopy);
                        //    }
                        //    UIcopy = GameObject.Instantiate(cameraTemp);
                        //    UIcopy.name = "UICopy";
                        //    cameras = UIcopy.GetComponentsInChildren<Camera>(true);
                            counterUI = 2;
                            break;
                    };
                        
                    foreach (Camera camera in cameras)
                    {
                        if (camera.targetTexture == oldRT)
                        {
                            camerasOrtho.Add(camera);
                            camera.targetTexture = newRT;                        
                        }
                    }

                    
                    fixUIBool = false;
                    fixUIBook = false;
                    fixOrthoPlay = false;
                    token = new GameObject();
                    token.name = "EnhancedResolutionToken";
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"preparingCam failed at {ex.Data} {ex.Source}");
                MelonLogger.Error($"Exception: {ex.Message}");
                MelonLogger.Error($"Stack Trace: {ex.StackTrace}");
            }

            try
            {
                if (GameObject.Find("__Prerequisites__/Character Origin/Character Root/Ellie_Default") != null && camerasOrtho != null )
                {
                    UIcopy?.SetActive(true);

                    //if (! fixUIBool)
                    //{
                    //    MelonLogger.Msg("adjust UI");
                    //    string[] keywords = { "Inventory" };
                    //    for (int i = 0; i < UI.transform.GetChildCount(); i++)
                    //    {
                    //        if (keywords.Any(UI.transform.GetChild(i).name.Contains)) UI.transform.GetChild(i).gameObject.SetActive(true);
                    //    }
                    //    fixUIBool = fixInterfaceUI(cameraTemp, UI);
                    //    for (int i = 0; i < UI.transform.GetChildCount(); i++)
                    //    {
                    //        if (keywords.Any(UI.transform.GetChild(i).name.Contains)) UI.transform.GetChild(i).gameObject.SetActive(false);
                    //    }
                    //    if(counterUI < 2)fixUIBool = fixInterfaceUI(UIcopy, UI);
                    //}
                    //if(PlayerState.gameState == PlayerState.gameStates.play && !fixUIBook)
                    //{
                    //    fixBookManager(UI.transform.Find("Manager").GetComponent<BookManager>(), UIcopy?.transform.Find("BookCanvas"), DiagEffects?.GetComponent<MainFader>());
                    //    fixUIBook = true;
                    //}

                    //float newYScaleBar = DiagEffects.Find("Bars/TopBar").localScale.y;
                    //if (oldYScaleBar != newYScaleBar)
                    //{
                    //    MelonLogger.Msg("adjust BarBoxes");
                    //    oldYScaleBar = newYScaleBar;
                    //    if (UIcopy != null)
                    //    {
                    //        Transform DiagEffTarget = FindByNameWithSlashes(UIcopy.transform, "Diag/Effects Camera");
                    //        setDimUI(UIcopy.transform, GameObject.Find("UI/").transform, DiagEffTarget, DiagEffects);
                    //    }
                    //}
                    if (PlayerState.gameState != PlayerState.gameStates.play || !fixOrthoPlay )
                    {
                        foreach (Camera cam in camerasOrtho)
                        {
                            cam.orthographicSize = 18;
                        }
                        fixOrthoPlay = (PlayerState.gameState != PlayerState.gameStates.play)? false : true;
                        if( counterUI < 2 ) fixOrthoPlay = (PlayerState.gameState != PlayerState.gameStates.inventory)? false : true;
                    }

                }

                if (GameObject.Find("CameraTemp") != null && token == null)
                {

                    GameObject.Find("CameraTemp").SetActive(true);
                }
            }
            catch (System.Exception ex)
            {
                MelonLogger.Error($"settingOrtho failed at {ex.Data} {ex.Source}");
                MelonLogger.Error($"Exception: {ex.Message}");
                MelonLogger.Error($"Stack Trace: {ex.StackTrace}");
            }
        }


        bool checkScene()
        {
            string[] keywords = {""};
            return !keywords.Any(SceneManager.GetActiveScene().name.Contains);
        }

        bool fixInterfaceUI(GameObject UI, GameObject UIOri)
        {
            Transform inventoryCanvas = UI.transform.Find("Inventory/InventoryCanvas/");
            Transform inventoryCanvasOri = UIOri.transform.Find("Inventory/InventoryCanvas/");
            if (inventoryCanvas == null || inventoryCanvasOri == null)
            {
                MelonLogger.Error($"fixInterfaceUI(): failed finding transform Inventory/InventoryCanvas/");
                return false;
            }
            for (int i = 0; i < inventoryCanvas.GetChildCount(); i++)
            {
                Transform objects = inventoryCanvas.GetChild(i).Find("Objects/");
                Transform objectsOri = inventoryCanvasOri.GetChild(i).Find("Objects/");
                if (objects == null)
                {
                    MelonLogger.Error($"fixInterfaceUI(): failed finding transform Inventory/InventoryCanvas/Interface");
                    return false;
                }
                for (int j = 0; j < objects.GetChildCount(); j++)
                {
                    Transform childObject = objects.GetChild(j);
                    Transform childObjectOri = objectsOri.GetChild(j);
                    if (childObject == null)
                    {
                        MelonLogger.Error($"fixInterfaceUI(): failed finding childObject from {objects.name}");
                        return false;
                    }
                    if(!recHelper(childObject, childObjectOri))return false;
                }
            }

            return true;
        }

        bool recHelper(Transform childObject, Transform childObjectOri)
        {
            
            for (int w = 0; w < childObject.GetChildCount(); w++)
            {
                Transform textObject = childObject.GetChild(w);
                Transform textObjectOri = childObjectOri.GetChild(w);
                if (textObject == null)
                {
                    MelonLogger.Error($"fixInterfaceUI(): failed  interfaceObj.GetChild(w) null");
                    continue;
                }
                if (!recHelper(textObject, textObjectOri)) return false;
                if (textObject.GetComponent<LocalizedTextMesh>() == null) continue;
                GameObject.Destroy(textObject.GetComponent<LocalizedTextMesh>());
                TextMeshProUGUI textMeshProUGUI = textObject.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI textMeshProUGUIOri = textObjectOri.GetComponent<TextMeshProUGUI>();
                if (textMeshProUGUI == null)
                {
                    MelonLogger.Error($"fixInterfaceUI(): failed finding textMeshProUGUI at {textObject.name}");
                    continue;
                }

                if (textMeshProUGUIOri.m_text == "") continue;
                if (textMeshProUGUIOri.m_text.Contains("!!MISSING STRING")) return false;
                textMeshProUGUI.m_text = textMeshProUGUIOri.m_text;
                textMeshProUGUI.old_text = textMeshProUGUIOri.old_text;
                textMeshProUGUI.enabled = false;
                textMeshProUGUI.enabled = true; // cycle the update
            }
            return true;
        }


        void setDimUI(Transform UI, Transform UIOri, Transform target, Transform targetOri)
        {

            Transform topBar = target.Find("Bars/TopBar");
            if (topBar != null)
            {
                float targetTop = targetOri.Find("Bars/TopBar").localScale.y;
                targetTop = (targetTop < 2) ? targetTop * 3 : targetTop; 
                topBar.localScale = new Vector3(3, targetTop, 3);
            }
            Transform bottomBar = target.Find("Bars/BottomBar");
            if (bottomBar != null)
            {
                float targetTop = targetOri.Find("Bars/BottomBar").localScale.y;
                targetTop = (targetTop < 2) ? targetTop * 3 : targetTop;
                bottomBar.localScale = new Vector3(3, targetTop, 3);
            }
            
        }

        Transform FindByNameWithSlashes(Transform parent, string exactName)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == exactName)
                    return child;
            }
            return null;
        }

        void fixBookManager(BookManager managerOri, Transform target, MainFader targetFader )
        {
            try
            {
                managerOri.BG = target.Find("BoxHolder/BG").GetComponent<UnityEngine.UI.Image>();
                managerOri.Canvas = target.gameObject;
                managerOri.DarkMask = target.Find("BoxHolder/DarkMask").GetComponent<UnityEngine.UI.Image>();
                managerOri.EffectsFader = targetFader;
                managerOri.PageIndicator = target.Find("BoxHolder/Page Indicator").GetComponent<TMPro.TextMeshProUGUI>();
                managerOri.TextBox = target.Find("BoxHolder/Book Content").GetComponent<TextMeshProUGUI>();
                managerOri.screen = UIcopy.transform.Find("Inventory/InventoryCanvas/Radio Diagnostics Mask/Objects/Memories").GetComponent<BookScreen>();
                target.Find("BoxHolder").localScale = new Vector3(3, 3, 3);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"fixBookManager() failed at {ex.Data} {ex.Source}");
                MelonLogger.Error($"Exception: {ex.Message}");
                MelonLogger.Error($"Stack Trace: {ex.StackTrace}");
            }

        }
    }
}
