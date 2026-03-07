using SnakeGame.Input;
using SnakeGame.States;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false; //скрыть курср

        //показ меню и выбор
        var menu = new MainMenuState();
        var mode = menu.Show();

        Console.WriteLine("Управление: стрелкии или WASD\n");

        var gameLogic = new SnakeGameLogic();
        var input = new ConsoleInput();

        gameLogic.InitializeInput(input);
        //gameLogic.GotoGameplay();

        //передача режима 
        bool coordMode = (mode == MenuMode.CoordinatesMode);
        gameLogic.GotoGameplay(showCoordinates: coordMode);

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