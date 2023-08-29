using BasicStore.Core.DomainObjects;

namespace BasicStore.Catalog.Domain
{
    public class Dimensions
    {
        public decimal Height { get; private set; }
        public decimal Width { get; private set; }
        public decimal Deep { get; private set; }

        public Dimensions(decimal height, decimal width, decimal deep)
        {
            AssertionConcern.ValidateIfLessThan(height, 1, "Height less or equal to 0");
            AssertionConcern.ValidateIfLessThan(width, 1, "Width less or equal to 0");
            AssertionConcern.ValidateIfLessThan(deep, 1, "Deep less or equal to 0");

            Height = height;
            Width = width;
            Deep = deep;
        }

        public string FormattedDescription()
        {
            return $"LxAxP: {Width} x {Height} x {Deep}";
        }

        public override string ToString()
        {
            return FormattedDescription();
        }
    }
}
