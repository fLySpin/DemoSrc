using EVerify;
using EVerify.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Entity;
using WebApplication.Entity.Data;
using WebApplication.Error;
using WebApplication.Utility;
using WebApplication.Utility.StringExtensions;

namespace TestEmployeeKiosk.EVerify
{
    public enum EmpInitDABPVerifTestMode
    {
        OfflineTestingModeRequestWriter,
        OnlineRequestResponceWriter,
    }

    public class EmpInitDABPVerifTestCase
    {
        public enum ListType
        {
            AB, C
        }

        public string TestCaseID { get; set; }
        public bool IsActive { get; set; }
        public CitizenshipStatus SitezenshipStatus { get; set; }
        public ListADocumentType ListADocumentType { get; set; }
        public ListBDocumentType? ListBDocumentType { get; set; }
        public ListCDocumentType? ListCDocumentType { get; set; }

        public string I9FormListADocumentType { get; set; }
        public string I9FormListBDocumentType { get; set; }
        public string I9FormListCDocumentType { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Ssn { get; set; }
        public string BirthDate { get; set; }
        public string ProvidedEmailAddress { get; set; }
        public string WorkState { get; set; }
        public string AlienNumber { get; set; }
        public string AdmissionNumber { get; set; }
        public string ForeignPassportNumber { get; set; }
        public COICountryCodes? ForeignPassport_CountryOfIssuance { get; set; }


        public string AB_DocNumber { get; set; }
        public string AB_DocExpDate { get; set; }
        public bool? AB_HasExpirationDate { get; set; }
        public IssuingAuthoritiesValidValues? AB_IssuingAuthority { get; set; }
        public string AB_IssuingAuthorityDescription { get; set; }
        public COICountryCodes? AB_CountryOfIssuance { get; set; }

        public string C_DocNumber { get; set; }
        public string C_DocExpDate { get; set; }
        public IssuingAuthoritiesValidValues? C_IssuingAuthority { get; set; }
        public string C_IssuingAuthorityDescription { get; set; }
        public COICountryCodes? C_CountryOfIssuance { get; set; }
        public EmpInitDABPVerifRequestBodyBLO EmpInitDABPVerifRequestExpected { get; set; }
        public EmpInitDABPVerifRequestBodyBLO EmpInitDABPVerifRequestActual { get; set; }
        public EmpInitVerfRespBLO EmpInitDABPVerifResponce { get; set; }
        public string ErrorOnInitialSubmit { get; set; }
        public string ExpectedMsgAllSubmitDocs { get; set; }

        public string GetOnboardingIssuingAuthorityTableRowID(ListType listType)
        {
            var table = new OnboardingIssuingAuthorityDataProvider().GetEntityList();

            var issuingAuthority = listType == ListType.AB ? AB_IssuingAuthority : C_IssuingAuthority;
            var issuingAuthorityDescription = listType == ListType.AB ? AB_IssuingAuthorityDescription : C_IssuingAuthorityDescription;

            if (!issuingAuthorityDescription.IsNullOrEmpty())
            {
                return table.Where(e => e.Description.EqualsIgnoreCase(issuingAuthorityDescription.ToString())).FirstOrDefault().IssuingAuthorityID;
            }
            else if (issuingAuthority != null)
            {
                return table.Where(e => e.IssuingAuthority.EqualsIgnoreCase(issuingAuthority.ToString())).FirstOrDefault().IssuingAuthorityID;
            }
            return "";
        }
    }

    [Ignore]
    [TestClass()]
    public class EmpInitDABPVerifTest : TestFixtureBase
    {
        private static string casesPath = "EVerify\\EmpInitDABPVerifTestCases.xml";
        //private static string tmpFilePath = "EVerify\\EmpInitDABPVerifTestCaseTmp.xml";
        private static string _employeeType = "FT";
        private EmpInitDABPVerifTestMode empInitDABPVerifTestMode = EmpInitDABPVerifTestMode.OfflineTestingModeRequestWriter;

        private void Commit()
        {
            if (empInitDABPVerifTestMode == EmpInitDABPVerifTestMode.OnlineRequestResponceWriter)
            {
                CommitChanges();
            }
        }

        private void Init()
        {
            ErrorHandler.Clear();
            Authenticate();
            EmployeeKioskEntityObjects.CompanyEssOption.EVerifyClientCompanyID = "18245";
            // commit right after log
            OnboardingEVerifyLog.TestCallbak = () =>
            {
                Commit();
                TestInitialize();
            };

            var empTypeList = CEmpType.GetEntityListForCompany(this.Co);
            if (empTypeList != null && empTypeList.Any())
                _employeeType = empTypeList[0].EmpType;
        }

        private static void CreateCase(EmpInitDABPVerifTestCase empInitDABPVerifTestCase, out OnboardingEInfo einfo)
        {
            bool success;
            var flow = new OnboardingFlowDataProvider().GetEntity(new Hashtable());
            var flowStep = new OnboardingFlowStepDataProvider().GetEntity(new Hashtable());

            einfo = OnboardingEInfo.CreateEntity(true);
            {
                einfo.FirstName = empInitDABPVerifTestCase.FirstName;
                einfo.LastName = empInitDABPVerifTestCase.LastName;
                einfo.MiddleName = empInitDABPVerifTestCase.MiddleName;
                einfo.Ssn = empInitDABPVerifTestCase.Ssn;
                einfo.BirthDate = empInitDABPVerifTestCase.BirthDate;
                einfo.HireDate = DateTime.Now.ToString("yyyy-MM-dd");
                einfo.FlowGuid = flow.GuidField;
                einfo.OnboardingFlowStepGuid = flowStep.GuidField;
                einfo.AnnualSalary = "5000";
                einfo.PayFrequency = "M";
                einfo.DefaultHours = "10";
                einfo.WorkState = empInitDABPVerifTestCase.WorkState;
                einfo.IsProvideEmailToDHS = true.ToString();
                einfo.ProvidedEmailAddress = empInitDABPVerifTestCase.ProvidedEmailAddress;
                einfo.StartDate = DateTime.Now.ToShortDateString();
                einfo.EmpType = _employeeType;

                var cc1Info = new CDept1DataProvider()
                    .GetEntityList(new Hashtable() { { CDept1.DBFieldName.Co.ToString(), EntityObjects.CInfo.Co } })
                    .FirstOrDefault();
                if (cc1Info != null)
                    einfo.Cc1 = cc1Info.Cc1;
            }

            success = einfo.Save();
            Assert.IsTrue(success, "Create OnboardingEinfo failed: " + ErrorHandler.GetErrorListString);

            var listADoc = new OnboardingI9Document();
            var listBDoc = new OnboardingI9Document();
            var listCDoc = new OnboardingI9Document();

            var listABDoc = new OnboardingI9Document()
            {
                Guidfield = "",
                CurrentEntityState = KYSSEntityBase.EntityState.NewUnmodified,
                OnboardingEInfoGuid = einfo.Guidfield,
                CountryOfIssuance = empInitDABPVerifTestCase.AB_CountryOfIssuance.ToString(),
                IssuingAuthorityID = empInitDABPVerifTestCase.GetOnboardingIssuingAuthorityTableRowID(EmpInitDABPVerifTestCase.ListType.AB),
                DocumentNumber = empInitDABPVerifTestCase.AB_DocNumber,
                ExpirationDate = empInitDABPVerifTestCase.AB_DocExpDate,
            };

            listABDoc.HasExpirationDate = empInitDABPVerifTestCase.AB_HasExpirationDate == null
                ? listABDoc.HasExpirationDate
                : empInitDABPVerifTestCase.AB_HasExpirationDate.ToString();

            success = listABDoc.Save();
            Assert.IsTrue(success, "Create Onboardingi9Document(list AB) failed");



            if (empInitDABPVerifTestCase.ListADocumentType == ListADocumentType.ListBandCDocuments)
            {
                listBDoc = listABDoc;
                listCDoc = new OnboardingI9Document()
                {
                    OnboardingEInfoGuid = einfo.Guidfield,
                    CountryOfIssuance = empInitDABPVerifTestCase.C_CountryOfIssuance.ToString(),
                    IssuingAuthorityID = empInitDABPVerifTestCase.GetOnboardingIssuingAuthorityTableRowID(EmpInitDABPVerifTestCase.ListType.C),
                    DocumentNumber = empInitDABPVerifTestCase.C_DocNumber,
                    ExpirationDate = empInitDABPVerifTestCase.C_DocExpDate,
                };
                success = listCDoc.Save();
                Assert.IsTrue(success, "Create Onboardingi9Document(list B) failed");
            }
            else
            {
                listADoc = listABDoc;
            }

            var listADocumentTypeID = "";
            var listBDocumentTypeID = "";
            var listCDocumentTypeID = "";

            var listAI9DocumentId = "";
            var listBI9DocumentId = "";
            var listCI9DocumentId = "";

            if (empInitDABPVerifTestCase.ListADocumentType == ListADocumentType.ListBandCDocuments)
            {
                listBDocumentTypeID =
                    OnboardingI9DocumentType.GetEntityByCitizenshipStatusAndEVerifyDocumentIdAndDocumentType(
                        (int)empInitDABPVerifTestCase.SitezenshipStatus,
                        empInitDABPVerifTestCase.ListBDocumentType.ToIntString(),
                        OnboardingI9Document.DocumentTypes.B).I9DocumentTypeID;
                listCDocumentTypeID =
                    OnboardingI9DocumentType.GetEntityByCitizenshipStatusAndEVerifyDocumentIdAndDocumentType(
                        (int)empInitDABPVerifTestCase.SitezenshipStatus,
                        empInitDABPVerifTestCase.ListCDocumentType.ToIntString(),
                        OnboardingI9Document.DocumentTypes.C).I9DocumentTypeID;
                listBI9DocumentId = empInitDABPVerifTestCase.ListBDocumentType.ToIntString();
                listCI9DocumentId = empInitDABPVerifTestCase.ListCDocumentType.ToIntString();
            }

            listAI9DocumentId = empInitDABPVerifTestCase.ListADocumentType.ToIntString();
            listADocumentTypeID = OnboardingI9DocumentType.GetEntityByCitizenshipStatusAndEVerifyDocumentIdAndDocumentType(
                      (int)empInitDABPVerifTestCase.SitezenshipStatus,
                      empInitDABPVerifTestCase.ListADocumentType.ToIntString(),
                      OnboardingI9Document.DocumentTypes.A).I9DocumentTypeID;

            var i9 = new OnboardingI9()
            {
                OnboardingEInfoGuid = einfo.Guidfield,
                CitizenshipStatusID = empInitDABPVerifTestCase.SitezenshipStatus.ToIntString(),
                AlienNumber = empInitDABPVerifTestCase.AlienNumber,
                AdmissionNumber = empInitDABPVerifTestCase.AdmissionNumber,
                ForeignPassportNumber = empInitDABPVerifTestCase.ForeignPassportNumber,
                CountryOfIssuance = empInitDABPVerifTestCase.ForeignPassport_CountryOfIssuance.ToString(),

                ListADocumentTypeID = listADocumentTypeID,
                ListBDocumentTypeID = listBDocumentTypeID,
                ListCDocumentTypeID = listCDocumentTypeID,
                ListAI9DocumentGuid = listADoc.Guidfield,
                ListBI9DocumentGuid = listBDoc.Guidfield,
                ListCI9DocumentGuid = listCDoc.Guidfield,
                ListAI9DocumentId = listAI9DocumentId,
                ListBI9DocumentId = listBI9DocumentId,
                ListCI9DocumentId = listCI9DocumentId,

                ActiveStage = OnboardingI9.ActiveStageEnum.Active.ToIntString(),
            };

            i9.ApplyDefaults();
            success = i9.Save();
            Assert.IsTrue(success, "Create Onboardingi9 failed");
        }

        [DeploymentItem("EVerify\\EmpInitDABPVerifTestCases.xml", "EVerify")]
        [TestMethod]
        public void EmpInitDABPVerifTestCases()
        {
            Init();

            var cases = casesPath.XmlFileToObject<List<EmpInitDABPVerifTestCase>>();
            var notPassedCases = new List<EmpInitDABPVerifTestCase>();
            foreach (var casee in cases.Where(c => c.IsActive))
            {
                OnboardingEInfo einfo = null;
                CreateCase(casee, out einfo);

                var requestActual = EVerifyService.AssembleI9CaseFlat(einfo.Guidfield).ToBLO<EmpInitDABPVerifRequestBodyBLO>();

                switch (empInitDABPVerifTestMode)
                {
                    case EmpInitDABPVerifTestMode.OfflineTestingModeRequestWriter:
                        if (!EmpInitDABPVerifRequestBodyBLO.Equals(requestActual, casee.EmpInitDABPVerifRequestExpected))
                        {
                            notPassedCases.Add(casee);
                        }

                        break;

                    case EmpInitDABPVerifTestMode.OnlineRequestResponceWriter:
                        var ret = EVerifyService.CreateCase(einfo.Guidfield);
                        if (!ret)
                        {
                            notPassedCases.Add(casee);
                        }

                        casee.EmpInitDABPVerifResponce = (EmpInitVerfRespBLO)EVerifyService.LastResponce;
                        break;
                }

                casee.EmpInitDABPVerifRequestActual = requestActual;
            }

            cases.ToXmlFile(casesPath);
            Assert.IsTrue(notPassedCases.Count() == 0, "EmpInitDABPVerifRequest compare failed in TestCaseID : " + string.Join(",", notPassedCases.Select(c => c.TestCaseID).ToArray()));
        }

        public List<EmpInitDABPVerifTestCase> EmpInitDABPVerifTestCaseID(Func<EmpInitDABPVerifTestCase, string> createCaseeUserMethod, string testCaseID, string casesPath, EmpInitDABPVerifTestMode empInitDABPVerifTestMode)

        {
            var cases = casesPath.XmlFileToObject<List<EmpInitDABPVerifTestCase>>();
            var notPassedCases = new List<EmpInitDABPVerifTestCase>();

            var activeCase = cases.First(e => e.TestCaseID == testCaseID);
            var onboardingEinfoGuid = createCaseeUserMethod(activeCase);

            if (!String.IsNullOrEmpty(onboardingEinfoGuid))
            {
                var requestActual = EVerifyService.AssembleI9CaseFlat(onboardingEinfoGuid).ToBLO<EmpInitDABPVerifRequestBodyBLO>();
                switch (empInitDABPVerifTestMode)
                {
                    case EmpInitDABPVerifTestMode.OfflineTestingModeRequestWriter:
                        if (!EmpInitDABPVerifRequestBodyBLO.Equals(requestActual, activeCase.EmpInitDABPVerifRequestExpected))
                        {
                            notPassedCases.Add(activeCase);
                        }
                        break;
                    case EmpInitDABPVerifTestMode.OnlineRequestResponceWriter:
                        var ret = EVerifyService.CreateCase(onboardingEinfoGuid);
                        if (!ret)
                        {
                            notPassedCases.Add(activeCase);
                        }
                        activeCase.EmpInitDABPVerifResponce = (EmpInitVerfRespBLO)EVerifyService.LastResponce;
                        break;
                }
                activeCase.EmpInitDABPVerifRequestActual = requestActual;
            }

            cases.ToXmlFile(casesPath);
            return notPassedCases;
        }

        public List<EmpInitDABPVerifTestCase> EmpInitDABPVerifTestCases(Func<EmpInitDABPVerifTestCase, string> createCaseeUserMethod, string casesPath, EmpInitDABPVerifTestMode empInitDABPVerifTestMode/*, out string onboardingEinfoGuid*/)
        {
            var cases = casesPath.XmlFileToObject<List<EmpInitDABPVerifTestCase>>();
            var notPassedCases = new List<EmpInitDABPVerifTestCase>();

            var activeCases = cases.Where(c => c.IsActive);
            foreach (var casee in activeCases)
            {
                var onboardingEinfoGuid = createCaseeUserMethod(casee);

                if (!String.IsNullOrEmpty(onboardingEinfoGuid))
                {
                    var requestActual = EVerifyService.AssembleI9CaseFlat(onboardingEinfoGuid).ToBLO<EmpInitDABPVerifRequestBodyBLO>();
                    switch (empInitDABPVerifTestMode)
                    {
                        case EmpInitDABPVerifTestMode.OfflineTestingModeRequestWriter:
                            if (!EmpInitDABPVerifRequestBodyBLO.Equals(requestActual, casee.EmpInitDABPVerifRequestExpected))
                            {
                                notPassedCases.Add(casee);
                            }
                            break;
                        case EmpInitDABPVerifTestMode.OnlineRequestResponceWriter:
                            var ret = EVerifyService.CreateCase(onboardingEinfoGuid);
                            if (!ret)
                            {
                                notPassedCases.Add(casee);
                            }
                            casee.EmpInitDABPVerifResponce = (EmpInitVerfRespBLO)EVerifyService.LastResponce;
                            break;
                    }
                    casee.EmpInitDABPVerifRequestActual = requestActual;
                }
            }
            cases.ToXmlFile(casesPath);
            return notPassedCases;
        }
    }
}

