using AutoMapper;
using CSTS.DAL.AutoMapper.DTOs;
using CSTS.DAL.Models;// افترض أن الكائنات الفعلية موجودة في هذا النطاق

public class Mapping : Profile
{
    public Mapping()
    {
        // تحويل بين الكائنات الفعلية و DTOs الخاصة بالتعليقات
        CreateMap<Comment, CommentResponseDTO>();
        CreateMap<CreateCommentDTO, Comment>();

        // تحويل بين الكائنات الفعلية و DTOs الخاصة بالتذاكر
        CreateMap<Ticket, TicketResponseDTO>();
        CreateMap<CreateTicketDTO, Ticket>();
        CreateMap<Ticket, TicketSummaryDTO>();
        CreateMap<UpdateTicketDTO, Ticket>();

        // تحويل بين الكائنات الفعلية و DTOs الخاصة بالمستخدمين
        CreateMap<User, UserResponseDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<User, UserSummaryDTO>();
        CreateMap<UpdateUserDTO, User>();

        // تحويل بين الكائنات الفعلية و DTOs الخاصة بعضو فريق الدعم
        CreateMap<User, SupportTeamMemberResponseDTO>();
        CreateMap<CreateSupportTeamMemberDTO, User>();

        // تحويل بين الكائنات الفعلية و DTOs الخاصة بالعملاء
        CreateMap<User, ClientResponseDTO>();
        CreateMap<CreateClientDTO, User>();
    }
}
