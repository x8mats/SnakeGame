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
    }
}
