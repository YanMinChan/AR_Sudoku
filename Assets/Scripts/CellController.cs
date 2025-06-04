using UnityEngine;

public class CellController : MonoBehaviour
{
    //[SerializeField] private GameObject objectToPlace;

    public static CellController currentlySelected; // The cell is selected by player

    //private bool isFilled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //GameObject[] numberBar = GameObject.FindGameObjectsWithTag("NumberBar");
    }

    /// <summary>
    /// Select the current cell
    /// </summary>
    public void SelectThisCell()
    {
        currentlySelected = this;
        Debug.Log("Selected cell:" + gameObject.name);
    }

    /// <summary>
    /// Generate a number object to fill cell
    /// </summary>
    public void FillNumber(GameObject numberPrefab) {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject number = Instantiate(numberPrefab, transform);
        number.transform.localPosition = Vector3.zero;
        number.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
