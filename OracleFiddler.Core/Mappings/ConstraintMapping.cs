using System.Collections.Generic;
using FluentNHibernate.Mapping;
using OracleFiddler.Core.Entities;
using System.Linq;
using OracleFiddler.Core.Infrastructure;

namespace OracleFiddler.Core.Mappings
{
    public class ConstraintMapping : ClassMap<Constraint>
    {
        private static readonly IDictionary<string, ConstraintType> types =
            new Dictionary<string, ConstraintType>
            {
                {"C", ConstraintType.CheckConstraintOnATable},
                {"P", ConstraintType.PrimaryKey},
                {"U", ConstraintType.UniqueKey},
                {"R", ConstraintType.ReferentialIntegrity},
                {"V", ConstraintType.WithCheckOptionOnAView},
                {"O", ConstraintType.WithReadOnlyOnAView}
            };

        public ConstraintMapping()
        {
            Table("ALL_CONSTRAINTS");
            Id(x => x.Name, "CONSTRAINT_NAME");
            var cases = types.Select(x => string.Format("WHEN '{0}' THEN {1}", x.Key, (int) x.Value)).JoinUsing(" ");
            Map(x => x.Type).Formula(string.Format("CASE constraint_type {0} END", cases)).CustomType<int>();
            References(x => x.ReferencedConstraint).Column("R_CONSTRAINT_NAME");
        }
    }
}