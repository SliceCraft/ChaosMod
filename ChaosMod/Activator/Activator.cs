namespace ChaosMod.Activator
{
    interface Activator
    {
        string getName();
        void Start();
        Effect ChooseEffect();
        void Stop();
    }
}
