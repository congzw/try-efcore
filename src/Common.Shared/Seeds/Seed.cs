using System.Collections.Generic;

namespace Common.Shared.Seeds
{
    public class Seed
    {
        public string TypeName { get; set; }
        public string Category { get; set; }

        public List<object> Items { get; set; } = new List<object>();
        public void AddItem<T>(T item)
        {
            Items.Add(item);
        }
        public List<T> AsItems<T>()
        {
            var entities = new List<T>();
            if (Items.IsNullOrEmpty())
            {
                return entities;
            }

            foreach (var item in Items)
            {
                if (item != null)
                {
                    //convert: JObject to T
                    var entity = item.ConvertTo<T>();
                    entities.Add(entity);
                }
            }

            return entities;
        }
        
        public static Seed Create(string category, string typeName)
        {
            var seed = new Seed();
            seed.Category = category;
            seed.TypeName = typeName;
            return seed;
        }
    }
}
