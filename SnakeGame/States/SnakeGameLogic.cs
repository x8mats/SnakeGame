using SnakeGame.Engine;
using SnakeGame.Input;
using SnakeGame.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.States
{
    // Игровая логика змейки, связывает ввод со смещением змейки
    public class SnakeGameLogic : BaseGameLogic
    {
        private readonly SnakeGameplayState _gameplayState = new();

        // Создание рендера и передача массива из двух цветов — фон и цвет змейки
        private readonly ConsoleRenderer _renderer = new(new[]
        {
            ConsoleColor.Black,  // 0 — фоновый цвет (пустые клетки)
            ConsoleColor.Green   // 1 — цвет головы змейки
        });

        public override void OnArrowUp() => _gameplayState.SetDirection(SnakeDir.Up);
        public override void OnArrowDown() => _gameplayState.SetDirection(SnakeDir.Down);
        public override void OnArrowLeft() => _gameplayState.SetDirection(SnakeDir.Left);
        public override void OnArrowRight() => _gameplayState.SetDirection(SnakeDir.Right);

        //Инициализирует состояние и запускает движение змейки и задает цвет фона
        public void GotoGameplay()
        {
            _renderer.bgColor = ConsoleColor.Black;
            _gameplayState.Reset();
        }

        public override void Update(float deltaTime)
        {
            _gameplayState.Update(deltaTime);

            // Очищаем буфер от прошлого кадра
            _renderer.Clear();

            // Рисуем голову змейки: берём первый элемент из тела
            var head = _gameplayState.Body[0];

            // SetPixel(x, y, символ, индекс_цвета)
            // Символ '@', индекс 1 зелёный цвет
            _renderer.SetPixel(head.X, head.Y, '@', 1);

            // Отрисовываем всё на экран
            _renderer.Render();
        }
    }
}
