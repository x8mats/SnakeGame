using SnakeGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.States
{
    /// <summary>
    /// Направления движения змейки.
    /// </summary>
    public enum SnakeDir
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Хранит X/Y координаты одной клетки тела змейки.
    /// </summary>
    public struct Cell
    {
        public int X;
        public int Y;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Состояние игровой сессии змейки:
    /// хранит тело, направление, обновляет позицию головы с заданной скоростью.
    /// </summary>
    public class SnakeGameplayState : BaseGameState
    {
        // Константа: количество шагов в секунду
        private const float MoveSpeed = 5f;

        public List<Cell> Body { get; } = new();
        private SnakeDir _currentDir;
        private float _timeToMove;

        /// <summary>
        /// Задаёт новое направление движения.
        /// Разворот на 180° игнорируется.
        /// </summary>
        public void SetDirection(SnakeDir dir)
        {
            if (!IsOpposite(_currentDir, dir))
                _currentDir = dir;
        }

        /// <summary>
        /// Возвращает клетку, смещённую на 1 шаг в направлении currentDir.
        /// </summary>
        private Cell ShiftTo(Cell origin)
        {
            return _currentDir switch
            {
                SnakeDir.Up => new Cell(origin.X, origin.Y + 1),
                SnakeDir.Down => new Cell(origin.X, origin.Y - 1),
                SnakeDir.Left => new Cell(origin.X - 1, origin.Y),
                SnakeDir.Right => new Cell(origin.X + 1, origin.Y),
                _ => origin
            };
        }

        /// <summary>
        /// Сбрасывает состояние: очищает тело, ставит голову в (0,0), направление — вправо.
        /// </summary>
        public override void Reset()
        {
            Body.Clear();
            _currentDir = SnakeDir.Right;
            Body.Add(new Cell(0, 0));
            _timeToMove = 0f;
        }

        /// <summary>
        /// Обновляет позицию головы раз в (1 / MoveSpeed) секунд.
        /// </summary>
        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;

            if (_timeToMove > 0f)
                return;

            _timeToMove = 1f / MoveSpeed;

            Cell head = Body[0];
            Cell nextCell = ShiftTo(head);

            // Убираем хвост, вставляем новую голову
            Body.RemoveAt(Body.Count - 1);
            Body.Insert(0, nextCell);

            Console.WriteLine($"X: {Body[0].X,5} | Y: {Body[0].Y,5}");
        }

        private static bool IsOpposite(SnakeDir a, SnakeDir b) =>
            (a == SnakeDir.Up && b == SnakeDir.Down) ||
            (a == SnakeDir.Down && b == SnakeDir.Up) ||
            (a == SnakeDir.Left && b == SnakeDir.Right) ||
            (a == SnakeDir.Right && b == SnakeDir.Left);
    }
}
