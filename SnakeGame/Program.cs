using SnakeGame.Input;
using SnakeGame.States;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false; //скрыть курср
        Console.WriteLine("Управление: стрелкии или WASD\n");

        var gameLogic = new SnakeGameLogic();
        var input = new ConsoleInput();

        gameLogic.InitializeInput(input);
        gameLogic.GotoGameplay();

        var lastFrameTime = DateTime.UtcNow;

        while (true)
        {
            // Считываем ввод
            input.Update();

            var frameStartTime = DateTime.UtcNow;
            float deltaTime = (float)(frameStartTime - lastFrameTime).TotalSeconds;

            gameLogic.Update(deltaTime);//обнвлене=ие логики

            lastFrameTime = frameStartTime;
        }
    }
}