using SnakeGame.Engine;
using SnakeGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.States
{
    /// <summary>
    /// Игровая логика змейки: связывает ввод со смещением змейки.
    /// </summary>
    public class SnakeGameLogic : BaseGameLogic
    {
        private readonly SnakeGameplayState _gameplayState = new();

        public override void OnArrowUp() => _gameplayState.SetDirection(SnakeDir.Up);
        public override void OnArrowDown() => _gameplayState.SetDirection(SnakeDir.Down);
        public override void OnArrowLeft() => _gameplayState.SetDirection(SnakeDir.Left);
        public override void OnArrowRight() => _gameplayState.SetDirection(SnakeDir.Right);

        /// <summary>Инициализирует состояние и запускает движение змейки.</summary>
        public void GotoGameplay() => _gameplayState.Reset();

        public override void Update(float deltaTime) => _gameplayState.Update(deltaTime);
    }
}
