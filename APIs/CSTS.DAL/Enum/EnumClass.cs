using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS.DAL.Enum
{ 
    public enum UserType
        {
            ExternalClient,
            SupportTeamMember,
            SupportManager
        }
    public enum UserStatus
    {
        Active,
        Deactivated
    }
    public enum TicketStatus
    {
        New,
        Assigned,
        InProgress,
        Closed
    }

}
