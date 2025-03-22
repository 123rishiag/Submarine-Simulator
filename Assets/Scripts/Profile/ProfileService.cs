using ServiceLocator.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ServiceLocator.Profile
{
    public class ProfileService
    {
        // Private Variables
        private List<ProfileData> profiles;
        private int selectedProfileIndex;
        private string savePath;

        // Private Services
        private UIService uiService;

        public ProfileService()
        {
            // Setting Variables
            profiles = new List<ProfileData>();
            selectedProfileIndex = -1;
            savePath = Application.persistentDataPath + "/profiles.json";
            // C:\Users\{UserName}\AppData\LocalLow\{CompanyName}\{ProjectName}\profiles.json
        }
        public void Init(UIService _uiService)
        {
            // Setting Services
            uiService = _uiService;

            // Setting Elements
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
            string firstName = uiService.GetController().profileFirstNameField.text.Trim();
            string lastName = uiService.GetController().profileLastNameField.text.Trim();
            string location = uiService.GetController().profileLocationField.text.Trim();
            string email = uiService.GetController().profileEmailField.text.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(location) || !IsValidEmail(email))
            {
                uiService.GetController().ShowValidationMessage("Invalid profile details.");
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

            uiService.GetController().EnableProfileForm(false);

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

            uiService.GetController().EnableProfileForm(false);
        }

        public void NewProfile()
        {
            uiService.GetController().EnableProfileForm(true);
        }

        private void EditProfile()
        {
            if (selectedProfileIndex < 0) return;

            ProfileData selectedProfile = profiles[selectedProfileIndex];
            uiService.GetController().profileFirstNameField.text = selectedProfile.firstName;
            uiService.GetController().profileLastNameField.text = selectedProfile.lastName;
            uiService.GetController().profileLocationField.text = selectedProfile.location;
            uiService.GetController().profileEmailField.text = selectedProfile.email;

            uiService.GetController().EnableProfileForm(true);
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
            foreach (Transform child in uiService.GetController().profileListContentPanel)
            {
                Object.Destroy(child.gameObject);
            }

            for (int i = 0; i < profiles.Count; ++i)
            {
                // Creating new Item
                GameObject profileItem = Object.Instantiate(
                    uiService.GetController().profileListItemPrefab, uiService.GetController().profileListContentPanel);

                // Fetching its First and Last Name
                profileItem.GetComponentInChildren<TextMeshProUGUI>().text = profiles[i].firstName + " " + profiles[i].lastName;

                // Adding its Listener
                int index = i;
                profileItem.GetComponent<Button>().onClick.RemoveAllListeners();
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
            uiService.GetController().profileFirstNameField.text = "";
            uiService.GetController().profileLastNameField.text = "";
            uiService.GetController().profileLocationField.text = "";
            uiService.GetController().profileEmailField.text = "";
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
            firstName = _firstName;
            lastName = _lastName;
            location = _location;
            email = _email;
        }
    }
}