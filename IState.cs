namespace PurpleFox.AI
{
    public interface IState
    {
        void OnEnter();

        void OnExit();

        void Update();
    }
}
