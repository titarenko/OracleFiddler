namespace OracleFiddler.Core.Entities
{
    public class Constraint
    {
        public virtual string Name { get; set; }

        public virtual ConstraintType Type { get; set; }

        public virtual Constraint ReferencedConstraint { get; set; }
    }
}