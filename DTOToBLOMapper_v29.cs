using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using EVerify.BLO;
using EVerify.Proxy;
using AutoMapper;
using Utility.CustomTextMessageEncoder;
using WebApplication.Entity;
using WebApplication.Error;
using WebApplication.Utility.StringExtensions;


namespace EVerify.Proxy_v29
{
    #region Request

    public partial class SubmitDhsReverifyRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitDhsReverifyRequestMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpDHSReVerifyRequestBodyBLO>(this);

            blo.CaseNbr = this.CaseNumber;
            blo.CardNbr = this.CardNumber;
            blo.NoForeignPassport = this.NoForeignPassportIndicator.ToString();
            blo.CountryOfIssuance = this.CountryOfIssuanceCode;
            blo.PassPortNumber = this.PassportNumber;

            blo.DTO = this;
            return blo;
        }
    }

    public class SubmitDhsReverifyRequestMapProfile : Profile
    {
        public SubmitDhsReverifyRequestMapProfile()
        {
            CreateMap<SubmitDhsReverifyRequest, EmpDHSReVerifyRequestBodyBLO>();
        }
    }

    public partial class SubmitSsaResubmittalRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitSsaResubmittalRequestMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpSSAResubmittalRequestBodyBLO>(this);

            blo.ClientSftwrVer = this.ClientSoftwareVersion;
            blo.CaseNbr = this.CaseNumber;
            blo.SocialSecurityNbr = this.Ssn;
            blo.BirthDate = this.BirthDate.ToShortDate();

            blo.DTO = this;
            return blo;
        }
    }

    public class SubmitSsaResubmittalRequestMapProfile : Profile
    {
        public SubmitSsaResubmittalRequestMapProfile()
        {
            CreateMap<SubmitSsaResubmittalRequest, EmpSSAResubmittalRequestBodyBLO>();
        }
    }

    public partial class ConfirmDocumentPhotoRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ConfirmDocumentPhotoRequestRequestMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<ConfirmPhotoRequestBLO>(this);
            blo.CaseNbr = this.CaseNumber;
            blo.PhotoConfirmation = this.PhotoConfirmedIndicator ? PhotoConfirmation.Y.ToString() : PhotoConfirmation.N.ToString();

            blo.DTO = this;
            return blo;
        }
    }

    public class ConfirmDocumentPhotoRequestRequestMapProfile : Profile
    {
        public ConfirmDocumentPhotoRequestRequestMapProfile()
        {
            CreateMap<ConfirmDocumentPhotoRequest, ConfirmPhotoRequestBLO>();
        }
    }
    public partial class CloseCaseRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CloseCaseRequestMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpCloseCaseRequestBodyBLO>(this);

            blo.CaseNbr = this.CaseNumber;
            blo.ClientSftwrVer = this.ClientSoftwareVersion;
            blo.CloseStatus = this.ClosureReasonCode;
            blo.CurrentlyEmployed = this.CurrentlyEmployed.ToString();

            blo.DTO = this;
            return blo;
        }
    }

    public class CloseCaseRequestMapProfile : Profile
    {
        public CloseCaseRequestMapProfile()
        {
            CreateMap<CloseCaseRequest, EmpCloseCaseRequestBodyBLO>();
        }
    }

    public partial class RetrieveLetterRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RetrieveLetterRequestMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var ret = mapper.Map<EmpRetrieveFANRequestBodyBLO>(this);
            ret.CaseNbr = this.CaseNumber;
            ret.Language = this.Language.ToIntString();
            ret.LetterTypeCode = this.LetterTypeCode.ToString();

            ret.DTO = this;
            return ret;
        }
    }

    public class RetrieveLetterRequestMapProfile : Profile
    {
        public RetrieveLetterRequestMapProfile()
        {
            CreateMap<RetrieveLetterRequest, EmpRetrieveFANRequestBodyBLO>();
        }
    }

    public partial class SubmitInitialVerificationRequest : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
                       {
                           cfg.AddProfile<SubmitInitialVerificationRequestMapProfile>();
                       });

            IMapper mapper = new Mapper(config);
            var ret = mapper.Map<EmpInitDABPVerifRequestBodyBLO>(this);
            if (this.ClientCompanyId == null)
            {
                throw new Exception("ClientCompanyId is empty!");
            }

            ret.ClientCompanyId = this.ClientCompanyId ?? 0;
            ret.ClientSftwrVer = this.ClientSoftwareVersion;
            ret.LastName = this.LastName;
            ret.FirstName = this.FirstName;
            ret.MiddleInitial = this.MiddleInitial;
            ret.OtherNamesUsed = this.OtherNamesUsed;
            ret.SSNNumber = this.Ssn;
            ret.EmailAddress = this.EmailAddress;
            ret.BirthDate = this.BirthDate.ToDate();
            ret.HireDate = this.HireDate.ToDate();
            ret.OverDueVerifyReason = this.LateVerificationReasonCodeField;
            ret.OverDueVerifyReasonOther = this.LateVerificationOtherReasonTextField;
            ret.CitizenshipStatus = this.CitizenshipStatusCodeField.ToInt();
            ret.AlienNumber = this.AlienNumberField;
            ret.I94Number = this.I94NumberField;
            ret.CardNbr = this.CardNumberField;
            ret.PassPortNumber = this.PassportNumberField;
            ret.NoForeignPassport = this.NoForeignPassportIndicatorField.ToString();
            ret.CountryOfIssuance = this.CountryOfIssuanceCodeField;
            ret.VisaNumber = this.VisaNumberField;
            ret.DocumentID = this.DocumentTypeIdField;
            ret.ListBDocumentId = this.ListBDocumentTypeIdField ?? 0;
            ret.ListCDocumentId = this.ListCDocumentTypeIdField ?? 0;
            ret.SupportingDocumentId = this.SupportingDocumentTypeIdField ?? 0;
            ret.StateIssuingAuthority = this.IssuingAuthorityCodeField;
            ret.DMVDocumentNbr = this.ListBCDocumentNumberField;
            ret.DMVDocNoExpirationDate = this.NoDocumentExpirationDateIndicatorField.ToString();
            ret.DocExpDate = this.DocumentExpirationDateField.ToDate();
            ret.SubmittingOfficial = this.SubmittingOfficialNameField;
            ret.SubmittersPhoneNbr = this.SubmittingOfficialPhoneNumberField;

            return ret;
        }
    }

    public class SubmitInitialVerificationRequestMapProfile : Profile
    {
        public SubmitInitialVerificationRequestMapProfile()
        {
            CreateMap<SubmitInitialVerificationRequest, EmpInitDABPVerifRequestBodyBLO>();
        }
    }

    #endregion

    #region Responce Level 2
    #endregion

    #region Responce Level 1

    public partial class SubmitInitialVerificationResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitInitialVerificationResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);
            var blo = mapper.Map<EmpInitVerfRespBLO>(this);

            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;
            blo.EligStatementDetailsTxt = this.EligibilityStatementDetails;


            blo.DTO = this;


            blo.CaseNbr = this.CaseNumber;
            blo.FirstName = this.SystemFirstName;
            blo.LastName = this.SystemLastName;

            return blo;
        }
    }

    public class SubmitInitialVerificationResultMapProfile : Profile
    {
        public SubmitInitialVerificationResultMapProfile()
        {
            CreateMap<SubmitInitialVerificationResult, EmpInitVerfRespBLO>();
        }
    }

    public partial class SubmitSsaReverifyResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitSsaReverifyResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);
            var blo = mapper.Map<EmpInitVerfRespBLO>(this);


            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;
            blo.EligStatementDetailsTxt = this.EligibilityStatementDetails;


            blo.DTO = this;


            blo.FirstName = this.SystemFirstName;
            blo.LastName = this.SystemLastName;

            return blo;
        }
    }

    public class SubmitSsaReverifyResultMapProfile : Profile
    {
        public SubmitSsaReverifyResultMapProfile()
        {
            CreateMap<SubmitSsaReverifyResult, EmpInitVerfRespBLO>();
        }
    }

    

    public partial class SubmitSsaReferralResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitSsaReferralResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpSubmitSSAReferralRespBLO>(this);

            blo.DTO = this;
            return blo;
        }
    }

    public class SubmitSsaReferralResultMapProfile : Profile
    {
        public SubmitSsaReferralResultMapProfile()
        {
            CreateMap<SubmitSsaReferralResult, EmpSubmitSSAReferralRespBLO>();
        }
    }


    public partial class SubmitSsaResubmittalResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitSsaResubmittalResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

         
            var blo = mapper.Map<EmpSSAResubmittalRespBLO>(this);
            
            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;

            blo.DTO = this;

            blo.FirstName = this.SystemFirstName;
            blo.LastName = this.SystemLastName;

            return blo;
        }
    }

    public class SubmitSsaResubmittalResultMapProfile : Profile
    {
        public SubmitSsaResubmittalResultMapProfile()
        {
            CreateMap<SubmitSsaResubmittalResult, EmpSSAResubmittalRespBLO>();
        }
    }



    public partial class SubmitDhsReverifyResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
             var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitDhsReverifyResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            
            var blo = mapper.Map<EmpInitVerfRespBLO>(this);


            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;


            blo.DTO = this;


            blo.FirstName = this.SystemFirstName;
            blo.LastName = this.SystemLastName;

            return blo;
        }
    }

    public class SubmitDhsReverifyResultMapProfile : Profile
    {
        public SubmitDhsReverifyResultMapProfile()
        {
            CreateMap<SubmitDhsReverifyResult, EmpInitVerfRespBLO>();
        }
    }

    public partial class RetrieveDocumentPhotoResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RetrieveDocumentPhotoResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpRetrievePhotoRespBLO>(this);



            blo.DTO = this;

            return blo;
        }
    }

    public class RetrieveDocumentPhotoResultMapProfile : Profile
    {
        public RetrieveDocumentPhotoResultMapProfile()
        {
            CreateMap<RetrieveDocumentPhotoResult, EmpRetrievePhotoRespBLO>();
        }
    }

    public partial class ConfirmDocumentPhotoResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ConfirmDocumentPhotoResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpConfirmPhotoRespBLO>(this);


            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;


            blo.DTO = this;


            return blo;
        }
    }

    public class ConfirmDocumentPhotoResultMapProfile : Profile
    {
        public ConfirmDocumentPhotoResultMapProfile()
        {
            CreateMap<ConfirmDocumentPhotoResult, EmpConfirmPhotoRespBLO>();
        }
    }


    public partial class SubmitDhsReferralResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitDhsReferralResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);
         
            var blo = mapper.Map<EmpSubmitDHSReferralRespBLO>(this);




            blo.DTO = this;


            blo.ContactDHSByDt = Convert.ToDateTime(this.ContactByDate);

            return blo;
        }
    }

    public class SubmitDhsReferralResultMapProfile : Profile
    {
        public SubmitDhsReferralResultMapProfile()
        {
            CreateMap<SubmitDhsReferralResult, EmpSubmitDHSReferralRespBLO>();
        }
    }

    public partial class SubmitAdditionalVerificationResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SubmitAdditionalVerificationResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpSubmitAdditVerifRespBLO>(this);




            blo.DTO = this;


            return blo;
        }
    }

    public class SubmitAdditionalVerificationResultMapProfile : Profile
    {
        public SubmitAdditionalVerificationResultMapProfile()
        {
            CreateMap<SubmitAdditionalVerificationResult, EmpSubmitAdditVerifRespBLO>();
        }
    }

    public partial class GetResolvedCasesResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetResolvedCasesResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);
            
            var blo = mapper.Map<EmpGetNextResolvedCaseNbrsRespBLO>(this);

            Fixer.TryFixInvalidWithGetCaseDetails(blo.CaseList);
            Fixer.RemoveInvalidFromCollection(blo.CaseList);

            //blo.CaseList = blo.CaseList.Where(x => Fixer.IsValid(x)).ToArray();
            blo.NumberOfCases = blo.CaseList.Count();

            if (this.CaseList.Count() > blo.CaseList.Count())
            {
                int count = this.CaseList.Count() - blo.CaseList.Count();
                string msg = string.Format("Mapping was missed for {0} cases; RawRespObject= ", count);
                Log_Error.AddError("Map<EmpGetNextResolvedCaseNbrsRespBLO>", "EVerifyService", msg + CustomTextMessageEncoder.LastResponceObjectRawString, true, false, "WP", string.Empty, "", "EVerify");
            }

            blo.DTO = this;
            return blo;
        }
    }

    public class GetResolvedCasesResultMapProfile : Profile
    {
        public GetResolvedCasesResultMapProfile()
        {
            CreateMap<GetResolvedCasesResult, EmpGetNextResolvedCaseNbrsRespBLO>();

            CreateMap<ResolvedCaseListRecord, CaseBLO>().AfterMap(
                (src, dest) =>
                {
                    try
                    {
                        dest.CaseNbr = src.CaseNumber;
                        dest.EmployerCaseID = src.EmployerCaseId;
                        dest.ResolveDate = src.ResolvedDate == null
                            ? DateTime.MinValue
                            : (DateTime) src.ResolvedDate;

                        if (!Fixer.IsValid(src))
                        {
                            Fixer.MarkInvalid(dest);
                        }
                        else
                        {
                            dest.ResponseCode = !src.ResolutionCode.IsNullOrEmpty()
                                ? src.ResolutionCode
                                : src.MessageCode.ToString();
                        }

                        dest.ResponseStmt = src.EligibilityStatement;
                        dest.TypeOfCase = (Proxy_v29.VerificationStepType) (int) src.VerificationStep;
                        dest.DTO = src;
                    }
                    catch
                    {
                        Fixer.MarkInvalid(dest);
                    }
                });
        }
    }

    public partial class ResolvedCaseListRecord : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ResolvedCaseListRecordMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<CaseBLO>(this);

            blo.DTO = this;
            return blo;
        }
    }

    public class ResolvedCaseListRecordMapProfile : Profile
    {
        public ResolvedCaseListRecordMapProfile()
        {
            CreateMap<ResolvedCaseListRecord, CaseBLO>().AfterMap(
                (src, dest) =>
                {
                    dest.CaseNbr = src.CaseNumber;
                    dest.EmployerCaseID = src.EmployerCaseId;
                    dest.ResolveDate = src.ResolvedDate == null ? DateTime.MinValue : (DateTime)src.ResolvedDate;
                    dest.ResponseCode = !src.ResolutionCode.IsNullOrEmpty() ? src.ResolutionCode : src.MessageCode.ToString();
                    dest.ResponseStmt = src.EligibilityStatement;
                    dest.TypeOfCase = (Proxy_v29.VerificationStepType)(int)src.VerificationStep;
                    dest.DTO = src;
                });
        }
    }

    public class GetCaseDetailsResultMapProfile : Profile
    {
        public GetCaseDetailsResultMapProfile()
        {
            CreateMap<GetCaseDetailsResult, EmpGtCseDetailsRespBLO>();
        }
    }


    public partial class ConfirmResolvedCasesReceivedResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ConfirmResolvedCasesReceivedResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpAckReceiptOfResolvedCaseNbrRespBLO>(this);



            blo.DTO = this;


            return blo;
        }
    }

    public class ConfirmResolvedCasesReceivedResultMapProfile : Profile
    {
        public ConfirmResolvedCasesReceivedResultMapProfile()
        {
            CreateMap<ConfirmResolvedCasesReceivedResult, EmpAckReceiptOfResolvedCaseNbrRespBLO>();
        }
    }


    public partial class GetCaseClosureReasonsResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetCaseClosureReasonsResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpClosureCodesRespBLO>(this);

            blo.NumberOfClosureCodes = this.ClosureReasonList.Count();
            blo.ClosureCodeList = this.ClosureReasonList.Select(x =>
            {
                return new ClosureCodeBLO()
                {
                    Code = x.Code,
                    Descr = x.Description,
                };
            }).ToArray();

            blo.DTO = this;
            return blo;
        }
    }

    public class GetCaseClosureReasonsResultMapProfile : Profile
    {
        public GetCaseClosureReasonsResultMapProfile()
        {
            CreateMap<GetCaseClosureReasonsResult, EmpClosureCodesRespBLO>();
        }
    }

    public partial class CloseCaseResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CloseCaseResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpCloseCaseRespBLO>(this);

            blo.DTO = this;

            return blo;
        }
    }

    public class CloseCaseResultMapProfile : Profile
    {
        public CloseCaseResultMapProfile()
        {
            CreateMap<CloseCaseResult, EmpCloseCaseRespBLO>();
        }
    }

    public partial class GetCaseAlertCountsResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetCaseAlertCountsResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = Mapper.Map<EmpCaseAlertsRespBLO>(this);

            blo.CloseCount = this.CasesToBeClosed;
            blo.UpdatedCasesCount = this.CasesWithNewUpdates;
            blo.ExpiringCasesCount = this.WorkAuthorizationDocsExpiring;

            blo.DTO = this;

            return blo;
        }
    }

    public class GetCaseAlertCountsResultMapProfile : Profile
    {
        public GetCaseAlertCountsResultMapProfile()
        {
            CreateMap<GetCaseAlertCountsResult, EmpCaseAlertsRespBLO>();
        }
    }

    public partial class GetCasesByAlertTypeResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetCasesByAlertTypeResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);


            var blo = mapper.Map<EmpAlertCasesRespBLO>(this);
            blo.CaseCount = this.TotalCases;
            blo.CaseNbrList = this.CaseList == null || !this.CaseList.Any() ?
                new ArrayOfStringBLO() :
                new ArrayOfStringBLO(this.CaseList.Select(e => e.CaseNumber).ToList());

            blo.DTO = this;

            return blo;
        }
    }

    public class GetCasesByAlertTypeResultMapProfile : Profile
    {
        public GetCasesByAlertTypeResultMapProfile()
        {
            CreateMap<GetCasesByAlertTypeResult, EmpAlertCasesRespBLO>();
        }
    }

    public partial class GetCitizenshipStatusesResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetCitizenshipStatusesResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpGetCitizenshipCodesRespBLO>(this);
            blo.NumberOfCitizenshipCodes = this.CitizenshipStatusList.Count();
            blo.CitizenshipCodeList = this.CitizenshipStatusList.Select(x =>
                                                                        {
                                                                            return new CitizenshipCodeBLO()
                                                                                   {
                                                                                       Code = x.Code,
                                                                                       Descr = x.Description,
                                                                                   };
                                                                        }).ToArray();

            blo.DTO = this;
            return blo;
        }
    }

    public class GetCitizenshipStatusesResultMapProfile : Profile
    {
        public GetCitizenshipStatusesResultMapProfile()
        {
            CreateMap<GetCitizenshipStatusesResult, EmpGetCitizenshipCodesRespBLO>();
        }
    }


    public partial class GetIssuingAuthoritiesResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetIssuingAuthoritiesResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpGetIssuingAuthoritiesRespBLO>(this);
            blo.NumberOfIssuingAuthorities = this.IssuingAuthorityList.Count();




            blo.DTO = this;


            return blo;
        }
    }

    public class GetIssuingAuthoritiesResultMapProfile : Profile
    {
        public GetIssuingAuthoritiesResultMapProfile()
        {
            CreateMap<GetIssuingAuthoritiesResult, EmpGetIssuingAuthoritiesRespBLO>();
            CreateMap<IssuingAuthority, IssuingAuthorityBLO>().AfterMap(
                (src, dest) =>
                {
                    dest.SupportingDocumentId =
                        src.SupportingDocumentTypeId == null ? 0 : (int)src.SupportingDocumentTypeId;
                    // 0 = null valid types 1 and 3
                });
        }
    }

    public partial class RetrieveLetterResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RetrieveLetterResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);


            var blo = mapper.Map<EmpRetrieveFANRespBLO>(this);

            blo.FAN = this.Letter;



            blo.DTO = this;


            return blo;
        }
    }

    public class RetrieveLetterResultMapProfile : Profile
    {
        public RetrieveLetterResultMapProfile()
        {
            CreateMap<RetrieveLetterResult, EmpRetrieveFANRespBLO>();
        }
    }

    public partial class GetDuplicateCaseListResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetDuplicateCaseListResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpGetDuplicateCaseListRespBLO>(this);




            blo.DTO = this;


            return blo;
        }
    }

    public class GetDuplicateCaseListResultMapProfile : Profile
    {
        public GetDuplicateCaseListResultMapProfile()
        {
            CreateMap<GetDuplicateCaseListResult, EmpGetDuplicateCaseListRespBLO>();

            CreateMap<DuplicateCaseRecord, DupCaseListItemBLO>().AfterMap(
                (src, dest) =>
                {
                    dest.CaseNbr = src.CaseNumber;
                    dest.DocumentSsn = src.Ssn;
                    dest.CreatedDate = Convert.ToDateTime(src.CaseCreatedDate);
                    dest.HireDate = Convert.ToDateTime(src.HireDate);
                    dest.CaseStatusDisplay = src.EligibilityStatement;
                    dest.CreatorUserName = src.CaseCreatorLogonId;
                    dest.DTO = src;
                }
            );
        }
    }

    public partial class ContinueDuplicateCaseWithChangeResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContinueDuplicateCaseWithChangeResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpInitVerfRespBLO>(this);

            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;
            blo.EligStatementDetailsTxt = this.EligibilityStatementDetails;

            blo.LastName = this.SystemLastName;
            blo.FirstName = this.SystemFirstName;

            blo.DTO = this;
            return blo;
        }
    }

    public class ContinueDuplicateCaseWithChangeResultMapProfile : Profile
    {
        public ContinueDuplicateCaseWithChangeResultMapProfile()
        {
            CreateMap<ContinueDuplicateCaseWithChangeResult, EmpInitVerfRespBLO>();
        }
    }


    public partial class ContinueDuplicateCaseResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContinueDuplicateCaseResultResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);

            var blo = mapper.Map<EmpInitVerfRespBLO>(this);

            blo.CaseStatus = this.MessageCode.ToString();
            blo.CaseStatusDisplay = this.EligibilityStatement;
            blo.EligStatementDetailsTxt = this.EligibilityStatementDetails;

            blo.LastName = this.SystemLastName;
            blo.FirstName = this.SystemFirstName;

            blo.DTO = this;
            return blo;
        }
    }

    public class ContinueDuplicateCaseResultResultMapProfile : Profile
    {
        public ContinueDuplicateCaseResultResultMapProfile()
        {
            CreateMap<ContinueDuplicateCaseResult, EmpInitVerfRespBLO>();
        }
    }

    public partial class GetCaseDhsReverifyFieldsResult : IDTOToBLOConverter
    {
        public object ToBLO()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GetCaseDhsReverifyFieldsResultResultMapProfile>();
            });

            IMapper mapper = new Mapper(config);
            
            var blo = mapper.Map<GetCaseDhsReverifyFieldsResultBLO>(this);

            blo.DTO = this;
            return blo;
        }
    }

    public class GetCaseDhsReverifyFieldsResultResultMapProfile : Profile
    {
        public GetCaseDhsReverifyFieldsResultResultMapProfile()
        {
            CreateMap<DhsReverifyField, DhsReverifyFieldBLO>();
            CreateMap<GetCaseDhsReverifyFieldsResult, GetCaseDhsReverifyFieldsResultBLO>()
                .ForMember(dest => dest.DhsReverifyFieldList, opts => opts.MapFrom(src => src.DhsReverifyFieldList));
        }
    }

    #endregion

    #region Utills
    public class BLOTo_v29
    {
        public static Proxy_v29.AlertType ConvertAlertType(EVerify.AlertType alertType)
        {
            return ConvertAlertType(alertType.ToString());
        }

        public static Proxy_v29.AlertType ConvertAlertType(string alertTypeStr)
        {
            EVerify.AlertType alertType;
            if (!EVerify.AlertType.TryParse(alertTypeStr, true, out alertType))
            {
                throw new Exception("Can't convert AlertType!");
            }

            switch (alertType)
            {
                case EVerify.AlertType.C:
                    return Proxy_v29.AlertType.CASES_TO_BE_CLOSED;
                case EVerify.AlertType.E:
                    return Proxy_v29.AlertType.WORK_AUTHORIZATION_DOCS_EXPIRING;
                case EVerify.AlertType.U:
                    return Proxy_v29.AlertType.CASES_WITH_NEW_UPDATES;
            }
            return Proxy_v29.AlertType.CASES_WITH_NEW_UPDATES;

        }
    }

    #endregion
}
