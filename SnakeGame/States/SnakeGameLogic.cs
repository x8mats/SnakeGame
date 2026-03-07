// SnakeGame/States/SnakeGameLogic.cs
using SnakeGame.Engine;
using SnakeGame.Input;
using SnakeGame.Rendering;
using System;

namespace SnakeGame.States
{
    public class SnakeGameLogic : BaseGameLogic
    {
        private readonly SnakeGameplayState _gameplayState = new();

        private readonly ShowTextState _showTextState = new ShowTextState(3f);

        private readonly ConsoleRenderer _renderer = new(new[]
        {
            ConsoleColor.Black,   // 0 — фон
            ConsoleColor.Green,   // 1 — голова
            ConsoleColor.DarkGreen, // 2 — тело
            ConsoleColor.Red,     // 3 — яблоко
            ConsoleColor.White    // 4 — текст
        });

        private enum Screen { Gameplay, ShowText }
        private Screen _currentScreen = Screen.Gameplay;

        private enum AfterText { RestartGame, NextLevel }
        private AfterText _afterText;

        private bool _coordMode = false;

        public override void OnArrowUp() => _gameplayState.SetDirection(SnakeDir.Up);
        public override void OnArrowDown() => _gameplayState.SetDirection(SnakeDir.Down);
        public override void OnArrowLeft() => _gameplayState.SetDirection(SnakeDir.Left);
        public override void OnArrowRight() => _gameplayState.SetDirection(SnakeDir.Right);

        public void GotoGameplay(bool showCoordinates = false)
        {
            _coordMode = showCoordinates;
            _gameplayState.ShowCoordinates = showCoordinates;
            _gameplayState.Reset();
            _currentScreen = Screen.Gameplay;
        }

        public override void Update(float deltaTime)
        {
            if (_currentScreen == Screen.ShowText)
            {
                _showTextState.Update(deltaTime);

                _renderer.Clear();
                _showTextState.Draw(_renderer);
                _renderer.Render();

                if (_showTextState.IsDone())
                {
                    if (_afterText == AfterText.NextLevel)
                    {
                        _gameplayState.NextLevel();
                    }
                    else
                    {
                        _gameplayState.Reset();
                    }
                    _currentScreen = Screen.Gameplay;
                }

                return;
            }

            if (_coordMode)
            {
                _gameplayState.Update(deltaTime);
                return;
            }

            _gameplayState.Update(deltaTime);


            if (_gameplayState.IsGameOver)
            {
                _showTextState.text = "  GAME OVER! Нажмите пробел...  ";
                _showTextState.Reset();
                _afterText = AfterText.RestartGame;
                _currentScreen = Screen.ShowText;
                return;
            }

            if (_gameplayState.IsLevelComplete)
            {
                int nextLvl = _gameplayState.Level + 1;
                _showTextState.text = $"   Уровень {nextLvl}!   ";
                _showTextState.Reset();
                _afterText = AfterText.NextLevel;
                _currentScreen = Screen.ShowText;
                return;
            }

            _renderer.Clear();

            //отрисовка яблока, 3 = красный
            if (_gameplayState.Apple.HasValue)
            {
                var apple = _gameplayState.Apple.Value;
                _renderer.SetPixel(apple.X, apple.Y, '*', 3);
            }

            //тело змейки, 2 = тёмно-зелёный, начиная с индекса 1
            for (int i = 1; i < _gameplayState.Body.Count; i++)
            {
                var seg = _gameplayState.Body[i];
                _renderer.SetPixel(seg.X, seg.Y, 'a', 2);
            }

            // голова поверх тела, 1 = зелёный)
            var head = _gameplayState.Body[0];
            _renderer.SetPixel(head.X, head.Y, '@', 1);

            string status = $" Уровень: {_gameplayState.Level}  " +
                            $"Яблоки: {_gameplayState.ApplesCollected}/{_gameplayState.ApplesGoal} ";
            _renderer.DrawString(status, 0, _renderer.height - 1, ConsoleColor.White);

            _renderer.Render();
        }
    }
}
