using System;

namespace SnakeGame.States
{
    public enum MenuMode
    {
        NotSelected,
        CoordinatesMode,
        GameMode
    }

    public class MainMenuState
    {
        public MenuMode SelectedMode { get; private set; } = MenuMode.NotSelected;

        public MenuMode Show()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("SNAKE GAME\n");
            Console.WriteLine("Выберите режим запуска:\n");
            Console.WriteLine("  1. Отображение координат (вывод X/Y при движении)");
            Console.WriteLine("  2. Игровой режим (символ '@' перемещается по консоли)");
            Console.WriteLine();
            Console.Write("Введите 1 или 2: ");

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.KeyChar == '1')
                {
                    SelectedMode = MenuMode.CoordinatesMode;
                    break;
                }
                if (key.KeyChar == '2')
                {
                    SelectedMode = MenuMode.GameMode;
                    break;
                }
            }

            Console.Clear();
            Console.CursorVisible = false;
            return SelectedMode;
        }
    }
}
