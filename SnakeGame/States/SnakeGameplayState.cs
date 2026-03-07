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
        public int Level { get; private set; } = 1;
        
        public int ApplesGoal => 3 + (Level - 1) * 2;
        
        public int ApplesCollected { get; private set; } = 0;
        
        private float MoveSpeed => 3f + (Level - 1) * 1.5f;
        
        public Cell? Apple { get; private set; } = null;
        
        private readonly Random _rng = new Random();
        
        public bool IsGameOver { get; private set; } = false;

        public bool IsLevelComplete { get; private set; } = false;

        public List<Cell> Body { get; } = new();
        private SnakeDir _currentDir;
        private float _timeToMove;

        private int _pendingGrowth = 0;

        //флаг для режима с координатами
        public bool ShowCoordinates { get; set; } = false;

        // Задаёт новое направление движения и разворот на 180° игнорируется
        public void SetDirection(SnakeDir dir)
        {
            if (!IsOpposite(_currentDir, dir))
                _currentDir = dir;
        }


        ////Возвращает клетку, смещённую на 1 шаг в направлении currentDir
        //private Cell ShiftTo(Cell origin)
        //{
        //    return _currentDir switch
        //    {
        //        SnakeDir.Up => new Cell(origin.X, origin.Y - 1),
        //        SnakeDir.Down => new Cell(origin.X, origin.Y + 1),
        //        SnakeDir.Left => new Cell(origin.X - 1, origin.Y),
        //        SnakeDir.Right => new Cell(origin.X + 1, origin.Y),
        //        _ => origin
        //    };
        //}

        //перемещение для игрового режима
        private Cell ShiftToGame(Cell origin)
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

        //перемещение для режима координат
        private Cell ShiftToCoords(Cell origin)
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

        public void ResetGame(bool resetLevel = true)
        {
            Body.Clear();
            _currentDir = SnakeDir.Right;
            _timeToMove = 0f;
            _pendingGrowth = 0;
            IsGameOver = false;
            IsLevelComplete = false;

            if (resetLevel)
            {
                Level = 1;
                ApplesCollected = 0;
            }

            if (ShowCoordinates)
                Body.Add(new Cell(0, 0));
            else
                Body.Add(new Cell(Console.WindowWidth / 2, Console.WindowHeight / 2));

            SpawnApple();
        }

        public override void Reset() => ResetGame(resetLevel: true);

        public void NextLevel()
        {
            Level++;
            ApplesCollected = 0;
            ResetGame(resetLevel: false);
        }

        private void SpawnApple()
        {
            int w = Console.WindowWidth;
            int h = Console.WindowHeight;

            Cell candidate;
            int attempts = 0;

            do
            {
                candidate = new Cell(_rng.Next(1, w - 1), _rng.Next(1, h - 2));
                attempts++;
            }
            while (IsOnSnake(candidate) && attempts < 100);

            Apple = candidate;
        }

        private bool IsOnSnake(Cell c)
        {
            foreach (var segment in Body)
            {
                if (segment.X == c.X && segment.Y == c.Y)
                    return true;
            }
            return false;
        }

        public override void Update(float deltaTime)
        {
            _timeToMove -= deltaTime;

            if (_timeToMove > 0f)
                return;

            _timeToMove = 1f / MoveSpeed;

            Cell head = Body[0];
            Cell nextCell = ShowCoordinates ? ShiftToCoords(head) : ShiftToGame(head);

            // Режим координат — отдельная логика без рендера и границ
            if (ShowCoordinates)
            {
                Body.RemoveAt(Body.Count - 1);
                Body.Insert(0, nextCell);
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"X: {Body[0].X,5} | Y: {Body[0].Y,5}");
                return;
            }

            if (IsOutOfBounds(nextCell))
            {
                IsGameOver = true;
                return;
            }

            bool ateApple = Apple.HasValue &&
                            nextCell.X == Apple.Value.X &&
                            nextCell.Y == Apple.Value.Y;

            if (ateApple)
            {
                ApplesCollected++;
                Apple = null;
                _pendingGrowth++;
            }

            Body.Insert(0, nextCell);

            if (_pendingGrowth > 0)
            {
                _pendingGrowth--;
            }
            else
            {
                Body.RemoveAt(Body.Count - 1);
            }

            if (ateApple)
            {
                if (ApplesCollected >= ApplesGoal)
                {
                    IsLevelComplete = true;
                }
                else
                {
                    SpawnApple();
                }
            }
        }

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
