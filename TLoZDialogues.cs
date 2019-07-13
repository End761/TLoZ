namespace TLoZ
{
    public static class TLoZDialogues
    {
        public static string[] Clothier_MasterSwordReactions;
        public static void Load()
        {
            Clothier_MasterSwordReactions = new string[]
            {
                "Don't swing that thing around, little boy. You don't want to banish your elders into the void, do you?",
                "I'm not a fan of the idea of being sealed yet again...",
                "My child, do I really look [c/ff0000:THAT] evil?"
            };
        }
        public static void Unload()
        {
            Clothier_MasterSwordReactions = null;
        }
    }
}
