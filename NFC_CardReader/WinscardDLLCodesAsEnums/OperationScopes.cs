using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC_CardReader
{
    /// <summary>
    /// The scope of our operations for our context
    /// </summary>
    public enum OperationScopes
    {
        /// <summary>
        ///  The context is a user context, and any database operations 
        ///  are performed within the domain of the user.
        /// </summary>
        SCARD_SCOPE_USER = 0,
        /// <summary>
        /// The context is that of the current terminal, and any database 
        /// operations are performed within the domain of that terminal.  
        /// (The calling application must have appropriate access permissions 
        /// for any database actions.)
        /// </summary>
        SCARD_SCOPE_TERMINAL = 1,
        /// <summary>
        /// The context is the system context, and any database operations 
        /// are performed within the domain of the system.  (The calling
        /// application must have appropriate access permissions for any 
        /// database actions.)
        /// </summary>
        SCARD_SCOPE_SYSTEM = 2
    }
}
