using UnityEngine;
using TMPro;

public class HatShopDisplay : MonoBehaviour
{
    public string hatName; // Name of the hat
    public GameObject textBox; // Reference to the UI TextBox (Panel or Text element)
    public TextMeshProUGUI hatInfoText; // Reference to the TextMeshProUGUI component for displaying hat info
    public HatShop hatShop; // Reference to the HatShop script
    public bool isShopping;
    private void Start()
    {
        // Initialize the reference to the HatShop script
        hatShop = GameObject.FindObjectOfType<HatShop>();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the collider
        if (other.CompareTag("Player"))
        {
            // Get the cost of the hat from the HatShop script
            int cost = GetHatCost(hatName);

            // Update the text with the hat name and cost
            hatInfoText.text = $"{hatName}\nCost: {cost} Coins";

            // Activate the text box
            textBox.SetActive(true);
            isShopping = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the collider
        if (other.CompareTag("Player"))
        {
            // Deactivate the text box
            textBox.SetActive(false);
            isShopping = false;
        }
    }

    // Method to get the cost of the hat based on its name
    private int GetHatCost(string hatName)
    {
        switch (hatName)
        {
            case "Crown":
                return hatShop.crownCost;
            case "Forest":
                return hatShop.forestCost;
            case "Funny":
                return hatShop.funnyCost;
                case "Sleep":
                return hatShop.sleepCost;
            case "Forge":
                return hatShop.forgeCost;
            case "Fancy":
                return hatShop.fancyCost;
            default:
                Debug.LogWarning($"Unknown hat name: {hatName}");
                return 0; // Default cost if the hat name is not recognized
        }
    }
}