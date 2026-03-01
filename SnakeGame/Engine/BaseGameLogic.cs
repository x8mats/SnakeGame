using SnakeGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Engine
{
    // Реализует IArrowListener те получает события от ConsoleInput.
    // Конкретные классы логики переопределяют реакции на стрелки
    public abstract class BaseGameLogic : IArrowListener
    {
        // Реализации по умолчанию — конкретный класс переопределяет нужные
        public virtual void OnArrowUp() { }
        public virtual void OnArrowDown() { }
        public virtual void OnArrowLeft() { }
        public virtual void OnArrowRight() { }

        // Подписывает этот экземпляр на события ConsoleInput
        public void InitializeInput(ConsoleInput input)
        {
            input.Subscribe(this);
        }

        public abstract void Update(float deltaTime);
    }
}
