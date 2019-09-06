namespace TLoZ.Time.States
{
    public abstract class State
    {
        protected State(object tracked)
        {
            Tracked = tracked;
        }


        public abstract void Restore();


        public object Tracked { get; }
    }
}