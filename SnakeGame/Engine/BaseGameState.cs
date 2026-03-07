using SnakeGame.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Engine
{
    public abstract class BaseGameState
    {
        //Обновление логики состояния
        public abstract void Update(float deltaTime);

        //Сброс состояния к начальному
        public abstract void Reset();

        // Опциональный рендер, по умолчанию ничего не рисует
        public virtual void Draw(ConsoleRenderer renderer) { }

        // Состояние завершено?
        public virtual bool IsDone() => false;
    }
}
