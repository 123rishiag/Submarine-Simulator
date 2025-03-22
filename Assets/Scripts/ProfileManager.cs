using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private UIController uiController;

    private List<ProfileData> profiles = new List<ProfileData>();
    private int selectedProfileIndex = -1;
    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/profiles.json";
        // C:\Users\{UserName}\AppData\LocalLow\{CompanyName}\{ProjectName}\profiles.json
        LoadProfiles();
    }
    private void LoadProfiles()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            profiles = JsonUtility.FromJson<ProfileListWrapper>(json).profiles;
        }
        RefreshProfileList();
    }

    public void SaveProfile()
    {
        string firstName = uiController.profileFirstNameField.text.Trim();
        string lastName = uiController.profileLastNameField.text.Trim();
        string location = uiController.profileLocationField.text.Trim();
        string email = uiController.profileEmailField.text.Trim();

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(location) || !IsValidEmail(email))
        {
            uiController.ShowValidationMessage("Invalid profile details.");
            return;
        }

        ProfileData newProfile = new ProfileData(firstName, lastName, location, email);

        // In case of existing
        if (selectedProfileIndex >= 0)
        {
            profiles[selectedProfileIndex] = newProfile;
        }
        // In Case of New Profile
        else
        {
            profiles.Add(newProfile);
        }

        uiController.EnableProfileForm(false);

        SaveToLocal();
        RefreshProfileList();
        ClearFields();
    }

    public void DeleteProfile()
    {
        if (selectedProfileIndex < 0) return;

        profiles.RemoveAt(selectedProfileIndex);
        SaveToLocal();
        RefreshProfileList();
        ClearFields();

        uiController.EnableProfileForm(false);
    }

    public void NewProfile()
    {
        uiController.EnableProfileForm(true);
    }

    private void EditProfile()
    {
        if (selectedProfileIndex < 0) return;

        ProfileData selectedProfile = profiles[selectedProfileIndex];
        uiController.profileFirstNameField.text = selectedProfile.firstName;
        uiController.profileLastNameField.text = selectedProfile.lastName;
        uiController.profileLocationField.text = selectedProfile.location;
        uiController.profileEmailField.text = selectedProfile.email;

        uiController.EnableProfileForm(true);
    }

    private void SaveToLocal()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath); // Removing old file
        }

        string json = JsonUtility.ToJson(new ProfileListWrapper(profiles), true);
        File.WriteAllText(savePath, json);
    }

    private void RefreshProfileList()
    {
        foreach (Transform child in uiController.profileListContentPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < profiles.Count; ++i)
        {
            // Creating new Item
            GameObject profileItem = Instantiate(uiController.profileListItemPrefab, uiController.profileListContentPanel);
            // Fetching its First and Last Name
            profileItem.GetComponentInChildren<TextMeshProUGUI>().text = profiles[i].firstName + " " + profiles[i].lastName;
            // Adding its Listener

            int index = i;
            profileItem.GetComponent<Button>().onClick.AddListener(() => SelectProfile(index));
            // Enabling the profile Item
            profileItem.gameObject.SetActive(true);
        }
    }

    private void SelectProfile(int _index)
    {
        selectedProfileIndex = _index;
        EditProfile();
    }

    private void ClearFields()
    {
        uiController.profileFirstNameField.text = "";
        uiController.profileLastNameField.text = "";
        uiController.profileLocationField.text = "";
        uiController.profileEmailField.text = "";
        selectedProfileIndex = -1;
    }

    // Getters
    private bool IsValidEmail(string _email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(_email);
            return addr.Address == _email;
        }
        catch
        {
            return false;
        }
    }
}

[System.Serializable]
public class ProfileListWrapper
{
    public List<ProfileData> profiles;
    public ProfileListWrapper(List<ProfileData> _profiles)
    {
        profiles = _profiles;
    }
}

[System.Serializable]
public class ProfileData
{
    public string firstName;
    public string lastName;
    public string location;
    public string email;

    public ProfileData(string _firstName, string _lastName, string _location, string _email)
    {
        this.firstName = _firstName;
        this.lastName = _lastName;
        this.location = _location;
        this.email = _email;
    }
}
