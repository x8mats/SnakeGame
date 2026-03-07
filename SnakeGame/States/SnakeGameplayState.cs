using SnakeGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.States
{
    // Направления движения змейки.
    public enum SnakeDir
    {
        Up,
        Down,
        Left,
        Right
    }
    
    // Хранит X/Y координаты одной клетки тела змейки
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

    
    // Состояние игровой сессии змейки, хранит тело, направление, обновляет позицию головы с заданной скоростью
    public class SnakeGameplayState : BaseGameState
    {
        // Константа количество шагов в секунду
        private const float MoveSpeed = 5f;
        public List<Cell> Body { get; } = new();
        private SnakeDir _currentDir;
        private float _timeToMove;

        //флаг для режима с координатами
        public bool ShowCoordinates { get; set; } = false;

        // Задаёт новое направление движения и разворот на 180° игнорируется
        public void SetDirection(SnakeDir dir)
        {
            if (!IsOpposite(_currentDir, dir))
                _currentDir = dir;
        }

        
        // Возвращает клетку, смещённую на 1 шаг в направлении currentDir
        private Cell ShiftTo(Cell origin)
        {
            return _currentDir switch
            {
                SnakeDir.Up => new Cell(origin.X, origin.Y - 1),
                SnakeDir.Down => new Cell(origin.X, origin.Y + 1),
                SnakeDir.Left => new Cell(origin.X - 1, origin.Y),
                SnakeDir.Right => new Cell(origin.X + 1, origin.Y),
                _ => origin
            };
        }


        // Сбрасывает состояние: очищает тело, ставит голову в (0,0), направление — вправо
        public override void Reset()
        {
            Body.Clear();
            _currentDir = SnakeDir.Right;
            // Старт примерно в центре консоли, чтобы было место для движения в любую сторону, потому что  если в начале игры уйти в потолок то игр а крашитсся сразу
            Body.Add(new Cell(Console.WindowWidth / 2, Console.WindowHeight / 2));
            _timeToMove = 0f;
        }


        // Обновляет позицию головы раз в (1 / MoveSpeed) секунд
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

            //Console.WriteLine($"X: {Body[0].X,5} | Y: {Body[0].Y,5}"); //вывод координат неактуален


            //Вывод координат если выбран такой решим
            if (ShowCoordinates)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"X: {Body[0].X,5} | Y: {Body[0].Y,5}");
            }

            // Если следующая клетка за границей — перезапускаем игру
            if (IsOutOfBounds(nextCell))
            {
                Reset();
                return;
            }
        }

        // Возвращает true если клетка вышла за пределы окна консоли
        private static bool IsOutOfBounds(Cell cell) =>
            cell.X < 0 ||
            cell.Y < 0 ||
            cell.X >= Console.WindowWidth ||
            cell.Y >= Console.WindowHeight;

        private static bool IsOpposite(SnakeDir a, SnakeDir b) =>
            (a == SnakeDir.Up && b == SnakeDir.Down) ||
            (a == SnakeDir.Down && b == SnakeDir.Up) ||
            (a == SnakeDir.Left && b == SnakeDir.Right) ||
            (a == SnakeDir.Right && b == SnakeDir.Left);
    }
}
