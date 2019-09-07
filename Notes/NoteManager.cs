using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using WebmilioCommons.Managers;

namespace TLoZ.Notes
{
    public class NoteManager : SingletonManager<NoteManager, Note>
    {
        private Dictionary<Type, Note> _notesByType = new Dictionary<Type, Note>();


        public override void DefaultInitialize()
        {
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo typeInfo in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(Note)) && !t.IsAbstract))
                    Add(Activator.CreateInstance(typeInfo) as Note);
            }

            base.DefaultInitialize();
        }

        public override Note Add(Note item)
        {
            Note note = base.Add(item);

            if (note != item)
                return note;

            _notesByType.Add(item.GetType(), item);
            return item;
        }


        public Note Get<T>() where T : Note => _notesByType[typeof(T)];


        public Note this[Type type] => _notesByType[type];
    }
}