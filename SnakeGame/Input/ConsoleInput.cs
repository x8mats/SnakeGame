using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Input
{
    // Интерфейс-слушатель нажатий стрелок.
    // Реализуется любым классом, который хочет реагировать на ввод.
    public interface IArrowListener
    {
        void OnArrowUp();
        void OnArrowDown();
        void OnArrowLeft();
        void OnArrowRight();
    }

    // Класс считывания ввода с консоли.
    // Хранит список подписчиков и оповещает их при нажатии стрелок / WASD.
    public class ConsoleInput
    {
        private readonly List<IArrowListener> _arrowListeners = new();

        // Добавляет слушателя в список подписчиков.
        public void Subscribe(IArrowListener listener)
        {
            _arrowListeners.Add(listener);
        }

        // Считывает все накопленные нажатия и оповещает подписчиков.
        // Неблокирующий — не останавливает игровой цикл.
        public void Update()
        {
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;

                foreach (var listener in _arrowListeners)
                {
                    switch (key)
                    {
                        case ConsoleKey.UpArrow or ConsoleKey.W: listener.OnArrowUp(); break;
                        case ConsoleKey.DownArrow or ConsoleKey.S: listener.OnArrowDown(); break;
                        case ConsoleKey.LeftArrow or ConsoleKey.A: listener.OnArrowLeft(); break;
                        case ConsoleKey.RightArrow or ConsoleKey.D: listener.OnArrowRight(); break;
                    }
                }
            }
        }
    }
}
