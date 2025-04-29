using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Books
{
    /// <summary>
    /// Reprezentuje złoczyńcę powiązanego z książką.
    /// </summary>
    public class Villain
    {
        /// <summary>
        /// Nazwa złoczyńcy.
        /// </summary>
        /// <remarks>
        /// Nazwa złoczyńcy, jak jest przedstawiona w książce.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// URL do strony internetowej lub zasobu powiązanego z tym złoczyńcą.
        /// </summary>
        /// <remarks>
        /// Może to być odnośnik do strony internetowej lub jakiegoś zasobu
        /// dotyczącego złoczyńcy.
        /// </remarks>
        public string Url { get; set; }
    }
}
