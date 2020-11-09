using System;

namespace Interactivity.Pagination
{
    /// <summary>
    /// Specifies which contents should be displayed in the footer of a <see cref="PaginatorBuilderOld"/>.
    /// </summary>
    [Flags]
    public enum PaginatorFooter
    {
        /// <summary>
        /// Display nothing in the footer.
        /// </summary>
        None = 1,

        /// <summary>
        /// Displays the current page number in the footer.
        /// </summary>
        PageNumber = 2,

        /// <summary>
        /// Displays the users who can interact with the <see cref="PaginatorOld"/>.
        /// </summary>
        Users = 4,
    }
}
