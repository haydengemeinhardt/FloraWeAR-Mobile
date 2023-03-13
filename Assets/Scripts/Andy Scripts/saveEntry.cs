using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using System;

public class saveEntry : MonoBehaviour
{
    // Initialized cloud stuff and signs the user in 
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    // I stole this from the samples, but it should save whatever is passed to it into the cloud
    private async Task ForceSaveSingleData(string key, string value)
    {
        try
        {
            Dictionary<string, object> oneElement = new Dictionary<string, object>();

            // It's a text input field, but let's see if you actually entered a number.
            if (Int32.TryParse(value, out int wholeNumber))
            {
                oneElement.Add(key, wholeNumber);
            }
            else if (Single.TryParse(value, out float fractionalNumber))
            {
                oneElement.Add(key, fractionalNumber);
            }
            else
            {
                oneElement.Add(key, value);
            }

            await CloudSaveService.Instance.Data.ForceSaveAsync(oneElement);

            Debug.Log($"Successfully saved {key}:{value}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.Log("This one");
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
    // I have this here as a function for cleanliness reasons, the bools stop it from trying to run itself while it's awaiting code completion 
    private async Task Save_Entry(string entry_key, string entry_value)
    {
        isSaving = true;
        await ForceSaveSingleData(entry_key, entry_value);
        isSaving = false;
    }

    public TextMeshProUGUI journalEntered;
    public TextMeshProUGUI prompt;
    public string file = "journals.txt";
    public GameObject saveButton;
    public TextAsset promptJson;
    bool isSaving = false;
    [System.Serializable]
    public class JournalEntry
    {
        public string journalText;
        public string promptText;
        public string savedAt;
    }

    public class Prompts
    {
        public string prompt_;
        // Doing it this way in case we want to add more to this later
    }

    public class PromptsList
    {
        public Prompts[] prompts; 
    }

    public PromptsList loadedPrompts = new PromptsList();

    void Start()
    {
        loadedPrompts = JsonUtility.FromJson<PromptsList>(promptJson.text);
    }

    // Theoretically, this saves the journal entry json to the cloud
    // I think it works? 
    public void Save()
    {
        JournalEntry thisEntry = new JournalEntry();
        thisEntry.journalText = journalEntered.text;
        thisEntry.promptText = prompt.text;
        thisEntry.savedAt = System.DateTime.Now.ToString();
        string json = JsonUtility.ToJson(thisEntry); 
        PlayerPrefs.SetString(("Journal Entry " + PlayerPrefs.GetInt("numberOfEntries", 0).ToString()), json);
        PlayerPrefs.SetInt("numberOfEntries", (PlayerPrefs.GetInt("numberOfEntries")) + 1);
        
        if (!(isSaving))
        {
            Save_Entry(("Journal Entry " + PlayerPrefs.GetInt("numberOfEntries", 0).ToString()), json);
        }
    }
}
