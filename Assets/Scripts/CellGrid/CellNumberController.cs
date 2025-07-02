using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

// Handle all numbers in the cell
public class CellNumberController: MonoBehaviour
{
    private string _color;
    private int _number;
    // private GameObject _numberPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // Fill number in the cell
    public CellNumberController SetNumber(int number)
    {
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        this.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        this.gameObject.SetActive(true);
        
        this.gameObject.GetComponent<NumberController>().enabled = false; // disable number controller script to avoid misclick and throwing error
        this.gameObject.tag = "Untagged";
        
        // GameObject prefab = NumberDatabase.Instance.GetNumber(number);
        //if (prefab != null) // Handles empty cell for 0
        //{
        //    // Number prefab transform and material setup
        //    // this._numberPrefab = Instantiate(prefab, transform);
        //    this._numberPrefab.transform.localPosition = Vector3.zero;
        //    this._numberPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
        //    this._numberPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //    this._numberPrefab.GetComponent<NumberController>().enabled = false; // disable number controller script to avoid misclick and throwing error
        //    this._numberPrefab.tag = "Untagged";
        //    this._numberPrefab.SetActive(true);
        //}
        return this;
    }

    /// <summary>
    /// Highlight the number with different color material
    /// red: invalid, blue: user filled number, black: puzzle
    /// </summary>
    public CellNumberController SetColor(string color)
    {
        Renderer rend = this.gameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            switch (color)
            {
                case "red":
                    rend.material = Resources.Load("Materials/Number_Red_Mat", typeof(Material)) as Material;
                    break;
                case "black":
                    rend.material = Resources.Load("Materials/Number_Black_Mat", typeof(Material)) as Material;
                    break;
                case "blue":
                    rend.material = Resources.Load("Materials/Number_Blue_Mat", typeof(Material)) as Material;
                    break;
            }
        }
        return this;
    }

    // Determine color of the number
    private string NumberColor(bool duplicateExist)
    {
        if (duplicateExist) return "red";
        else return "blue";
    }


}
