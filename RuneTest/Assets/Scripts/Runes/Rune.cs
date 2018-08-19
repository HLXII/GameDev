using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

/// <summary>
/// The Base Rune GameObject
/// </summary>
public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, Selectable
{

    // RuneData to store the specific instance of the rune
    private RuneData runeData;
    public RuneData RuneData { get { return runeData; } 
        set 
        { 
            runeData = value;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = runeData.RuneTemplate.animatorController;
        } 
    }

    // If rune is selected
    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (value)
            {
                if (canvas == null)
                {
                    canvas = GameObject.Find("Canvas").transform;
                    buildCanvas = canvas.GetComponent<BuildCanvas>();
                }
                Instantiate(buildCanvas.runeSelectOutline, transform);
            }
            else
            {
                if (transform.childCount > 0)
                {
                    for (int i = transform.childCount - 1; i >= 0; i--)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }

    // Current rotation of rune
    private int rotation;
    public int Rotation { get { return rotation; } set { rotation = value; } }

    // Dragging variables
    private bool drag;

    // References to external objects
    protected DataManager dataManager;

    protected BuildSignalManager signalReciever;
    public BuildSignalManager SignalReceiver { get { return signalReciever; } set { signalReciever = value; } }

    protected BuildCanvas buildCanvas;

    private Transform canvas;
    private Transform table;
    private Transform page;

    private Bounds pageBounds;

    private GameObject previous_parent;
    private int previous_index;

    protected void Start()
    {

        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        canvas = GameObject.Find("Canvas").transform;
        table = GameObject.Find("TableContent").transform;
        page = GameObject.Find("PageContent").transform;

        buildCanvas = canvas.GetComponent<BuildCanvas>();

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
            rotation = (rotation + 1 + (int)runeData.RuneTemplate.runeType) % (int)runeData.RuneTemplate.runeType;
            transform.Rotate(Vector3.forward * 360 / (int)runeData.RuneTemplate.runeType);

            //Debug.Log("New Rotation : " + rotation);
        }
        else if (drag && (Input.mouseScrollDelta.x + Input.mouseScrollDelta.y) < 0f)
        {
            // Updating connection ports
            rotation = (rotation - 1 + (int)runeData.RuneTemplate.runeType) % (int)runeData.RuneTemplate.runeType;
            transform.Rotate(Vector3.back * 360 / (int)runeData.RuneTemplate.runeType);

            //Debug.Log("New Rotation : " + rotation);
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

                    if (swap_rune.GetComponent<Rune>().RuneData.RuneTemplate.isMovable && new_index != previous_index)
                    {
                        // Dropped onto moveable rune that is not the original position

                        // Swapping runes
                        Debug.Log(swap_rune.GetComponent<Rune>().RuneData);
                        Debug.Log(previous_index);
                        Debug.Log(swap_rune.GetComponent<Rune>().Rotation);
                        buildCanvas.addToPage(swap_rune.GetComponent<Rune>().RuneData, previous_index, swap_rune.GetComponent<Rune>().Rotation);
                        buildCanvas.addToPage(runeData, new_index, rotation);

                        Destroy(gameObject);

                        buildCanvas.updatePage();

                    }
                    else
                    {
                        // Dropped onto non-swappable rune
                        // Returning to original position (With additional things because of rotations
                        buildCanvas.addToPage(runeData, previous_index, rotation);

                        Destroy(gameObject);

                        buildCanvas.updatePage();

                    }


                }
                else
                {
                    // Dropped onto empty space

                    // Send to table
                    buildCanvas.addToTable(runeData);

                    buildCanvas.removeFromPage(previous_index);

                    Destroy(gameObject);

                    buildCanvas.updatePage();
                    buildCanvas.updateTable();

                }

            }
            else
            {
                // Dropped onto table

                // Send to table 
                buildCanvas.addToTable(runeData);

                buildCanvas.removeFromPage(previous_index);

                Destroy(gameObject);

                buildCanvas.updatePage();
                buildCanvas.updateTable();
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

                    if (swap_rune.GetComponent<Rune>().RuneData.RuneTemplate.isMovable)
                    {
                        // Dropped onto a movable rune

                        // Checking if swap_rune is not empty
                        if (!(swap_rune.GetComponent<Rune>().RuneData.RuneTemplate.id.Contains("Empty")))
                        {
                            buildCanvas.addToTable(swap_rune.GetComponent<Rune>().RuneData);
                        }

                        // Swapping runes
                        buildCanvas.addToPage(runeData, new_index, rotation);

                        buildCanvas.removeFromTable(runeData);

                        Destroy(gameObject);

                        buildCanvas.updatePage();
                        buildCanvas.updateTable();

                    }
                    else
                    {
                        // Dropped onto non-swappable rune

                        // Returning to original position
                        returnToOriginalPosition();

                    }

                }
                else
                {
                    // Dropped onto empty space

                    // Return to original position
                    returnToOriginalPosition();

                }

            }
            else
            {
                // Dropped onto table

                // Return to original position
                returnToOriginalPosition();

            }
        }

    }

    private void returnToOriginalPosition()
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

    public void sendEnergy()
    {

        //// Looping through all energy output
        //for (int i = 0; i < energyOut.Length; i++) {
        //	// If there is energy to be outputted
        //	if (energyOut [i] != null) {
        //		int outPort = (connections [i] + rotation) % sides;
        //		int neighborInPort = (outPort + sides / 2) % sides;
        //		// If there is a neighbor to output to
        //		if (neighbors [outPort] != null) {
        //			neighbors [outPort].GetComponent<Rune> ().receiveEnergy (energyOut [i], neighborInPort);
        //		// There is no neighbor to output to, cause some kind of malfunction? ***
        //		} else {
        //			signalReciever.receiveSignal ("Energy leak");
        //			gameObject.GetComponent<Animator> ().SetTrigger ("error");
        //		}
        //	}
        //}

        //// Clear energyOut
        //clearEnergyOut();

    }

    public void receiveEnergy(Energy energyIn, int port)
    {
        //for (int i = 0; i < connections.Length; i++) {
        //	if ((connections [i] + rotation) % sides == port) {
        //		this.energyIn [i] = energyIn;
        //		break;
        //	}
        //}
    }

    // Finds connected neighbors
    public virtual void findNeighbors() { }

    // Checks if there is a connection in the direction
    public bool isConnected(int direction)
    {
        //foreach (int connection in connections) {
        //	if (direction == (connection + rotation) % sides) {
        //		return true;
        //	}
        //}
        return false;
    }

    public bool checkNeighbors()
    {
        ////Debug.Log (id);
        //foreach (int connection in connections) {
        //	//Debug.Log ("C " + connection + "ACTUAL: " + ((connection + rotation) % sides));
        //	if (neighbors [(connection + rotation) % sides] == null) {
        //		//Debug.Log (Id);
        //		Debug.Log (rotation);
        //		foreach (GameObject n in neighbors) {
        //			Debug.Log (n);
        //		}
        //		//Debug.Log (neighbors[(connection + rotation) % sides]);
        //		return false;
        //	}
        //}
        //Debug.Log ("GOOD");
        return true;
    }

    public virtual string getInfo()
    {
        return "";//runeData.Id;
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

        if (runeData.RuneTemplate.isMovable)
        {

            drag = true;

            // Storing previous location data
            previous_parent = transform.parent.gameObject;
            previous_index = transform.GetSiblingIndex();

            // Creating empty rune to hold position
            GameObject empty = Instantiate(buildCanvas.rune, previous_parent.transform);
            empty.GetComponent<Rune>().RuneData = new RuneData(runeData.RuneTemplate.runeType.ToString() + " Empty");
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

        if (runeData.RuneTemplate.isMovable)
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

        /*
            if (movable) {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0) + mouse_offset;
            }*/

        drag = false;

        Drop();
        StopAllCoroutines();
        transform.localScale = new Vector3(1, 1, 1);

        previous_parent = null;
        previous_index = 0;

    }

    //public override string ToString ()
    //{
    //	string o = "";
    //	o += runeData.ToString ();
    //	o += "\nR: " + rotation.ToString();
    //	o += "\nConnections: ";
    //	foreach (int connection in connections) {
    //		o += connection.ToString () + " ";
    //	}
    //	o += "\nActual: ";
    //	foreach (int connection in connections) {
    //		o += ((connection+rotation) % sides).ToString () + " ";
    //	}
    //	o += "\nEnergyIn: ";
    //	foreach (Energy e in energyIn) {
    //		if (e == null) {
    //			o += "null ";
    //		} else {
    //			o += e.ToString () + " ";
    //		}
    //	}
    //	o += "\nEnergyOut: ";
    //	foreach (Energy e in energyOut) {
    //		if (e == null) {
    //			o += "null ";
    //		} else {
    //			o += e.ToString () + " ";
    //		}
    //	}
    //	if (neighbors != null) {
    //		o += "\nNeighbors: ";
    //		foreach (GameObject neighbor in neighbors) {
    //			if (neighbor == null) {
    //				o += "null ";
    //			} else {
    //				//o += neighbor.GetComponent<Rune> ().Id + " ";
    //			}
    //		}
    //	}
    //	return o;
    //}

    public virtual void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }

        if (runeData.RuneTemplate.isInteractable)
        {
            OnSelect();
        }
    }

    public void OnSelect()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").transform;
        }

        canvas.GetComponent<BuildCanvas>().runeSelect.GetComponent<RuneSelect>().Rune = gameObject;

        selected = true;
        //Instantiate(canvas.GetComponent<InventoryCanvas>().itemSelectOutline, transform);
    }

    public void DeSelect()
    {
        selected = false;
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
