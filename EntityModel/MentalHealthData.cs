﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace EntityModel
{
    public class MentalHealthData : ServiceData
    {
        public const string TABLE_NAME = "tira_substanceuses";
        public const string PRIMARY_KEY = "TODO";
        public const string SERVICE_NAME = "Mental Health";

        [JsonProperty(PropertyName = "TODO")]
        public int InPatientTotal { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public int InPatientOpen { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public bool InPatientHasWaitlist { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public bool InPatientWaitlistIsOpen { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public int OutPatientTotal { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public int OutPatientOpen { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public bool OutPatientHasWaitlist { get; set; }

        [JsonProperty(PropertyName = "TODO")]
        public bool OutPatientWaitlistIsOpen { get; set; }

        public override IContractResolver ContractResolver() { return Resolver.Instance; }
        public override string TableName() { return TABLE_NAME; }
        public override string PrimaryKey() { return PRIMARY_KEY; }
        public override ServiceType ServiceType() { return EntityModel.ServiceType.MentalHealth; }
        public override string ServiceTypeName() { return SERVICE_NAME; }

        public override List<UpdateSteps> UpdateSteps()
        {
            return new List<UpdateSteps>()
            {
                new UpdateSteps()
                {
                    Name = "Mental Health In-Patient",
                    TotalPropertyName = nameof(this.InPatientTotal),
                    OpenPropertyName = nameof(this.InPatientOpen),
                    HasWaitlistPropertyName = nameof(this.InPatientHasWaitlist),
                    WaitlistIsOpenPropertyName = nameof(this.InPatientWaitlistIsOpen)
                },
                new UpdateSteps()
                {
                    Name = "Mental Health Out-Patient",
                    TotalPropertyName = nameof(this.OutPatientTotal),
                    OpenPropertyName = nameof(this.OutPatientOpen),
                    HasWaitlistPropertyName = nameof(this.OutPatientHasWaitlist),
                    WaitlistIsOpenPropertyName = nameof(this.OutPatientWaitlistIsOpen)
                }
            };
        }

        public override void CopyStaticValues<T>(T data)
        {
            var d = data as MentalHealthData;

            this.InPatientTotal = d.InPatientTotal;
            this.OutPatientTotal = d.OutPatientTotal;

            this.InPatientHasWaitlist = d.InPatientHasWaitlist;
            this.OutPatientHasWaitlist = d.OutPatientHasWaitlist;

            base.CopyStaticValues(data);
        }

        public class Resolver : ContractResolver<CaseManagementData>
        {
            public static Resolver Instance = new Resolver();

            private Resolver()
            {
                AddMap(x => x.Id, "TODO");
                AddMap(x => x.ServiceId, "TODO");
            }
        }
    }
}
