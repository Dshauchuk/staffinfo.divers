using AutoMapper;
using Staffinfo.Divers.Data.Poco;
using Staffinfo.Divers.Models;

namespace Staffinfo.Divers.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User mapping

            CreateMap<UserPoco, User>()
                .ForMember(b => b.UserId, p => p.MapFrom(src => src.UserId))
                .ForMember(b => b.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(b => b.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(b => b.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(b => b.Login, p => p.MapFrom(src => src.Login))
                .ForMember(b => b.NeedToChangePwd, p => p.MapFrom(src => src.NeedToChangePwd))
                .ForMember(b => b.Role, p => p.MapFrom(src => src.Role))
                .ForMember(b => b.RegistrationTimestamp, p => p.MapFrom(src => src.RegistrationTimestamp));

            CreateMap<User, UserPoco>()
                .ForMember(b => b.UserId, p => p.MapFrom(src => src.UserId))
                .ForMember(b => b.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(b => b.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(b => b.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(b => b.Login, p => p.MapFrom(src => src.Login))
                .ForMember(b => b.NeedToChangePwd, p => p.MapFrom(src => src.NeedToChangePwd))
                .ForMember(b => b.Role, p => p.MapFrom(src => src.Role))
                .ForMember(b => b.RegistrationTimestamp, p => p.MapFrom(src => src.RegistrationTimestamp));

            CreateMap<EditUserModel, UserPoco>()
                .ForMember(b => b.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(b => b.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(b => b.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(b => b.NeedToChangePwd, p => p.MapFrom(src => src.NeedToChangePwd))
                .ForMember(b => b.Role, p => p.MapFrom(src => src.Role));

            CreateMap<UserPoco, EditUserModel>()
                .ForMember(b => b.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(b => b.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(b => b.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(b => b.NeedToChangePwd, p => p.MapFrom(src => src.NeedToChangePwd))
                .ForMember(b => b.Role, p => p.MapFrom(src => src.Role));

            #endregion

            #region RescueStation mapping

            CreateMap<RescueStation, RescueStationPoco>()
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId))
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.CreatedAt, p => p.MapFrom(src => src.CreatedAt))
                .ForMember(r => r.UpdatedAt, p => p.MapFrom(src => src.UpdatedAt));

            CreateMap<RescueStationPoco, RescueStation>()
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId))
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.CreatedAt, p => p.MapFrom(src => src.CreatedAt))
                .ForMember(r => r.UpdatedAt, p => p.MapFrom(src => src.UpdatedAt));

            CreateMap<EditRescueStationModel, RescueStationPoco>()
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId));

            CreateMap<RescueStationPoco, EditRescueStationModel>()
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId));

            CreateMap<EditRescueStationModel, RescueStation>()
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId));

            CreateMap<RescueStation, EditRescueStationModel>()
                .ForMember(r => r.StationName, p => p.MapFrom(src => src.StationName))
                .ForMember(r => r.StationId, p => p.MapFrom(src => src.StationId));

            #endregion

            #region Diver mapping

            CreateMap<Diver, DiverPoco>()
                .ForMember(r => r.DiverId, p => p.MapFrom(src => src.DiverId))
                .ForMember(r => r.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(r => r.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(r => r.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(r => r.PhotoUrl, p => p.MapFrom(src => src.PhotoUrl))
                .ForMember(r => r.BirthDate, p => p.MapFrom(src => src.BirthDate))
                .ForMember(r => r.RescueStation, p => p.MapFrom(src => src.RescueStation))
                .ForMember(r => r.RescueStationId, p => p.MapFrom(src => src.RescueStation != null ? src.RescueStation.StationId : (int?)null))
                .ForMember(r => r.MedicalExaminationDate, p => p.MapFrom(src => src.MedicalExaminationDate))
                .ForMember(r => r.Address, p => p.MapFrom(src => src.Address))
                .ForMember(r => r.Qualification, p => p.MapFrom(src => src.Qualification))
                .ForMember(r => r.PersonalBookNumber, p => p.MapFrom(src => src.PersonalBookNumber))
                .ForMember(r => r.PersonalBookIssueDate, p => p.MapFrom(src => src.PersonalBookIssueDate))
                .ForMember(r => r.PersonalBookProtocolNumber, p => p.MapFrom(src => src.PersonalBookProtocolNumber))
                .ForMember(r => r.CreatedAt, p => p.MapFrom(src => src.CreatedAt))
                .ForMember(r => r.UpdatedAt, p => p.MapFrom(src => src.UpdatedAt))
                .ForMember(r => r.WorkingTime, p => p.MapFrom(src => src.WorkingTime));

            CreateMap<EditDiverModel, DiverPoco>()
                .ForMember(r => r.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(r => r.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(r => r.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(r => r.PhotoUrl, p => p.MapFrom(src => src.PhotoUrl))
                .ForMember(r => r.BirthDate, p => p.MapFrom(src => src.BirthDate))
                .ForMember(r => r.RescueStationId, p => p.MapFrom(src => src.RescueStationId))
                .ForMember(r => r.MedicalExaminationDate, p => p.MapFrom(src => src.MedicalExaminationDate))
                .ForMember(r => r.Address, p => p.MapFrom(src => src.Address))
                .ForMember(r => r.Qualification, p => p.MapFrom(src => src.Qualification))
                .ForMember(r => r.PersonalBookNumber, p => p.MapFrom(src => src.PersonalBookNumber))
                .ForMember(r => r.PersonalBookIssueDate, p => p.MapFrom(src => src.PersonalBookIssueDate))
                .ForMember(r => r.PersonalBookProtocolNumber, p => p.MapFrom(src => src.PersonalBookProtocolNumber))
                .ForMember(r => r.WorkingTime, p => p.MapFrom(src => src.WorkingTime));

            CreateMap<DiverPoco, EditDiverModel>()
                .ForMember(r => r.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(r => r.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(r => r.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(r => r.PhotoUrl, p => p.MapFrom(src => src.PhotoUrl))
                .ForMember(r => r.BirthDate, p => p.MapFrom(src => src.BirthDate))
                .ForMember(r => r.RescueStationId, p => p.MapFrom(src => src.RescueStationId))
                .ForMember(r => r.MedicalExaminationDate, p => p.MapFrom(src => src.MedicalExaminationDate))
                .ForMember(r => r.Address, p => p.MapFrom(src => src.Address))
                .ForMember(r => r.Qualification, p => p.MapFrom(src => src.Qualification))
                .ForMember(r => r.PersonalBookNumber, p => p.MapFrom(src => src.PersonalBookNumber))
                .ForMember(r => r.PersonalBookIssueDate, p => p.MapFrom(src => src.PersonalBookIssueDate))
                .ForMember(r => r.PersonalBookProtocolNumber, p => p.MapFrom(src => src.PersonalBookProtocolNumber))
                .ForMember(r => r.WorkingTime, p => p.MapFrom(src => src.WorkingTime));

            CreateMap<DiverPoco, Diver>()
                .ForMember(r => r.DiverId, p => p.MapFrom(src => src.DiverId))
                .ForMember(r => r.LastName, p => p.MapFrom(src => src.LastName))
                .ForMember(r => r.FirstName, p => p.MapFrom(src => src.FirstName))
                .ForMember(r => r.MiddleName, p => p.MapFrom(src => src.MiddleName))
                .ForMember(r => r.PhotoUrl, p => p.MapFrom(src => src.PhotoUrl))
                .ForMember(r => r.BirthDate, p => p.MapFrom(src => src.BirthDate))
                .ForMember(r => r.RescueStation, p => p.MapFrom(src => src.RescueStation))
                .ForMember(r => r.MedicalExaminationDate, p => p.MapFrom(src => src.MedicalExaminationDate))
                .ForMember(r => r.Address, p => p.MapFrom(src => src.Address))
                .ForMember(r => r.Qualification, p => p.MapFrom(src => src.Qualification))
                .ForMember(r => r.PersonalBookNumber, p => p.MapFrom(src => src.PersonalBookNumber))
                .ForMember(r => r.PersonalBookIssueDate, p => p.MapFrom(src => src.PersonalBookIssueDate))
                .ForMember(r => r.PersonalBookProtocolNumber, p => p.MapFrom(src => src.PersonalBookProtocolNumber))
                .ForMember(r => r.CreatedAt, p => p.MapFrom(src => src.CreatedAt))
                .ForMember(r => r.UpdatedAt, p => p.MapFrom(src => src.UpdatedAt))
                .ForMember(r => r.WorkingTime, p => p.MapFrom(src => src.WorkingTime));

            #endregion

            #region DivingTime mapping

            CreateMap<DivingTime, DivingTimePoco>()
                .ForMember(r => r.DiverId, p => p.MapFrom(src => src.DiverId))
                .ForMember(r => r.WorkingMinutes, p => p.MapFrom(src => src.WorkingMinutes))
                .ForMember(r => r.Year, p => p.MapFrom(src => src.Year));

            CreateMap<DivingTimePoco, DivingTime>()
                .ForMember(r => r.DiverId, p => p.MapFrom(src => src.DiverId))
                .ForMember(r => r.WorkingMinutes, p => p.MapFrom(src => src.WorkingMinutes))
                .ForMember(r => r.Year, p => p.MapFrom(src => src.Year));

            #endregion
        }
    }
}