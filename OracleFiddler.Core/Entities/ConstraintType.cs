namespace OracleFiddler.Core.Entities
{
    public enum ConstraintType
    {
        /// <summary>
        /// C (check constraint on a table).
        /// </summary>
        CheckConstraintOnATable,

        /// <summary>
        /// P (primary key).
        /// </summary>
        PrimaryKey,

        /// <summary>
        /// U (unique key).
        /// </summary>
        UniqueKey,

        /// <summary>
        /// R (referential integrity).
        /// </summary>
        ReferentialIntegrity,        

        /// <summary>
        /// V (with check option, on a view).
        /// </summary>
        WithCheckOptionOnAView,

        /// <summary>
        /// O (with read only, on a view).
        /// </summary>
        WithReadOnlyOnAView
    }
}