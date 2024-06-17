using System;
using System.Collections.Generic;

namespace GoogleImporter { 
    public class MainSettingsParser : IGoogleSheetParser {
        private readonly GameSettings _gameSettings;
        private MainSettings _currentMainSettings;

        public MainSettingsParser(GameSettings gameSettings) {
            _gameSettings = gameSettings;
            _gameSettings.Main = new List<MainSettings>();
        }

        public void Parse(string header, string token) {
            switch (header) {
                case "NumberOfFruits":
                    _currentMainSettings = new MainSettings {
                        NumberOfFruits = Convert.ToInt32(token)
                    };
                    _gameSettings.Main.Add(_currentMainSettings);
                    break;
                case "SpeedOfStateChange":
                    _currentMainSettings.SpeedOfStateChange = Convert.ToInt32(token);
                    break;
                case "TextSpeed":
                    _currentMainSettings.TextSpeed = Convert.ToInt32(token);
                    break;
                case "TextSize":
                    _currentMainSettings.TextSize = Convert.ToInt32(token);
                    break;
                case "TextFont":
                    _currentMainSettings.TextFont = token;
                    break;
                case "WordSpacing":
                    _currentMainSettings.WordSpacing = Convert.ToInt32(token);
                    break;
                case "QuestionBlockHeight":
                    _currentMainSettings.QuestionBlockHeight = Convert.ToInt32(token);
                    break;
                case "QuestionBlockTextSize":
                    _currentMainSettings.QuestionBlockTextSize = Convert.ToInt32(token);
                    break;
                case "QuestionBlockTextAlignment":
                    _currentMainSettings.QuestionBlockTextAlignment = token;
                    break;
                case "AnswerBlockWidth":
                    _currentMainSettings.AnswerBlockWidth = Convert.ToInt32(token);
                    break;
                default:
                    throw new Exception($"Invalid header: {header}");
            }
        }
    }
}