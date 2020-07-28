using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlacementController : MonoBehaviour
{
    public static PlacementController instance;

    [SerializeField]
    private GameObject placableObjectPrefab;

    private GameObject currentPlaceableObject;

    [SerializeField]
    private FixedJoystick rotationHandle;

    private float objectRotation;

    // GUI
    public GameObject[] archerControlsGUI;
    public GameObject builderControlsGUI; 

    public int numOfPlacableObjects = 5;

    private int fingerID = -1;

    public GameObject smokeBuild;

    public Text fenceAmountText;

    void Awake()
    {
#if !UNITY_EDITOR
     fingerID = 0; 
#endif
        instance = this;

        if (PlayerPrefs.HasKey("numOfPlacableObjects"))
        {
            // we had a previous session
            numOfPlacableObjects = PlayerPrefs.GetInt("numOfPlacableObjects", 5);
        }
        else { SavePrefs(); }
    }

    void Start()
    {
        fenceAmountText.text = numOfPlacableObjects.ToString();
    }

    private void Update()
    {
        HandleNewOjectPlacement();
        CloseBuilder();

        if (currentPlaceableObject != null)
        {
            MoveCurrentTracableObjectToFinger();
            RotatePlaceableObject();
            ReleaseObject();
        }
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("numOfPlacableObjects", numOfPlacableObjects);
    }

    private void HandleNewOjectPlacement()
    {
        if (CrossPlatformInputManager.GetButtonDown("PICK_UP"))
        {
            if(numOfPlacableObjects > 0)
            {
                SHOW_ARCHER_CONTROLS(false);
                builderControlsGUI.SetActive(true);
                if (currentPlaceableObject == null)
                {
                    //BUILDER_VIEW_ANIME.SetTrigger("darkenScreen");
                    currentPlaceableObject = Instantiate(placableObjectPrefab);
                }
                else
                {
                    Destroy(currentPlaceableObject);
                }
            }
        }
    }

    private void CloseBuilder()
    {
        if (CrossPlatformInputManager.GetButtonDown("CLOSE_BUILDER"))
        {
            if(currentPlaceableObject != null) { Destroy(currentPlaceableObject); }
            //BUILDER_VIEW_ANIME.SetTrigger("undarkenScreen");
            builderControlsGUI.SetActive(false);
            SHOW_ARCHER_CONTROLS(true);
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void MoveCurrentTracableObjectToFinger()
    {
        if (IsPointerOverUIObject()) { Debug.Log("GUUU UUUUUUUUUUUUUUUUUUUUUUUUUUUU UUU UU III");  return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotatePlaceableObject() // TO:DO
    {
        objectRotation += /*Input.mouseScrollDelta.y*/rotationHandle.input.x;
        currentPlaceableObject.transform.Rotate(Vector3.up, objectRotation * /*10f*/.5f);
    }

    private void ReleaseObject()
    {
        if (CrossPlatformInputManager.GetButtonDown("PLACE"))
        {
            GameObject smoke = Instantiate(smokeBuild, currentPlaceableObject.transform, false);
            Destroy(smoke, 3f);
            currentPlaceableObject = null;
            numOfPlacableObjects--;
            SavePrefs();
            fenceAmountText.text = numOfPlacableObjects.ToString();
        }
    }

    private void SHOW_ARCHER_CONTROLS(bool show)
    {
        foreach(GameObject GUI in archerControlsGUI)
        {
            GUI.SetActive(show);
        }
    }

    public void updateTextAmount()
    {
        fenceAmountText.text = numOfPlacableObjects.ToString();
    }
}
