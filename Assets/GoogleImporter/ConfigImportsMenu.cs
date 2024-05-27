using GoogleSpreadsheets;
using UnityEditor;

namespace GoogleImporter {
    public class ConfigImportsMenu {
        private const string SPREADSHEET_ID = "1KVhpDv9oo4nV-WHAsPmrvEQB9kJcRKRO6WPoongrFR0";
        private const string SHEETS_NAME = "MainSettings";
        private const string CREDENTIALS_PATH = "textbelt-423208-c4bb244fe225.json";

        [MenuItem("Configs/Import Settings")]
        private static async void LoadSettings() {
            var sheetsImporter = new GoogleSheetsImporter(CREDENTIALS_PATH, SPREADSHEET_ID);
            var gameSettings = new GameSettings();
            var mainParser = new MainSettingsParser(gameSettings);
            await sheetsImporter.DownloadAndParseSheet(SHEETS_NAME, mainParser);
        }
    }
}