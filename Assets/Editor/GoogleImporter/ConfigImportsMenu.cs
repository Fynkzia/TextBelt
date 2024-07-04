using GoogleSpreadsheets;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GoogleImporter {
    public class ConfigImportsMenu {
        private const string SPREADSHEET_ID = "1KVhpDv9oo4nV-WHAsPmrvEQB9kJcRKRO6WPoongrFR0";
        private const string SHEETS_NAME = "MainSettings";
        private const string CREDENTIALS_PATH = "textbelt-423208-c4bb244fe225.json";
        private const string KEY = "MainSettings";
        private const string FILE_DATA_PATH = "/Resources/MainData.json";

        [MenuItem("Configs/Import Settings")]
        private static async void LoadMainSettings() {
            var sheetsImporter = new GoogleSheetsImporter(CREDENTIALS_PATH, SPREADSHEET_ID);
            var gameSettings = LoadSettings();
            var mainParser = new MainSettingsParser(gameSettings);
            await sheetsImporter.DownloadAndParseSheet(SHEETS_NAME, mainParser);

            var jsonForSaving = JsonUtility.ToJson(gameSettings);
            string fileDataPath = Application.dataPath + FILE_DATA_PATH;
            File.WriteAllText(fileDataPath, jsonForSaving);
        }

        public static GameSettings LoadSettings() {
            string fileDataPath = Application.dataPath + FILE_DATA_PATH;
            if (File.Exists(fileDataPath)) {
                var jsonLoaded = Resources.Load<TextAsset>(fileDataPath);
                var gameSettings = !string.IsNullOrEmpty(jsonLoaded.text)
                   ? JsonUtility.FromJson<GameSettings>(jsonLoaded.text)
                   : new GameSettings();
                return gameSettings;
            }

            return new GameSettings();
        }
    }
}