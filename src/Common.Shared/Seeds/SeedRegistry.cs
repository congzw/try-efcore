using System.Collections.Generic;
using System.Linq;

namespace Common.Shared.Seeds
{
    public class SeedRegistry
    {
        public List<Seed> Seeds { get; set; } = new List<Seed>();

        public Seed GetSeed(string category, string typeName, bool autoCreate)
        {
            var theOne = Seeds.SingleOrDefault(x => x.Category.MyEquals(category) && x.TypeName.MyEquals(typeName));
            if (theOne == null && autoCreate)
            {
                theOne = Seed.Create(category, typeName);
                Seeds.Add(theOne);
            }
            return theOne;
        }

        private bool _isSetupApplied;
        public bool IsSetupApplied()
        {
            return _isSetupApplied;
        }
        public void ApplySetup(IEnumerable<ISeedProvider> providers)
        {
            if (_isSetupApplied)
            {
                return;
            }

            if (providers == null)
            {
                return;
            }

            foreach (var provider in providers)
            {
                provider.Setup(this);
            }
            _isSetupApplied = true;
        }
    }
}