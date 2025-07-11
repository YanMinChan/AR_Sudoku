using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

// Handle the digit GameObject and its color in each cell
public class CellNumberController: MonoBehaviour
{

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

        NumberController numCtr = this.gameObject.GetComponent<NumberController>();
        Destroy(numCtr); // destroy number controller for the annoying exception throwing when misclick

        this.gameObject.tag = "Untagged";
        
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
