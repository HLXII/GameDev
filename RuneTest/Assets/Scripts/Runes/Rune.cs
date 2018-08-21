using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

/// <summary>
/// The Base Rune GameObject
/// </summary>
public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, Selectable
{

    // RuneSlot to store the specific instance of the rune
    private RuneSlot runeSlot;
    public RuneSlot RuneSlot { get { return runeSlot; }
        set
        {
            runeSlot = value;

            runeSlot.OnRuneDataChange += RuneDataChangeHandler;
            RuneDataChangeHandler(runeSlot.RuneData);

            if (runeSlot.RuneData.RuneTemplate.runeCovers != null && runeSlot.RuneData.RuneTemplate.runeCovers.Length > 0)
                this.RuneCover.sprite = runeSlot.RuneData.RuneTemplate.runeCovers[runeSlot.Rotation];

            gameObject.GetComponent<Animator>().runtimeAnimatorController = runeSlot.RuneData.RuneTemplate.animatorController;
        }
    }

    // If rune is selected
    private bool selected;
    public bool Selected { get { return selected; } set { selected = value; } }

    // Dragging variables
    private bool drag;

    // References to external objects
    protected DataManager dataManager;

    protected BuildSignalManager signalReciever;
    public BuildSignalManager SignalReceiver { get { return signalReciever; } set { signalReciever = value; } }

    protected BuildCanvas buildCanvas;
    public BuildCanvas BuildCanvas
    {
        get
        {
            if (buildCanvas == null)
            {
                buildCanvas = this.Canvas.GetComponent<BuildCanvas>();
            }
            return buildCanvas;
        }
    }

    private Transform canvas;
    public Transform Canvas
    {
        get
        {
            if (canvas == null) {
                canvas = GameObject.Find("Canvas").transform;
            }
            return canvas;
        }
    }
    private Transform table;
    private Transform page;

    // References to internal images
    private Image runeBase;
    private Image RuneBase {
        get {
            if (runeBase == null) {
                runeBase = gameObject.GetComponent<Image>();
            }
            return runeBase;
        }
    }
    private Image runeCover;
    private Image RuneCover
    {
        get
        {
            if (runeCover == null)
            {
                runeCover = transform.GetChild(0).GetComponent<Image>();
            }
            return runeCover;
        }
    }
    private Image runeEnergy;
    private Image RuneEnergy
    {
        get
        {
            if (runeEnergy == null)
            {
                runeEnergy = transform.GetChild(1).GetComponent<Image>();
            }
            return runeEnergy;
        }
    }

    private Bounds pageBounds;

    private GameObject previous_parent;
    private int previous_index;

    protected void Start()
    {

        // Finding DataManager
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        // Loading references to external transforms
        canvas = GameObject.Find("Canvas").transform;
        table = GameObject.Find("TableContent").transform;
        page = GameObject.Find("PageContent").transform;

        // Loading BuildCanvas
        buildCanvas = canvas.GetComponent<BuildCanvas>();

        // Generating pageBounds
        RectTransform pageView = (RectTransform)GameObject.Find("Page").transform;
        pageBounds = new Bounds(pageView.localPosition, pageView.rect.size);

        selected = false;

        previous_parent = null;
        previous_index = 0;

    }

    protected void Update()
    {
        if (drag && (Input.mouseScrollDelta.x + Input.mouseScrollDelta.y) > 0f)
        {
            // Updating connection ports
            runeSlot.Rotation = (runeSlot.Rotation + 1 + (int)runeSlot.RuneData.RuneTemplate.sides) % (int)runeSlot.RuneData.RuneTemplate.sides;
            //Debug.Log("New Rotation : " + runeSlot.Rotation);

            this.RuneCover.sprite = runeSlot.RuneData.RuneTemplate.runeCovers[runeSlot.Rotation];
        }
        else if (drag && (Input.mouseScrollDelta.x + Input.mouseScrollDelta.y) < 0f)
        {
            // Updating connection ports
            runeSlot.Rotation = (runeSlot.Rotation - 1 + (int)runeSlot.RuneData.RuneTemplate.sides) % (int)runeSlot.RuneData.RuneTemplate.sides;
            //Debug.Log("New Rotation : " + runeSlot.Rotation);

            this.RuneCover.sprite = runeSlot.RuneData.RuneTemplate.runeCovers[runeSlot.Rotation];
        }
    }


    private void RuneDataChangeHandler(RuneData runeData)
    {
        // Calculating total rank of rune

        // Determining whether rune requires a cover
        if (runeData.RuneTemplate.runeCovers != null && runeData.RuneTemplate.runeCovers.Length > 0)
        {
            // Setting rune cover
            this.RuneCover.enabled = true;
            this.RuneCover.sprite = runeData.RuneTemplate.runeCovers[runeSlot.Rotation];
        }
        else
        {
            this.RuneCover.enabled = false;
        }

        // Setting raycast depending on interactability
        this.RuneBase.raycastTarget = runeData.RuneTemplate.isInteractable;

        // Interactability also determines whether there is a rune base
        if (runeData.RuneTemplate.isInteractable)
        {
            this.RuneBase.enabled = true;
            this.RuneBase.sprite = this.BuildCanvas.runeBases[runeData.Rank];
        }
        else
        {
            this.RuneBase.enabled = false;
        }
    }

    private void Drop()
    {

        //Debug.Log ("Dropped Rune from " + previous_parent.name);

        //Finding drop location
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse = new Vector3(mouse.x, mouse.y, 0);
        string dropLocation = (pageBounds.Contains(mouse)) ? "Page" : "Table";

        // Raycasting to find where rune was dropped
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Page Runes"));

        if (previous_parent.name == "PageContent")
        {
            // Dropping a page rune

            //Debug.Log ("Dropping Page Rune");

            if (dropLocation == "Page")
            {
                // Dropped onto page

                if (hit.collider != null)
                {
                    // Dropped onto rune

                    GameObject swap_rune = hit.collider.gameObject;
                    int new_index = swap_rune.transform.GetSiblingIndex();

                    if (swap_rune.GetComponent<Rune>().RuneSlot.RuneData.RuneTemplate.isMovable && new_index != previous_index)
                    {
                        // Dropped onto moveable rune that is not the original position

                        // Swapping runes
                        buildCanvas.AddToPage(swap_rune.GetComponent<Rune>().RuneSlot, previous_index);
                        buildCanvas.AddToPage(runeSlot, new_index);

                        Destroy(gameObject);

                        buildCanvas.UpdatePage();
                        buildCanvas.UpdateSelection();
                    }
                    else
                    {
                        // Dropped onto non-swappable rune
                        // Returning to original position (With additional things because of rotations
                        buildCanvas.AddToPage(runeSlot, previous_index);

                        Destroy(gameObject);

                        buildCanvas.UpdatePage();
                        buildCanvas.UpdateSelection();
                    }


                }
                else
                {
                    // Dropped onto empty space

                    // Send to table
                    buildCanvas.AddToTable(runeSlot);

                    buildCanvas.RemoveFromPage(previous_index);

                    Destroy(gameObject);

                    buildCanvas.UpdatePage();
                    buildCanvas.UpdateTable();
                    buildCanvas.UpdateSelection();

                }

            }
            else
            {
                // Dropped onto table

                // Send to table
                buildCanvas.AddToTable(runeSlot);

                buildCanvas.RemoveFromPage(previous_index);

                Destroy(gameObject);

                buildCanvas.UpdatePage();
                buildCanvas.UpdateTable();
                buildCanvas.UpdateSelection();
            }

        }
        else
        {
            // Dropping a table rune

            //Debug.Log("Dropping Table Rune");

            if (dropLocation == "Page")
            {
                // Dropped onto page

                if (hit.collider != null)
                {
                    // Dropped onto rune

                    GameObject swap_rune = hit.collider.gameObject;
                    int new_index = swap_rune.transform.GetSiblingIndex();

                    if (swap_rune.GetComponent<Rune>().RuneSlot.RuneData.RuneTemplate.isMovable)
                    {
                        // Dropped onto a movable rune

                        // Checking if swap_rune is not empty
                        if (swap_rune.GetComponent<Rune>().RuneSlot.RuneData.RuneTemplate.id != buildCanvas.emptyId)
                        {
                            buildCanvas.AddToTable(swap_rune.GetComponent<Rune>().RuneSlot);
                        }

                        // Swapping runes
                        buildCanvas.AddToPage(runeSlot, new_index);

                        buildCanvas.RemoveFromTable(runeSlot);

                        Destroy(gameObject);

                        buildCanvas.UpdatePage();
                        buildCanvas.UpdateTable();
                        buildCanvas.UpdateSelection();
                    }
                    else
                    {
                        // Dropped onto non-swappable rune

                        // Returning to original position
                        ReturnToOriginalPosition();

                    }

                }
                else
                {
                    // Dropped onto empty space

                    // Return to original position
                    ReturnToOriginalPosition();

                }

            }
            else
            {
                // Dropped onto table

                // Return to original position
                ReturnToOriginalPosition();

            }
        }

    }

    private void ReturnToOriginalPosition()
    {
        Destroy(previous_parent.transform.GetChild(previous_index).gameObject);

        transform.SetParent(previous_parent.transform);
        transform.SetSiblingIndex(previous_index);
        if (previous_parent.name == "PageContent")
        {
            gameObject.layer = LayerMask.NameToLayer("Page Runes");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Table Runes");
        }
    }

    private IEnumerator expandAnimation(Vector3 new_scale)
    {
        Vector3 original_scale = transform.localScale;
        float rate = 10.0f;
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(original_scale, new_scale, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
    }

    private IEnumerator shrinkAnimation(Vector3 scale)
    {
        Vector3 original_scale = transform.localScale;
        Vector3 new_scale = new Vector3(Screen.width / 1600f, Screen.width / 1600f, 1);
        new_scale = scale;
        float rate = 10.0f;
        float t = 0.0f;
        while (t < 1.0)
        {
            t += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(original_scale, new_scale, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Checking if left button click
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (runeSlot.RuneData.RuneTemplate.isMovable)
        {

            drag = true;

            // Storing previous location data
            previous_parent = transform.parent.gameObject;
            previous_index = transform.GetSiblingIndex();

            // Creating empty rune to hold position
            GameObject empty = Instantiate(buildCanvas.rune, previous_parent.transform);
            empty.GetComponent<Rune>().RuneSlot = new RuneSlot(new RuneData(buildCanvas.voidId));
            empty.transform.SetSiblingIndex(previous_index);

            if (previous_parent.name == "TableContent")
            {
                empty.layer = LayerMask.NameToLayer("Table Runes");
            }
            else if (previous_parent.name == "PageContent")
            {
                empty.layer = LayerMask.NameToLayer("Page Runes");
            }

            // Start size transition animation
            if (previous_parent.name == "TableContent")
            {
                StartCoroutine(shrinkAnimation(page.parent.localScale));
            }

            // Setting parent and layer data
            transform.SetParent(canvas);
            gameObject.layer = LayerMask.NameToLayer("Generic Runes");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        // Checking if left button click
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (runeSlot.RuneData.RuneTemplate.isMovable)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        // Checking if left button click
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (runeSlot.RuneData.RuneTemplate.isMovable)
        {
            drag = false;

            Drop();
            StopAllCoroutines();
            transform.localScale = new Vector3(1, 1, 1);

            previous_parent = null;
            previous_index = 0;
        }

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }

        if (runeSlot.RuneData.RuneTemplate.isInteractable)
        {
            OnSelect();
        }
    }

    public void OnSelect()
    {

        this.BuildCanvas.runeSelect.GetComponent<RuneSelect>().Rune = gameObject;

        Instantiate(buildCanvas.runeSelectOutline, transform);

        selected = true;
    }

    public void DeSelect()
    {
        selected = false;
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
    }
}
