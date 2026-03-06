using SnakeGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Engine
{
    /// <summary>
    /// Базовый абстрактный класс игровой логики.
    /// Реализует IArrowListener — получает события от ConsoleInput.
    /// Конкретные классы логики переопределяют реакции на стрелки.
    /// </summary>
    public abstract class BaseGameLogic : IArrowListener
    {
        // Реализации по умолчанию — конкретный класс переопределяет нужные
        public virtual void OnArrowUp() { }
        public virtual void OnArrowDown() { }
        public virtual void OnArrowLeft() { }
        public virtual void OnArrowRight() { }

        /// <summary>
        /// Подписывает этот экземпляр на события ConsoleInput.
        /// </summary>
        public void InitializeInput(ConsoleInput input)
        {
            input.Subscribe(this);
        }

        public abstract void Update(float deltaTime);
    }
}
