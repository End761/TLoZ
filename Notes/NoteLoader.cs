using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Managers;

namespace TLoZ.Notes
{
    public static class NoteLoader
    {
        private static Dictionary<string, Type> _noteTypesByName = new Dictionary<string, Type>();


        internal static void Load()
        {
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo typeInfo in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(Note)) && !t.IsAbstract))
                    Add(Activator.CreateInstance(typeInfo) as Note);
            }
        }

        private static void Add(Note item)
        {
            _noteTypesByName.Add(item.UnlocalizedName, item.GetType());
        }


        public static Note NewNote<T>() where T : Note => Activator.CreateInstance<T>();
        public static Note NewNote(Type type) => Activator.CreateInstance(type) as Note;
        public static Note NewNote(string unlocalizedName) => Activator.CreateInstance(_noteTypesByName[unlocalizedName]) as Note;


        public static int Count => _noteTypesByName.Count;
    }
}