using UnityEngine;

/**
 * This component spawns the given laser-prefab whenever the player clicks a given key.
 * It also updates the "scoreText" field of the new laser.
 * Additionally, it supports cheat codes to switch between weapons.
 */
public class ModifiedLaserShooter : ClickSpawner
{
    [SerializeField]
    [Tooltip("The default laser prefab (LaserWithScoreAdder)")]
    private GameObject defaultLaserPrefab;

    [SerializeField]
    [Tooltip("The modified laser prefab (ModifiedLaserWithScoreAdder)")]
    private GameObject modifiedLaserWithScoreAdder;

    [SerializeField]
    [Tooltip("Cheat code to activate the modified weapon (e.g., 'ABC')")]
    private string cheatCodeToActivate = "ABC";

    [SerializeField]
    [Tooltip("Cheat code to revert to the default weapon (e.g., 'XYZ')")]
    private string cheatCodeToRevert = "XYZ";

    private string currentInput = ""; // Tracks the player's current input
    private bool isSpecialWeaponActive = false; // Tracks if the special weapon is active

    private NumberField scoreField; // Reference to the score display

    private void Start()
    {
        scoreField = GetComponentInChildren<NumberField>();
        if (!scoreField)
        {
            Debug.LogError($"No child of {gameObject.name} has a NumberField component!");
        }

        // Set default prefab initially
        prefabToSpawn = defaultLaserPrefab;
        Debug.Log($"Initialized with default prefab: {prefabToSpawn.name}");
    }

    private void Update()
    {
        HandleCheatCodeInput();

        // Handle spawning on key press
        if (spawnAction.WasPressedThisFrame())
        {
            spawnObject();
        }
    }

    private void HandleCheatCodeInput()
    {
        foreach (char c in Input.inputString.ToUpper()) // Process input as uppercase
        {
            currentInput += c;

            // Check for activating modified weapon
            if (currentInput.EndsWith(cheatCodeToActivate.ToUpper()))
            {
                ActivateModifiedWeapon();
                currentInput = ""; // Reset input
            }

            // Check for reverting to default weapon
            if (currentInput.EndsWith(cheatCodeToRevert.ToUpper()))
            {
                RevertToDefaultWeapon();
                currentInput = ""; // Reset input
            }
        }

        // Truncate input if it gets too long
        int maxLength = Mathf.Max(cheatCodeToActivate.Length, cheatCodeToRevert.Length);
        if (currentInput.Length > maxLength)
        {
            currentInput = currentInput.Substring(currentInput.Length - maxLength);
        }
    }

    private void ActivateModifiedWeapon()
    {
        if (modifiedLaserWithScoreAdder)
        {
            prefabToSpawn = modifiedLaserWithScoreAdder; // Switch prefab
            isSpecialWeaponActive = true; // Enable special weapon mode
            Debug.Log($"Activated modified weapon: {prefabToSpawn.name}");
        }
        else
        {
            Debug.LogError("Modified laser prefab is not assigned!");
        }
    }

    private void RevertToDefaultWeapon()
    {
        if (defaultLaserPrefab)
        {
            prefabToSpawn = defaultLaserPrefab; // Revert to default prefab
            isSpecialWeaponActive = false; // Disable special weapon mode
            Debug.Log($"Reverted to default weapon: {prefabToSpawn.name}");
        }
        else
        {
            Debug.LogError("Default laser prefab is not assigned!");
        }
    }

    protected override GameObject spawnObject()
    {
        // Call base class spawn logic
        GameObject newObject = base.spawnObject();

        // Adjust score based on weapon type
        ScoreAdder newObjectScoreAdder = newObject.GetComponent<ScoreAdder>();
        if (newObjectScoreAdder)
        {
            int pointsToAdd = isSpecialWeaponActive ? 2 : 1; // Double points for special weapon
            newObjectScoreAdder.SetScoreField(scoreField).SetPointsToAdd(pointsToAdd);
        }

        return newObject;
    }
}
